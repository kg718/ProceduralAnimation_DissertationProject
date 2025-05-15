using UnityEngine;

public class bodyRotation : MonoBehaviour
{
    public InverseKinematics leftIK;
    public InverseKinematics rightIK;

    public GameObject leftSocket;
    public GameObject rightSocket;
    public InverseKinematics extraIK;

    [SerializeField] GameObject leftLeg;
    [SerializeField] GameObject rightLeg;
    [SerializeField] private float height;
    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 desiredLeftPos;
    private Vector3 desiredRightPos;
    private float currentStepTimer = 0;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float simmilarityThreshold;

    private void Start()
    {
        leftPos = leftLeg.transform.localPosition;
        rightPos = rightLeg.transform.localPosition;
    }

    private void Update()
    {
        Vector3 _averagefootPos = leftIK.GetEndEffectorPosition() - rightIK.GetEndEffectorPosition();
        currentStepTimer += Time.deltaTime * moveSpeed;

        if (_averagefootPos.y < 0 && Mathf.Abs(_averagefootPos.y) > simmilarityThreshold) // Right foot is higher
        {
            desiredLeftPos = new Vector3(leftLeg.transform.localPosition.x, leftPos.y - height, leftLeg.transform.localPosition.z);
            desiredRightPos = new Vector3(rightLeg.transform.localPosition.x, rightPos.y + height, rightLeg.transform.localPosition.z);
        }
        if (_averagefootPos.y > 0 && Mathf.Abs(_averagefootPos.y) > simmilarityThreshold) // Left foot is higher
        {
            desiredLeftPos = new Vector3(leftLeg.transform.localPosition.x, leftPos.y + height, leftLeg.transform.localPosition.z);
            desiredRightPos = new Vector3(rightLeg.transform.localPosition.x, rightPos.y - height, rightLeg.transform.localPosition.z);
        }
        if(Mathf.Abs(_averagefootPos.y) <= simmilarityThreshold) // Feet are at simmilar height
        {
            desiredLeftPos = leftPos;
            desiredRightPos = rightPos;
        }

        if(currentStepTimer >= 1)
        {
            currentStepTimer = 0;
        }

        leftLeg.transform.localPosition = leftLeg.transform.localPosition + (desiredLeftPos - leftLeg.transform.localPosition) * currentStepTimer;
        rightLeg.transform.localPosition = rightLeg.transform.localPosition + (desiredRightPos - rightLeg.transform.localPosition) * currentStepTimer;
    }
}
