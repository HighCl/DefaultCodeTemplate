//[RequireComponent(typeof(PlayerMove))]
//[RequireComponent(typeof(PlayerAttack))]
//[RequireComponent(typeof(PlayerAnimation))]
//[RequireComponent(typeof(PlayerStat))]
//[RequireComponent(typeof(PlayerState))]
namespace DefaultSetting
{
    public class Save_PlayerController : BaseUnitController
    {
        //private Save_HBRSPlayerMove _move;
        //public Save_HBRSPlayerMove Move
        //{
        //    get
        //    {
        //        if (_move == null)
        //        {
        //            _move = GetComponent<Save_HBRSPlayerMove>();
        //        }
        //        return _move;
        //    }
        //}
        //private Save_PlayerAttack _attack;
        //public Save_PlayerAttack Attack
        //{
        //    get
        //    {
        //        if (_attack == null)
        //        {
        //            _attack = GetComponent<Save_PlayerAttack>();
        //        }
        //        return _attack;
        //    }
        //}
        //private Save_PlayerAnimation _anim;
        //public Save_PlayerAnimation Anim
        //{
        //    get
        //    {
        //        if (_anim == null)
        //        {
        //            _anim = GetComponent<Save_PlayerAnimation>();
        //        }
        //        return _anim;
        //    }
        //}

        //private PlayerStat _stat;
        //public PlayerStat Stat
        //{
        //    get
        //    {
        //        if (_stat == null)
        //        {
        //            _stat = GetComponent<PlayerStat>();
        //        }
        //        return _stat;
        //    }
        //}
        //private Save_PlayerState _state;
        //public Save_PlayerState State
        //{
        //    get
        //    {
        //        if (_state == null)
        //        {
        //            _state = GetComponent<Save_PlayerState>();
        //        }
        //        return _state;
        //    }
        //}

        //protected override void Awake()
        //{
        //    base.Awake();

        //    Stat.MyAwake();

        //    State.MyAwake();
        //    Move.MyAwake();
        //    Attack.MyAwake();
        //    Anim.MyAwake();
        //}

        //protected override void Update()
        //{
        //    if (!isAlive)
        //        return;

        //    if (Managers.Game.IsClear)
        //        return;
        //}

        //protected override void FixedUpdate()
        //{
        //    if (!isAlive)
        //        return;

        //    if (Managers.Game.IsClear)
        //        return;

        //    Move.MyFixedUpdate();
        //    Attack.MyFixedUpdate();
        //    Anim.MyFixedUpdate();
        //    State.MyFixedUpdate();
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (!isAlive)
        //        return;

        //    if (Managers.Game.IsClear)
        //        return;
        //}

        ////-------------------------------------

        //public override void OnHitUnit(float getDamage, bool CheckInvincibility = true)
        //{
        //    if (CheckInvincibility && State.IsInvincibility)
        //        return;

        //    //데미지 처리
        //    Stat.Hp -= getDamage;

        //    //상태 처리
        //    if (Attack.IsAttacking)
        //        Attack.cancelAttacking = true;

        //    if (State.IsDashing)
        //        Move.cancelDashing = true;

        //    if (State.IsJumping)
        //        Move.cancelJumping = true;

        //    Anim.anim_TriggerHit = true;

        //    State.HitInvincibility();

        //    Move.currentJumpCount = 1;

        //    //사후 조건
        //    //Managers.Game.OnHitPlayer();
        //    //Managers.Sound.Play(Managers.Data.MstMaster.PlayerSound.HitClip);
        //}

        //public override void OnUnitDie()
        //{
        //    base.OnUnitDie();
        //    Move.OnUnitDie();
        //    Anim.OnUnitDie();

        //    //Managers.Game.OnPlayerDie();
        //}
    }

}