using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
//#endif 

// on ne peut pas build avec using UnityEditor

public static class Materials
{
    const string PATH = "Assets/Materials/Material";
    static public List<Material> ListMaterials;

    static public void Init()
    {
        ListMaterials = new List<Material>()
        {
            //faire pour chaque materials(si fait d'un coup, impossible de savoir quel material est quel)
            AssetDatabase.LoadAssetAtPath(PATH + "Platforme.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "Personnage.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "Pic.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "Tour.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "FinÉtage.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "Point.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "Multiplicateur.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath(PATH + "CheckPoint.mat", typeof(Material)) as Material
        };
    }

    static public Material Get(int n) 
    {
        return ListMaterials[n]; 
    }
}

public enum NomMaterial
{
    Plateforme, 
    Personnage,
    Pic,
    Tour, 
    FinÉtage,
    Point,
    Multiplicateur,
    CheckPoint,
}
