using UnityEngine;

public class IKJoint : MonoBehaviour
{
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

        Vector3 _jointDir = nextJoint.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(_jointDir);
    }
}
