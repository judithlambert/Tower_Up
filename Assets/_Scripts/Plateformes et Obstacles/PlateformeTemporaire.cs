using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTemporaire : Plateforme
{
    public const string String = Plateforme.String + "Temporaire";
    
    enum Update
    {
        rappetisse, 
        tombe
    }

    bool wasTouched =false;
    float timeTouched=0;
    bool isInitialisé = false;
    Color color;
    float Temps;
    int TypeDeUpdate;

    public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float inclinaison, float rayon, float rugosité, float temps, Material material, int typeUptade)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        Rugosité = rugosité;
        TypeDeUpdate = typeUptade;

        Temps = temps;

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
        GetComponent<MeshCollider>().convex = true; // nécéssaire?
        GetComponent<Rigidbody>().isKinematic = true;


        color = GetComponent<Renderer>().material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision plateforme temporaire");
        if (CollisionDessus(collision) && !wasTouched) { wasTouched = true; timeTouched = Time.time; Debug.Log("collision plateforme temporaire réussis");}
    }

    float deltaTime, pourcentageTemps;
    void FixedUpdate()
    {
        deltaTime = Time.time - timeTouched;
        pourcentageTemps = deltaTime / Temps;
        if (wasTouched)
        {
            switch (TypeDeUpdate)
            {
                case (int)Update.rappetisse:
                    UpdateRappetisse();
                    break;
                case (int)Update.tombe:
                    UpdateTombe();
                    break;
            }
            float R = pourcentageTemps * (1 - color.r) + color.r;
            float B = color.b - pourcentageTemps * (color.b);
            float G = color.g - pourcentageTemps * (color.g);
            GetComponent<Renderer>().material.color = new Color(R, G, B);
        }
    }
    void UpdateTombe()
    {
        if (Time.time != 0 && deltaTime >= Temps)
        {
            GetComponent<Rigidbody>().useGravity = true; GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    void UpdateRappetisse() 
    // devrait disparaitre progressivement selon le temps dès que toucher ou attendre le temps
    // ratio largeur et rayon tour
    {
        if (Time.time <= timeTouched + Temps)
        {
            Maths.SetGlobalScale(gameObject.transform, new Vector3(1- pourcentageTemps, 1, 1 - pourcentageTemps));
        }
    }
}
