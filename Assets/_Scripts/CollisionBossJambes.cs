using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossJambes : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        DataÉtage.BossScript.Dommage(5);
    }
}
