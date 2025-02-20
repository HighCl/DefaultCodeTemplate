using UnityEngine;

namespace DefaultSetting
{
    public class PlayerMove_2DMove : BasePlayerMove
    {
        Transform moveTr = null;

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
            Debug.Log("PlayerMove_2DMove 실행중");

            if (moveTr == null)
            {
                Debug.LogWarning("transform이 없음");
                return;
            }

            float h = Input.GetAxis("Horizontal");
            Vector2 moveDir = new Vector3(h, 0).normalized;

            moveTr.Translate(moveDir * movePower * Time.deltaTime);
        }
    }
}