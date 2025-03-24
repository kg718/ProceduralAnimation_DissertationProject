using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureMovement : MonoBehaviour
{
    private Creature creature;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    private float moveMult;
    private Vector2 inputDir;

    private void Awake()
    {
        creature = GetComponent<Creature>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        //RaycastHit _hit;
        //Physics.Raycast(transform.position, Vector3.down, out _hit, 10);
        //transform.position = new Vector3(transform.position.x, _hit.point.y + 3, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(inputDir.y) > 0 && moveMult < moveSpeed)
        {
            moveMult += acceleration * inputDir.y;
        }

        if(Mathf.Abs(inputDir.y) == 0 && Mathf.Abs(moveMult) > 0)
        {
            if(Mathf.Abs(moveMult) < 0.05)
            {
                moveMult = 0;
            }

            if(moveMult > 0)
            {
                moveMult -= deceleration;
            }
            else if(moveMult < 0)
            {
                moveMult += deceleration;
            }
        }
        creature.segments[0].gameObject.transform.position += creature.segments[0].gameObject.transform.forward * moveMult;
        //transform.position += transform.forward * moveMult;

        if (Mathf.Abs(inputDir.x) > 0)
        {
            creature.segments[0].gameObject.transform.Rotate(new Vector3(0, inputDir.x * rotateSpeed, 0));
            //transform.Rotate(new Vector3(0, inputDir.x * rotateSpeed, 0));
        }
    }

    private void OnMovement(InputValue _value)
    {
        inputDir = _value.Get<Vector2>();
    }
}
