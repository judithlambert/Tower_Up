using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossJambes : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<Boss>().Dommage(5, collision);
    }
}
