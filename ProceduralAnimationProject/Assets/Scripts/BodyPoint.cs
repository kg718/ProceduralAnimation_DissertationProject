using UnityEngine;

public class BodyPoint : MonoBehaviour
{
    [SerializeField] private float radius;

    public Vector3 topPoint;
    public Vector3 rightPoint;
    public Vector3 bottomPoint;


    void Start()
    {
        DetermineCircle();
    }

    void Update()
    {
        
    }

    public void DetermineCircle()
    {
        topPoint = new Vector3 (transform.position.x, transform.position.y + radius, transform.position.z);
        rightPoint = new Vector3 (transform.position.x + radius, transform.position.y, transform.position.z);
        bottomPoint = new Vector3 (transform.position.x, transform.position.y - radius, transform.position.z);
    }
}
