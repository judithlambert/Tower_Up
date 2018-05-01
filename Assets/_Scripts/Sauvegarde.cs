using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class Sauvegarde
{
    public const string CHEMIN_SAVE = "Assets/Resources/Saves/SaveFile.txt";

    public static int Load()
    {
        FileStream file = File.Open(CHEMIN_SAVE, FileMode.Open, FileAccess.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();
        string nb="0";
        if (file.Length != 0)
            nb = bf.Deserialize(file).ToString();
        file.Close();
        return int.Parse(nb);
    }

    public static void Save()
    {
        if (File.Exists(CHEMIN_SAVE))
        {
            FileStream file = File.Open(CHEMIN_SAVE, FileMode.Open, FileAccess.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, DataÉtage.nbÉtage);
            file.Close();
        }
    }
}
