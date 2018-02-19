using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
//#endif 

// on ne peut pas build avec using UnityEditor

public static class Materials
{
    static public List<Material> ListMaterials;

    static public void Init()
    {
        ListMaterials = new List<Material>()
        {
            //faire pour chaque materials(si fait d'un coup, impossible de savoir quel material est quel)
            AssetDatabase.LoadAssetAtPath("Assets/Materials/MaterialPlatforme.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath("Assets/Materials/MaterialPersonnage.mat", typeof(Material)) as Material,
            AssetDatabase.LoadAssetAtPath("Assets/Materials/MaterialPic.mat", typeof(Material)) as Material
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
    Pic
}
