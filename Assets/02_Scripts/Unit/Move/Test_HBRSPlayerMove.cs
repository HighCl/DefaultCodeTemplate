using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;

#pragma warning disable 0414
namespace DefaultSetting
{
    public class Test_HBRSPlayerMove : BasePlayerMove
    {
        //Collider2D coll;
        //Rigidbody2D rig;

        //[field: SerializeField]
        //public Vector3 moveDir { get; protected set; } = new Vector3(1, 0, 0);
        [SerializeField]
        private Vector3 _lookDir;
        public Vector3 LookDir
        {
            get
            {
                return _lookDir;

            }
            set
            {
                _lookDir = value;
            }
        }

        [Header("제약 조건 변수")]
        [SerializeField] private float xWalkVelocityRange = 30;
        [SerializeField] private float xStopVelocityRange = 45;
        [SerializeField] private float _fallClamp = -70;
        [SerializeField] private LayerMask floorLayerMask;
        [SerializeField] private LayerMask wallLayerMask;
        [SerializeField] private Define.Tag grabTag = Define.Tag.CanGrabWall;
        private string cachedCanGrabWallStr = null;

        [Header("이동 변수")]
        public float moveForce = 600;
        private float moveSoundCurrentTime = 0;

        [Header("점프 변수")]
        public float jumpTime = 0.4f;
        [ReadOnly] public bool isWallKickFirstJump = false;
        public float firstJumpTime_0_1 = 0.3f;
        public float remainJumpTime_0_1 = 0.5f;

        [ReadOnly] public float currentJumpCount = 1f;
        float NormalJumpCount = 1;
        float DoubleJumpCount = 2;

        public bool isJumping { get; protected set; } = false;
        [ReadOnly] public bool cancelJumping = false;
        public float groundJumpPower = 35;
        public Vector2 wallKickPower = new Vector2(300, 35);
        public float wallSlowdownPower = 2.5f;

        public float defaultJumpCountCoyotaTime = 0.2f;
        [ReadOnly] public float currentJumpCountCoyotaTime = 0f;

        public float defaultWallJumpCoyotaTime = 0.2f;
        [ReadOnly] public float currentWallJumpCoyotaTime = 0f;
        enum LastWallDir
        {
            NotSetting,
            LeftWall,
            RightWall,
        }
        LastWallDir lastWallDir = LastWallDir.NotSetting;

        public GameObject jumpDust;
        public float destroyJumpDustTime = 2f;

        Coroutine coJump = null;

        [field: SerializeField, Header("대쉬 변수")]
        public bool isDashing { get; protected set; } = false;
        [ReadOnly] public bool cancelDashing = false;
        //TODO: 그냥 함수 호출시키는 것이 훨씬 더 간단할 것이다.
        public bool immediateReadyDash;

        Coroutine coDash;
        [SerializeField] float dashingTime = 0.3f;
        [SerializeField] float dashingCooldown = 1f;
        [SerializeField] float dashPower = 20f;

        [field: Header("상태 변수"), SerializeField, ReadOnly]
        public bool isGround { get; protected set; } = false;
        [SerializeField, ReadOnly]
        private bool _isUpWall = false;
        public bool IsUpWall
        {
            get
            {
                return _isUpWall;
            }
            protected set
            {
                if (value)
                {
                    cancelJumping = true;
                }

                _isUpWall = value;
            }
        }
        [field: SerializeField, ReadOnly]
        public bool isGrabLeftWall { get; protected set; } = false;
        [field: SerializeField, ReadOnly]
        public bool isGrabRightWall { get; protected set; } = false;

        public bool IsGrabWall { get { return !isGround && (isGrabLeftWall || isGrabRightWall); } }
        public bool isSkying { get { return !isGround && !IsGrabWall; } } //하늘에 떠있는 상태



        //입력 키 체크
        [Header("입력 키 체크")]
        private Vector2 _inputHorizontal;
        public Vector2 InputHorizontal
        {
            get
            {
                float h = Input.GetAxis("Horizontal");
                _inputHorizontal = new Vector2(h, 0);
                return _inputHorizontal;
            }
        }

