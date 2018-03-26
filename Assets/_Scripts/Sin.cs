﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sin
{
    public static float Points()
    {
        return Mathf.Sin(UI.tempsPassé * Point.VITESSE_TRANSLATION);
    }
    public static float PMobiles(float vitesse)
    {
        return Mathf.Sin(UI.tempsPassé * vitesse / 10);
    }
}