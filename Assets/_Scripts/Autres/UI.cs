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

    public string Score { get { return "SCORE : " + ((int)(Points * (1 + Multiplicateur / 100) /tempsPassé * 10)).ToString(); } }

    float tempsPassé = 0;
    public string TempsPassé { get { return ((int)(tempsPassé / 60)).ToString() + ":" + ((int)(tempsPassé % 60)).ToString("00") + ":" + tempsPassé.ToString().Split(trim).Last().Substring(0, 2); } }

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
        tempsPassé += Time.deltaTime;
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
        tempsTxt.text = TempsPassé;
        scoreTxt.text = Score;
    }

    public void Réinitialiser()
    {
        tempsPassé = 0;
        Points = 0;
        Multiplicateur = 0;
    }
}
