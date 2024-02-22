using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Start()
    {
        // Create a new mesh
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        // Define the vertices of the cube
        vertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f, -0.5f, -0.5f),
            new Vector3( 0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f,  0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f, -0.5f,  0.5f),
            new Vector3( 0.5f,  0.5f,  0.5f),
            new Vector3(-0.5f,  0.5f,  0.5f)
        };

        // Define the triangles of the cube
        triangles = new int[]
        {
            0, 2, 1, // Front
            0, 3, 2,
            1, 6, 5, // Right
            1, 2, 6,
            5, 7, 4, // Back
            5, 6, 7,
            4, 3, 0, // Left
            4, 7, 6,
            4, 1, 5, // Top
            4, 0, 1,
            3, 6, 2, // Bottom
            3, 7, 6
        };

        // Assign the vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Set initial position, rotation, and scale
        transform.position = new Vector3(0f, 2f, 0f); // Change the position as needed
        transform.rotation = Quaternion.Euler(new Vector3(45f, 30f, 15f)); // Change the rotation as needed
        transform.localScale = new Vector3(2f, 1f, 3f); // Change the scale as needed
    }
}
