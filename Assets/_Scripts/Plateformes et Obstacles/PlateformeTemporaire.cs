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

    public void Initialisation(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float temps, int typeUptade, float rotation, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        TypeDeUpdate = typeUptade;
        Rotation = rotation;

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

        Positionnement();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        ratio = ((DataÉtage.LARGEUR_PLATEFORME / 2 + DataÉtage.PersonnageGameObject.transform.lossyScale.y) / (DataÉtage.RAYON_TOUR + DataÉtage.LARGEUR_PLATEFORME));

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
        if (wasTouched && pourcentageTemps<=1.6)
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
        //if (Time.time != 0 && deltaTime >= Temps)
        if (pourcentageTemps == 1)
        {
            GetComponent<Rigidbody>().useGravity = true; GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    float ratio;
    void UpdateRappetisse() 
    // devrait disparaitre progressivement selon le temps dès que toucher ou attendre le temps
    // ratio largeur et rayon tour
    {
        if (Time.time <= timeTouched + 1.5*Temps)
        {
            Maths.SetGlobalScale(gameObject.transform, new Vector3(1 - ratio * pourcentageTemps, 1, 1 - ratio * pourcentageTemps));
        }
    }
}
