using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseJump : MonoBehaviour
    {
        [SerializeField] protected KeyCode jumpKeyCode = KeyCode.Space;

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

        public float JumpPower = 10f;

        public virtual void MyAwake(BaseUnitController baseUnitController)
        {
            Debug.Log("Not Override Awake");
        }

        public virtual void MyUpdate()
        {
            Debug.Log("Not Override FixedUpdate");
        }

        public virtual void MyFixedUpdate()
        {
            Debug.Log("Not Override FixedUpdate");
        }

        public virtual void OnJump()
        {
            Debug.Log("Base Move - OnMove");
        }
    }
}