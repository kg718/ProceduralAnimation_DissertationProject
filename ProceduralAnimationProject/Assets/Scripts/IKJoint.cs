using UnityEngine;

public class IKJoint : MonoBehaviour
{
    [SerializeField] private IKType.IKMode ikMode = IKType.IKMode.FABRIK;
    public GameObject legSegment;
    [SerializeField] private IKJoint nextJoint;
    //[SerializeField] private bool endEffector = false;

    void Start()
    {

    }

    void Update()
    {
        if (nextJoint == null)
        {
            return;
        }

        if (ikMode == IKType.IKMode.FABRIK)
        {
            Vector3 _jointDir = nextJoint.transform.position - transform.position;
            if(_jointDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(_jointDir);
            }
        }
    }

    public void SetNextJoint(IKJoint _joint)
    {
        nextJoint = _joint;
    }
}
