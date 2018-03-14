using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine;

public class UI : MonoBehaviour
{
    Text tempsTxt, pointsEtMultiplicateurTxt, multiplicateurTxt, scoreTxt, vieTxt;
    bool pointsUpdate, multiplicateurUpdate;
    char[] trim = new char[] { '.' };

    int points = 0;
    public int Points { get { return points; } set { points = value; pointsUpdate = true; } }

    float multiplicateur = 0;
    public float Multiplicateur { get { return multiplicateur; } set { multiplicateur = value; multiplicateurUpdate = true; } }

    void Start ()
    {
        tempsTxt = GetComponentsInChildren<Text>().Where(x => x.name == "Temps").First();
        pointsEtMultiplicateurTxt = GetComponentsInChildren<Text>().Where(x => x.name == "Points et Multiplicateur").First();
        scoreTxt = GetComponentsInChildren<Text>().Where(x => x.name == "Score").First();
        vieTxt = GetComponentsInChildren<Text>().Where(x => x.name == "Vie").First();

        pointsUpdate = multiplicateurUpdate = true;
    }
	
	void Update ()
    {       
        if (pointsUpdate || multiplicateurUpdate)
        {
            pointsEtMultiplicateurTxt.text = Points.ToString() + " pts" + " + " + Multiplicateur.ToString() + " %";
            pointsUpdate = false;
            multiplicateurUpdate = false;
        }
        if (DataÉtage.PersonnageScript.updateVie)
        {
            vieTxt.text = "nb de vies: " + DataÉtage.PersonnageScript.Vie;
            DataÉtage.PersonnageScript.updateVie = false;
        }
        tempsTxt.text = ((int)(Time.time / 60)).ToString() + ":" + ((int)(Time.time % 60)).ToString("00") + ":" + Time.time.ToString().Split(trim).Last().Substring(0, 2);
        scoreTxt.text = "SCORE : " + ((int)(Points * (1 + Multiplicateur / 100) / Time.time * 10)).ToString();
    }
}
