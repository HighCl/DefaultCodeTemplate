using UnityEngine;

namespace DefaultSetting
{
    public abstract class BasePlayerMove : BaseMove
    {
        [SerializeField, ReadOnly]
        protected PlayerController playerController;

        public override BaseUnitController Controller
        {
            get
            {
                if (playerController == null)
                {
                    playerController = GetComponent<PlayerController>();
                }
                return playerController;
            }
            set
            {
                // 여기에 추가적인 로직을 넣을 수 있음
                playerController = (PlayerController)value;
            }
        }

        public override void OnMove()
        {
            Debug.Log("Temp_PlayerMove - OnMove");
        }
    }
}