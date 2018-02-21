using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeTemporaire : Plateforme
{
    public const string String = Plateforme.String + "Temporaire";


    bool wasTouched =false;
    float timeTouched=0;
    bool isInitialisé = false;
    Color color;
    float Temps;

    public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float inclinaison, float rayon, float rugosité, float temps, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
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
        GetComponent<MeshCollider>().convex = true; // nécéssaire?
        GetComponent<Rigidbody>().isKinematic = true;

        color = GetComponent<Renderer>().material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CollisionDessus(collision) && !wasTouched) { wasTouched = true; timeTouched = Time.time; }
    }

    // faire que dans l'instantiation, on peux choisir le type de disparition de chq plateforme
    void FixedUpdate()
    {
        UpdateRappetisse();
    }


    void UpdateTombe()
    {
        float deltaTime = Time.time - timeTouched;
        float pourcentageTemps = deltaTime / Temps;

        if (wasTouched && Time.time != 0 && deltaTime >= Temps)
        {
            GetComponent<Rigidbody>().useGravity = true; GetComponent<Rigidbody>().isKinematic = false;
        }
        if (wasTouched)
        {
            float R = pourcentageTemps * (1 - color.r) + color.r;
            float B = color.b - pourcentageTemps * (color.b);
            float G = color.g - pourcentageTemps * (color.g);
            GetComponent<Renderer>().material.color = new Color(R, G, B);
        }
    }

    void UpdateRappetisse()
    {
        float deltaTime = Time.time - timeTouched;
        float pourcentageTemps = deltaTime / Temps;

        if (wasTouched && Time.time <= timeTouched + Temps)
        {

            Maths.SetGlobalScale(gameObject.transform, new Vector3(1- pourcentageTemps, 1, 1 - pourcentageTemps));

            float R = pourcentageTemps * (1 - color.r) + color.r;
            float B = color.b - pourcentageTemps * (color.b);
            float G = color.g - pourcentageTemps * (color.g);
            GetComponent<Renderer>().material.color = new Color(R, G, B);
        }
    }
}
