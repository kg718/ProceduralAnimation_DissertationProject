using UnityEngine;

[RequireComponent(typeof(InverseKinematics))]
public class LegAnimation : MonoBehaviour
{
    [SerializeField] private LayerMask walkableLayers;
    [SerializeField] private float stepDistance;
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    [SerializeField] private FootOrientation footOrientation;

    private float currentStepTimer = 1;
    private bool isStepping = false;
    private bool canStep = true;

    private BodySegment segment;
    private InverseKinematics legIK;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    void Start()
    {
        legIK = GetComponent<InverseKinematics>();
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 20, walkableLayers);
        legIK.SetTargetPosition(hit.point);
    }

    void Update()
    {
        if(currentStepTimer < 1)
        {
            currentStepTimer += Time.deltaTime * stepSpeed;
            Vector3 _lerpedPosition = oldPosition + (newPosition - oldPosition) * currentStepTimer; // Uses linear interpolation to create smooth animation over time
            _lerpedPosition.y += Mathf.Sin(currentStepTimer * Mathf.PI) * stepHeight; // Get the lerped target position on a curve

            legIK.SetIKTarget(_lerpedPosition);
        }
        else // Leg is not currently stepping
        {
            if(segment != null && isStepping)
            {
                segment.UpdateNextLeg();
            }
            oldPosition = newPosition;
            isStepping = false;
        }
        RaycastHit _hit;
        Physics.Raycast(transform.position, Vector3.down, out _hit, 20, walkableLayers);
        //Rotates foot to match the shape of the terrain
        footOrientation.OrientFoot(_hit);
        legIK.SetTargetPosition(_hit.point);

        //Starting a new step
        if (Mathf.Sqrt(Mathf.Pow((legIK.GetTargetPosition().x - legIK.GetEndEffectorPosition().x), 2) + Mathf.Pow((legIK.GetTargetPosition().y - legIK.GetEndEffectorPosition().y), 2)) >= stepDistance && !isStepping)
        {
            if (!canStep || currentStepTimer < 0)
            {
                return;
            }
            currentStepTimer = 0;
            newPosition = legIK.GetTargetPosition();
            isStepping = true;
        }
    }

    public bool GetIsStepping()
    {
        return isStepping;
    }

    public void SetStepAbility(bool _canStep) // Only one leg can step at any given time
    {
        canStep = _canStep;
    }

    public void SetSegment(BodySegment _segment)
    {
        segment = _segment;
    }

    //Leg Parameter Updates

    public void UpdateStepLength(float _stepLength)
    {
        stepDistance = _stepLength;
    }

    public void UpdateStepHeight(float _stepHeight)
    {
        stepHeight = _stepHeight;
    }

    public void UpdateStepSpeed(float _stepSpeed)
    {
        stepSpeed = _stepSpeed;
    }
}
