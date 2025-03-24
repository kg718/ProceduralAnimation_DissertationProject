using System.Collections;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    [SerializeField] private LegAnimation leftLeg;
    [SerializeField] private LegAnimation rightLeg;
    [SerializeField] private leg nextLeg;
    [SerializeField] private float followSpeed;

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
        transform.rotation = lookDir;
        if (_moveDir.magnitude > 2.5)
        {
            transform.position += _moveDir.normalized * followSpeed;
        }
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
