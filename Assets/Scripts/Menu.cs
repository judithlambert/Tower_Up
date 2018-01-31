using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public const string CHEMIN_SAVE = "Assets/Resources/SaveFile.txt";

    StreamReader saveReader;


    // Use this for initialization
    void Start () {
        saveReader = new StreamReader(CHEMIN_SAVE);

    }


    void NewGame()
    {
        DataÉtage.nbÉtage = 0;
        SceneManager.LoadScene("ScnÉtage");
    }
    void ResumeGame()
    {
        DataÉtage.nbÉtage = int.Parse(saveReader.ReadLine());
        SceneManager.LoadScene("ScnÉtage");
    }
   

    // Update is called once per frame
    void Update () {
		
	}


}
