using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseAttack : MonoBehaviour
    {
        protected BaseUnitController _controller;

        public virtual BaseUnitController Controller
        {
            get { return _controller; }
            set { _controller = value; }
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
            //OnMove();
        }

        //public virtual void OnMove()
        //{
        //    Debug.Log("Base Move - OnMove");
        //}
    }
}