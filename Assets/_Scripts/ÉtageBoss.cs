using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉtageBoss : MonoBehaviour
{
    float deltaTemps;

    void Update()
    {
        deltaTemps += Time.deltaTime;
        if (deltaTemps >= 2)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Projectile"), new Vector3(0, 6, 0), Quaternion.identity);
            deltaTemps = 0;
        }
    }
}
