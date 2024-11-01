using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 10f;
    private Collider2D[] colliders;
    private GameObject player;

    private void Start()
    {
        player = GameManager.i.GetPlayerGO();
    }


    private void Update()
    {
        if (GameManager.i.GetIsPaused()) return;
        else
        {
            colliders = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

            foreach (Collider2D collider in colliders)
            {
                ICollectable collectable = collider.GetComponent<ICollectable>();
                if (collectable != null) collectable.SetTarget(player.transform.position);
            }
        }

    }
}
