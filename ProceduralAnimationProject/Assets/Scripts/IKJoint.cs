using UnityEngine;

public class IKJoint : MonoBehaviour
{
    [SerializeField] private bool usingFABRIK = true;
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

        if (usingFABRIK)
        {
            Vector3 _jointDir = nextJoint.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(_jointDir);
        }
    }
}
