using UnityEngine;

namespace DefaultSetting
{
    public class PlayerStat : BaseStat
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

        [field: SerializeField] public bool HasMove { get; private set; } = true;
        [field: SerializeField] public bool HasJump { get; private set; } = true;
        [field: SerializeField] public bool HasDoubleJump { get; private set; } = true;
        [field: SerializeField] public bool HasWallJump { get; private set; } = true;
        [field: SerializeField] public bool HasDash { get; private set; } = true;
        [field: SerializeField] public bool HasAttack { get; private set; } = true;
        [field: SerializeField] public bool HasGrappling { get; private set; } = true;

        //플레이어는 따로 매니저가 세팅해주므로 행동X
        protected override void SetUnitState() { SetPlayerStat(); }

        //매니저가 이 친구를 실행해서 기본 스텟들을 세팅해준다.라는 개념으로 접근하자.
        public void SetPlayerStat()
        {
            //MstDefaultStageDataEntity defaultData = Managers.Data.stageDefaultData;

            //maxhp = defaultData.maxHP;
            //Hp = maxhp;
            //damage = defaultData.damage;
            //weight = defaultData.weight;

            //HasMove = defaultData.hasMove;
            //HasJump = defaultData.hasJump;
            //HasDoubleJump = defaultData.hasDoubleJump;
            //HasWallJump = defaultData.hasWallJump;
            //HasDash = defaultData.hasDash;
            //HasAttack = defaultData.hasAttack;
            //HasGrappling = defaultData.hasGrappling;
        }

        protected override void OnUnitDie()
        {
            Save_PlayerController controller = GetComponent<Save_PlayerController>();
            if (controller.isAlive)
                controller.OnUnitDie();
        }
    }
}