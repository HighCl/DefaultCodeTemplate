using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyAttack : MonoBehaviour
{
    [SerializeField, ReadOnly] private EnemyController _controller;

    [SerializeField] private bool _hasAttack = true;
    public bool IsAttacking = false;

    public SphereCollider attackSensor;

    public void MyAwake(EnemyController controller)
    {
        this._controller = controller;
    }

    public void MyUpdate()
    {
        switch (_controller.FSM)
        {
            case EnemyFSM.Patrol:
                break;
            case EnemyFSM.Chase:
                break;
            case EnemyFSM.Attack:
                OnAttack();
                break;
            case EnemyFSM.Die:
                break;
            default:
                break;
        }
    }

    void OnAttack()
    {
        if (!_hasAttack)
            return;

        //TODO: Attack 구현
    }

    public void MyDrawGizmosSelected()
    {
    }
}
