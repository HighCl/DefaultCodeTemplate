using UnityEngine;

namespace DefaultSetting
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private PlayerController _controller;

        [field: SerializeField] public float maxHP { get; private set; } = 10;
        [field: SerializeField, ReadOnly] public float currentHP = 0;
        [field: SerializeField] public float Damage { get; private set; } = 3;

        public void MyAwake(PlayerController controller)
        {
            _controller = controller;
            currentHP = maxHP;
        }
    }
}