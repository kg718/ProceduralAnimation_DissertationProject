using System.Collections.Generic;
using UnityEngine;

public class CreatureGeneration : MonoBehaviour
{
    [SerializeField] private Creature creature;
    [SerializeField] private CreatureEditing editing;
    [SerializeField] List<Material> materials;

    private int mat = 0;
    private int altMat = 0;
    private bool alternateColours;
    private int segmentCount = 3;
    private float maxBodyScale;
    private float minBodyScale;
    private int largestSegment;

    private float nextSegmentScale = 3f;
    private float totatSegmentScale = 0f;

    private int legSegmentCount = 3;
    private float legSegmentLength = 3f;
    private bool manyLegs;
    private bool thirdLegs;


    void Start()
    {
        GenerateParameters();
        ApplyParametersToCreature();
    }

    public void GenerateParameters()
    {
        mat = Random.Range(0, materials.Count);
        altMat = Random.Range(0, materials.Count);
        if (Random.Range(0, 2) == 1)
        {
            alternateColours = true;
        }
        else
        {
            alternateColours = false;
        }
        segmentCount = Random.Range(3, 11);
        largestSegment = Random.Range(0, segmentCount);
        maxBodyScale = Random.Range(3f, 9f);
        minBodyScale = Random.Range(3f, maxBodyScale);
        nextSegmentScale = minBodyScale;

        legSegmentCount = Random.Range(3, 5);
        legSegmentLength = Random.Range(1f, 5f);
        if(Random.Range(0, 10) < 3)
        {
            manyLegs = false;
        }
        else
        {
            manyLegs = true;
        }
        if (Random.Range(0, 10) < 4)
        {
            thirdLegs = true;
        }
        else
        {
            thirdLegs = false;
        }

        totatSegmentScale = 0f;
    }

    public void ApplyParametersToCreature()
    {
        ApplyBodySize();
        ApplyLegEdits();
        ApplyColours();
    }

    public void ApplyBodySize()
    {
        editing.UpdateBodySegmentCount(segmentCount);
        for (int i = 0; i < creature.segments.Length; i++)
        {
            if(i < largestSegment) // Segments in front of the largest Segment
            {
                creature.segments[i].EditScale(nextSegmentScale);
                totatSegmentScale += nextSegmentScale;
                nextSegmentScale = Random.Range(nextSegmentScale, maxBodyScale); // Segments get larger
            }
            if(i == largestSegment)
            {
                totatSegmentScale += nextSegmentScale;
                creature.segments[i].EditScale(maxBodyScale);
            }
            if (i > largestSegment) // Segments behind the largest Segment
            {
                creature.segments[i].EditScale(nextSegmentScale);
                totatSegmentScale += nextSegmentScale;
                nextSegmentScale = Random.Range(minBodyScale, nextSegmentScale); // Segments get smaller
            }
        }
    }

    public void ApplyLegEdits()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            //Determines if a segment should have legs
            if(i == 0 || ((i == segmentCount - 2 && segmentCount % 2 != 0) || (i == segmentCount - 1 && segmentCount % 2 == 0))) //Ensures symetrical gait is maintained
            {
                //Creatures should always have legs at the front and back to make it look like it can support itself
                creature.segments[i].EditLegs(true);
                if(thirdLegs)
                {
                    creature.segments[i].EditExtraLeg(true);
                }
                else
                {
                    creature.segments[i].EditExtraLeg(false);
                }
            }
            else if(manyLegs)
            {
                creature.segments[i].EditLegs(true);
                if (thirdLegs)
                {
                    creature.segments[i].EditExtraLeg(true);
                }
                else
                {
                    creature.segments[i].EditExtraLeg(false);
                }
            }
            else
            {
                creature.segments[i].EditLegs(false);
                creature.segments[i].EditExtraLeg(false);
            }

            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentCount(legSegmentCount);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentLength(legSegmentLength);
            //Called Twice to Correct Generation errors in the legs
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentCount(legSegmentCount);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentLength(legSegmentLength);
            creature.segments[i].GetComponent<BodySegment>().EditStepLength(0.7f);
            //Adjust the speed of step animation based on the total size of the creature
            if (totatSegmentScale > 55f)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(4f); //slow
            }
            else if(totatSegmentScale > 35f)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(5f); //average
            }
            else if (totatSegmentScale > 20)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(7f); //fast
            }
            else
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(9f); //very fast
            }


        }
    }

    private void ApplyColours()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            int _segmentMat = mat;
            int _altSegmentMat = altMat;
            if(alternateColours && i % 2 != 0)
            {
                _segmentMat = altMat;
                _altSegmentMat = mat;
            }

            creature.segments[i].gameObject.GetComponent<MeshRenderer>().material = materials[_segmentMat];
            //Left Leg
            List<Transform> _leftJoints = creature.segments[i].GetComponent<bodyRotation>().leftIK.GetJoints();
            for (int j = 0; j < _leftJoints.Count; j++)
            {
                _leftJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_segmentMat];
                if (j == _leftJoints.Count - 1)
                {
                    _leftJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_altSegmentMat];
                }
            }
            creature.segments[i].GetComponent<bodyRotation>().leftSocket.GetComponent<MeshRenderer>().material = materials[_altSegmentMat];
            //Right Leg
            List<Transform> _rightJoints = creature.segments[i].GetComponent<bodyRotation>().rightIK.GetJoints();
            for (int j = 0; j < _rightJoints.Count; j++)
            {
                _rightJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_segmentMat];
                if (j == _rightJoints.Count - 1)
                {
                    _rightJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_altSegmentMat];
                }
            }
            creature.segments[i].GetComponent<bodyRotation>().rightSocket.GetComponent<MeshRenderer>().material = materials[_altSegmentMat];
            //Extra Leg
            if(thirdLegs)
            {
                List<Transform> _extraJoints = creature.segments[i].GetComponent<bodyRotation>().extraIK.GetJoints();
                for (int j = 0; j < _extraJoints.Count; j++)
                {
                    _extraJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_segmentMat];
                    if (j == _extraJoints.Count - 1)
                    {
                        _extraJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[_altSegmentMat];
                    }
                }
            }
        }
    }
}
