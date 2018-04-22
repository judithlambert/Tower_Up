using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossCorps : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        DataÉtage.BossScript.Dommage(100);
    }
}
