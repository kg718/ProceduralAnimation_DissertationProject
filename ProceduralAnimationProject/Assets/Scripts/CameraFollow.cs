using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followOffset;
    [SerializeField] private float heightOffset;

    void Update()
    {
        transform.rotation = target.rotation;
        transform.position = target.position - target.forward * followOffset;
        transform.position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
    }
}
