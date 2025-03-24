using UnityEngine;

[RequireComponent(typeof(FABRIK))]
public class LegAnimation : MonoBehaviour
{
    [SerializeField] private LayerMask walkableLayers;
    [SerializeField] private float stepDistance;
    [SerializeField] private float stepHeight;
    [SerializeField] private float stepSpeed;
    private float currentStepTimer = 1;
    private bool isStepping = false;
    private bool canStep = true;

    private BodySegment segment;
    private FABRIK legFabrik;

    private Vector3 oldPosition;
    private Vector3 newPosition;

    void Start()
    {
        legFabrik = GetComponent<FABRIK>();
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 20, walkableLayers);
        legFabrik.SetTargetPosition(hit.point);
    }

    void Update()
    {
        if(currentStepTimer < 1)
        {
            currentStepTimer += Time.deltaTime * stepSpeed;
            Vector3 _lerpedPosition = oldPosition + (newPosition - oldPosition) * currentStepTimer;
            _lerpedPosition.y += Mathf.Sin(currentStepTimer * Mathf.PI) * stepHeight;

            legFabrik.SetFABRIKTarget(_lerpedPosition);
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
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, 20, walkableLayers);
        legFabrik.SetTargetPosition(hit.point);

        if (Mathf.Sqrt(Mathf.Pow((legFabrik.GetTargetPosition().x - legFabrik.GetEndEffectorPosition().x), 2) + Mathf.Pow((legFabrik.GetTargetPosition().y - legFabrik.GetEndEffectorPosition().y), 2)) >= stepDistance && !isStepping)
        {
            if (!canStep ||currentStepTimer < 0)
            {
                return;
            }
            currentStepTimer = 0;
            newPosition = legFabrik.GetTargetPosition();
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
