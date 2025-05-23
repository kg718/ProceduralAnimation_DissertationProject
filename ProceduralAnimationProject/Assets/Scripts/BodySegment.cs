using System.Collections;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    [SerializeField] private LegAnimation leftLeg;
    [SerializeField] private LegAnimation rightLeg;
    [SerializeField] private LegAnimation extraLeg;
    [SerializeField] private leg nextLeg;
    [SerializeField] private float followSpeed;
    [SerializeField] private LayerMask groundLayers;

    [SerializeField] private float bodyHeight = 3;

    private BodySegment previousSegment;
    private bool isHead = false;
    private bool isFalling;

    public enum leg
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

    private void FixedUpdate()
    {
        if (isHead)
        {
            if (isFalling)
            {
                //Lower the y coordinate to simmulate falling
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
            }
        }
    }

    void Update()
    {
        if(isHead)
        {
            return; // Head of creature does not need to follow any other segment
        }
        Vector3 _moveDir = previousSegment.transform.position - transform.position; // Distance from previous segment
        Quaternion lookDir = Quaternion.LookRotation(_moveDir);
        if(!isFalling)
        {
            _moveDir = new Vector3(_moveDir.x, 0, _moveDir.z); // When on the ground the y coordinate should be controlled by the terrain not the other segments
        }
        transform.rotation = lookDir; // Faces towards the previous segment
        if (_moveDir.magnitude > 2.3) // Too far away from body
        {
            transform.position += _moveDir.normalized * followSpeed;
        }
        if (_moveDir.magnitude < 2.7) // Too close to body
        {
            transform.position -= _moveDir.normalized * followSpeed;
        }

        RaycastHit _hit;
        Physics.Raycast(transform.position, Vector3.down, out _hit, groundLayers); // Finds distance to the ground and returns the point that the raycast hits the ground
        if(!isFalling)
        {
            //When falling, take the y coordinate of the previous segment into account when following, this lets the body trail behind 
            transform.position = new Vector3(transform.position.x, _hit.point.y + bodyHeight, transform.position.z);
        }
    }

    //Determine which leg can and cannot step next
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

    public void StartFalling()
    {
        isFalling = true;
    }

    public leg GetNextLeg()
    {
        return nextLeg;
    }

    public void SetNextLeg(leg _nextLeg)
    {
        nextLeg = _nextLeg;
    }

    public void EditLegs(bool _hasLegs)
    {
        if (_hasLegs)
        {
            leftLeg.gameObject.SetActive(true);
            rightLeg.gameObject.SetActive(true);
        }
        else
        {
            leftLeg.gameObject.SetActive(false);
            rightLeg.gameObject.SetActive(false);
        }
    }

    public void EditExtraLeg(bool _hasLeg)
    {
        if(extraLeg == null)
        {
            return;
        }
        if (_hasLeg)
        {
            extraLeg.gameObject.SetActive(true);
        }
        else
        {
            extraLeg.gameObject.SetActive(false);
        }
    }

    public void EditLegSegmentCount(int _desiredSegmentCount)
    {
        if (_desiredSegmentCount < leftLeg.GetComponent<InverseKinematics>().GetJoints().Count)
        {
            for (int i = 0; i < Mathf.Abs(_desiredSegmentCount - leftLeg.GetComponent<InverseKinematics>().GetJoints().Count); i++)
            {
                leftLeg.GetComponent<InverseKinematics>().RemoveJoint();
            }
        }
        if (_desiredSegmentCount > leftLeg.GetComponent<InverseKinematics>().GetJoints().Count)
        {
            for (int i = 0; i < _desiredSegmentCount - leftLeg.GetComponent<InverseKinematics>().GetJoints().Count; i++)
            {
                leftLeg.GetComponent<InverseKinematics>().AddJoint();
            }
        }
        if (_desiredSegmentCount < rightLeg.GetComponent<InverseKinematics>().GetJoints().Count)
        {
            for (int i = 0; i < Mathf.Abs(_desiredSegmentCount - rightLeg.GetComponent<InverseKinematics>().GetJoints().Count); i++)
            {
                rightLeg.GetComponent<InverseKinematics>().RemoveJoint();
            }
        }
        if (_desiredSegmentCount > rightLeg.GetComponent<InverseKinematics>().GetJoints().Count)
        {
            for (int i = 0; i < _desiredSegmentCount - rightLeg.GetComponent<InverseKinematics>().GetJoints().Count; i++)
            {
                rightLeg.GetComponent<InverseKinematics>().AddJoint();
            }
        }
        if(extraLeg != null)
        {
            if (_desiredSegmentCount < extraLeg.GetComponent<InverseKinematics>().GetJoints().Count)
            {
                for (int i = 0; i < Mathf.Abs(_desiredSegmentCount - extraLeg.GetComponent<InverseKinematics>().GetJoints().Count); i++)
                {
                    extraLeg.GetComponent<InverseKinematics>().RemoveJoint();
                }
            }
            if (_desiredSegmentCount > extraLeg.GetComponent<InverseKinematics>().GetJoints().Count)
            {
                for (int i = 0; i < _desiredSegmentCount - extraLeg.GetComponent<InverseKinematics>().GetJoints().Count; i++)
                {
                    extraLeg.GetComponent<InverseKinematics>().AddJoint();
                }
            }
        }


        EditBodyHeight(leftLeg.GetComponent<InverseKinematics>().GetTotalLength() - 1);
        if (isHead)
        {
            transform.parent.GetComponent<CreatureMovement>().bodyHeight = leftLeg.GetComponent<InverseKinematics>().GetTotalLength() - 1;
            transform.parent.GetComponent<CreatureMovement>().groundDetectionRange = leftLeg.GetComponent<InverseKinematics>().GetTotalLength() * 5f;
        }
    }

    public void EditLegSegmentLength(float _length)
    {
        leftLeg.GetComponent<InverseKinematics>().AdjustJointSegmentLength(_length);
        rightLeg.GetComponent<InverseKinematics>().AdjustJointSegmentLength(_length);
        if (extraLeg != null)
        {
            extraLeg.GetComponent <InverseKinematics>().AdjustJointSegmentLength(_length);
        }
    }

    public void EditScale(float _scale)
    {
        transform.localScale = new Vector3(_scale, _scale, _scale);

        leftLeg.gameObject.transform.localScale = new Vector3(1 / _scale, 1 / _scale, 1 / _scale);
        rightLeg.gameObject.transform.localScale = new Vector3(1 / _scale, 1 / _scale, 1 / _scale);
        if (extraLeg != null)
        {
            extraLeg.gameObject.transform.localScale = new Vector3(1 / _scale, 1 / _scale, 1 / _scale);
        }
    }

    public void EditStepLength(float _stepLength)
    {
        leftLeg.GetComponent<LegAnimation>().UpdateStepLength(_stepLength);
        rightLeg.GetComponent<LegAnimation>().UpdateStepLength(_stepLength);
    }

    public void EditStepHeight(float _stepHeight)
    {
        leftLeg.GetComponent<LegAnimation>().UpdateStepHeight(_stepHeight);
        rightLeg.GetComponent<LegAnimation>().UpdateStepHeight(_stepHeight);
    }

    public void EditStepSpeed(float _stepSpeed)
    {
        leftLeg.UpdateStepSpeed(_stepSpeed);
        rightLeg.UpdateStepSpeed(_stepSpeed);
    }

    public void EditBodyHeight(float _bodyHeight)
    {
        bodyHeight = _bodyHeight;
    }
}
