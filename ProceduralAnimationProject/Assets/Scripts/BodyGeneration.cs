using UnityEngine;

public class BodyGeneration : MonoBehaviour
{
    Mesh myMesh;

    [SerializeField] private BodyPoint[] points;

    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        myMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = myMesh;

        myMesh.vertices = new Vector3[]
        {
            Vector3.zero, Vector3.right, Vector3.up
        };
        myMesh.triangles = new int[] {
            0, 2, 1
        };
        myMesh.normals = new Vector3[]{
            Vector3.back, Vector3.back, Vector3.back
        };
    }

    public void CreateTriangle()
    {

    }

    public void CreateShape()
    {
        vertices = new Vector3[] {



        };
    }


}
