using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class Sauvegarde
{
    static FileStream save;
    public const string CHEMIN_SAVE = "Assets/Resources/SaveFile.txt";

    public static StreamReader saveReader;
    public static StreamWriter saveWriter;

    public static void Initialisation()
    {
        save = new FileStream(CHEMIN_SAVE, FileMode.Open);
        saveWriter = new StreamWriter(CHEMIN_SAVE);
        saveReader = new StreamReader(CHEMIN_SAVE);
        saveReader = new StreamReader(save);
        saveWriter.Write("test");
    }

    //public static void Save()
    //{
    //    saveWriter.Write(DataÉtage.nbÉtage);
    //}


    //using (FileStream fs = File.Create(path))
    //{

    //}

    //using (FileStream fs = File.OpenRead(path))
    //{

    //}
}
