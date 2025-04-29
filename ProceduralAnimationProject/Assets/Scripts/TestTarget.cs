using TMPro;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    [SerializeField] private GameObject targetSphere;
    [SerializeField] private TestCCD ccd;
    [SerializeField] private TestFABRIK fabrik;

    [SerializeField] private TextMeshProUGUI ccdTimerText;
    [SerializeField] private TextMeshProUGUI fabrikTimerText;

    [SerializeField] private TextMeshProUGUI ccdStartPointText;
    [SerializeField] private TextMeshProUGUI ccdDistanceText;
    [SerializeField] private TextMeshProUGUI fabrikStartPointText;
    [SerializeField] private TextMeshProUGUI fabrikDistanceText;

    [SerializeField] private float randomPosRange;

    private float ccdTimer = 0f;
    private float fabrikTimer = 0f;

    void Start()
    {
        SetRandomTarget();
    }

    void Update()
    {
        if(ccd.GetIterating())
        {
            ccdTimer += Time.deltaTime;
        }
        else
        {
            ccdTimerText.text = ccdTimer.ToString() + "Seconds";
        }
        if(fabrik.GetIterating())
        {
            fabrikTimer += Time.deltaTime;
        }
        else
        {
            fabrikTimerText.text = fabrikTimer.ToString() + "Seconds";
        }
    }

    public void OnSetTarget()
    {
        Vector3 _startPosCCD = ccd.GetEndEffectorPosition();
        ccdStartPointText.text = "Start Position:" + _startPosCCD.ToString();
        Vector3 _startPosFABRIK = fabrik.GetEndEffectorPosition();
        fabrikStartPointText.text = "Start Position:" + _startPosFABRIK.ToString();
        Vector3 targetPoint = new Vector3(0, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
        targetSphere.transform.position = targetPoint;
        ccd.SetIterating(true);
        ccd.SetTargetPosition(targetPoint);
        ccd.SetIKTarget(targetPoint);
        ccd.ResetIterationCount();
        ccdTimer = 0f;
        fabrik.SetIterating(true);
        fabrik.SetTargetPosition(targetPoint);
        fabrik.SetIKTarget(targetPoint);
        fabrik.ResetIterationCount();
        fabrikTimer = 0f;
        ccdDistanceText.text = "Distance Traveled:" + (targetSphere.transform.position - _startPosCCD).magnitude.ToString();
        fabrikDistanceText.text = "Distance Traveled:" + (targetSphere.transform.position - _startPosFABRIK).magnitude.ToString();
    }

    public void SetRandomTarget()
    {
        Vector3 _startPosCCD = ccd.GetEndEffectorPosition();
        ccdStartPointText.text = "Start Position:" + _startPosCCD.ToString();
        Vector3 _startPosFABRIK = fabrik.GetEndEffectorPosition();
        fabrikStartPointText.text = "Start Position:" + _startPosFABRIK.ToString();
        float xPos = Random.Range(-randomPosRange, randomPosRange);
        float yPos = Random.Range(-randomPosRange, randomPosRange);
        float zPos = Random.Range(-randomPosRange, randomPosRange);
        Vector3 targetPoint = new Vector3(xPos, yPos, zPos);
        targetSphere.transform.position = targetPoint;
        ccd.SetIterating(true);
        ccd.SetTargetPosition(targetPoint);
        ccd.SetIKTarget(targetPoint);
        ccd.ResetIterationCount();
        ccdTimer = 0f;
        fabrik.SetIterating(true);
        fabrik.SetTargetPosition(targetPoint);
        fabrik.SetIKTarget(targetPoint);
        fabrik.ResetIterationCount();
        fabrikTimer = 0f;
        ccdDistanceText.text = "Distance Traveled:" + (targetSphere.transform.position - _startPosCCD).magnitude.ToString();
        fabrikDistanceText.text = "Distance Traveled:" + (targetSphere.transform.position - _startPosFABRIK).magnitude.ToString();
    }
}
