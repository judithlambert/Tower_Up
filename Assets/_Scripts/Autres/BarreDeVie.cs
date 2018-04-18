using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BarreDeVie : MonoBehaviour {
    // créer seulement pour ÉtageBoss

    float NbDeVie { get { return DataÉtage.BossScript.NbDeVie; } }
    float NbDeVieInitial { get { return Boss.NbDeVieInitial; } }

    Vector2 Dimension = new Vector2(250, 25);
    float offset = 1;

    RectTransform Encadré;
    Image Vie;
    void Start() {

        Encadré = GetComponent<Image>().GetComponent<RectTransform>();
        Vie = GetComponentInChildren<Image>();

        Encadré.pivot = new Vector2(0.5f, -1);
        //GetComponent<Image>().transform.position = new Vector2(0, 0);
        Encadré.localPosition = new Vector2(0, 0);
        //Encadré.anchoredPosition.Set(0, 0);
        Encadré.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 5, 10);
        Encadré.sizeDelta = Dimension;

        //Encadré.position = new Vector2(50,10);


        //Vie.GetComponent<RectTransform>().localPosition = new Vector2(offset / 2, 0);

    }
	
	void Update ()
    {

        Vie.GetComponent<RectTransform>().localScale = new Vector2(NbDeVie/NbDeVieInitial, 1);
    }
}
