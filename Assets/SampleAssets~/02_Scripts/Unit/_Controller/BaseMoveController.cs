using UnityEngine;

namespace DefaultSetting
{
    public class BaseMoveController : MonoBehaviour
    {
        private BaseUnitController baseUnitController;

        [SerializeField]
        private BaseMove _baseMove;
        public BaseMove BaseMove
        {
            get { return _baseMove; }
            set
            {
                if (_baseMove == null)
                    _baseMove = value;
                else
                {
                    _baseMove = value;
                    _baseMove.MyAwake(baseUnitController);
                }
            }
        }


        [field: SerializeField]
        public BaseJump BaseJump { get; private set; }
        public virtual void MyAwake(BaseUnitController baseUnitController)
        {
            this.baseUnitController = baseUnitController;

            if (BaseMove == null)
                Debug.LogWarning($"{baseUnitController.gameObject.name}\nBaseMove가 없습니다.");
            if (BaseJump == null)
                Debug.LogWarning($"{baseUnitController.gameObject.name}\nBaseJump가 없습니다.");

            BaseMove?.MyAwake(baseUnitController);
            BaseJump?.MyAwake(baseUnitController);
        }

        public virtual void MyUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                BaseMove = GetComponent<PlayerMove_2DMove>();
            if (Input.GetKeyDown(KeyCode.X))
                BaseMove = GetComponent<PlayerMove_TopView>();

            BaseMove?.MyUpdate();
            BaseJump?.MyUpdate();
        }

        public virtual void MyFixedUpdate()
        {
            BaseMove?.MyFixedUpdate();
            BaseJump?.MyFixedUpdate();
        }
    }
}
