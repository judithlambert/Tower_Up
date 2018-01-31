using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformeTemporaire : Platforme
{
    bool isTouched=false;
    float timeTouched=0;
    bool isInitialisé = false;
    Color color;
    float Temps;

    public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float rayon, float rugosité, float temps, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Rayon = rayon;
        Rugosité = rugosité;

        Temps = temps;
        isInitialisé = true;

        //CréationObject(material);

        Maillage = new Mesh
        {
            name = "Plateforme"
        };

        CalculerDonnéesDeBase();
        GénérerTriangles();

        gameObject.AddComponent<MeshFilter>().mesh = Maillage;
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.AddComponent<MeshRenderer>().material = material;
        gameObject.AddComponent<MeshCollider>().sharedMesh = Maillage;
        GetComponent<MeshCollider>().convex = true;
        GetComponent<Rigidbody>().isKinematic = true;

        color = GetComponent<Renderer>().material.color;
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision Enter");
        if (CollisionDessus(collision) && !isTouched) { Debug.Log("collision Dessus"); isTouched = true; timeTouched = Time.time; }
    }

    void FixedUpdate()
    {
        float deltaTime = Time.time - timeTouched;
        if (isTouched && Time.time!=0 && deltaTime >= Temps && isInitialisé) {
            GetComponent<Rigidbody>().useGravity = true; GetComponent<Rigidbody>().isKinematic = false;
            isTouched = false; Debug.Log("tombe");
           
        }
        if(isTouched)
        {
            float R = (deltaTime / Temps) * (1 -color.r) + color.r;
            float B = color.b - (deltaTime / Temps) * (color.b);
            float G = color.g - (deltaTime / Temps) * (color.g);
            GetComponent<Renderer>().material.color = new Color(R, G, B);
        }

    }
}
