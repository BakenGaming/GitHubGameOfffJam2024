using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.i.GetIsPaused()) return;

        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null) collectable.Collect();

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameManager.i.GetIsPaused()) return;

        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null) collectable.Collect();

    }
}
