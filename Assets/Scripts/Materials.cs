using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    public Material[] materials;

    private void Awake()
    {
        materials = Resources.LoadAll("Materials") as Material[];
    }
}
