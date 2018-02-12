using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Maths
{
    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }

    const float FACTEUR_CONVERSION_DEGRÉ_RADIAN = 2 * Mathf.PI / 360;
    static public float DegréEnRadian(float angleDegré)
    {
        return angleDegré * FACTEUR_CONVERSION_DEGRÉ_RADIAN;
    }

    static public Vector3 VecteurPolaire(Vector3 vecteur)
    {
        return new Vector3(Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2)),
                           Mathf.Acos(vecteur.z / Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2))),
                           Mathf.Atan(vecteur.y / vecteur.x));
    }
}