        public bool IsHorizontalPressed
        {
            get
            {
                if (InputHorizontal != Vector2.zero)
                    return true;
                else
                    return false;
            }
        }

        //눌러지면 바로 꺼지도록 해야 함.
        //눌러진 것을 동기화하는 함수가 필요하고
        //동기화된 

        [SerializeField]
        private bool _waitJumpDown = false;
        public bool WaitJumpDown
        {
            get
            {
                return false;
                //return Managers.Input.WaitJumpDown;
            }
            set
            {
                //Managers.Input.WaitJumpDown = value;
            }
        }

        public bool WaitJumpPressed
        {
            get
            {
                return Input.GetKey(KeyCode.Space);
            }
        }


        public override void MyAwake(BaseUnitController baseUnitController)
        {
            playerController = GetComponent<PlayerController>();
            coll = GetComponent<Collider2D>();
            rig = GetComponent<Rigidbody2D>();

            CashingFunction();
            SetStatData();
        }

        public override void MyFixedUpdate()
        {
            Debug.Log("a");

            if (playerController.State.IsInabilityActTime)
                return;

            //상태 확인
            CheckState();

            //행동(키 입력 관련 처리들)
            Move2D();
            CheckJump();
            CheckDash();

            //행동 이후 처리(키 입력과 관계없는 게임 로직 처리)
            CheckWall();

            //사후 조건
            //로프 velocity 값 범위 제약
            ClampVelocity();
            SubVariableDeltaTime();

        }

        public void OnUnitDie()
        {
        }

        private void CashingFunction()
        {
            playerController = GetComponent<PlayerController>();
            rig = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
            cachedCanGrabWallStr = grabTag.ToString();
        }

        public void SetStatData()
        {
            //MstDefaultStageDataEntity defaultData = Managers.Data.stageDefaultData;
            ////가져온 데이터를 설정

            //moveForce = defaultData.moveForce;
            //groundJumpPower = defaultData.groundJumpPower;
            //wallKickPower.x = defaultData.wallKickXJumpPower;
            //wallKickPower.y = defaultData.wallKickYJumpPower;
            //wallSlowdownPower = defaultData.wallSlowdownPower;
            //dashingTime = defaultData.dashingTime;
            //dashPower = defaultData.dashPower;

            //xWalkVelocityRange = defaultData.xWalkVelocityRange;
            //xStopVelocityRange = defaultData.xStopVelocityRange;
            //_fallClamp = defaultData.fallClamp;
        }

