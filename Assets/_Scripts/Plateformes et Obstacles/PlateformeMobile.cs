using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMobile : Plateforme
{
    public const string String = Plateforme.String + "Mobile";

    bool touching=false;

    float Temps, Distance, Vitesse, rotation;
    // distance est une amplitude, en degré
    int TypeMouvement;
   enum Mouvement { horizontal, vertical, diagonal}
    public void Initialisation(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float vitesse, float distance, int mouvement, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude; ;
        Largeur = largeur;
        Épaisseur = épaisseur;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Rayon = rayon;

        Vitesse = vitesse;
        Distance = distance;
        TypeMouvement = mouvement;
        

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
    { 
        rotation = -(((Distance - Amplitude) / 2) * (Mathf.Sin(Time.time * Vitesse / 10)) + ((Distance - Amplitude) / 2)) - transform.rotation.eulerAngles.y;
        switch (TypeMouvement)
        {
            case 0:
                transform.Rotate(Vector3.up, rotation);
                break;
            case 1:
                transform.Translate(new Vector3(0, rotation, 0)); //maybbeee idk
                break;
            case 2:
                transform.Rotate(Vector3.up, rotation); // not this
                break;
        }
        
        if (touching) { DataÉtage.Personnage.transform.RotateAround(Vector3.zero,Vector3.up,rotation); }
    }
}
