using System.Collections;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    [SerializeField] private LegAnimation leftLeg;
    [SerializeField] private LegAnimation rightLeg;
    [SerializeField] private leg nextLeg;
    [SerializeField] private float followSpeed;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private float bodyHeight = 3;

    private BodySegment previousSegment;
    private bool isHead = false;

    enum leg
    {
        UNDETERMINED,
        LEFT,
        RIGHT
    }

    void Start()
    {
        leftLeg.SetSegment(this);
        rightLeg.SetSegment(this);
        if (nextLeg == leg.LEFT)
        {
            leftLeg.SetStepAbility(true);
            rightLeg.SetStepAbility(false);
        }
        if (nextLeg == leg.RIGHT)
        {
            leftLeg.SetStepAbility(false);
            rightLeg.SetStepAbility(true);
        }
    }

    void Update()
    {
        if(isHead)
        {
            return;
        }
        Vector3 _moveDir = previousSegment.transform.position - transform.position;
        Quaternion lookDir = Quaternion.LookRotation(_moveDir);
        _moveDir = new Vector3(_moveDir.x, 0, _moveDir.z);
        transform.rotation = lookDir;
        if (_moveDir.magnitude > 2.3)
        {
            transform.position += _moveDir.normalized * followSpeed;
        }
        if (_moveDir.magnitude < 2.7)
        {
            transform.position -= _moveDir.normalized * followSpeed;
        }

        RaycastHit _hit;
        Physics.Raycast(transform.position, Vector3.down, out _hit, groundLayers);
        transform.position = new Vector3(transform.position.x, _hit.point.y + bodyHeight, transform.position.z);
    }

    public void UpdateNextLeg()
    {
        switch(nextLeg)
        {
            case leg.LEFT:
                nextLeg = leg.RIGHT;
                leftLeg.SetStepAbility(false);
                rightLeg.SetStepAbility(true);
                break;
            case leg.RIGHT:
                nextLeg = leg.LEFT;
                leftLeg.SetStepAbility(true);
                rightLeg.SetStepAbility(false);
                break;
        }
    }

    public void SetPreviousSegment(BodySegment _segment)
    {
        previousSegment = _segment;
    }
    
    public void SetHead()
    {
        isHead = true;
    }
}
