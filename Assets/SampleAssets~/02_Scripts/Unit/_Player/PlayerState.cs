using UnityEngine;

namespace DefaultSetting
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] private PlayerController _controller;

        public bool IsMoving => _controller.Move.IsMoving;
        public bool IsAttacking => _controller.Attack.IsAttacking;

        public void MyAwake(PlayerController controller)
        {
            _controller = controller;
        }
    }
}