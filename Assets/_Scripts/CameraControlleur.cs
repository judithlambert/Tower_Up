using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlleur : MonoBehaviour
{
    //void Start()
    //{
    //    transform.position = new Vector3(transform.position.x, 0, DataÉtage.PersonnageGameObject.transform.position.z) * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage);
    //    transform.LookAt(DataÉtage.PersonnageGameObject.transform);
    //}

    void Update ()
    {
        transform.position = new Vector3(DataÉtage.PersonnageGameObject.transform.position.x * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage),
                                         (DataÉtage.PersonnageGameObject.transform.position.y + DataÉtage.RayonCamera / 10),
                                         DataÉtage.PersonnageGameObject.transform.position.z * (DataÉtage.RayonCamera / DataÉtage.RayonTrajectoirePersonnage));
        transform.LookAt(new Vector3(DataÉtage.Origine.x, DataÉtage.nbÉtage < 5 ? DataÉtage.PersonnageGameObject.transform.position.y : GameObject.FindGameObjectWithTag("Boss").transform.position.y + 4, DataÉtage.Origine.z));
    }
}