        void CheckState()
        {
            //TODO : 지형 체크를 별개의 클래스로 만들어 관리 고려, GrabWall, Grappling, WallPower 등등
            //TODO : 상태 처리를 함수로 변경, DrawRay 사용 여부를 옵션으로 표시

            RaycastHit2D hit;

            //땅 체크
            CheckGroundState();


            CheckWallState();


            if (isGround || IsGrabWall)
            {
                immediateReadyDash = true;
            }

            SetJumpCount();

            void CheckGroundState()
            {
                //Debug.DrawRay(transform.position + new Vector3(coll.bounds.extents.x - 0.1f, 0, 0), Vector3.down * (coll.bounds.extents.y + 0.1f), Color.white, 0.1f);
                bool groundFrontCheck = (bool)Physics2D.Raycast(transform.position + new Vector3(-coll.bounds.extents.x + 0.1f, 0, 0), Vector2.down, coll.bounds.extents.y + 0.1f, floorLayerMask);
                bool groundBackwardCheck = (bool)Physics2D.Raycast(transform.position + new Vector3(coll.bounds.extents.x - 0.1f, 0, 0), Vector2.down, coll.bounds.extents.y + 0.1f, floorLayerMask);
                isGround = groundFrontCheck || groundBackwardCheck;
            }
            void CheckWallState()
            {
                //위쪽 벽 체크
                IsUpWall = (bool)Physics2D.Raycast(transform.position, Vector2.up, coll.bounds.extents.y + 1f, wallLayerMask);

                if (isGround)
                {
                    isGrabRightWall = false;
                    isGrabLeftWall = false;
                    return;
                }

                int rayCount = 5;
                float f = 0.05f; //보정치
                float startPoint = -coll.bounds.extents.y - f;
                float stepLength = ((coll.bounds.extents.y + f) * 2) / (rayCount - 1);

                //왼쪽 벽 체크
                if (InputHorizontal.x < 0)
                {
                    for (int i = 0; i < rayCount; i++)
                    {
                        //Debug.DrawRay(transform.position + new Vector3(0, startPoint + stepLength * i, 0), Vector3.left * (coll.bounds.extents.x + 0.1f), Color.white, 0.1f);
                        hit = Physics2D.Raycast(transform.position + new Vector3(0, startPoint + stepLength * i, 0), Vector2.left, coll.bounds.extents.x + 0.1f, wallLayerMask);
                        if (hit.transform?.tag == cachedCanGrabWallStr)
                        {
                            currentWallJumpCoyotaTime = defaultWallJumpCoyotaTime;
                            lastWallDir = LastWallDir.LeftWall;
                            isGrabLeftWall = true;
                            break;
                        }

                        if (i == rayCount - 1)
                        {
                            isGrabLeftWall = false;
                        }
                    }
                }
                else
                {
                    isGrabLeftWall = false;
                }

                //오른쪽 벽 체크
                if (InputHorizontal.x > 0)
                {
                    for (int i = 0; i < rayCount; i++)
                    {
                        //Debug.DrawRay(transform.position + new Vector3(0, startPoint + stepLength * i, 0), Vector3.right * (coll.bounds.extents.x + 0.1f), Color.white, 0.1f);
                        hit = Physics2D.Raycast(transform.position + new Vector3(0, startPoint + stepLength * i, 0), Vector2.right, coll.bounds.extents.x + 0.1f, wallLayerMask);
                        if (hit.transform?.tag == cachedCanGrabWallStr)
                        {
                            currentWallJumpCoyotaTime = defaultWallJumpCoyotaTime;
                            lastWallDir = LastWallDir.RightWall;
                            isGrabRightWall = true;
                            break;
                        }

                        if (i == rayCount - 1)
                        {
                            isGrabRightWall = false;
                        }
                    }
                }
                else
                {
                    isGrabRightWall = false;
                }
            }
            void SetJumpCount()
            {
                if (isGround)
                {
                    currentJumpCountCoyotaTime = defaultJumpCountCoyotaTime;
                    currentJumpCount = DoubleJumpCount;
                }
                else if (IsGrabWall)
                {
                    currentJumpCountCoyotaTime = defaultJumpCountCoyotaTime;
                    currentJumpCount = DoubleJumpCount;
                }
                else if (isSkying)
                {
                    if (currentJumpCount == 2 && 0 < currentJumpCountCoyotaTime)
                        currentJumpCount = 2;

                    currentJumpCount = Mathf.Clamp(currentJumpCount, 0, 1);
                }
            }
        }


        public void Move2D()
        {
            if (isWallKickFirstJump)
                return;

            if (isDashing)
                return;

            if (!IsHorizontalPressed)
                return;

            //if (isGround && moveSoundCurrentTime < 0)
            //{
            //    moveSoundCurrentTime = Managers.Data.MstMaster.PlayerSound.MoveClip.length;
            //    Managers.Sound.Play(Managers.Data.MstMaster.PlayerSound.MoveClip);
            //}

            float x = InputHorizontal.x;
            if (!isGround && isGrabLeftWall)
            {
                x = Mathf.Clamp(x, 0, 1);
            }

            if (!isGround && isGrabRightWall)
            {
                x = Mathf.Clamp(x, -1, 0);
            }

            moveDir = new Vector3(x, 0, 0).normalized;
            LookDir = new Vector3(x, 0, 0).normalized;

            //반대 방향 이동 시 감속해야 함.
            if (x != 0 && Mathf.Sign(x) != Mathf.Sign(rig.velocity.x))
                rig.velocity = new Vector2(rig.velocity.x * 0.85f, rig.velocity.y);

            //addforce 이동
            if (Mathf.Abs(rig.velocity.x) < xWalkVelocityRange)
            {
                rig.AddForce(moveDir * moveForce);
            }
        }

