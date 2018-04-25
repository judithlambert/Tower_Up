using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossFesses : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<Boss>().Dommage(30);
    }
}
