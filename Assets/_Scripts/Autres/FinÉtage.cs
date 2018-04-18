using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinÉtage : CheckPoint
{
    public const string String = "FinÉtage";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Personnage"))
        {
            Debug.Log("fin étage");
            DataÉtage.étageFini = true;
            Destroy(Drapeau);
        }
    }
}
