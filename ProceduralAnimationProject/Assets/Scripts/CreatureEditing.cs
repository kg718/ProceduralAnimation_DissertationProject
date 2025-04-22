using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void UpdateLegs()
    {
        hasLegs = panel.hasLegsToggle.isOn;
        for (int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].GetComponent<BodySegment>().EditLegs(hasLegs);
        }
    }
}
