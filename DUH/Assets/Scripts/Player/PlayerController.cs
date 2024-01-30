using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
public bool FacingLeft { get{ return facingLeft ; } set{ facingLeft = value ; } }
public static PlayerController Instance;    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer; 
    private bool isDashing = false;
    private bool facingLeft = false;
    private PlayerControls playerControls;
    private float startingMoveSpeed;
    private void Start()
    {
        playerControls.Movement.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;

    }



    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // Set animator parameters based on movement input
        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);

        bool isMoving = movement.magnitude > 0.1f; 


    }


    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // Flip the player model based on the position of the mouse cursor compared to the player's position
        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true; 
            FacingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false; 
            FacingLeft = false;
        }
    }
private void Dash()
{
    if(!isDashing){
    moveSpeed *= dashSpeed;
    isDashing = true;
    myTrailRenderer.emitting = true;
    StartCoroutine(EndDashRoutine());
    }

}

private IEnumerator EndDashRoutine()
{
    float dashTime = 0.2f;
    float dashCD = .25f;
    yield return new WaitForSeconds(dashTime);
    moveSpeed = startingMoveSpeed;
    myTrailRenderer.emitting = false;
    yield return new WaitForSeconds(dashCD);
    isDashing = false;
}

    
}
