using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinÉtage : CheckPoint
{
    public const string String = "FinEtage";

    public void Initialisation2(float angle, float hauteur) // presque même que FinÉtage
    {
        Initialisation(angle, hauteur);
        Drapeau.GetComponent<MeshRenderer>().material = Materials.Get((int)NomMaterial.FinÉtage); // devrait etre fait dans drapeur animé
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Personnage"))
        {
            Debug.Log("fin étage");
            DataÉtage.étageFini = true;
            Destroy(Drapeau);
        }
    }
    public void OnDestroy()
    {
        Destroy(Drapeau);
    }
}
