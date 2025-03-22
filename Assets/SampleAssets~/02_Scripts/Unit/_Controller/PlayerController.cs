using UnityEngine;

namespace DefaultSetting
{
    public class PlayerController : BaseUnitController
    {
        [SerializeField]
        private PlayerMoveController _playerMoveCtrl;

        public override BaseMoveController MoveController
        {
            get
            {
                if (_playerMoveCtrl == null)
                {
                    _playerMoveCtrl = GetComponent<PlayerMoveController>();
                }
                return _playerMoveCtrl;
            }
            set
            {
                // 여기에 추가적인 로직을 넣을 수 있음
                _playerMoveCtrl = (PlayerMoveController)value;
            }
        }
        [SerializeField]
        private Temp_PlayerAttack _playerAttack;
        public override BaseAttack Attack
        {
            get
            {
                if (_playerAttack == null)
                {
                    _playerAttack = GetComponent<Temp_PlayerAttack>();
                }
                return _playerAttack;
            }
            set
            {
                // 여기에 추가적인 로직을 넣을 수 있음
                _playerAttack = (Temp_PlayerAttack)value;
            }
        }
        [SerializeField]
        private Temp_PlayerAnimation _playerAnim;
        public override BaseAnimation Animation
        {
            get
            {
                if (_playerAnim == null)
                {
                    _playerAnim = GetComponent<Temp_PlayerAnimation>();
                }
                return _playerAnim;
            }
            set
            {
                _playerAnim = (Temp_PlayerAnimation)value;
            }
        }

        [SerializeField]
        private PlayerStat _playerStat;
        public override BaseStat Stat
        {
            get
            {
                if (_playerStat == null)
                {
                    _playerStat = GetComponent<PlayerStat>();
                }
                return _playerStat;
            }
            set
            {
                _playerStat = (PlayerStat)value;
            }
        }
        [SerializeField]
        private Temp_PlayerState _playerState;
        public override BaseState State
        {
            get
            {
                if (_playerState == null)
                {
                    _playerState = GetComponent<Temp_PlayerState>();
                }
                return _playerState;
            }
            set
            {
                _playerState = (Temp_PlayerState)value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (_playerMoveCtrl != null)
                _playerMoveCtrl?.MyAwake(this);
            if (_playerAttack != null)
                _playerAttack?.MyAwake(this);
            if (_playerAnim != null)
                _playerAnim?.MyAwake(this);
            if (_playerStat != null)
                _playerStat?.MyAwake(this);
            if (_playerState != null)
                _playerState?.MyAwake(this);
        }

        protected override void Update()
        {
            if (_playerMoveCtrl != null)
                _playerMoveCtrl?.MyUpdate();
            if (_playerAttack != null)
                _playerAttack?.MyUpdate();
            if (_playerAnim != null)
                _playerAnim?.MyUpdate();
            if (_playerStat != null)
                _playerStat?.MyUpdate();
            if (_playerState != null)
                _playerState?.MyUpdate();
        }

        protected override void FixedUpdate()
        {
            if (_playerMoveCtrl != null)
                _playerMoveCtrl?.MyFixedUpdate();
            if (_playerAttack != null)
                _playerAttack?.MyFixedUpdate();
            if (_playerAnim != null)
                _playerAnim?.MyFixedUpdate();
            if (_playerStat != null)
                _playerStat?.MyFixedUpdate();
            if (_playerState != null)
                _playerState?.MyFixedUpdate();
        }
    }
}