using UnityEngine;
using UnityEngine.InputSystem;

public class CreatureMovement : MonoBehaviour
{
    private Creature creature;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [SerializeField] private LayerMask groundLayers;
    public float groundDetectionRange = 25f;
    private float moveMult;
    private Vector2 inputDir;

    public float bodyHeight = 5f;
    public bool isFalling = false;

    private void Awake()
    {
        creature = GetComponent<Creature>();
    }

    void Update()
    {
        RaycastHit _hit;
        Physics.Raycast(creature.segments[0].transform.position, Vector3.down, out _hit, groundDetectionRange, groundLayers);
        
        Debug.DrawRay(creature.segments[0].transform.position, Vector3.down,Color.cyan , groundDetectionRange); // Draws the ground detection raycast in edit mode

        if(_hit.point == Vector3.zero) // Cannot detect ground beneath segment
        {
            SimulateGravity();
        }
        else
        {
            isFalling = false;
            creature.segments[0].transform.position = new Vector3(creature.segments[0].transform.position.x, _hit.point.y + bodyHeight, creature.segments[0].transform.position.z);
        }
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

            //Decelerate movement
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

        if (Mathf.Abs(inputDir.x) > 0)
        {
            creature.segments[0].gameObject.transform.Rotate(new Vector3(0, inputDir.x * rotateSpeed, 0));
        }
    }

    private void OnMovement(InputValue _value)
    {
        inputDir = _value.Get<Vector2>();
    }

    public void SimulateGravity()
    {
        isFalling = true;
        
        foreach (BodySegment _segment in creature.segments)
        {
            _segment.StartFalling();
        }
    }
}
