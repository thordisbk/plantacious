using UnityEngine;
using System.Collections;

public class PerlinAnimateMesh : MonoBehaviour
{
    public float perlinScale = 4.56f;
    public float waveSpeed = 1f;
    public float waveHeight = 2f;

    private Mesh mesh;
    private Renderer rendererComp;


    void Update()
    {
        AnimateMesh();
    }

    void AnimateMesh()
    {
        if (!mesh)
            mesh = GetComponent<MeshFilter>().mesh;
            rendererComp = GetComponent<Renderer>();

        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            float pX = (vertices[i].x * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed);
            float pY = (vertices[i].y * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed);
            float pZ = (vertices[i].y * perlinScale) + (Time.timeSinceLevelLoad * waveSpeed);

            //vertices[i].x = (Mathf.PerlinNoise(pZ, pY)) * waveHeight;
            //vertices[i].y = (Mathf.PerlinNoise(pX, pY)) * waveHeight;
            vertices[i].z = (Mathf.PerlinNoise(pX, pZ)) * waveHeight;
        }

        mesh.vertices = vertices;
    }
}