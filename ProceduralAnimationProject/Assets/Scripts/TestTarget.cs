using TMPro;
using UnityEngine;

public class TestTarget : MonoBehaviour
{
    [SerializeField] private GameObject targetSphere;
    [SerializeField] private TestCCD ccd;
    [SerializeField] private TestFABRIK fabrik;

    [SerializeField] private TextMeshProUGUI ccdTimerText;
    [SerializeField] private TextMeshProUGUI fabrikTimerText;

    private float ccdTimer = 0f;
    private float fabrikTimer = 0f;

    void Start()
    {
        
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
    }
}
