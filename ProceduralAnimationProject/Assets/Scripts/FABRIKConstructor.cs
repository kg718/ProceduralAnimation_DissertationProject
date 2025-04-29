using UnityEngine;

public class FABRIKConstructor : IKConstructor
{
    [SerializeField] private InverseKinematics IK;
    [SerializeField] private GameObject segmentPrefab;
    public int segmentCount;
    public int segmentLength;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void UpdateChain()
    {
        if(IK.GetJoints().Count > segmentCount)
        {
            int _iterations = 8 - segmentCount;
            for (int i = _iterations; i > 0; i--)
            {
                //creature.segments[8 - i].gameObject.SetActive(false);
            }
            //activeSegments = Mathf.RoundToInt(panel.bodySegmentCountSlider.value);
        }
        if(IK.GetJoints().Count < segmentCount)
        {

        }
    }
}
