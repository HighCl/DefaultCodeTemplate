using System.Collections;
using UnityEngine;
//using UnityEngine.InputSystem;

namespace DefaultSetting
{
    public class InputManager : MonoBehaviour
    {
#if ENABLE_INPUT_SYSTEM

        #region Input Setting, Rebind
        private PlayerInput input;
        public void Init()
        {
            print("InputManager:\n뉴 입력 시스템이 적용중입니다.");

            input = GetComponent<PlayerInput>();
            input.actions = Managers.Resource.Load<InputActionAsset>("PlayerInput");
            input.actions.FindActionMap("Player").Enable();
        }

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
        void OnTest()
        {
            //플레이가 테스트인 경우에만 작동
            if (Managers.Game.playState == Define.PlayState.Test)
            {
                Managers.Test.OnTest();
            }
        }
#elif ENABLE_LEGACY_INPUT_MANAGER

        [SerializeField] private KeyCode JumpKeyCode = KeyCode.Space;

        //입력 Down, Up, Pressed 확인
        [Header("점프키"), SerializeField, ReadOnly]
        private bool _waitJumpDown;
        public bool WaitJumpDown
        {
            get { return _waitJumpDown; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitJumpDown);

                _waitJumpDown = value;
            }
        }
        [SerializeField]
        private float waitJumpDownTime = 0.2f;
        Coroutine coWaitJumpDown;

        [field: SerializeField, ReadOnly]
        public bool WaitJumpPressed { get; private set; }

        [SerializeField, ReadOnly]
        private bool _waitJumpUp;
        public bool WaitJumpUp
        {
            get { return _waitJumpUp; }
            set
            {
                if (value == false)
                    StopCoroutine(coWaitJumpUp);

                _waitJumpUp = value;
            }
        }
        [SerializeField]
        private float waitJumpUpTime = 0.2f;
        Coroutine coWaitJumpUp;
        void UpdateJumpKey()
        {
            //다운
            if (Input.GetKeyDown(JumpKeyCode))
            {
                if (coWaitJumpDown != null)
                    StopCoroutine(coWaitJumpDown);

                coWaitJumpDown = StartCoroutine(CoWaitRopeDown());
            }

            //업
            if (Input.GetKeyUp(JumpKeyCode))
            {
                if (coWaitJumpUp != null)
                    StopCoroutine(coWaitJumpUp);

                coWaitJumpUp = StartCoroutine(CoWaitJumpUp());
            }

            //Pressed
            if (Input.GetKey(JumpKeyCode))
                WaitJumpPressed = true;
            else
                WaitJumpPressed = false;

            IEnumerator CoWaitRopeDown()
            {
                _waitJumpDown = true;
                yield return new WaitForSeconds(waitJumpDownTime);
                _waitJumpDown = false;
                coWaitJumpDown = null;
            }

            IEnumerator CoWaitJumpUp()
            {
                _waitJumpUp = true;
                yield return new WaitForSeconds(waitJumpUpTime);
                _waitJumpUp = false;
                coWaitJumpUp = null;
            }
        }

        KeyCode _testKeyCode = KeyCode.T;

        public void Init()
        {
            print("InputManager\n레거시 입력 시스템이 적용중입니다.");
        }

        //필요 시 Managers에서 실행하는 것으로 변경하기
        public void OnUpdate()
        {
            UpdateJumpKey();

            if (Input.GetKeyDown(_testKeyCode))
            {
                OnTest();
            }
        }

        void OnTest()
        {
            //플레이가 테스트인 경우에만 작동
            if (Managers.Data.playState == Define.PlayState.Test)
            {
                Managers.Test.OnTest();
            }
        }
#endif
    }
}
