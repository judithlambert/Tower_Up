using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine;

public class UI : MonoBehaviour
{
    Text temps;
    Text points;
    Personnage personnage;
    char[] trim = new char[] { '.' };

	void Start ()
    {
        temps = GetComponentsInChildren<Text>().Where(x => x.name == "Temps").First();
        points = GetComponentsInChildren<Text>().Where(x => x.name == "Points").First();
    }
	
	void Update ()
    {
        temps.text = ((int)(Time.time / 60)).ToString() + ":" + ((int)(Time.time % 60)).ToString("00") + ":" + Time.time.ToString().Split(trim).Last().Substring(0, 2);
        if (DataÉtage.PersonnageScript.PointsUpdate)
        {
            points.text = DataÉtage.PersonnageScript.Points.ToString();
        }
    }
}
