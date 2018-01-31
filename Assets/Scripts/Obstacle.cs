using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Platforme {



    void Start()
    {

	}
    //override protected Vector3 Sommet(float angleAjouté, float inclinaisonAjouté, bool sommetDuDessous, bool sommetSuppérieur)
    //{
    //    Debug.Log("sommet obstacle");

    //    return new Vector3(Origine.x + Mathf.Cos(AngleDébut + angleAjouté) * (Rayon + (sommetSuppérieur ? Largeur : 0)),
    //                       Origine.y + Hauteur + inclinaisonAjouté + (sommetDuDessous ? 0:Épaisseur),
    //                       Origine.z + Mathf.Sin(AngleDébut + angleAjouté) * (Rayon + (sommetSuppérieur ? Largeur: 0)));
    //}


    //virtual protected void OnTriggerEnter(Collider other) 
    //{
    //    DataÉtage.Personnage.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //}


    void Update () {
		
	}
}
