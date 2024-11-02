using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Ghost : MonoBehaviour
{
    private float fadeSpeed = 5f;
    private SpriteRenderer sr;
    private Color spriteColor;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        spriteColor = UtilsClass.GetColorFromString("FFFFFF");    
    }

    private void Update() 
    {
        transform.localScale -= Vector3.one * 2f * Time.deltaTime;
        spriteColor.a -= fadeSpeed * Time.deltaTime;
        sr.color = spriteColor;
        if (spriteColor.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
