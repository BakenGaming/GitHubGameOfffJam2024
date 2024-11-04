using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour, IExplodeable
{
    public static event Action<Block> OnTNTActivated;
    [SerializeField] private GameObject TNTIcon;
    [SerializeField] private Color normalColor, cheatColor, finalCheatColor;
    private float cheatTimer;

    private void OnEnable() 
    {
        UIController.OnHideBlocks += HideBlocks;    
    }

    private void OnDisable() 
    {
        UIController.OnHideBlocks -= HideBlocks;        
    }
    private void Start() 
    {
        TNTIcon.SetActive(false);
    }
    public void ActivateTNT()
    {
        TNTIcon.SetActive(true);
        OnTNTActivated?.Invoke(this);        
    }

    public void Explode()
    {
        Instantiate(GameAssets.i.pfTNTExplodeParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //======= Cheats ========

    private void HideBlocks(float _time, bool _isFinalCheat)
    {
        cheatTimer = _time;
        if(_isFinalCheat)
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().color = finalCheatColor;
        }
        else StartCoroutine("TemporaryBlockHideCheat");
    }

    IEnumerator TemporaryBlockHideCheat()
    {
        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = cheatColor;
        yield return new WaitForSecondsRealtime(cheatTimer);
        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = normalColor;
    }
}
