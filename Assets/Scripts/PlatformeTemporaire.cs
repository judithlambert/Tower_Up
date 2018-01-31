using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeTemporaire : Platforme
{

    float Temps;

    void Initialisation(float angleDébut, float amplitude, float rayon, float largeur, float épaisseur, float hauteur, float rugosité, float temps, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Rayon = rayon;
        Rugosité = rugosité;

    Temps = temps;

        Maillage = new Mesh
        {
            name = "Plateforme"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        //gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        GetComponent<MeshCollider>().convex = true;

    }

    bool isTouched;
    float timeTouched;
    private void OnTriggerEnter(Collider other)
    {
        if (timeTouched == 0) { timeTouched = Time.time; }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time-timeTouched==Temps) { Destroy(gameObject); }
    }
}
