using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sin
{
    public static float Point, PMobile;
}

public class Temps : MonoBehaviour
{
    void Update()
    {
        Sin.Point = Mathf.Sin(Time.time * Point.VITESSE_TRANSLATION);
        Sin.PMobile = Mathf.Cos(Time.time * PlateformeMobile.Vitesse / 10);
    }
}