        void CheckWall()
        {
            if (!IsGrabWall)
                return;

            if (isDashing)
                return;

            LookDir = isGrabLeftWall == true ? Vector3.left : Vector3.right;

            if (rig.velocity.y < 0)
            {
                //rig.velocity = new Vector3(rig.velocity.x, Mathf.Clamp(rig.velocity.y, -wallSlowdownPower, rig.velocity.y));
                rig.velocity = new Vector3(rig.velocity.x, -wallSlowdownPower);
            }
        }

        void CheckJump()
        {
            if (!(WaitJumpDown && currentJumpCount > 0))
                return;

            //if (!controller.Stat.HasJump)
            //    return;

            if (isDashing)
                return;

            WaitJumpDown = false;
            currentJumpCount--;

            if (coJump != null)
                StopCoroutine(coJump);

            coJump = StartCoroutine(CoJump());

            //TODO: 벽 점프 제약, 2단 점프 제약은 나중에 하기
            //TODO : 점프 횟수를 int형이 아닌 2단점프 bool타입 변수로 설정하기
            IEnumerator CoJump()
            {
                print("A");
                //사전 조건
                isJumping = true;
                cancelJumping = false;
                //playerController.Animation.anim_TriggerJump = true;
                isWallKickFirstJump = false;
                float startSlow = -1f;
                float jumpPowerY = IsGrabWall ? wallKickPower.y : groundJumpPower;

                //jumpDust = Managers.Resource.Instantiate(Define.JUMP_DUST_PATH, transform.position + Vector3.down * coll.bounds.extents.y, default);
                //if (LookDir == Vector3.right)
                //    jumpDust.transform.position += Vector3.right * transform.localScale.x * 0.6f;
                //Managers.Resource.Destroy(jumpDust, destroyJumpDustTime);

                //Managers.Sound.Play(Managers.Data.MstMaster.PlayerSound.JumpClip);

                //로직
                float currentTime = 0;
                Vector3 firstJumpDir = GetFirstJumpDir(jumpPowerY);

                print(WaitJumpPressed);
                float interpolationRatio = 0f;
                while (currentTime < jumpTime && WaitJumpPressed)
                {
                    print(WaitJumpPressed);
                    if (cancelJumping)
                        break;

                    interpolationRatio = currentTime / jumpTime;

                    //초기 점프(벽점일경우 이동 불가 상태로 만들어야 함.)
                    if (interpolationRatio < firstJumpTime_0_1)
                    {
                        if (isWallKickFirstJump)
                        {
                            if (firstJumpDir.x < 0)
                                moveDir = LookDir = Vector3.left;
                            else
                                moveDir = LookDir = Vector3.right;

                            rig.velocity = firstJumpDir;
                        }
                        else
                        {
                            rig.velocity = new Vector2(rig.velocity.x, jumpPowerY);
                        }
                    }
                    else if (interpolationRatio < remainJumpTime_0_1)
                    {
                        isWallKickFirstJump = false;
                        rig.velocity = new Vector2(rig.velocity.x, jumpPowerY);
                    }
                    else
                    {
                        isWallKickFirstJump = false;

                        if (startSlow == -1f)
                            startSlow = currentTime;

                        float t = (currentTime - startSlow) / (jumpTime - startSlow);

                        float calcJumpPower = jumpPowerY - jumpPowerY * Extension.EaseOutCubic(t) * 0.9f;

                        rig.velocity = new Vector2(rig.velocity.x, calcJumpPower);
                    }

                    currentTime += Time.fixedDeltaTime;
                    yield return cashingWaitForFixedUpdate;
                }

                //사후 조건
                rig.velocity = new Vector2(rig.velocity.x, 0);
                isJumping = false;
                isWallKickFirstJump = false;
                coJump = null;
            }

            Vector3 GetFirstJumpDir(float jumpPowerY)
            {
                if (currentWallJumpCoyotaTime > 0) //벽점프 코요테타임이 남아있다면
                {
                    //currentWallJumpCoyotaTime = 0;
                    //밀어내는 점프중에는 이동 입력을 막는다.
                    isWallKickFirstJump = true;

                    switch (lastWallDir)
                    {
                        case LastWallDir.LeftWall:
                            return new Vector2(wallKickPower.x, jumpPowerY);
                        case LastWallDir.RightWall:
                            return new Vector2(-wallKickPower.x, jumpPowerY);
                        default:
                            Debug.LogError("마지막 벽 설정이 되어있지 않음\n");
                            return new Vector2(wallKickPower.x, jumpPowerY);
                    }
                }
                else
                {
                    isWallKickFirstJump = false;
                    return new Vector2(rig.velocity.x, jumpPowerY);
                }
            }
        }

