using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action<float,bool> OnHideBlocks;
    public static event Action<MoveDirections> OnDirectionSelected;
    public static event Action OnSequenceComplete;
    public static event Action OnSequenceReset;
    public static event Action OnCheckDirections;
    [SerializeField] private GameObject sequencePanel;
    [SerializeField] private GameObject cheatPrompt;
    [SerializeField] private GameObject[] sequenceImages;
    [SerializeField] private GameObject upArrow, downArrow, leftArrow, rightArrow;
    private int sequencePositionIndex=0, cheatLevel=0;
    private float moveInputTime=.5f, moveInputTimer=0f;
    private bool canAddDirection = true, sequenceComplete = false, canCheat = true, isCheating = false;

    private void OnEnable() 
    {
        ArrowButton.OnArrowClicked += SelectDirection;  
        BlockManager.OnCheatComplete += EndCheat;  
    }

    private void OnDisable() 
    {
        ArrowButton.OnArrowClicked -= SelectDirection;    
        BlockManager.OnCheatComplete += EndCheat;  
    }
    private void Awake() 
    {
        for (int i = 0; i < sequenceImages.Length; i++)
        {
            sequenceImages[i].SetActive(false);
        }    
    }
    private void Start() 
    {
        OnCheckDirections?.Invoke(); 
        EvaluateArrows();   
    }

    public void SelectDirection(MoveDirections dir)
    {
        if(isCheating) return;
        if(canAddDirection && sequencePositionIndex < sequenceImages.Length)
        {
            canAddDirection = false;
            OnDirectionSelected?.Invoke(dir);
            UpdateSequenceUI(dir);
            moveInputTimer = moveInputTime;
        }
        OnCheckDirections?.Invoke();
        EvaluateArrows();
    }

    public void CompleteSequence()
    {
        if(!sequenceComplete) {OnSequenceComplete?.Invoke(); sequenceComplete=true;}
    }

    private void Update() 
    {
        EvaluateArrows();
        moveInputTimer -= Time.deltaTime;
        if(moveInputTimer <= 0) canAddDirection = true;    
    }

    private void UpdateSequenceUI(MoveDirections dir)
    {
        switch (dir)
            {
                case MoveDirections.up:
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteUp;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.down:
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteDown;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.left:
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteLeft;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                case MoveDirections.right:
                sequenceImages[sequencePositionIndex].GetComponent<Image>().sprite = GameAssets.i.arrowSpriteRight;
                sequenceImages[sequencePositionIndex].SetActive(true);
                sequencePositionIndex++;
                break;

                default: break;
            }
    }

    public void ResetSequence()
    {
        if(sequenceComplete) return;
        for (int i = 0; i < sequenceImages.Length; i++)
        {
            sequenceImages[i].GetComponent<Image>().sprite = null;
            sequenceImages[i].SetActive(false);
        }
        sequencePositionIndex = 0;
        OnSequenceReset?.Invoke();
    }

    public void Cheat()
    {
        if(!canCheat) return;
        isCheating = true;
        cheatPrompt.SetActive(true);
    }

    public void CancelCheat()
    {
        cheatPrompt.SetActive(false);
        isCheating = false;
    }
    public void CheatForReal()
    {
        cheatPrompt.SetActive(false);
        isCheating = true;
        switch (cheatLevel)
        {
            case 0:
            Debug.Log("Cheat Level 1");
            OnHideBlocks?.Invoke(3f, false);
            break;

            case 1:
            Debug.Log("Cheat Level 2");
            OnHideBlocks?.Invoke(7f, false);
            break;

            case 2:
            Debug.Log("Cheat Level 3");
            OnHideBlocks?.Invoke(0f, true);
            break;

            default: break;
        }
        cheatLevel++;
        if (cheatLevel > 2) canCheat = false;
    }

    private void EndCheat()
    {
        isCheating = false;
    }

    private void EvaluateArrows()
    {
        if(GameManager.i.GetMovementScript().GetUpDir()) upArrow.SetActive(true);
        else upArrow.SetActive(false);
        if(GameManager.i.GetMovementScript().GetDownDir()) downArrow.SetActive(true);
        else downArrow.SetActive(false);
        if(GameManager.i.GetMovementScript().GetLeftDir()) leftArrow.SetActive(true);
        else leftArrow.SetActive(false);
        if(GameManager.i.GetMovementScript().GetRightDir()) rightArrow.SetActive(true);
        else rightArrow.SetActive(false);
    }
}

            
