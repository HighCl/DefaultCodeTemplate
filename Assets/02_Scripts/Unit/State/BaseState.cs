using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseState : MonoBehaviour
    {
        protected BaseUnitController _controller;

        public virtual BaseUnitController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        //애니메이션은 이곳을 통해 여러 상태를 가져다가 사용할 수 있게 하자

        //TODO : 실시간으로 연동 안됨. Update 등을 통해 실시간으로 갱신하고 가져가게 하면 될 것 같다.
        [ReadOnly][SerializeField] private bool _isDashing;
        public bool IsDashing
        {
            get
            {
                //_isDashing = controller.Move.isDashing;
                return _isDashing;
            }
        }

        //TODO: 시간이 조금씩 엇나감. 현재 시간에 더해주는 방식으로 변경해야 할 듯
        //무적 상태 확인
        [Header("무적상태 확인")]
        [ReadOnly]
        public float normalInvincibilityTime = 1f;
        [ReadOnly]
        public float currentinvincibilityTime;
        public bool IsInvincibility
        {
            get
            {
                return currentinvincibilityTime > 0;
            }
        }

        //TODO: 시간이 조금씩 엇나감. 현재 시간에 더해주는 방식으로 변경해야 할 듯
        //행동 불능 상태 확인
        [Header("행동불능 상태 확인")]
        [ReadOnly]
        public float normalInabilityActTime = 0.3f;
        [ReadOnly]
        public float currentInabilityActTime;
        public bool IsInabilityActTime
        {
            get
            {
                return currentInabilityActTime > 0;
            }
        }

        private void OnEnable()
        {
            //BaseUnitController controller = GetComponent<BaseUnitController>();
            //controller.Move = this;
        }

        public virtual void MyAwake(BaseUnitController baseUnitController)
        {
            //Debug.Log("Not Override");
        }

        public virtual void MyUpdate() { }

        public virtual void MyFixedUpdate()
        {
            currentinvincibilityTime -= Time.fixedDeltaTime;
            currentInabilityActTime -= Time.fixedDeltaTime;
        }

        //피격 시 무적 상태로 변경하는 함수
        public void HitInvincibility()
        {
            currentinvincibilityTime = normalInvincibilityTime;
            currentInabilityActTime = normalInabilityActTime;
        }
    }
}