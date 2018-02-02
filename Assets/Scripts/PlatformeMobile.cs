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

    //    CréationObjet(material);

    //}
    void Start()
    {

    }

    bool touching=false;

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
        transform.Rotate(Vector3.up, Mathf.Sin(Time.time*5)/2);
        if (touching) { DataÉtage.Personnage.transform.RotateAround(Vector3.zero,Vector3.up, Mathf.Sin(Time.time * 5) / 2); }
    }
}
