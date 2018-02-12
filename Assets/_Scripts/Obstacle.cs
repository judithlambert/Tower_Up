using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle :  MonoBehaviour // pt inutile
{
    // pas sure si cel marche vrm pour les sous classe
    public void OnTriggerEnter(Collider other) // faire que seulement personnage trigger
    {
        DataÉtage.Personnage.GetComponent<Personnage>().Die();
    }
}
