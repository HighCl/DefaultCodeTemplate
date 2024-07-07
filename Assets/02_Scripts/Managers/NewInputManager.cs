using DefaultSetting;
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
            Managers.Input.OnTest();
        }

        #region Input Setting, Rebind
        //바인드된 키가 있으면 수정해주는 함수
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
        #region Binding Keys
        //--------------------------------------------------------------------------------------------------------------------------------------
        //아래로는 각 키의 바인드 내용
        //--------------------------------------------------------------------------------------------------------------------------------------

        //입력 Down 확인
        [Header("임시 키"), SerializeField, ReadOnly]
        private bool _tempKey;
        public bool TempKeyDown
        {
            get { return _tempKey; }
            set
            {
                if (value == false)
                {
                    StopCoroutine(coWaitTempKeyDown);
                }
                _tempKey = value;
            }
        }
        public float waitTempKeyDownTime = 0.2f;
        Coroutine coWaitTempKeyDown;
        void OnTemp()
        {
            if (coWaitTempKeyDown != null)
                StopCoroutine(coWaitTempKeyDown);

            coWaitTempKeyDown = StartCoroutine(CoWaitPauseDown());

            IEnumerator CoWaitPauseDown()
            {
                _tempKey = true;
                yield return new WaitForSeconds(waitTempKeyDownTime);
                _tempKey = false;
                coWaitTempKeyDown = null;
            }
        }

        //입력 Vector2 확인 
        [field: Header("좌우 입력"), SerializeField, ReadOnly]
        public Vector2 inputHorizontal;
        [ReadOnly]
        public bool isHorizontalPressed = false;
        void OnMoveHorizontal(InputValue inputValue)
        {
            inputHorizontal = inputValue.Get<Vector2>();
            if (inputHorizontal.x == 0)
                isHorizontalPressed = false;
            else
                isHorizontalPressed = true;
        }

        [field: Header("마우스 좌표"), SerializeField, ReadOnly]
        public Vector2 MousePos { get; private set; }
        void OnMousePosition(InputValue inputValue)
        {
            MousePos = inputValue.Get<Vector2>();
        }

        //입력 Down, Up, Pressed 확인
        [Header("로프키"), SerializeField, ReadOnly]
        private bool _waitRopeDown;
        public bool WaitRopeDown
        {
            get { return _waitRopeDown; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitRopeDown);

                _waitRopeDown = value;
            }
        }
        [SerializeField]
        private float waitRopeDownTime = 0.2f;
        Coroutine coWaitRopeDown;

        [field: SerializeField, ReadOnly]
        public bool WaitRopePressed { get; private set; }

        [SerializeField, ReadOnly]
        private bool _waitRopeUp;
        public bool WaitRopeUp
        {
            get { return _waitRopeUp; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitRopeUp);

                _waitRopeUp = value;
            }
        }
        [SerializeField]
        private float waitRopeUpTime = 0.2f;
        Coroutine coWaitRopeUp;
        void OnRope(InputValue inputValue)
        {
            var value = inputValue.Get<float>();

            //다운
            if (value == 1)
            {
                if (coWaitRopeDown != null)
                    StopCoroutine(coWaitRopeDown);

                coWaitRopeDown = StartCoroutine(CoWaitRopeDown());
            }

            //업
            if (value == 0)
            {
                if (coWaitRopeUp != null)
                    StopCoroutine(coWaitRopeUp);

                coWaitRopeUp = StartCoroutine(CoWaitRopeUp());
            }
            //Pressed
            if (inputValue.Get<float>() == 1)
                WaitRopePressed = true;
            else
                WaitRopePressed = false;

            IEnumerator CoWaitRopeDown()
            {
                _waitRopeDown = true;
                yield return new WaitForSeconds(waitRopeDownTime);
                _waitRopeDown = false;
                coWaitRopeDown = null;
            }

            IEnumerator CoWaitRopeUp()
            {
                _waitRopeUp = true;
                yield return new WaitForSeconds(waitRopeUpTime);
                _waitRopeUp = false;
                coWaitRopeUp = null;
            }
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
