using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoeursDeVie : MonoBehaviour
{
    const int ESPACEMENENT = 5;
    [SerializeField] Image Coeur;
    List<Image> ListeCoeurs;
    float largeur;

    private void Start()
    {
        ListeCoeurs = new List<Image>();
        largeur = Coeur.rectTransform.rect.width;
        for(int i = 1; i <= DataÉtage.PersonnageScript.Vie; ++i)
        {
            Image nouveauCoeur = Instantiate(Coeur, Vector2.zero, Quaternion.identity, transform);
            nouveauCoeur.transform.localPosition = new Vector2((largeur + ESPACEMENENT) * (i - 1), 0);
            ListeCoeurs.Add(nouveauCoeur);
        }
    }

    private void Update()
    {
        if (DataÉtage.PersonnageScript.updateVie)
        {
            foreach(var c in ListeCoeurs)
            {
                c.gameObject.SetActive(ListeCoeurs.IndexOf(c) < DataÉtage.PersonnageScript.Vie);
            }
        }
    }
}
