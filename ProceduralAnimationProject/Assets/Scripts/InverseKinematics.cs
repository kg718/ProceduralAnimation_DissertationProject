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
}
