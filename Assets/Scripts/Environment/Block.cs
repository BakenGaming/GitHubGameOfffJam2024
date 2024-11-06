using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour, IExplodeable
{
    public static event Action<Block> OnTNTActivated;
    public static event Action OnTNTExploded;
    [SerializeField] private GameObject TNTIcon;
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
        GameManager.i.UpdateDynamiteCount(false);
        OnTNTExploded?.Invoke();
        Destroy(gameObject);
    }


}
