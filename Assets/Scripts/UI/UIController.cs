using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action<MoveDirections> OnDirectionSelected;
    public static event Action OnSequenceComplete;
    [SerializeField] private GameObject sequencePanel;
    [SerializeField] private GameObject[] sequenceImages;
    private int sequencePositionIndex=0;
    private float moveInputTime=.5f, moveInputTimer=0f;
    private bool canAddDirection = true, sequenceComplete = false;

    private void OnEnable() 
    {
        ArrowButton.OnArrowClicked += SelectDirection;    
    }

    private void OnDisable() 
    {
        ArrowButton.OnArrowClicked -= SelectDirection;    
    }
    private void Awake() 
    {
        for (int i = 0; i < sequenceImages.Length; i++)
        {
            sequenceImages[i].SetActive(false);
        }    
    }

    public void SelectDirection(MoveDirections dir)
    {
        Debug.Log(dir);
        if(canAddDirection && sequencePositionIndex < sequenceImages.Length)
        {
            canAddDirection = false;
            OnDirectionSelected?.Invoke(dir);
            UpdateSequenceUI(dir);
            moveInputTimer = moveInputTime;
        }
    }

    public void CompleteSequence()
    {
        if(!sequenceComplete) {OnSequenceComplete?.Invoke(); sequenceComplete=true;}
    }

    private void Update() 
    {
        moveInputTimer -= Time.deltaTime;
        if(moveInputTimer <= 0) canAddDirection = true;    
    }

    private void UpdateSequenceUI(MoveDirections dir)
    {
        switch (dir)
            {
                case MoveDirections.up:
                Debug.Log("UP");
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteUp;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.down:
                Debug.Log("DOWN");
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteDown;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.left:
                Debug.Log("LEFT");
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteLeft;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.right:
                Debug.Log("RIGHT");
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteRight;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                default: break;
            }
    }
}

            
