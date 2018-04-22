using System.Collections.Generic;
using UnityEngine;

public static class Materials
{
    const string PATH = "Materials/Material";
    static public List<Material> ListMaterials;

    static public void Init()
    {
        ListMaterials = new List<Material>();
        ListMaterials.Add(Resources.Load<Material>(PATH + "Platforme"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "Personnage"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "Pic"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "Tour"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "FinÉtage"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "Point"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "Multiplicateur"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "CheckPoint"));
        ListMaterials.Add(Resources.Load<Material>(PATH + "CheckPointDéclanché"));

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
    CheckPointDéclanché
}
