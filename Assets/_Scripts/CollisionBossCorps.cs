﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBossCorps : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<Boss>().Dommage(2, collision);
    }
}
