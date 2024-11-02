using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
 public static OnPlayerDieDelegate OnPlayerDie;
 public delegate void OnPlayerDieDelegate(HazardType type);

 private void OnTriggerEnter2D(Collider2D trigger) 
 {
     if(trigger.tag == "Hazard")
     {
        OnPlayerDie.Invoke(trigger.gameObject.GetComponent<HazardProperties>().GetHazardType());
        Instantiate(GameAssets.i.playerDeathParticles, transform.position, Quaternion.identity);
     }
 }
}
