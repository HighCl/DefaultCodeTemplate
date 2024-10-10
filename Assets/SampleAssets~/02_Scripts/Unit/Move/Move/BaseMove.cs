using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseMove : MonoBehaviour
    {
        [SerializeField, ReadOnly] protected bool isInit = false;

        protected BaseUnitController _controller;

        public virtual BaseUnitController Controller
        {
            get
            {
                if (_controller == null)
                {
                    _controller = GetComponentInParent<BaseUnitController>();
                }
                return _controller;
            }
            set
            {
                _controller = value;
            }
        }

        protected Collider2D coll = null;
        protected Rigidbody2D rig = null;
        protected WaitForFixedUpdate cashingWaitForFixedUpdate;

        //public bool isWalking { get { return moveDir != 0; } }
        public Vector2 moveDir;
        public float movePower = 10f;

        private void OnEnable()
        {

        }

        public virtual void MyAwake(BaseUnitController baseUnitController)
        {
            //Debug.Log("Not Override Not Move");
        }

        public virtual void MyUpdate() { }

        public virtual void MyFixedUpdate() { }

        public virtual void OnMove()
        {
            Debug.Log("Base Move - OnMove");
        }
    }
}