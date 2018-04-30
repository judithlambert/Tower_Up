using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public static class Sauvegarde
{
    public const string CHEMIN_SAVE = "Assets/Saves/SaveFile.txt";

    public static StreamReader saveReader;
    public static StreamWriter saveWriter;

    static FileStream save = new FileStream(CHEMIN_SAVE, FileMode.Open, FileAccess.ReadWrite);

    public static int Load()
    {
        saveReader = new StreamReader(save);
        return saveReader.Read();
    }
    public static void Save()
    {
        saveWriter = new StreamWriter(save);
        saveWriter.Write(DataÉtage.nbÉtage);
        saveWriter.Close();
    }



}
