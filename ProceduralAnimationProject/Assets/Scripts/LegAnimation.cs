using UnityEngine;

[RequireComponent(typeof(InverseKinematics))]
public class LegAnimation : MonoBehaviour
{
    [SerializeField] private IKType.IKMode ikMode = IKType.IKMode.FABRIK;
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
            Vector3 _lerpedPosition = oldPosition + (newPosition - oldPosition) * currentStepTimer;
            _lerpedPosition.y += Mathf.Sin(currentStepTimer * Mathf.PI) * stepHeight;

            legIK.SetIKTarget(_lerpedPosition);
        }
        else
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
        footOrientation.OrientFoot(_hit);
        legIK.SetTargetPosition(_hit.point);


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

    private void FixedUpdate()
    {
        
    }

    public bool GetIsStepping()
    {
        return isStepping;
    }

    public void SetStepAbility(bool _canStep)
    {
        canStep = _canStep;
    }

    public void SetSegment(BodySegment _segment)
    {
        segment = _segment;
    }
}
