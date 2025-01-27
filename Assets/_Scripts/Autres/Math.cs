﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Maths
{
    const int CERCLE_EN_DEGRÉ = 360;

    // il est ici puisqu'il ne doit pas être reset
    static public bool messageProjDéjaAfficher;


    public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
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

    //static public Quaternion Vector3àQuaternion(Vector3 vector)
    //{
    //    return Quaternion.Euler(vector);
    //}

    public static bool EstDansLeRange(float n, float borne1, float borne2)
    {
        float min, max;
        if(borne1>borne2) { max = borne1; min = borne2; }
        else              { max = borne2; min = borne1; }
        return (n >= min && n <= max);
    }
    public static bool EstDansLeRange(float n, float borne1, float borne2, float incertitude)
    {
        float min, max;
        if (borne1 > borne2) { max = borne1; min = borne2; }
        else { max = borne2; min = borne1; }
        return (n >= min-incertitude && n <= max+incertitude);
    }


    public static string Vecteur2String(Vector3 vector, string nom)
    {
        return nom + " : (" + vector.x + ", " + vector.y + ", " + vector.z + ")";
    }

    public static T[,] CopieProfonde<T> (ref T[,] t)
    {
        T[,] c = new T[t.GetLength(0),t.GetLength(1)];
        for(int i = 0; i < t.GetLength(0); ++i)
        {
            for (int j = 0; j < t.GetLength(1); ++j)
            {
                c[i, j] = t[i, j];
            }
        }
        return c;
    }

}
