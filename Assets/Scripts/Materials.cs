using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public static class Materials
{
    static public List<Material> ListMaterials;

    static public void init()
    {
        ListMaterials = new List<Material>();
        // faire pour chaque materials (si fait d'un coup, impossible de savoir quel material est quel)
        ListMaterials.Add(AssetDatabase.LoadAssetAtPath("Assets/Materials/MaterialPlatforme.mat", typeof(Material)) as Material);
    }

    static public Material Get(int n) 
    {
        return ListMaterials[n]; 
    }
}

public enum NomMaterial
{
    Plateforme, 
    Personnage
}
