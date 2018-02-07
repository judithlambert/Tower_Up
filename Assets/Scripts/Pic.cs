using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pic : Obstacle
{
    int nbTranches=20;
    float Rayon=2; // le rayon du pic devrait etre selon la largeur de la plateforme
    float Hauteur=5;
    Vector3 Position;

    Mesh Maillage;
    Vector3 origine; // origine vas etre le point du milieu de la base au niveau du sol
    Vector3[] Sommets;
    float deltaAngle, deltaTexture;
    int nbSommets, nbTriangle;

    public void Initialisation(float positionX, float élévationEnY, float positionZ, float rayon, float hauteur, Material material)
    {
        Position= origine = new Vector3(positionX*DataÉtage.RayonTrajectoirePersonnage, élévationEnY, positionZ * DataÉtage.RayonTrajectoirePersonnage);
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
        nbSommets = nbTranches + 2;
        nbTriangle = nbTranches;
        deltaAngle = 2 * Mathf.PI / nbTranches;
        deltaTexture = 1f / nbTranches;
    }
    
    void GénérerSommets()
    {
        Sommets = new Vector3[nbSommets];
        Sommets[0] = Vector3.up * Hauteur;
        for(int n = 1; n < nbSommets; ++n)
        {
            Sommets[n] = new Vector3(Mathf.Sin(n * -deltaAngle), 0, Mathf.Cos(n * deltaAngle)) * Rayon;
        }
        Maillage.vertices = Sommets;
    }

    void GénérerCoordonnéesDeTexture()
    {
        Vector2[] CoordonnéesDeTexture = new Vector2[nbSommets];
        CoordonnéesDeTexture[0] = new Vector2(0, 1);
        for(int n = 1; n < nbSommets; ++n)
        {
            CoordonnéesDeTexture[n] = new Vector2(n * deltaTexture, 0);
        }
        Maillage.uv = CoordonnéesDeTexture;
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
        Maillage.triangles = Triangles;
        Maillage.RecalculateNormals();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.ToString().Contains("Personnage")) { DataÉtage.Personnage.GetComponent<Personnage>().Die(); }
    }
}