        void CheckDash()
        {
            //if (!Managers.Input.WaitDashDown)
            //    return;

            //if (!controller.Stat.HasDash)
            //    return;

            if (coDash != null)
                return;

            //데쉬 실행
            //Managers.Input.WaitDashDown = false;
            coDash = StartCoroutine(CoDash());
            //IDE에서 추천해준 코드
            //coDash ??= StartCoroutine(CoDash());

            IEnumerator CoDash()
            {
                yield return 0;
                ////print("dash Start");

                ////사전조건
                //isDashing = true;
                //cancelDashing = false;
                //cancelJumping = true;
                //immediateReadyDash = false;
                //controller.Anim.anim_TriggerDash = true;
                //controller.Anim.immediateMakeShadow = true;

                //float currentGravityScale = this.rig.gravityScale;

                //if (isGrabLeftWall)
                //    moveDir = LookDir = Vector3.right;
                //if (isGrabRightWall)
                //    moveDir = LookDir = Vector3.left;

                ////로직
                //Managers.Sound.Play(Managers.Data.MstMaster.PlayerSound.DashClip);
                //rig.gravityScale = 0f;
                //rig.velocity = new Vector2(transform.localScale.x * LookDir.x * dashPower, 0f);

                //float currentTime = 0;
                ////대쉬중일 때
                //while (currentTime / dashingTime < 1)
                //{
                //    if (cancelDashing)
                //        break;

                //    if (rig.velocity.y != 0)
                //        rig.velocity = new Vector2(transform.localScale.x * moveDir.x * dashPower, 0f);

                //    currentTime += Time.fixedDeltaTime;
                //    yield return cashingWaitForFixedUpdate;
                //}
                //rig.gravityScale = currentGravityScale;
                //rig.velocity = Vector2.zero;
                //isDashing = false;
                ////print("dash End");

                ////대쉬 쿨타임 대기
                //currentTime = 0f;
                //while (currentTime / dashingCooldown < 1)
                //{
                //    if (immediateReadyDash || IsGrappling)
                //        break;

                //    currentTime += Time.fixedDeltaTime;
                //    yield return cashingWaitForFixedUpdate;
                //}
                ////사후조건
                ////print("dash cooldown End");
                //coDash = null;
            }
        }

        public void ClampVelocity()
        {
            //이동 멈춤값 제어
            ControlStopMoveVelocity();

            //낙하 속도 최대값 제어
            ControlFallingVelocity();

            void ControlStopMoveVelocity()
            {
                //if (controller.State.IsGrappleShot)
                //    return;

                //if (IsGrappling)
                //    return;

                if (isWallKickFirstJump)
                    return;

                if (isDashing)
                    return;

                if (IsHorizontalPressed)
                    return;

                if (Mathf.Abs(rig.velocity.x) > xStopVelocityRange)
                    return;

                rig.velocity = Vector2.Lerp(rig.velocity, new Vector2(0, rig.velocity.y), 0.3f);
            }

#pragma warning disable CS8321 // 로컬 함수가 선언되었지만 사용되지 않음
            void ControlFallingVelocity()
            {
                //if (IsGrappling)
                //    return;

                //if (rig.velocity.y < _fallClamp)
                //    rig.velocity = new Vector2(rig.velocity.x, _fallClamp);
            }
#pragma warning restore CS8321 // 로컬 함수가 선언되었지만 사용되지 않음
        }

        private void SubVariableDeltaTime()
        {
            moveSoundCurrentTime -= Time.fixedDeltaTime;
            currentJumpCountCoyotaTime -= Time.fixedDeltaTime;
            currentWallJumpCoyotaTime -= Time.fixedDeltaTime;
        }
    }
}
#pragma warning restore 0414