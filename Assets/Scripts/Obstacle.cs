using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle :  MonoBehaviour {



    void Start()
    {

	}
    public void OnTriggerEnter(Collider other) // faire que seulement personnage trigger
    {
        DataÉtage.Personnage.GetComponent<Personnage>().Die();
    }

    void Update () {
		
	}
}
