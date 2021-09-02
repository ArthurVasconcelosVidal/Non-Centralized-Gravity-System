using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;

public class MeshTrianglePoints : MonoBehaviour{

    [SerializeField] MeshFilter meshFilter;
    [SerializeField] GameObject referenceObject = null;
    //List<Vector3> triangleCenters = new List<Vector3>();
    List<Vector3> lineCenters = new List<Vector3>();
    //KDTree triangleCentersTree = new KDTree();
    KDTree lineCentersTree = new KDTree();
    KDQuery query = new KDQuery();

    void Awake() {
        //Config
        if (referenceObject == null)
            referenceObject = this.gameObject;

        //Geting Values
        List<Vector3> meshVertices = new List<Vector3>();
        meshFilter.sharedMesh.GetVertices(meshVertices);
        var triangleIndices = meshFilter.sharedMesh.GetTriangles(0);

        //New method in test
        for (int i = 0; i < triangleIndices.Length; i += 6){
            Vector3 lineSegmentPoint = (meshVertices[triangleIndices[i]] + meshVertices[triangleIndices[i + 1]]) / 2;
            lineSegmentPoint = Vector3.Scale(lineSegmentPoint, referenceObject.transform.localScale); // scaling based on object
            lineSegmentPoint = Quaternion.Euler(referenceObject.transform.rotation.eulerAngles) * lineSegmentPoint; // rotating based on object
            lineSegmentPoint += transform.position;
            lineCenters.Add(lineSegmentPoint);
        }

        lineCentersTree = BuildingKDTree();
    }

    /* Old method, works perfectly but the new method have half of the points
     * 
    // Start is called before the first frame update
    void Awake() {
        //Config
        if (referenceObject == null)
            referenceObject = this.gameObject;

        //Geting Values
        List<Vector3> meshVertices = new List<Vector3>();
        meshFilter.sharedMesh.GetVertices(meshVertices);
        var triangleIndices = meshFilter.sharedMesh.GetTriangles(0);

        //Getting point triangles and fixing positon,rotation and scale
        for (int i = 0; i < triangleIndices.Length; i += 3){
            Vector3 triangleCenter = Mathfs.Triangle.Centroid(meshVertices[triangleIndices[i]], meshVertices[triangleIndices[i + 1]], meshVertices[triangleIndices[i + 2]]); // calculating the centroid point
            triangleCenter = Vector3.Scale(triangleCenter, referenceObject.transform.localScale); // scaling based on object
            triangleCenter = Quaternion.Euler(referenceObject.transform.rotation.eulerAngles) * triangleCenter; // rotating based on object
            triangleCenter += transform.position;
            triangleCenters.Add(triangleCenter);
        }

        triangleCentersTree = BuildingKDTree();
        //MeshPointsPositionUpdate(); // offset
    }
    */

    /*Update
        void Update()
    {
        // foreach (var triangle in lineCenters){
        //      Debug.DrawRay(triangle, triangle.normalized*5, Color.red);
        //}

        foreach (var line in lineCenters)
        {
            Debug.DrawRay(line, line.normalized * 10, Color.blue);
        }
    }
    */


    KDTree BuildingKDTree(){
    int maxPointsPerLeafNode = 32; //Padrão
    //KDTree KDTree = new KDTree(triangleCenters.ToArray(), maxPointsPerLeafNode);
    KDTree KDTree = new KDTree(lineCenters.ToArray(), maxPointsPerLeafNode);
    return KDTree;
    }

    /*//Not used
     * 
    void MeshPointsPositionUpdate() {
        for (int i = 0; i < triangleCentersTree.Count; i++){
            triangleCentersTree.Points[i] = transform.position + triangleCenters[i];
            Debug.DrawRay(triangleCentersTree.Points[i], triangleCentersTree.Points[i].normalized *5, Color.red);
        }
        triangleCentersTree.Rebuild();
    }
    */

    public Vector3 NearestTriangleCenter(Vector3 objectPos){
        List<int> results = new List<int>();
        //query.ClosestPoint(triangleCentersTree, objectPos, results);
        //return triangleCenters[results[0]];
        query.ClosestPoint(lineCentersTree, objectPos, results);
        return lineCenters[results[0]];
    }
}
