using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeMobile : Plateforme
{
    public const string String = Plateforme.String + "Mobile";

    bool touching=false;

    static public float Temps, Distance, Vitesse, rotation, translation;
    // distance est une amplitude, en degré
    int TypeMouvement;
    enum Mouvement { horizontal, vertical, diagonal}
    public void Initialisation(float angleDébut, float amplitude, float hauteur, float inclinaison, float épaisseur, float largeur, float rayon, float vitesse, float distance, int mouvement, float rotation, Material material)
    {
        AngleDébut = angleDébut;
        Amplitude = amplitude;
        Hauteur = hauteur;
        Inclinaison = inclinaison;
        Épaisseur = épaisseur;
        Largeur = largeur; 
        Rayon = rayon;
        Vitesse = vitesse;
        Distance = mouvement == 0 ? distance : distance * DataÉtage.DELTA_HAUTEUR;
        Rotation = rotation;
        TypeMouvement = mouvement;


        CréationObject(material);

        //temporaire
        translation = Vitesse / 20;
        //Distance = Distance * DataÉtage.DELTA_HAUTEUR;

        Positionnement();
        CréationPointCollision();
        GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

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
        switch (TypeMouvement)
        {
            case 0:
                rotation = -(((Distance - Amplitude) / 2) * Sin.PMobile + ((Distance - Amplitude) / 2)) - transform.rotation.eulerAngles.y - AngleDébut;
                transform.Rotate(Vector3.up, rotation);
                if (touching) { DataÉtage.PersonnageGameObject.transform.RotateAround(Vector3.zero, Vector3.up, rotation); }
                break;
            case 1:
                transform.position = new Vector3(0, Distance / 2 * Sin.PMobile + Hauteur - Distance / 2, 0);
                //transform.Translate(new Vector3(0, translation, 0)); //maybbeee idk
                //if (transform.position.y >= Distance + Hauteur || transform.position.y <= Hauteur)
                //{ translation = -translation; Debug.Log("translation changed"); }
                break;
            case 2:
                transform.Rotate(Vector3.up, rotation); // not this
                break;
        }

        
    }
}
