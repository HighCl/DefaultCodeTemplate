using UnityEngine;

namespace DefaultSetting
{
    [RequireComponent(typeof(PlayerMove))]
    [RequireComponent(typeof(PlayerAttack))]
    [RequireComponent(typeof(PlayerAnimation))]
    [RequireComponent(typeof(PlayerStat))]
    [RequireComponent(typeof(PlayerState))]
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] public Collider2D Coll { get; private set; }
        [field: SerializeField] public Rigidbody2D Rig { get; private set; }

        [field: SerializeField] public PlayerMove Move { get; private set; }
        [field: SerializeField] public PlayerAttack Attack { get; private set; }
        [field: SerializeField] public PlayerAnimation Anim { get; private set; }
        [field: SerializeField] public PlayerStat Stat { get; private set; }
        [field: SerializeField] public PlayerState State { get; private set; }

        public bool isAlive = true;
        public bool isDrawGizmos = true;

        public bool IsMove => true;

        private void Reset()
        {
            Stat = GetComponent<PlayerStat>();

            Coll = GetComponent<Collider2D>();
            Rig = GetComponent<Rigidbody2D>();

            Move = GetComponent<PlayerMove>();
            Attack = GetComponent<PlayerAttack>();
            Anim = GetComponent<PlayerAnimation>();
            State = GetComponent<PlayerState>();
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

        private void OnDrawGizmosSelected()
        {
            if (!isDrawGizmos)
                return;

            //Move.MyDrawGizmosSelected();
            //Attack.MyDrawGizmosSelected();
        }
    }
}
