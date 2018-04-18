using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BarreDeVie : MonoBehaviour {
    // créer seulement pour ÉtageBoss

    float NbDeVie { get { return DataÉtage.BossScript.NbDeVie; } }
    float NbDeVieInitial { get { return Boss.NbDeVieInitial; } }

    Vector2 Dimension = new Vector2(100,10);
    float offset = 1;

    [SerializeField] Image Encadré, Vie;
    void Start() {
    
        //Encadré.transform.SetGlobalScale((Ratio + new Vector2(0.1f, 0.1f)) * Dimension);
        //Vie.transform.SetGlobalScale(Ratio * Dimension);

        Encadré.GetComponent<RectTransform>().sizeDelta = Dimension;

        //Vie.GetComponent<RectTransform>().localPosition = new Vector2(offset/2,0);

    }
	
	void Update ()
    {

        Vie.GetComponent<RectTransform>().localScale = new Vector2(NbDeVie/NbDeVieInitial, 1);
    }
}
