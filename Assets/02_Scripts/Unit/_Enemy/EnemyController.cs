using DefaultSetting.Utility;
using UnityEngine;

public enum EnemyFSM
{
    None,
    Patrol,
    Chase,
    Attack,
    Die,
}

[RequireComponent(typeof(EnemyMove))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(EnemyAnimation))]
[RequireComponent(typeof(EnemyStat))]
[RequireComponent(typeof(EnemyState))]
public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public EnemyFSM FSM { get; private set; }
    [field: SerializeField] public Collider2D Coll { get; private set; }
    [field: SerializeField] public Rigidbody2D Rig { get; private set; }

    [field: SerializeField] public EnemyMove Move { get; private set; }
    [field: SerializeField] public EnemyAttack Attack { get; private set; }
    [field: SerializeField] public EnemyAnimation Anim { get; private set; }
    [field: SerializeField] public EnemyStat Stat { get; private set; }
    [field: SerializeField] public EnemyState State { get; private set; }

    public bool isAlive = true;
    public bool isDrawGizmos = true;

    public bool IsMove => true;

    private void Reset()
    {
        Stat = GetComponent<EnemyStat>();

        Coll = GetComponent<Collider2D>();
        Rig = GetComponent<Rigidbody2D>();

        Move = GetComponent<EnemyMove>();
        Attack = GetComponent<EnemyAttack>();
        Anim = GetComponent<EnemyAnimation>();
        State = GetComponent<EnemyState>();
    }

    void Awake()
    {
        isAlive = true;
        Stat.MyAwake(this);

        Move.MyAwake(this);
        Attack.MyAwake(this);

        State.MyAwake(this);
    }

    void Update()
    {
        if (!isAlive)
            return;

        //Move.MyUpdate();
        //Attack.MyUpdate();

        //Anim.MyUpdate();
    }

    private void FixedUpdate()
    {
        //Move.MyFixedUpdate();
        //Attack.MyFixedUpdate();
        //Anim.MyFixedUpdate();
    }

    public void OnHit(float damage)
    {
        //DebugUtility.Log($"피격");

        Stat.currentHP -= damage;
        if (Stat.currentHP <= 0)
            OnDie();
    }

    void OnDie()
    {
        //DebugUtility.Log($"사망");

        isAlive = false;
        Anim.OnDie();
        Coll.enabled = false;
        Rig.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void OnPlayerDie()
    {
        if (!isAlive)
            return;

        DebugUtility.Log("플레이어 사망");
    }

    public void SetFSM(EnemyFSM fsm) => this.FSM = fsm;

    private void OnDrawGizmosSelected()
    {
        if (!isDrawGizmos)
            return;

        //Move.MyDrawGizmosSelected();
        //Attack.MyDrawGizmosSelected();
    }
}
