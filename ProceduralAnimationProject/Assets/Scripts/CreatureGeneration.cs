using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CreatureGeneration : MonoBehaviour
{
    [SerializeField] private Creature creature;
    [SerializeField] private CreatureEditing editing;
    [SerializeField] List<Material> materials;

    private int mat = 0;
    private int altMat = 0;
    private int segmentCount = 3;
    private float maxBodyScale;
    private float minBodyScale;
    private int largestSegment;

    private float nextSegmentScale = 3f;
    private float totatSegmentScale = 0f;

    private int legSegmentCount = 3;
    private float legSegmentLength = 3f;
    private bool manyLegs;


    void Start()
    {
        GenerateParameters();
        ApplyParametersToCreature();
    }

    void Update()
    {
        
    }

    public void GenerateParameters()
    {
        mat = Random.Range(0, materials.Count);
        altMat = Random.Range(0, materials.Count);
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
            if(i < largestSegment)
            {
                creature.segments[i].EditScale(nextSegmentScale);
                totatSegmentScale += nextSegmentScale;
                nextSegmentScale = Random.Range(nextSegmentScale, maxBodyScale);
            }
            if(i == largestSegment)
            {
                totatSegmentScale += nextSegmentScale;
                creature.segments[i].EditScale(maxBodyScale);
            }
            if (i > largestSegment)
            {
                creature.segments[i].EditScale(nextSegmentScale);
                totatSegmentScale += nextSegmentScale;
                nextSegmentScale = Random.Range(minBodyScale, nextSegmentScale);
            }
        }
    }

    public void ApplyLegEdits()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            if(i == 0 || ((i == segmentCount - 2 && segmentCount % 2 != 0) || (i == segmentCount - 1 && segmentCount % 2 == 0)))
            {
                creature.segments[i].EditLegs(true);
            }
            else if(manyLegs)
            {
                creature.segments[i].EditLegs(true);
            }
            else
            {
                creature.segments[i].EditLegs(false);
            }

            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentCount(legSegmentCount);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentLength(legSegmentLength);
            //Called Twice to Correct Generation errors in the legs
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentCount(legSegmentCount);
            creature.segments[i].GetComponent<BodySegment>().EditLegSegmentLength(legSegmentLength);
            creature.segments[i].GetComponent<BodySegment>().EditStepLength(0.7f);
            if (totatSegmentScale > 55f)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(4f);
            }
            else if(totatSegmentScale > 35f)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(5f);
            }
            else if (totatSegmentScale > 20)
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(7f);
            }
            else
            {
                creature.segments[i].GetComponent<BodySegment>().EditStepSpeed(9f);
            }


        }
    }

    private void ApplyColours()
    {
        for (int i = 0; i < creature.segments.Length; i++)
        {
            creature.segments[i].gameObject.GetComponent<MeshRenderer>().material = materials[mat];
            List<Transform> _leftJoints = creature.segments[i].GetComponent<bodyRotation>().leftIK.GetJoints();
            for (int j = 0; j < _leftJoints.Count; j++)
            {
                _leftJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[mat];
                if (j == _leftJoints.Count - 1)
                {
                    _leftJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[altMat];
                }
            }
            creature.segments[i].GetComponent<bodyRotation>().leftSocket.GetComponent<MeshRenderer>().material = materials[altMat];
            List<Transform> _rightJoints = creature.segments[i].GetComponent<bodyRotation>().rightIK.GetJoints();
            for (int j = 0; j < _rightJoints.Count; j++)
            {
                _rightJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[mat];
                if (j == _rightJoints.Count - 1)
                {
                    _rightJoints[j].GetChild(0).GetComponent<MeshRenderer>().material = materials[altMat];
                }
            }
            creature.segments[i].GetComponent<bodyRotation>().rightSocket.GetComponent<MeshRenderer>().material = materials[altMat];
        }
    }
}
