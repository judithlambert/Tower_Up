﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    GameObject drapeau;

    // position et rotation

    public const string String = "Checkpoint";
    public void Initialisation(float angle, float hauteur) // presque même que FinÉtage
    {
        Vector3 position = transform.position = new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage,
                                                            hauteur + DataÉtage.DELTA_HAUTEUR,
                                                            Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RayonTrajectoirePersonnage); // même chose que plus bas

        transform.rotation = Quaternion.Euler(0, -angle, 0);



        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.AddComponent<MeshFilter>().mesh = cube.GetComponent<MeshFilter>().mesh;
        Destroy(cube);
        gameObject.AddComponent<Rigidbody>().isKinematic = true;
        //gameObject.AddComponent<MeshRenderer>(); // doit etre enlever quand fini
        gameObject.AddComponent<MeshCollider>().convex = true;
        GetComponent<MeshCollider>().isTrigger = true;

        Maths.SetGlobalScale(transform, new Vector3(DataÉtage.LARGEUR_PLATEFORME, DataÉtage.DELTA_HAUTEUR * 2, 1));

        drapeau = new GameObject("Drapeau");
        drapeau.AddComponent<DrapeauAnimé>().Initialisation(new Vector3(Mathf.Cos(Maths.DegréEnRadian(angle)) * DataÉtage.RAYON_TOUR, // même chose que plus haut
                                         hauteur + DataÉtage.DELTA_HAUTEUR / 2,
                                         Mathf.Sin(Maths.DegréEnRadian(angle)) * DataÉtage.RAYON_TOUR),
                                         Materials.Get((int)NomMaterial.CheckPoint));
        drapeau.transform.rotation = Quaternion.Euler(0, -angle, 0);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Personnage"))
        {
            Debug.Log("checkPoint");
            DataÉtage.PersonnageScript.PositionCheckPoint = transform.position;
        }
    }

    public void Destroy()
    {
        Destroy(drapeau);
        Destroy(gameObject);
    }
}