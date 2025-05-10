using UnityEngine;

public class Creature : MonoBehaviour
{
    public BodySegment[] segments;

    void Start()
    {
        segments[0].SetHead();
        for (int i = 1; i < segments.Length; i++)
        {
            segments[i].SetPreviousSegment(segments[i - 1]);
        }
    }

    void Update()
    {
        for (int i = 1; i < segments.Length; i++)
        {
            if(i % 2 == 0)
            {
                segments[i].SetNextLeg(segments[0].GetNextLeg());
            }
            else
            {
                if (segments[0].GetNextLeg() == BodySegment.leg.LEFT)
                {
                    segments[i].SetNextLeg(BodySegment.leg.RIGHT);
                }
                if (segments[0].GetNextLeg() == BodySegment.leg.RIGHT)
                {
                    segments[i].SetNextLeg(BodySegment.leg.LEFT);
                }
            }
        }
    }
}
