using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MoveDirections
{
    up, down, left, right
}
public class GridMovement : MonoBehaviour
{
    public static event Action<MoveDirections> OnSequenceSelectionAdded;
    private bool isMoving, canAddDirection=true;
    private Transform movePoint;
    private List<MoveDirections> movementSequence;
    private float movementDelay = 1f;

    public void Initialize() 
    {
        movementSequence = new List<MoveDirections>(); 
        movePoint = transform.Find("MovePoint");
        movePoint.SetParent(null);   
    }

    private void OnEnable() 
    {
        UIController.OnDirectionSelected += BuildSequence;
        UIController.OnSequenceComplete += ExecuteSequence;
    }

    private void OnDisable() 
    {
        UIController.OnDirectionSelected -= BuildSequence;
        UIController.OnSequenceComplete -= ExecuteSequence;
    }

    private void BuildSequence(MoveDirections dir)
    {
        movementSequence.Add(dir);
    }

    private void ExecuteSequence()
    {
        StartCoroutine("StartMovement");
    }

    IEnumerator StartMovement()
    {
        for (int i = 0; i < movementSequence.Count; i++)
        {
            switch (movementSequence[i])
            {
                case MoveDirections.up:
                movePoint.position += new Vector3(0f,4f, 0f);
                break;

                case MoveDirections.down:
                movePoint.position += new Vector3(0f,-4f, 0f);
                break;

                case MoveDirections.left:
                movePoint.position += new Vector3(-4f,0f, 0f);
                break;

                case MoveDirections.right:
                movePoint.position += new Vector3(4f,0f, 0f);
                break;

                default: break;
            }
            yield return new WaitForSeconds(movementDelay);
        }
    }

}
