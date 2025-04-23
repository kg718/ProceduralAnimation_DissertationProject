using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureEditingPanel : MonoBehaviour
{
    public Slider bodySegmentCountSlider;
    public Slider bodySegmentScaleSlider;
    public Toggle hasLegsToggle;

    [Space]
    [SerializeField] private TextMeshProUGUI bodySegmentCountText;
    [SerializeField] private TextMeshProUGUI bodySegmentScaleText;

    private void Start()
    {
        UpdatebodySegmentCountText();
        UpdatebodySegmentScaleText();
    }

    public void UpdatebodySegmentCountText()
    {
        bodySegmentCountText.text = bodySegmentCountSlider.value.ToString();
    }

    public void UpdatebodySegmentScaleText()
    {
        bodySegmentScaleText.text = bodySegmentScaleSlider.value.ToString("F2");
    }

}
