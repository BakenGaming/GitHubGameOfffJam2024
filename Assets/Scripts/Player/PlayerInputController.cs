using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public static OnPlayerDashingDelegate OnPlayerDashing;
    public delegate void OnPlayerDashingDelegate(float timer);

    private static PlayerInputController _i;
    public static PlayerInputController i { get { return _i; } }
    [Header("SETUP")]
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private BoxCollider2D playerCollider;
    private GameObject flashLightMask;
    private InputController playerInput;
    private InputAction move, jump, dash, flashLight, interact;
    private Vector2 moveInput;
    private RaycastHit2D wallDetect;
    private Color rayColorL, rayColorR;
    private Camera mainCam;

    private float jumpBufferCount, jumpBufferLength = .5f, hangTimeCounter, hangTime = .2f,
        dashCD=1, dashTimer=0, originalGravity, wallJumpingDirection;
    private bool facingRight=true, isDashing=false, flashLightActive=false, isWallSliding=false, canWallJump,
        flashLightInactive;
    private PlayerStats playerStats;

    private void Awake() 
    {
        _i = this;
    }

    public void Initialize()
    {
        mainCam = Camera.main;
        playerStats = StaticVariables.i.GetPlayerStats();
                
        playerInput = new InputController();

        move = playerInput.PlayerControls.Move;
        move.Enable();

        /*jump = playerInput.PlayerControls.Jump;
        jump.performed += HandleJump;
        jump.Enable();

        dash = playerInput.PlayerControls.Dash;
        dash.performed += HandleDash;
        dash.Enable();

        interact = playerInput.PlayerControls.Interact;
        interact.performed += HandleInteract;
        interact.Enable();

        flashLight = playerInput.PlayerControls.FlashLight;
        flashLight.started += HandleFlashLight;
        flashLight.Enable();

        flashLight = playerInput.PlayerControls.FlashLight;
        flashLight.canceled += HandleFlashLight;
        flashLight.Enable();*/

        flashLightMask = transform.Find("FlashLight").gameObject;
        DeactivateFlashLight();
        DamageHandler.OnPlayerDie += HandleDeath;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        dash.Disable();
        flashLight.Disable();

        DamageHandler.OnPlayerDie -= HandleDeath;
    }

    private void HandleDash(InputAction.CallbackContext context)
    {
        if(flashLightActive) return;
        if(dashTimer <= 0) StartCoroutine("Dash");
    }
    private void HandleInteract(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    private void HandleJump(InputAction.CallbackContext context)
    {
        if(CanJump())
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerStats.jumpPower);
            jumpBufferCount = jumpBufferLength;
            hangTimeCounter = hangTime;
            return;
        }
        else if (Walled() && !Grounded() && moveInput.x != 0 && canWallJump)
        {
            wallJumpingDirection = -transform.localScale.x;
            playerRB.velocity = new Vector2(wallJumpingDirection * playerStats.jumpPower, playerStats.jumpPower);
            canWallJump = false;
        }
    }
    private void HandleFlashLight(InputAction.CallbackContext context)
    {
        if(flashLightInactive) ActivateFlashLight();
        else DeactivateFlashLight();
    }
    
    private void ActivateFlashLight() 
        {flashLightMask.SetActive(true); flashLightInactive = false;}
    private void DeactivateFlashLight() 
        {flashLightMask.SetActive(false); flashLightInactive = true;}
    void Update()
    {
        if(Grounded()) 
        {
            hangTimeCounter = hangTime;
            canWallJump = true;    
        }

        WallSlide();
        UpdateTimers();
        
        if(!GameManager.i.GetIsPaused() && !flashLightActive) moveInput = move.ReadValue<Vector2>();
        else playerRB.velocity = new Vector2(0, playerRB.velocity.y);
    }
    private void FixedUpdate() 
    {
        if(GameManager.i.GetIsPaused() || flashLightActive) 
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
            return;
        }
        
        if(isDashing) return;

        FlipPlayer();
        Vector2 moveSpeed = moveInput.normalized;
        playerRB.velocity = new Vector2(moveSpeed.x * playerStats.speed, playerRB.velocity.y);  
    }
    private void UpdateTimers()
    {
        hangTimeCounter -= Time.deltaTime;
        jumpBufferCount -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
    }

    private void WallSlide()
    {
        if(Walled() && !Grounded() && moveInput.x != 0)
        {
            isWallSliding = true;
            playerRB.velocity = new Vector2(playerRB.velocity.x, Mathf.Clamp(playerRB.velocity.y, -playerStats.wallSlidingSpeed, float.MaxValue));
        }
        else isWallSliding = false;
    }
    IEnumerator Dash()
    {
        isDashing = true;
        originalGravity = playerRB.gravityScale;
        playerRB.gravityScale = 0f;
        playerRB.velocity = new Vector2(transform.localScale.x * playerStats.dashForce, 0f);
        StartCoroutine("SpawnGhostTrail");    
        yield return new WaitForSeconds(.1f);
        isDashing = false;
        playerRB.gravityScale = originalGravity;
        dashTimer = dashCD;
        OnPlayerDashing?.Invoke(dashTimer);
    }

    IEnumerator SpawnGhostTrail()
    {
        for(int i=0; i < 7; i++)
        {
            GameObject newGhost = Instantiate(GameAssets.i.playerGhost, transform.position, Quaternion.identity);
            if(!facingRight) newGhost.transform.localScale = new Vector3(-1f, 1f, 1f);
            yield return new WaitForSeconds(.005f);
        }
    }
    private bool Grounded()
    {
        float extraDistance = .3f;
        Color rayColor;

        RaycastHit2D rayCastHit = Physics2D.BoxCast(playerCollider.bounds.center,
            playerCollider.bounds.size, 0f, Vector2.down, extraDistance, StaticVariables.i.GetGroundLayer());

        if (rayCastHit.collider != null) rayColor = Color.green;
        else rayColor = Color.red;

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x, 0),
            Vector2.down * (playerCollider.bounds.extents.y + extraDistance), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, 0),
            Vector2.down * (playerCollider.bounds.extents.y + extraDistance), rayColor);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y + extraDistance),
            Vector2.right * (playerCollider.bounds.extents.x * 2f), rayColor);

        return rayCastHit.collider != null;
    }
    private bool Walled()
    {
        float extraDistance = .3f;
        
        if(facingRight)
            wallDetect = Physics2D.BoxCast(playerCollider.bounds.center,
                playerCollider.bounds.size, 0f, Vector2.right, extraDistance, StaticVariables.i.GetWallLayer());
        else    
            wallDetect = Physics2D.BoxCast(playerCollider.bounds.center,
                playerCollider.bounds.size, 0f, Vector2.left, extraDistance, StaticVariables.i.GetWallLayer());

        if (wallDetect.collider != null && facingRight) rayColorR = Color.yellow;
        else rayColorR = Color.red;
        
        if (wallDetect.collider != null && !facingRight) rayColorL = Color.magenta;
        else rayColorL = Color.red;

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(0, playerCollider.bounds.extents.y),
            Vector2.right * (playerCollider.bounds.extents.x + extraDistance), rayColorR);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y),
            Vector2.right * (playerCollider.bounds.extents.x + extraDistance), rayColorR);
        Debug.DrawRay(playerCollider.bounds.center + new Vector3(playerCollider.bounds.extents.x + extraDistance, playerCollider.bounds.extents.y),
            Vector2.down * (playerCollider.bounds.extents.x * 2f), rayColorR);

        Debug.DrawRay(playerCollider.bounds.center + new Vector3(0, playerCollider.bounds.extents.y),
            Vector2.left * (playerCollider.bounds.extents.x + extraDistance), rayColorL);
        Debug.DrawRay(playerCollider.bounds.center - new Vector3(0, playerCollider.bounds.extents.y),
            Vector2.left * (playerCollider.bounds.extents.x + extraDistance), rayColorL);
        Debug.DrawRay(playerCollider.bounds.center + new Vector3(-playerCollider.bounds.extents.x - extraDistance, playerCollider.bounds.extents.y),
            Vector2.down * (playerCollider.bounds.extents.x * 2f), rayColorL);

        return wallDetect.collider != null;
    }

    private void FlipPlayer()
    {
        if (moveInput.x < 0)
        {
            playerRB.transform.localScale = new Vector3(-1f, 1f, 1f);
            facingRight = false;
        }
        else if (moveInput.x > 0)
        {
            playerRB.transform.localScale = Vector3.one;
            facingRight = true;
        }
    }

    private bool CanJump()
    {
        if(GameManager.i.GetIsPaused() || flashLightActive) return false;
        
        if(jumpBufferCount <= 0f && hangTimeCounter >= 0f) return true;
        else return false;        
    }

    public bool GetFacingDirection(){return facingRight;}
    private void HandleDeath(HazardType type){Destroy(gameObject);}
}
