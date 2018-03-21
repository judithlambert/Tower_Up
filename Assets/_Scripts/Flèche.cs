using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Flèche : MonoBehaviour
{
    public const string String = "Fleche";
    const string PATH = "Assets/Sprites/";
    const float SCALE = 0.2f;
    Texture2D tex;
    public void Initialisation(float angle, float hauteur, float rayon, float rotation)
    {
        tex = AssetDatabase.LoadAssetAtPath(PATH + "fleche.png", typeof(Texture2D)) as Texture2D;
        gameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create( tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        Maths.SetGlobalScale(transform, new Vector3(SCALE, SCALE, SCALE));
        transform.position = new Vector3(rayon, hauteur + DataÉtage.DELTA_HAUTEUR/2, 0);
        GetComponent<SpriteRenderer>().transform.Rotate(transform.up, -90);
        GetComponent<SpriteRenderer>().transform.Rotate(transform.right, rotation);
        transform.RotateAround(DataÉtage.Origine, Vector3.down, angle);
    }
}
