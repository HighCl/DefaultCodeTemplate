using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseStat : MonoBehaviour
    {
        protected BaseUnitController _controller;

        public virtual BaseUnitController Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }

        [SerializeField]
        [ReadOnly]
        private float _hp;
        [SerializeField]
        public float Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = Mathf.Clamp(value, 0, maxhp);
                if (_hp == 0)
                {
                    OnUnitDie();
                }
            }
        }
        [SerializeField]
        public float maxhp;
        [SerializeField]
        public float damage;

        private void OnEnable()
        {
            //BaseUnitController controller = GetComponent<BaseUnitController>();
            //controller.Move = this;
        }

        public virtual void MyAwake(BaseUnitController baseUnitController)
        {
            SetUnitState();
        }
        public virtual void MyUpdate() { }
        public virtual void MyFixedUpdate() { }

        protected virtual void SetUnitState()
        {
            _hp = maxhp;
        }

        protected virtual void OnUnitDie()
        {
        }

    }
}