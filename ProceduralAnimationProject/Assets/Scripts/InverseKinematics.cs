using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    [SerializeField] protected GameObject targetObject;

    [SerializeField] protected List<Transform> joints = new List<Transform>();
    [SerializeField] protected List<float> lengths = new List<float>();

    protected float totalLength;
    protected Vector3 startPosition;
    protected Vector3 targetPosition;

    protected int jointCount;

    public virtual Vector3 GetTargetPosition()
    {
        return new Vector3();
    }

    public virtual void SetTargetPosition(Vector3 _position)
    {
        
    }

    public virtual Vector3 GetEndEffectorPosition()
    {
        return new Vector3();
    }

    public virtual void SetIKTarget(Vector3 _position)
    {

    }
}
