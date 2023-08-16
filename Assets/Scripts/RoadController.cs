using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Splines;
using UnityEngine.Scripting;
using Unity.Mathematics;

[ExecuteInEditMode()]
public class RoadController : MonoBehaviour
{

    [SerializeField]
    private SplineContainer road;

    [SerializeField]
    [Range(0f, 5f)]
    private float roadWidth;

    [SerializeField]
    private int splineIndex;
    [SerializeField]
    [Range(0f, 1f)]
    private float gameTime; 

    float3 position; 
    float3 forward;
    float3 upVector;
    float3 edge1;
    float3 edge2;

    [SerializeField]
    private int totalVertices;
    List<Vector3> edge1Vertices;
    List<Vector3> edge2Vertices;

    public static MeshFilter roadMeshFilter;
    


    private void Awake() {
        roadMeshFilter = (MeshFilter)gameObject.GetComponent("MeshFilter");
    }

    private void Update() { 
        GetVertices();
    }

    private void OnEnable() {
        Spline.Changed += OnSplineChanged;
        GetVertices();
    }

    private void OnDisable() {
        Spline.Changed -= OnSplineChanged;
    }

    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3) {
        //Rebuild();
        BuildRoad();
    }

    private void OnDrawGizmos() {
        for(int i = 0; i < totalVertices; i++) {
            Vector3 drawEdge1 = edge1Vertices[i];
            Vector3 drawEdge2 = edge2Vertices[i];

            Handles.DotHandleCap(0, drawEdge1, Quaternion.identity, 0.05f, EventType.Repaint);
            Handles.DotHandleCap(0, drawEdge2, Quaternion.identity, 0.05f, EventType.Repaint);
        }
        
    }

    private void GetVertices() {
        edge1Vertices = new List<Vector3>();
        edge2Vertices = new List<Vector3>();

        float step = 1f / (float)totalVertices;
        for(int i = 0; i < totalVertices; i++) {
            float t = step * i;
            GetSplineWidth(t, out Vector3 edge1, out  Vector3 edge2);
            edge1Vertices.Add(edge1);
            edge2Vertices.Add(edge2);
            

        }

    }

    private void GetSplineWidth(float t, out Vector3 edge1, out Vector3 edge2) {
        road.Evaluate(t, out position, out forward, out upVector);
        //Debug.Log(position);

       float3 right = Vector3.Cross(forward, upVector).normalized;
       edge1 = position + (right * roadWidth);
       edge2 = position + (-right * roadWidth);
       //Debug.Log("Edge1 Position: " + edge1);
       //Debug.Log("Edge2 Position: " + edge2);

    }

    private void BuildRoad() {
        Mesh roadMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        int offset = 0;

        for(int i = 1; i <= totalVertices; i++) {
            Vector3 point1 = edge1Vertices[i-1] - transform.position;
            Vector3 point2 = edge2Vertices[i-1] - transform.position;
            Vector3 point3;
            Vector3 point4;

            if(i == totalVertices) {
                point3 = edge1Vertices[0] - transform.position;
                point4 = edge2Vertices[0] - transform.position;
            } else {
                point3 = edge1Vertices[i] - transform.position;
                point4 = edge2Vertices[i] - transform.position;
            }
            offset = 4 * (i-1);
            int triangle1 = offset + 0;
            int triangle2 = offset + 2;
            int triangle3 = offset + 3;

            int triangle4 = offset + 3;
            int triangle5 = offset + 1;
            int triangle6 = offset + 0;

            vertices.AddRange(new List<Vector3> { point1, point2, point3, point4 });
            triangles.AddRange(new List<int> { triangle1, triangle2, triangle3, triangle4, triangle5, triangle6 });
        }

        roadMesh.SetVertices(vertices);
        roadMesh.SetTriangles(triangles, 0);
        roadMesh.RecalculateNormals(); 
        roadMeshFilter.mesh = roadMesh; 

    }


}
