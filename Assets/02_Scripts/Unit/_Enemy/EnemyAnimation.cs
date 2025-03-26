using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;
    [SerializeField] private Animator _animator;

    private static readonly int _anim_Idle = Animator.StringToHash("IdleAnim");
    private static readonly int _anim_Walk = Animator.StringToHash("WalkAnim");
    private static readonly int _anim_Attack = Animator.StringToHash("AttackAnim");
    private static readonly int _anim_Die = Animator.StringToHash("DieAnim");
    private static readonly float _anim_crossfadeTime = 0.2f;

    enum AnimationType
    {
        Idle,
        Walk,
        Attack,
        Die,
    }
    private AnimationType _currentAnim;

    private bool IsMove => _controller.State.IsMoving;
    private bool IsAttack => _controller.State.IsAttacking;

    private void Reset()
    {
        _controller = GetComponent<EnemyController>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void MyAwake(EnemyController controller)
    {
        _controller = controller;
    }

    public void MyUpdate()
    {
        if (!_controller.isAlive)
            return;

        CheckAnimation();
    }

    public void MyFixedUpdate()
    {
    }

    public void OnDie()
    {
        SwitchAnim(AnimationType.Die);
    }

    void CheckAnimation()
    {
        if (IsAttack)
        {
            SwitchAnim(AnimationType.Attack);
            return;
        }

        if (IsMove == false)
        {
            SwitchAnim(AnimationType.Idle);
        }
        else
        {
            SwitchAnim(AnimationType.Walk);
        }
    }

    void SwitchAnim(AnimationType animType)
    {
        if (_currentAnim == animType)
            return;

        switch (animType)
        {
            case AnimationType.Idle:
                _animator.CrossFade(_anim_Idle, _anim_crossfadeTime);
                break;
            case AnimationType.Walk:
                _animator.CrossFade(_anim_Walk, _anim_crossfadeTime);
                break;
            case AnimationType.Attack:
                _animator.CrossFade(_anim_Attack, _anim_crossfadeTime);
                break;
            case AnimationType.Die:
                _animator.CrossFade(_anim_Die, _anim_crossfadeTime);
                break;
            default:
                break;
        }
        _currentAnim = animType;
    }
}
