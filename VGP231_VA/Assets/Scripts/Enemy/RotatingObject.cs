using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationSpeed = Vector3.zero;

    private void Update()
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime);
    }
}
