using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public const string CHEMIN_SAVE = "Assets/Resources/SaveFile.txt";
    StreamReader saveReader;

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
        if (save == null) { Debug.Log("il n'y a aucune game de déja commencer"); NewGame(); }
        else { DataÉtage.nbÉtage = int.Parse(save); SceneManager.LoadScene("ScnÉtage"); }
    }

    public void Setting()
    {

    }
}
