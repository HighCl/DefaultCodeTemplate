using System;
using System.Collections;
using UnityEngine;
#if INPUT_TYPE_NEW
using UnityEngine.InputSystem;
#endif

namespace DefaultSetting
{
#if INPUT_TYPE_NEW
    [RequireComponent(typeof(PlayerInput))]
    public class NewInputManager : MonoBehaviour
    {
        #region Class

        [Serializable]
        class BooleanWrapper
        {
            [field: SerializeField] public bool Value { get; set; } = false;
        }

        [Serializable]
        class KeyGroup
        {
            [SerializeField, ReadOnly] private BooleanWrapper _waitKeyDown = new BooleanWrapper();
            [SerializeField, ReadOnly] private BooleanWrapper _waitKeyUp = new BooleanWrapper();
            [SerializeField, ReadOnly] public bool WaitKeyPressed;

            [SerializeField] private float _waitKeyDownTime = 0.2f;
            [SerializeField] private float _waitKeyUpTime = 0.2f;

            private Coroutine _coWaitKeyDown;
            private Coroutine _coWaitKeyUp;

            public bool WaitKeyDown
            {
                get => _waitKeyDown.Value;
                set
                {
                    if (value == false && _coWaitKeyDown != null)
                        Managers.Input.StopCoroutine(_coWaitKeyDown);

                    _waitKeyDown.Value = value;
                }
            }
            public bool WaitKeyUp
            {
                get => _waitKeyUp.Value;
                set
                {
                    if (value == false && _coWaitKeyUp != null)
                        Managers.Input.StopCoroutine(_coWaitKeyUp);

                    _waitKeyUp.Value = value;
                }
            }

            public void OnStartWaitKeyDown() => StartOrRestartCoroutine(_coWaitKeyDown, Co_KeyControl(_waitKeyDown, _waitKeyDownTime, _coWaitKeyDown));
            public void OnStartWaitKeyUp() => StartOrRestartCoroutine(_coWaitKeyUp, Co_KeyControl(_waitKeyUp, _waitKeyUpTime, _coWaitKeyUp));

            void StartOrRestartCoroutine(Coroutine coroutine, IEnumerator routine)
            {
                if (coroutine != null)
                    Managers.Input.StopCoroutine(coroutine);

                coroutine = Managers.Input.StartCoroutine(routine);
            }

            IEnumerator Co_KeyControl(BooleanWrapper booleanWrapper, float waitTime, Coroutine myCoroutine)
            {
                booleanWrapper.Value = true;
                yield return new WaitForSeconds(waitTime);
                booleanWrapper.Value = false;
                myCoroutine = null;
            }
        }

        #endregion

        private PlayerInput playerInput;

        public void Init()
        {
            print("InputManager:\n뉴 입력 시스템이 적용중입니다.");

            playerInput = GetComponent<PlayerInput>();
            playerInput.actions = Managers.Resource.Load<InputActionAsset>("PlayerInput");
            playerInput.actions.FindActionMap("Player").Enable();
        }

        public void OnUpdate()
        {

        }

        public void OnTest()
        {
#if UNITY_EDITOR
            Managers.Test.OnTest();
#endif
        }

        #region Input Setting, Rebind, KeyBind Function

        /// <summary> 바인드된 키가 있으면 수정해주는 함수 </summary>
        public void SetBindingKey()
        {
            string rebinds = Managers.Data.CustomGetPrefsString(Define.REBINDS_KEY, string.Empty);

            if (string.IsNullOrEmpty(rebinds))
            {
                ResetAllBindings();
                return;
            }

            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
        public void ResetAllBindings()
        {
            foreach (InputActionMap map in playerInput.actions.actionMaps)
            {
                map.RemoveAllBindingOverrides();
            }
        }
        public void SaveRebindKey()
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson();
            Managers.Data.CustomSetPrefsString(Define.REBINDS_KEY, rebinds);
        }

        //키의 이름을 보내주면 그 키의 바인딩된 값을 넘겨주는 함수가 필요하겠지?
        //목표: 키의 바인딩된 값을 넘겨주는 함수

        //1. 

