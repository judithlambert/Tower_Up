using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BarreDeVie : MonoBehaviour
{
    // créer seulement pour ÉtageBoss
    Boss BossScript;

    float NbDeVie { get { return BossScript.NbDeVie; } }
    float NbDeVieInitial { get { return Boss.NbDeVieInitial; } }

    Vector2 Dimension = new Vector2(250, 25);
    float offset = 1;

    RectTransform Encadré, Vie;
    void Start()
    {
        BossScript = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();

        Encadré = GetComponent<Image>().GetComponent<RectTransform>();
        Vie = GameObject.Find("Vie").GetComponent<Image>().GetComponent<RectTransform>();

        Encadré.pivot = new Vector2(0.5f, -1);
        Encadré.localPosition = new Vector2(0, 0);
        Encadré.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 5, 10);
        Encadré.sizeDelta = Dimension; // vie doit etre plus petit que encadré
        Vie.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 1, 250);

        //Vie.localPosition = new Vector2(offset / 2, 0);

    }

    void Update ()
    {

        Vie.GetComponent<RectTransform>().localScale = new Vector2(NbDeVie/NbDeVieInitial, 1);
    }
}
