using UnityEngine;

namespace DefaultSetting
{
    public class Temp_PlayerAttack : BaseAttack
    {
        [SerializeField]
        private PlayerController _playerController;

        public override BaseUnitController Controller
        {
            get
            {
                if (_playerController == null)
                {
                    _playerController = GetComponent<PlayerController>();
                }
                return _playerController;
            }
            set
            {
                // 여기에 추가적인 로직을 넣을 수 있음
                _playerController = (PlayerController)value;
            }
        }

        private void OnEnable()
        {
            //BaseUnitController controller = GetComponent<BaseUnitController>();
            //controller.Move = this;
        }

        public override void MyAwake(BaseUnitController baseUnitController)
        {
            //Debug.Log("Not Override");
        }

        public override void MyFixedUpdate()
        {
            //OnMove();
        }

        //public virtual void OnMove()
        //{
        //    Debug.Log("Base Move - OnMove");
        //}
    }
}