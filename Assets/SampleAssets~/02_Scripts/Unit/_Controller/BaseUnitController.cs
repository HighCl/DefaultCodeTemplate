using UnityEngine;

namespace DefaultSetting
{
    public abstract class BaseUnitController : MonoBehaviour
    {
        private Transform _unitTr;
        public virtual Transform UnitTr
        {
            get
            {
                if (_unitTr == null)
                {
                    _unitTr = transform;
                }
                return _unitTr;
            }
        }
        private Rigidbody2D _unitRig;
        public virtual Rigidbody2D UnitRig
        {
            get
            {
                if (_unitRig == null)
                {
                    _unitRig = GetComponent<Rigidbody2D>();
                }
                return _unitRig;
            }
        }


        public bool isAlive;
        private Collider2D _collider;
        public Collider2D Collider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComponent<Collider2D>();
                }
                return _collider;
            }
            set
            {
                _collider = value;
            }
        }

        private GameObject _dieCollider = null;
        protected GameObject DieCollider
        {
            get
            {
                if (_dieCollider == null)
                {
                    Transform tempTr = transform.Find("DieCollider");
                    if (tempTr != null)
                    {
                        _dieCollider = tempTr.gameObject;
                        _dieCollider.layer = (int)Define.Layer.DieCollider;
                    }
                    else
                    {
                        GameObject go = new GameObject();
                        go.name = "AutoAdd_DieCollider";
                        go.AddComponent<CircleCollider2D>();
                        go.transform.parent = transform;
                        go.layer = (int)Define.Layer.DieCollider;
                        _dieCollider = go;
                        Debug.Log($"{gameObject.name}_AutoAdd_DieCollider");
                    }
                }
                return _dieCollider;
            }
        }

        protected BaseMoveController _move;

        public virtual BaseMoveController MoveController
        {
            get { return _move; }
            set { _move = value; }
        }

        protected BaseAttack _attack;
        public virtual BaseAttack Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        private BaseAnimation _anim;

        public virtual BaseAnimation Animation
        {
            get { return _anim; }
            set { _anim = value; }
        }

        private BaseStat _stat;

        public virtual BaseStat Stat
        {
            get { return _stat; }
            set { _stat = value; }
        }

        private BaseState _state;

        public virtual BaseState State
        {
            get
            {
                return _state;
            }
            set { _state = value; }
        }

        protected virtual void Reset() { }
        protected virtual void Awake()
        {
            isAlive = true;
            DieCollider.SetActive(false);
        }

        protected virtual void Start() { }

        protected virtual void Update() { }

        protected virtual void FixedUpdate() { }

        public virtual void OnHitUnit(float enemyDamage, bool CheckInvincibility = true) { Debug.Log("충돌 설정X"); }
        public virtual void OnUnitDie()
        {
            isAlive = false;
            DieCollider.SetActive(true);
            Collider.enabled = false;
        }
    }
}