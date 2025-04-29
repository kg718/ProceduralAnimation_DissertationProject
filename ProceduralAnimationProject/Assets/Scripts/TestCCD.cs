using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestCCD : InverseKinematics
{
    [SerializeField] private TextMeshProUGUI iterationText;
    [SerializeField] private TextMeshProUGUI endPositionText;

    private int iterationCount = 0;
    private bool isIterating = true;
    [SerializeField] private float threshold;

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
        if (Mathf.Abs((targetPosition - GetEndEffectorPosition()).magnitude) <= threshold)
        {
            SetIterating(false);
            print("CCD:" + iterationCount);
            iterationText.text = "Iterations: " + iterationCount.ToString();
            endPositionText.text = "End Position:" + GetEndEffectorPosition().ToString();
        }
    }

    private void Iterate()
    {
        iterationCount++;
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

    public void SetIterating(bool _iterating)
    {
        isIterating = _iterating;
    }

    public bool GetIterating()
    {
        return isIterating;
    }

    public override List<Transform> GetJoints()
    {
        return joints;
    }

    public void ResetIterationCount()
    {
        iterationCount = 0;
    }
}
