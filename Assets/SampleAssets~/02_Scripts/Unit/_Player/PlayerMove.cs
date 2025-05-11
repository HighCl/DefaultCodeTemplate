using UnityEngine;

namespace DefaultSetting
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private PlayerController _controller;

        private float _speed = 4;
        public bool IsMoving = false;

        public void MyAwake(PlayerController controller)
        {
            _controller = controller;
        }

        public void MyUpdate()
        {
        }

        public void MyFixedUpdate()
        {
        }

        public void MyDrawGizmosSelected()
        {
        }
    }
}
