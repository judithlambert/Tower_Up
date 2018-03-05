using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pic : Obstacle
{
    public const string String = "Pic";

    int nbTranches = 20;
    float Rayon = 2;
    float Hauteur = 5;

    Vector3 Position;

    Mesh Maillage;
    Vector3[] Sommets;
    float deltaAngle, deltaTexture;
    int nbSommets;

    //[SerializeField] Material matériel;
    //private void Awake()
    //{
    //    Initialisation(0, 0, 2, 6, matériel);
    //}

    public void Initialisation(float angle, float hauteur, float élévationEnY, float rayon, Material material)
    {
        Position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage, élévationEnY, Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage);
        Rayon = rayon;
        Hauteur = hauteur;

        Maillage = new Mesh
        {
            name = "Pic"
        };

        transform.position = Position;

        CalculerDonnéesDeBase();
        GénérerSommets();
        GénérerCoordonnéesDeTexture();
        GénérerListeTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        GetComponent<MeshCollider>().convex = true;
        //GetComponent<MeshCollider>().isTrigger = true;
    }

    void CalculerDonnéesDeBase()
    {
        nbSommets = nbTranches * 2 + 1;
        deltaAngle = 2 * Mathf.PI / nbTranches;
        deltaTexture = 1f / nbTranches;
    }
    
    void GénérerSommets()
    {
        Sommets = new Vector3[nbSommets];  
        for(int n = 0; n < nbSommets - 2; n += 2)
        {
            Sommets[n] = new Vector3(Mathf.Sin(n / 2 * -deltaAngle), 0, Mathf.Cos(n / 2 * deltaAngle)) * Rayon;
            Sommets[n + 1] = Vector3.up * Hauteur;
        }
        Sommets[Sommets.Length - 1] = Sommets[0];
        Maillage.vertices = Sommets;
    }

    void GénérerCoordonnéesDeTexture()
    {
        Vector2[] CoordonnéesDeTexture = new Vector2[nbSommets];
        for(int n = 0; n < nbSommets - 2; n += 2)
        {
            CoordonnéesDeTexture[n] = new Vector2(n / 2 * deltaTexture, 0);
            CoordonnéesDeTexture[n + 1] = new Vector2(n / 2 * deltaTexture + deltaTexture / 2, 1);
        }
        CoordonnéesDeTexture[CoordonnéesDeTexture.Length - 1] = new Vector2(1, 0);
        Maillage.uv = CoordonnéesDeTexture;
    }

    void GénérerListeTriangles()
    {
        int[] Triangles = new int[nbTranches * 3];
        for(int n = 0; n < Triangles.Length - 2; n += 3)
        {
            Triangles[n] = (int)(n * 2 / 3f);
            Triangles[n + 1] = (int)(n * 2 / 3f) + 1;
            Triangles[n + 2] = (int)(n * 2 / 3f) + 2;
        }
        Maillage.triangles = Triangles;
        Maillage.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.ToString().Contains("Personnage")) { DataÉtage.Personnage.GetComponent<Personnage>().Die(); }
    }
}
