using System.Collections.Generic;
using UnityEngine;

public class FABRIK : InverseKinematics
{
    private bool iteratingForward = false;
    private bool isIterating = true;

    [SerializeField] private Transform jointParent;
    [SerializeField] private GameObject jointPrefab;

    void Start()
    {
        startPosition = transform.position;
        jointCount = joints.Count - 1;
        foreach(float _length in lengths)
        {
            totalLength += _length;
        }
        StartIteration(iteratingForward); // Can start by iterating forward or backward
    }

    void Update()
    {
        startPosition = transform.position;
        if(!isIterating)
        {
            return;
        }
        // Move when target is out of range
        if((targetPosition - startPosition).magnitude > totalLength)
        {
            joints[0].position = startPosition;
            Vector3 _jointDir = (targetPosition - startPosition).normalized;
            for (int i = 1; i <= jointCount; i++)
            {
                joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
            }
            return;
        }

        if(iteratingForward)
        {
            IterateForward();
        }
        else
        {
            IterateBackward();
        }
    }

    private void IterateForward()
    {
        //Iterate Down the chain
        for (int i = 1; i <= jointCount; i++)
        {
            Vector3 _jointDir = (joints[i].position - joints[i - 1].position).normalized;
            joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
            if (i == jointCount) // end effector
            {
                StartIteration(false);
            }
        }
    }

    private void IterateBackward()
    {
        //Iterate Up the chain
        for (int i = jointCount - 1; i >= 0; i--)
        {
            Vector3 _jointDir = (joints[i].position - joints[i + 1].position).normalized;
            joints[i].position = joints[i + 1].position + _jointDir * lengths[i];
            if (i == 0) // first joint in the chain
            {
                StartIteration(true);
            }
        }
    }

    private void StartIteration(bool _forward)
    {
        if (_forward)
        {
            joints[0].position = startPosition; // Sets base of the chain to the correct position
            iteratingForward = true;
        }
        else
        {
            joints[jointCount].position = targetPosition; // Sets end effector to target position
            iteratingForward = false;
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

    public override void AddJoint()
    {
        GameObject _newJoint = Instantiate(jointPrefab);
        joints.Insert(jointCount - 1, _newJoint.transform);
        _newJoint.transform.parent = jointParent;
        lengths.Add(lengths[0]);
        AdjustJoints();
    }

    public override void RemoveJoint()
    {
        jointCount--;
        joints[joints.Count - 2].gameObject.SetActive(false);
        Destroy(joints[joints.Count - 2].gameObject, 5f);
        joints.RemoveAt(joints.Count - 2); // Can't remove the last joint in the chain because that is the foot
        AdjustJoints();
        lengths.RemoveAt(lengths.Count - 2);
    }

    public void AdjustJoints()
    {
        for (int i = 0; i < jointCount; i++)
        {
            joints[i].localPosition = new Vector3(0, -(lengths[i] * i), 0);
            if(i != joints.Count)
            {
                joints[i].gameObject.GetComponent<IKJoint>().SetNextJoint(joints[i + 1].gameObject.GetComponent<IKJoint>());
            }
        }
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
            if(i != joints.Count)
            {
                lengths[i] = _length;
            }
        }
    }

    //For displaying the IK target in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
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
}