        public string GetBindingKey(string actionName)
        {
            string returnStr = null;

            //action이 필요하다.
            InputActionMap inputActionMap = playerInput.actions.FindActionMap("Player");
            InputAction currentInputAction = inputActionMap.FindAction(actionName);
            //print(currentInputAction);

            if (currentInputAction == null)
            {
                Debug.LogWarning("InputAction을 찾지 못함");
                return null;
            }

            int bindingIndex;
            if (actionName == "MoveHorizontal")
            {
                bindingIndex = currentInputAction.GetBindingIndexForControl(currentInputAction.controls[0]);

                //인덱스를 통해 키를 반환한다.
                string currentRightKey = InputControlPath.ToHumanReadableString(
                    currentInputAction.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);

                string currentLeftKey = InputControlPath.ToHumanReadableString(
                    currentInputAction.bindings[bindingIndex + 1].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);

                returnStr = $"{currentRightKey} {currentLeftKey}";
            }
            else
            {
                bindingIndex = currentInputAction.GetBindingIndexForControl(currentInputAction.controls[0]);

                //인덱스를 통해 키를 반환한다.
                string currentKey = InputControlPath.ToHumanReadableString(
                    currentInputAction.bindings[bindingIndex].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
                returnStr = currentKey;
            }

            return returnStr;
        }

        #endregion

        //아래로는 각 키의 바인드 내용
        #region Binding Keys

        [field: Header("마우스 좌표")]
        [field: ReadOnly] public Vector2 MousePos { get; private set; }
        void OnMousePosition(InputValue inputValue)
        {
            MousePos = inputValue.Get<Vector2>();
        }


        [field: Header("움직임 입력")]
        [ReadOnly] public Vector2 inputMoveDir;
        [ReadOnly] public bool isMovePressed = false;
        void OnMove(InputValue inputValue)
        {
            inputMoveDir = inputValue.Get<Vector2>();

            isMovePressed = inputMoveDir != default ? true : false;
        }


        [Header("Dash Key")]
        [SerializeField] private KeyGroup _dashKeyGroup;
        public bool WaitDashDown
        {
            get { return _dashKeyGroup.WaitKeyDown; }
            set { _dashKeyGroup.WaitKeyDown = value; }
        }
        public bool WaitDashUp
        {
            get { return _dashKeyGroup.WaitKeyUp; }
            set { _dashKeyGroup.WaitKeyUp = value; }
        }
        void OnDash(InputValue inputValue)
        {
            var value = inputValue.Get<float>();

            _dashKeyGroup.WaitKeyPressed = value == 1; //Pressed
            if (value == 1) _dashKeyGroup.OnStartWaitKeyDown(); //다운
            if (value == 0) _dashKeyGroup.OnStartWaitKeyUp(); //업
        }


        [Header("Attack Key")]
        [SerializeField] private KeyGroup _attackKeyGroup;
        public bool WaitAttackDown
        {
            get { return _attackKeyGroup.WaitKeyDown; }
            set { _attackKeyGroup.WaitKeyDown = value; }
        }
        public bool WaitAttackUp
        {
            get { return _attackKeyGroup.WaitKeyUp; }
            set { _attackKeyGroup.WaitKeyUp = value; }
        }
        void OnAttack(InputValue inputValue)
        {
            var value = inputValue.Get<float>();

            _attackKeyGroup.WaitKeyPressed = value == 1; //Pressed
            if (value == 1) _attackKeyGroup.OnStartWaitKeyDown(); //다운
            if (value == 0) _attackKeyGroup.OnStartWaitKeyUp(); //업
        }


        [Header("Temp Key")]
        [SerializeField] private KeyGroup _tempKeyGroup;
        public bool WaitTempDown
        {
            get => _tempKeyGroup.WaitKeyDown;
            set => _tempKeyGroup.WaitKeyDown = value;
        }
        public bool WaitTempUp
        {
            get => _tempKeyGroup.WaitKeyUp;
            set => _tempKeyGroup.WaitKeyUp = value;
        }
        void OnTemp(InputValue inputValue)
        {
            var value = inputValue.Get<float>();

            _tempKeyGroup.WaitKeyPressed = value == 1; //Pressed
            if (value == 1) _tempKeyGroup.OnStartWaitKeyDown(); //다운
            if (value == 0) _tempKeyGroup.OnStartWaitKeyUp(); //업
        }

        #endregion
    }
#else
    public class NewInputManager : MonoBehaviour
    {
        [Header("New Input System is Not Used")]
        [SerializeField, ReadOnly] private bool _;

        public void Init()
        {
            Debug.LogError("잘못된 접근입니다.");
        }

        public void OnUpdate()
        {
            Debug.LogError("잘못된 접근입니다.");
        }

        public void OnTest()
        {
            Debug.LogError("잘못된 접근입니다.");
        }
    }
#endif
}
