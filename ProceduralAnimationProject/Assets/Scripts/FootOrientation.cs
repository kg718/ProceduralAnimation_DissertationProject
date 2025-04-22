using UnityEngine;

public class FootOrientation : MonoBehaviour
{
    public void OrientFoot(RaycastHit _hit)
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, _hit.normal);
    }
}
