using TMPro;
using UnityEngine;

public class TestFABRIK : InverseKinematics
{
    [SerializeField] private TextMeshProUGUI iterationText;

    private int iterationCount = 0;

    private bool iteratingForward = false;
    private bool isIterating = true;
    [SerializeField] private float threshold;

    private bool startAtTarget = false;

    void Start()
    {
        startPosition = transform.position;
        jointCount = joints.Count - 1;
        foreach (float _length in lengths)
        {
            totalLength += _length;
        }
        StartIteration(iteratingForward);
    }

    void Update()
    {
        startPosition = transform.position;
        if (!isIterating)
        {
            return;
        }
        if ((targetPosition - startPosition).magnitude > totalLength)
        {
            joints[0].position = startPosition;
            Vector3 _jointDir = (targetPosition - startPosition).normalized;
            for (int i = 1; i <= jointCount; i++)
            {
                joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
            }
            return;
        }

        if (iteratingForward)
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
        for (int i = 1; i <= jointCount; i++)
        {
            Vector3 _jointDir = (joints[i].position - joints[i - 1].position).normalized;
            joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
            if (i == jointCount)
            {
                StartIteration(false);
            }
            if(i == 1)
            {
                if (Mathf.Abs((targetPosition - GetEndEffectorPosition()).magnitude) <= threshold && startAtTarget)
                {
                    SetIterating(false);
                    print("FABRIK:" + iterationCount);
                    iterationText.text = "Iterations: " + iterationCount.ToString();
                }
            }
        }
    }

    private void IterateBackward()
    {
        iterationCount++;
        for (int i = jointCount - 1; i >= 0; i--)
        {
            Vector3 _jointDir = (joints[i].position - joints[i + 1].position).normalized;
            joints[i].position = joints[i + 1].position + _jointDir * lengths[i];
            if (i == 0)
            {
                StartIteration(true);
            }
        }
        if (Mathf.Abs((joints[0].position - transform.position).magnitude) <= threshold)
        {
            startAtTarget = true;
            //SetIterating(false);
        }
        else
        {
            startAtTarget = false;
        }
    }

    private void StartIteration(bool _forward)
    {
        if (_forward)
        {
            joints[0].position = startPosition;
            iteratingForward = true;
        }
        else
        {
            joints[jointCount].position = targetPosition;
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

    public void SetIterating(bool _iterating)
    {
        isIterating = _iterating;
    }

    public bool GetIterating()
    {
        return isIterating;
    }

    public void ResetIterationCount()
    {
        iterationCount = 0;
    }
}
