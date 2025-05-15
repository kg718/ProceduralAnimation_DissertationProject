using System.Collections.Generic;
using UnityEngine;

public class CCD : InverseKinematics
{
    [SerializeField] private GameObject jointPrefab;

    private bool isIterating = true;

    void Start()
    {
        startPosition = transform.position;
        jointCount = joints.Count - 1;
        foreach (float _length in lengths)
        {
            totalLength += _length;
        }
    }

    void Update()
    {
        if (!isIterating)
        {
            return;
        }
        Iterate();
    }

    private void Iterate()
    {
        for (int i = jointCount - 1; i >= 0; i--)
        {
            Vector3 _jointToEffector = joints[jointCount].transform.position - joints[i].transform.position; // the vector representing the direction from the current joint to the end effector
            Vector3 _jointToTarget = targetPosition - joints[i].transform.position; // the vector representing the direction from the current joint to the target position

            Quaternion _fromToRotation; // _jointToEffector needs to become equal to _jointToTarget
            Vector3 _axis = Vector3.Cross(_jointToEffector, _jointToTarget);
            float _angle = _jointToEffector.magnitude * _jointToTarget.magnitude + Vector3.Dot(_jointToTarget, _jointToEffector);
            _fromToRotation = new Quaternion(_axis.x, _axis.y, _axis.z, _angle);
            _fromToRotation = _fromToRotation.normalized * joints[i].transform.rotation;
            joints[i].transform.rotation = _fromToRotation;
        }
    }

    public override Vector3 GetTargetPosition()
    {
        return targetObject.transform.position;
    }

    public override void SetTargetPosition(Vector3 _position)
    {
        targetObject.transform.position = _position;
    }

    public override Vector3 GetEndEffectorPosition()
    {
        return joints[jointCount].position;
    }

    public override void SetIKTarget(Vector3 _position)
    {
        targetPosition = _position;
    }

    //For displaying the IK target in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetObject.transform.position, 0.5f);
    }

    public override List<Transform> GetJoints()
    {
        return joints;
    }

    public override float GetTotalLength()
    {
        totalLength = 0;
        foreach (float _length in lengths)
        {
            totalLength += _length;
        }
        if (joints.Count == 2)
        {
            totalLength = lengths[0];
        }
        return totalLength;
    }

    public override void AddJoint()
    {
        GameObject _newJoint = Instantiate(jointPrefab);
        //Transform _footTransform = joints[jointCount - 1].transform;
        joints.Insert(joints.Count - 1, _newJoint.transform);
        _newJoint.transform.parent = joints[joints.Count - 3].transform;
        joints[joints.Count - 1].transform.parent = _newJoint.transform;
        lengths.Add(lengths[0]);
        AdjustJoints();
    }

    public override void RemoveJoint()
    {
        joints[joints.Count - 1].transform.parent = joints[joints.Count - 3].transform;


        jointCount--;
        //Destroy(joints[joints.Count - 2].gameObject);
        joints[joints.Count - 2].gameObject.SetActive(false);
        joints.RemoveAt(joints.Count - 2);
        AdjustJoints();
        lengths.RemoveAt(lengths.Count - 2);
    }

    public void AdjustJoints()
    {
        for (int i = 0; i < joints.Count - 1; i++)
        {
            if(i != 0)
            {
                joints[i].localPosition = new Vector3(0, 0, lengths[i]);
            }
            if (joints[i].transform.childCount > 0)
            {
                for(int j = 0; j < joints[i].childCount; j++)
                {
                    joints[i].gameObject.GetComponent<IKJoint>().SetNextJoint(joints[i].transform.GetChild(j).gameObject.GetComponent<IKJoint>());
                }
            }
        }
        joints[joints.Count - 1].localPosition = new Vector3(0, 0, lengths[lengths.Count - 1]);
        jointCount = joints.Count - 1;

        totalLength = 0;
        foreach (float _length in lengths)
        {
            totalLength += _length;
        }
        if (joints.Count == 2)
        {
            totalLength = lengths[0];
        }
    }

    public override void AdjustJointSegmentLength(float _length)
    {
        for (int i = 0; i < jointCount; i++)
        {
            joints[i].GetComponent<IKJoint>().legSegment.transform.localScale = new Vector3(0.2f, _length, 0.2f);
            joints[i].GetComponent<IKJoint>().legSegment.transform.localPosition = new Vector3(0.0f, 0.0f, _length / 2);
            if (i != joints.Count)
            {
                lengths[i] = _length;
            }
        }
    }



}
