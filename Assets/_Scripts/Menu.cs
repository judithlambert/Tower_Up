using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Menu : MonoBehaviour
{
    int étage = 1;

    GameObject MessageErreur;
    Text MssgErreurTxt;

    void Start()
    {
        //Sauvegarde.Initialisation();
        MessageErreur = GameObject.FindGameObjectWithTag("MessageErreur");
        MssgErreurTxt = MessageErreur.GetComponentInChildren<Text>();
        MessageErreur.SetActive(false);
    }

    public void NewGame()
    {
        DataÉtage.nbÉtage = 1;
        SceneManager.LoadScene("ScnÉtage");
    }

    public void ResumeGame()
    {
        //string save = Sauvegarde.saveReader.ReadLine();
        //if (save == null) //if (File.Exists(path))
        //{
        //    MessageErreur.SetActive(true);
        //    MssgErreurTxt.text = "il n'y a aucune partie de déja commencer";
        //    StartCoroutine(WaitUtilOK());
        //}
        //else { DataÉtage.nbÉtage = int.Parse(save); SceneManager.LoadScene("ScnÉtage"); }
    }

    public void Niveaux()
    {
        étage = GetComponentsInChildren<Dropdown>().Where(x => x.name.Contains("Niveau")).First().value + 1;
    }

    public void Jouer()
    {
        DataÉtage.nbÉtage = étage;
        SceneManager.LoadScene("ScnÉtage");
    }

    public void Difficulté()
    {
        DataÉtage.difficulté = GetComponentsInChildren<Dropdown>().Where(x => x.name.Contains("Difficulté")).First().value;
    }

    public void Quitter()
    {
        Application.Quit();
    }

    bool OK = false;

    public void BtnOk()
    {
        OK = true;
    }

    IEnumerator WaitUtilOK()
    {
        yield return new WaitUntil(() => OK);
        NewGame();
    }

}
