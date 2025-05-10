using UnityEngine;

public class CreatureEditing : MonoBehaviour
{
    [SerializeField] private CreatureEditingPanel panel;
    [SerializeField] private Creature creature;
    private int activeSegments = 10;
    private bool hasLegs = true;

    [Space]
    [SerializeField] private GameObject segmentPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void UpdateBodySegmentCount()
    {
        if (activeSegments > Mathf.RoundToInt(panel.bodySegmentCountSlider.value))
        {
            int _iterations = 10 - Mathf.RoundToInt(panel.bodySegmentCountSlider.value);
            for (int i = _iterations; i > 0; i--)
            {
                creature.segments[10 - i].gameObject.SetActive(false);
            }
            activeSegments = Mathf.RoundToInt(panel.bodySegmentCountSlider.value);
        }
        if (activeSegments < Mathf.RoundToInt(panel.bodySegmentCountSlider.value))
        {
            for(int i = 1; i < Mathf.RoundToInt(panel.bodySegmentCountSlider.value); i++)
            {
                creature.segments[i].gameObject.SetActive(true);
            }
            activeSegments = Mathf.RoundToInt(panel.bodySegmentCountSlider.value);
        }
    }

    public void UpdateBodySegmentCount(int _segmentCount)
    {
        if (activeSegments > _segmentCount)
        {
            int _iterations = 10 - _segmentCount;
            for (int i = _iterations; i > 0; i--)
            {
                creature.segments[10 - i].gameObject.SetActive(false);
            }
            activeSegments = _segmentCount;
        }
        if (activeSegments < _segmentCount)
        {
            for (int i = 1; i < _segmentCount; i++)
            {
                creature.segments[i].gameObject.SetActive(true);
            }
            activeSegments = _segmentCount;
        }
    }

    public void UpdateBodySegmentScale()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].EditScale(panel.bodySegmentScaleSlider.value);
        }
    }

    public void UpdateLegs()
    {
        hasLegs = panel.hasLegsToggle.isOn;
        for (int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].GetComponent<BodySegment>().EditLegs(hasLegs);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentLength(panel.legSegmentLengthSlider.value);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentCount(Mathf.RoundToInt(panel.legSegmentCountSlider.value));
        }
    }

    public void UpdateStepLength()
    {
        for(int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].EditStepLength(panel.stepLengthSlider.value);
        }
    }

    public void UpdateStepHeight()
    {
        for(int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].EditStepHeight(panel.stepHeightSlider.value);
        }
    }

    public void UpdateStepSpeed()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].EditStepSpeed(panel.stepSpeedSlider.value);
        }
    }
}
