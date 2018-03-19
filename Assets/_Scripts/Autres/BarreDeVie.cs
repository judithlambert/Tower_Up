using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreDeVie : MonoBehaviour
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
            Image nouveauCoeur = Instantiate(Coeur, Vector3.zero, Quaternion.identity, transform);
            nouveauCoeur.transform.position = new Vector3(nouveauCoeur.GetComponentInParent<Transform>().position.x + (largeur + ESPACEMENENT) * (i - 1), nouveauCoeur.transform.position.y);
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
