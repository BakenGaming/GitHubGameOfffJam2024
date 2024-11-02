using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        Debug.Log("Collected");
        Destroy(gameObject);
    }
}
