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
    static public float RadianEnDegré(float angleRadian)
    {
        return angleRadian * (1/FACTEUR_CONVERSION_DEGRÉ_RADIAN);
    }
    /// <summary>x,y,z = rayon,longitude,latitude</summary>
    static public Vector3 VecteurCartésienÀPolaire(Vector3 vecteur)
    {
        return new Vector3(Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2)),
                           Mathf.Acos(vecteur.z / Mathf.Sqrt(Mathf.Pow(vecteur.x, 2) + Mathf.Pow(vecteur.y, 2) + Mathf.Pow(vecteur.z, 2))),
                           Mathf.Atan(vecteur.y / vecteur.x));  //Mathf.Atan2()
    }

    /// <summary>retourne anlge en radian</summary>
    public static float PositionXYàAngle(float x, float y)
    {
        //return 2 * Mathf.Atan(y / (x + Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)))) + Mathf.PI;
        float angle= Mathf.Atan2(y, x);
        if (angle < 0)
            angle += 2 * Mathf.PI;
        return angle;
    }


    /// <returns>angle en degrée</returns>
    static public float ArcDeCercleÀAngle(float longueur, float rayon)
    {
        return longueur / rayon;
    }
    static public float AngleÀArcDeCercle(float angle, float rayon)
    {
        return angle * rayon;
    }

    static public Quaternion Vector3àQuaternion(Vector3 vector)
    {
        return Quaternion.Euler(vector);
    }

}
