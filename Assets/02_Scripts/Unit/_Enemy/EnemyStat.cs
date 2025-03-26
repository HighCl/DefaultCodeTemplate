using UnityEngine;

namespace DefaultSetting
{
    public class EnemyStat : MonoBehaviour
    {
        [SerializeField] private EnemyController _controller;

        [field: SerializeField] public float maxHP { get; private set; } = 10;
        [field: SerializeField, ReadOnly] public float currentHP = 0;
        [field: SerializeField] public float Damage { get; private set; } = 3;

        public void MyAwake(EnemyController controller)
        {
            _controller = controller;
            currentHP = maxHP;
        }
    }
}