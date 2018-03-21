using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class Menu : MonoBehaviour {

    public const string CHEMIN_SAVE = "Assets/Ressources/SaveFile.txt";

    StreamReader saveReader;
    int étage = 1;

    void Start ()
    {
        saveReader = new StreamReader(CHEMIN_SAVE);
    }

    public void NewGame()
    {
        DataÉtage.nbÉtage = 1;
        SceneManager.LoadScene("ScnÉtage");
    }

    public void ResumeGame()
    {
        string save = saveReader.ReadLine();
        if (save == null) { Debug.Log("il n'y a aucune partie de déja commencer"); NewGame(); }
        else { DataÉtage.nbÉtage = int.Parse(save); SceneManager.LoadScene("ScnÉtage"); }
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
}
