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

        InvokeRepeating("Iterate", 1f, 0.5f);
    }

    void Update()
    {
        SetCCDTarget(targetObject.transform.position);
        if (!isIterating)
        {
            return;
        }
    }

    private void Iterate()
    {
        for (int i = jointCount - 1; i >= 0; i--)
        {
            Vector3 _jointToEffector = joints[jointCount].transform.position - joints[i].transform.position;
            Vector3 _jointToTarget = targetPosition - joints[i].transform.position;

            //Vector3 _jointDir = nextJoint.transform.position - transform.position;
             Quaternion _fromToRotation = Quaternion.FromToRotation(_jointToEffector, _jointToTarget) * joints[i].transform.rotation;
            joints[i].transform.rotation = _fromToRotation;
            //joints[i].transform.rotation = Quaternion.LookRotation(_jointToTarget);
            
        }
    }

    public void SetCCDTarget(Vector3 _position)
    {
        targetPosition = _position;
    }
}
