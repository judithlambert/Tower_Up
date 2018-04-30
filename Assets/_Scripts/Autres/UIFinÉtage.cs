using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class UIFinÉtage : MonoBehaviour
{
    Text tempsTxt, scoreTxt;


    private void Start()
    {
        //GetComponentsInChildren<Image>().Where(x => x.name.Contains("Background")).First().enabled = false;
        DonnéesDeBase();
    }

    public void DonnéesDeBase()
    {
        GetComponentsInChildren<Text>().Where(x => x.name == "Temps").First().text = DataÉtage.UiScript.TempsPassé;
        GetComponentsInChildren<Text>().Where(x => x.name == "Score").First().text = DataÉtage.UiScript.Score;
        //GetComponentsInChildren<Dropdown>().Where(x => x.name.Contains("Difficulté")).First().value = DataÉtage.difficulté;
    }

    public void ProchainÉtage()
    {
        DataÉtage.nouvelÉtage = true;
    }

    public void Recommencer()
    {
        DataÉtage.Recommencer();
        //DataÉtage.nbÉtage -= 1;
        //DataÉtage.nouvelÉtage = true;
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("ScnMenuPrincipal");
    }

    public void FinÉtage()
    {
        Start();
        GetComponentsInChildren<Button>().Where(x => x.name.Contains("Prochain")).First().interactable = true;
    }

    private void OnEnable()
    {
        GetComponentsInChildren<Image>().Where(x => x.name.Contains("Background")).First().enabled = DataÉtage.étageFini;
    }

    //public void Difficulté()
    //{
    //    DataÉtage.difficulté = GetComponentsInChildren<Dropdown>().Where(x => x.name.Contains("Difficulté")).First().value;
    //}
}
