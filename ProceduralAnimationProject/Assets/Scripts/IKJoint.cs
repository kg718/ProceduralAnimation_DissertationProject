using UnityEngine;

public class IKJoint : MonoBehaviour
{
    [SerializeField] private IKType.IKMode ikMode = IKType.IKMode.FABRIK;
    public GameObject legSegment;
    [SerializeField] private IKJoint nextJoint;

    void Update()
    {
        if (nextJoint == null)
        {
            return;
        }

        if (ikMode == IKType.IKMode.FABRIK) // CCD uses rotation of parented joints to affect the child joint's position, so the joints don't need to be individually rotated
        {
            Vector3 _jointDir = nextJoint.transform.position - transform.position;
            if(_jointDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_jointDir); // Rotate so that segment always faces the next joint
            }
        }
    }

    public void SetNextJoint(IKJoint _joint)
    {
        nextJoint = _joint;
    }
}
