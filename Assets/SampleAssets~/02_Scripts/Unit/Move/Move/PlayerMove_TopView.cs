using UnityEngine;

namespace DefaultSetting
{
    public class PlayerMove_TopView : BasePlayerMove
    {
        [SerializeField, ReadOnly] Transform moveTr = null;

        public override void MyAwake(BaseUnitController baseUnitController)
        {
            Controller = baseUnitController;
            moveTr = Controller.UnitTr;
        }

        public override void MyFixedUpdate()
        {
            OnMove();
        }

        public override void OnMove()
        {
            Debug.Log("PlayerMove_TopView 실행중");

            if (moveTr == null)
            {
                Debug.LogWarning("transform이 없음");
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector2 moveDir = new Vector3(h, v).normalized;

            moveTr.Translate(moveDir * movePower * Time.deltaTime);
        }
    }
}