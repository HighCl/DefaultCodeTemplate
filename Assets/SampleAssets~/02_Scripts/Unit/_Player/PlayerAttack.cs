using UnityEngine;

namespace DefaultSetting
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField, ReadOnly] private PlayerController _controller;

        [SerializeField] private bool _hasAttack = true;
        public bool IsAttacking = false;

        public SphereCollider attackSensor;

        public void MyAwake(PlayerController controller)
        {
            this._controller = controller;
        }

        public void MyUpdate()
        {
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
}