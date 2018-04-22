using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossFesses : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        DataÉtage.BossScript.Dommage(30);
    }
}
