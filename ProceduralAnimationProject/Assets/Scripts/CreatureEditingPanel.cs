using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureEditingPanel : MonoBehaviour
{
    public Slider bodySegmentCountSlider;
    public Toggle hasLegsToggle;

    [Space]
    [SerializeField] private TextMeshProUGUI bodySegmentCountText;

    public void UpdatebodySegmentCountText()
    {
        bodySegmentCountText.text = bodySegmentCountSlider.value.ToString();
    }

}
