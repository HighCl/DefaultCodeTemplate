using UnityEngine;

namespace DefaultSetting
{
    public class PlayerJump_EasyJump : BaseJump
    {
        public override void MyAwake(BaseUnitController baseUnitController)
        {
            Controller = baseUnitController;
            rig = Controller.UnitRig;
        }

        public override void MyUpdate()
        {
            if (Input.GetKeyDown(jumpKeyCode))
            {
                Debug.Log("Jump");
                OnJump();
            }
        }

        public override void MyFixedUpdate() { }

        public override void OnJump()
        {
            if (rig == null)
            {
                Debug.LogWarning("rigidbody가 없음");
                return;
            }

            rig.AddForce(Vector3.up * JumpPower);
        }
    }
}