using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pic : MonoBehaviour
{
    [SerializeField] int nbTranches;
    [SerializeField] float rayon;
    [SerializeField] float hauteur;

    Mesh maillage;
    Vector3 origine;
    Vector3[] Sommets;
    float deltaAngle, deltaTexture;
    int nbSommets, nbTriangle;

    private void Awake()
    {
        CalculerDonnéesDeBase();
        GénérerSommets();
        GénérerCoordonnéesDeTexture();
        GénérerListeTriangles();
    }

    void CalculerDonnéesDeBase()
    {
        origine = transform.position;
        maillage = new Mesh();
        GetComponent<MeshFilter>().mesh = maillage;
        maillage.name = "Pic";
        nbSommets = nbTranches + 2;
        nbTriangle = nbTranches;
        deltaAngle = 2 * Mathf.PI / nbTranches;
        deltaTexture = 1f / nbTranches;
    }
    
    void GénérerSommets()
    {
        Sommets = new Vector3[nbSommets];
        Sommets[0] = origine + Vector3.up * hauteur;
        for(int n = 1; n < nbSommets; ++n)
        {
            Sommets[n] = origine + new Vector3(Mathf.Sin(n * -deltaAngle), 0, Mathf.Cos(n * deltaAngle)) * rayon;
        }
        maillage.vertices = Sommets;
    }

    void GénérerCoordonnéesDeTexture()
    {
        Vector2[] CoordonnéesDeTexture = new Vector2[nbSommets];
        CoordonnéesDeTexture[0] = new Vector2(0, 1);
        for(int n = 1; n < nbSommets; ++n)
        {
            CoordonnéesDeTexture[n] = new Vector2(n * deltaTexture, 0);
        }
        maillage.uv = CoordonnéesDeTexture;
    }

    void GénérerListeTriangles()
    {
        int[] Triangles = new int[nbTriangle * 3];
        for(int n = 0; n < nbTriangle; ++n)
        {
            Triangles[n * 3] = n + 1;
            Triangles[n * 3 + 1] = 0;
            Triangles[n * 3 + 2] = n + 2;
        }
        maillage.triangles = Triangles;
        maillage.RecalculateNormals();
    }
}
