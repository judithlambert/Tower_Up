using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMobile : Plateforme
{
    public const string String = Plateforme.String + "Mobile";

    bool touching=false;

    float Temps, Distance, Vitesse, rotation;
    // distance est une amplitude, en degré

    public void Initialisation(float angleDébut, float amplitude, float largeur, float épaisseur, float hauteur, float inclinaison, float rayon, float rugosité, float vitesse, float distance, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;
        Rugosité = rugosité;

        Vitesse = vitesse;
        Distance = distance;

        CréationObject(material);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Personnage") && CollisionDessus(collision))
        touching = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Contains("Personnage"))
            touching = false;
    }

    void Update()
    { // à tester pour distance et vitesse sont réellement respecter
        // bug avec le personnage qui tourne sur lui-même
        rotation = -(((Distance - Amplitude) / 2) * Vitesse * (Mathf.Sin(Time.time)) + ((Distance - Amplitude) / 2)); // Vitesse a pt corigé
        transform.rotation = Quaternion.Euler(new Vector3(0,rotation));
        if (touching) { DataÉtage.Personnage.transform.RotateAround(Vector3.zero,Vector3.up,rotation); }
    }
}
