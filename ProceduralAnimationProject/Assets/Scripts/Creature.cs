using UnityEngine;

public class Creature : MonoBehaviour
{
    public BodySegment[] segments;

    void Start()
    {
        segments[0].SetHead(); // The first segment in the array is the head of the creature
        for (int i = 1; i < segments.Length; i++)
        {
            segments[i].SetPreviousSegment(segments[i - 1]);
        }
    }

    void Update()
    {
        for (int i = 1; i < segments.Length; i++)
        {
            //Maintaining gait symmetry
            if(i % 2 == 0) // segment that steps at the same pace as the head
            {
                segments[i].SetNextLeg(segments[0].GetNextLeg());
            }
            else // segment that steps in alternation to the head
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
