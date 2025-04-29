using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureEditingPanel : MonoBehaviour
{
    public Slider bodySegmentCountSlider;
    public Slider bodySegmentScaleSlider;
    public Toggle hasLegsToggle;
    public Slider legSegmentCountSlider;
    public Slider legSegmentLengthSlider;
    public Slider stepLengthSlider;
    public Slider stepHeightSlider;
    public Slider stepSpeedSlider;

    [Space]
    [SerializeField] private TextMeshProUGUI bodySegmentCountText;
    [SerializeField] private TextMeshProUGUI bodySegmentScaleText;
    [SerializeField] private TextMeshProUGUI legSegmentCountText;
    [SerializeField] private TextMeshProUGUI legSegmentLengthText;
    [SerializeField] private TextMeshProUGUI stepLengthText;
    [SerializeField] private TextMeshProUGUI stepHeightText;
    [SerializeField] private TextMeshProUGUI stepSpeedText;

    private void Start()
    {
        UpdatebodySegmentCountText();
        UpdatebodySegmentScaleText();
        UpdateLegSegmentCountText();
        UpdateLegSegmentLenthText();
        UpdateStepLengthText();
        UpdateStepHeightText();
        UpdateStepSpeedText();
    }

    public void UpdatebodySegmentCountText()
    {
        bodySegmentCountText.text = bodySegmentCountSlider.value.ToString();
    }

    public void UpdatebodySegmentScaleText()
    {
        bodySegmentScaleText.text = bodySegmentScaleSlider.value.ToString("F2");
    }

    public void UpdateLegSegmentCountText()
    {
        legSegmentCountText.text = legSegmentCountSlider.value.ToString();
    }

    public void UpdateLegSegmentLenthText()
    {
        legSegmentLengthText.text = legSegmentLengthSlider.value.ToString("F2");
    }

    public void UpdateStepLengthText()
    {
        stepLengthText.text = stepLengthSlider.value.ToString("F2");
    }
    
    public void UpdateStepHeightText()
    {
        stepHeightText.text = stepHeightSlider.value.ToString("F2");
    }

    public void UpdateStepSpeedText()
    {
        stepSpeedText.text = stepSpeedSlider.value.ToString("F2");
    }

}
