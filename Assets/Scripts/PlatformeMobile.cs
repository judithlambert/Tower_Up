using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeMobile : Platforme
{

    float Temps, Distance, Vitesse;
    //public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float rayon, float rugosité, float vitesse, float distance, Material material)
    //{
    //    AngleDébut = angleDébut;
    //    Amplitude = amplitude; ;
    //    Largeur = largeur;
    //    Épaisseur = épaisseur;
    //    Hauteur = hauteur;
    //    Rayon = rayon;
    //    Rugosité = rugosité;
    
    //    Vitesse = vitesse;
    //    Distance = distance;

    //    Maillage = new Mesh
    //    {
    //        name = "Plateforme"
    //    };

    //    CalculerDonnéesDeBase();
    //    GénérerTriangles();

    //    gameObject.AddComponent<MeshFilter>().mesh = Maillage;
    //    //gameObject.AddComponent<Rigidbody>().useGravity = false;
    //    gameObject.AddComponent<MeshRenderer>().material = material;
    //    gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
    //    GetComponent<MeshCollider>().convex = true;

    //}
    void Start()
    {

    }

    void Update()
    {
        transform.position = new Vector3(Origine.x * Mathf.Sin(Time.time),
                                         Origine.y,
                                         Origine.z * Mathf.Sin(Time.time));
    }
}
