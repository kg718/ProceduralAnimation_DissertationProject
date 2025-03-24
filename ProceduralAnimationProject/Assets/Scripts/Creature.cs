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
        
    }
}
