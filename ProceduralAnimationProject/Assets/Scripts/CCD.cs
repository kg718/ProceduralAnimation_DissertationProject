using System.Collections.Generic;
using UnityEngine;

public class CCD : InverseKinematics
{
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
            Vector3 _jointToEffector = joints[jointCount].transform.position - joints[i].transform.position;
            Vector3 _jointToTarget = targetPosition - joints[i].transform.position;

            Quaternion _fromToRotation = Quaternion.FromToRotation(_jointToEffector, _jointToTarget) * joints[i].transform.rotation;
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
        return totalLength;
    }
}
