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
    public static event Action OnSequenceExecuted;
    public static event Action<ObjectType> OnKeyCollected;
    public static event Action<ObjectType> OnGoldCollected;
    private bool isMoving=false, canAddDirection=true, dirU, dirD, dirL, dirR, moveMentComplete=false, movingOver=false;
    private Transform movePoint;
    private Vector3 startingPoint, playerMovePosition;
    private List<MoveDirections> movementSequence;
    private int index = 0;

    public void Initialize() 
    {
        movementSequence = new List<MoveDirections>(); 
        movePoint = transform.Find("MovePoint");
        startingPoint = movePoint.position;
        playerMovePosition = startingPoint;
        movePoint.SetParent(null);   
    }

    private void OnEnable() 
    {
        UIController.OnDirectionSelected += BuildSequence;
        UIController.OnSequenceComplete += ExecuteSequence;
        UIController.OnCheckDirections += CheckDirection;
        UIController.OnSequenceReset += ResetSequence;
        BlockManager.OnBlocksExploded += PlayerMovementExecution;
    }

    private void OnDisable() 
    {
        UIController.OnDirectionSelected -= BuildSequence;
        UIController.OnSequenceComplete -= ExecuteSequence;
        UIController.OnCheckDirections -= CheckDirection;
        UIController.OnSequenceReset -= ResetSequence;
        BlockManager.OnBlocksExploded -= PlayerMovementExecution;
    }

    private void BuildSequence(MoveDirections dir)
    {
        movementSequence.Add(dir);
        MoveCurrentPoint(dir);
    }

    private void ResetSequence()
    {
        movementSequence.Clear();
        movePoint.position = startingPoint;
        CheckDirection();
    }

    private void MoveCurrentPoint(MoveDirections dir)
    {
        switch (dir)
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
    }

    private void ExecuteSequence()
    {
        movePoint.position = startingPoint;
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
            IExplodeable explodeable;
            Collider2D collider;
            collider = Physics2D.OverlapCircle(movePoint.position, 1f);
            explodeable = collider.gameObject.GetComponent<IExplodeable>();
            if(explodeable != null) explodeable.ActivateTNT();
            
            yield return new WaitForSeconds(StaticVariables.i.GetGameStats().movementDelay);
        }
        OnSequenceExecuted?.Invoke();
    }

    private void PlayerMovementExecution()
    {
        if(index >= movementSequence.Count) 
        {
            movingOver = true;
            isMoving = false;
            return;
        }
        switch (movementSequence[index])
        {
            case MoveDirections.up:
            playerMovePosition += new Vector3(0f,4f, 0f);
            break;

            case MoveDirections.down:
            playerMovePosition += new Vector3(0f,-4f, 0f);
            break;

            case MoveDirections.left:
            playerMovePosition += new Vector3(-4f,0f, 0f);
            break;

            case MoveDirections.right:
            playerMovePosition += new Vector3(4f,0f, 0f);
            break;

            default: break;
        }
        isMoving = true;
        index++;
    }

    private void Update() 
    {
        if(movingOver)
        {
            FinalCheck();
            movingOver=false;
        }
        if(isMoving)
        {
            transform.position = Vector2.Lerp(transform.position, playerMovePosition, StaticVariables.i.GetGameStats().speed * Time.deltaTime);
            if(Vector2.Distance(transform.position, playerMovePosition) < .05f)
            {
                isMoving = false;
                PlayerMovementExecution();
            }
        }    
    }

    private void FinalCheck()
    {
        //check if player has key, if not, die
        Debug.Log("Game Over");
    }

    public void CheckDirection()
    {
        if(Physics2D.OverlapCircle(new Vector2(movePoint.position.x - 4f, movePoint.position.y), 1f, StaticVariables.i.GetWallLayer()) == null)
        {
            dirL = false;
            Collider2D collider = Physics2D.OverlapCircle(new Vector2(movePoint.position.x - 4f, movePoint.position.y), 1f, StaticVariables.i.GetExitPointLayer());
            if (collider != null)
            {
                dirL = true;
            }
        }
        else dirL = true;
        if(Physics2D.OverlapCircle(new Vector2(movePoint.position.x + 4f, movePoint.position.y), 1f, StaticVariables.i.GetWallLayer()) == null)
        {
            dirR = false;
            Collider2D collider = Physics2D.OverlapCircle(new Vector2(movePoint.position.x + 4f, movePoint.position.y), 1f, StaticVariables.i.GetExitPointLayer());
            if (collider != null)
            {
                dirR = true;
            }
        }
        else dirR = true;
        if(Physics2D.OverlapCircle(new Vector2(movePoint.position.x, movePoint.position.y - 4f), 1f, StaticVariables.i.GetWallLayer()) == null)
        {
            dirD = false;
            Collider2D collider = Physics2D.OverlapCircle(new Vector2(movePoint.position.x, movePoint.position.y - 4f), 1f, StaticVariables.i.GetExitPointLayer());
            if (collider != null)
            {
                dirD = true;
            }
        }
        else dirD = true;
        if(Physics2D.OverlapCircle(new Vector2(movePoint.position.x, movePoint.position.y + 4f), 1f, StaticVariables.i.GetWallLayer()) == null)
        {
            dirU = false;
            Collider2D collider = Physics2D.OverlapCircle(new Vector2(movePoint.position.x, movePoint.position.y + 4f), 1f, StaticVariables.i.GetExitPointLayer());
            if (collider != null)
            {
                dirU = true;
            }
        }
        else dirU = true;

    }
    public bool GetUpDir(){return dirU;}
    public bool GetDownDir(){return dirD;}
    public bool GetLeftDir(){return dirL;}
    public bool GetRightDir(){return dirR;}

    private void OnDrawGizmos() 
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector2(movePoint.position.x - 4f, movePoint.position.y), 1f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(new Vector2(movePoint.position.x + 4f, movePoint.position.y), 1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(movePoint.position.x, movePoint.position.y + 4f), 1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(new Vector2(movePoint.position.x, movePoint.position.y - 4f), 1f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(movePoint.position, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        IObject iObject = other.GetComponent<IObject>();
        if (iObject.GetObjectType() == ObjectType.key)
        {
            Destroy(other.gameObject);
            GameManager.i.GetPointSystem().AdjustPointTotal(GameManager.i.GetGamePoints().keyPoints, true);
            OnKeyCollected?.Invoke(ObjectType.key);
        }
        if (iObject.GetObjectType() == ObjectType.gold)
        {
            Destroy(other.gameObject);
            GameManager.i.GetPointSystem().AdjustPointTotal(GameManager.i.GetGamePoints().goldPoints, true);
            OnGoldCollected?.Invoke(ObjectType.gold);
        }
    }

}
