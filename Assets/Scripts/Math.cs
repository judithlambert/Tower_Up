using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Maths
{
    const float FACTEUR_CONVERSION_DEGRÉ_RADIAN = 2 * Mathf.PI / 360;
    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
    static public float DegréEnRadian(float angleDegré)
    {
        return angleDegré * FACTEUR_CONVERSION_DEGRÉ_RADIAN;
    }
}
