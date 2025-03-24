using System.Collections.Generic;
using UnityEngine;

public class FABRIK : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    [SerializeField] private List<Transform> joints = new List<Transform>();
    [SerializeField] private List<float> lengths = new List<float>();

    private float totalLength;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private int jointCount;

    private bool iteratingForward = false;
    private bool isIterating = true;


    void Start()
    {
        startPosition = transform.position;
        jointCount = joints.Count - 1;
        foreach(float _length in lengths)
        {
            totalLength += _length;
        }
        StartIteration(iteratingForward);
    }

    void Update()
    {
        startPosition = transform.position;
        if(!isIterating)
        {
            return;
        }
        if((targetPosition - startPosition).magnitude > totalLength)
        {
            joints[0].position = startPosition;
            Vector3 _jointDir = (targetPosition - startPosition).normalized;
            for (int i = 1; i <= jointCount; i++)
            {
                joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
                //joints[i].rotation = Quaternion.LookRotation(_jointDir);
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
        for (int i = 1; i <= jointCount; i++)
        {
            Vector3 _jointDir = (joints[i].position - joints[i - 1].position).normalized;
            joints[i].position = joints[i - 1].position + _jointDir * lengths[i - 1];
            //joints[i].rotation = Quaternion.LookRotation(_jointDir);
            if (i == jointCount)
            {
                StartIteration(false);
            }
        }
    }

    private void IterateBackward()
    {
        for (int i = jointCount - 1; i >= 0; i--)
        {
            Vector3 _jointDir = (joints[i].position - joints[i + 1].position).normalized;
            joints[i].position = joints[i + 1].position + _jointDir * lengths[i];
            if (i == 0)
            {
                StartIteration(true);
            }
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

    //public void SetTarget(Vector3 _newTarget)
    //{
    //    footTargetPosition = _newTarget;
    //}

    public Vector3 GetTargetPosition()
    {
        return targetObject.transform.position;
    }

    public void SetTargetPosition(Vector3 _position)
    {
        targetObject.transform.position = _position;
    }

    public Vector3 GetEndEffectorPosition()
    {
        return joints[jointCount].position;
    }

    public void SetFABRIKTarget(Vector3 _position)
    {
        targetPosition = _position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetObject.transform.position, 0.5f);
    }
}
