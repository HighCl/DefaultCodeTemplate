using UnityEngine;

namespace DefaultSetting
{
    public class EnemyState : MonoBehaviour
    {
        [SerializeField] private EnemyController _controller;

        public bool IsMoving => _controller.Move.IsMoving;
        public bool IsAttacking => _controller.Attack.IsAttacking;

        public void MyAwake(EnemyController controller)
        {
            _controller = controller;
        }
    }
}