using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;

    private float _speed = 4;
    public bool IsMoving = false;

    public void MyAwake(EnemyController controller)
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
