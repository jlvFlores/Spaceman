using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del movimiento
    public float jumpForce = 6f;
    public float runningSpeed = 1f;

    public bool flipX;

    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer mySpriteRenderer;
    
    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_GROUND = "isOnGround";

    public LayerMask groundMask;

    void Awake ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start() {
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_GROUND, false);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Jump();
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            MoveRight();
            if(mySpriteRenderer != null) {
                mySpriteRenderer.flipX = false;
            }
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            MoveLeft();
            if(mySpriteRenderer != null) {
                mySpriteRenderer.flipX = true;
            }
        }
        
        animator.SetBool(STATE_ON_GROUND, IsTouchingGround());

        Debug.DrawRay(this.transform.position, Vector2.down*1.5f, Color.red);
    }

    void FixedUpdate() {
        // if(rigidBody.velocity.x < runningSpeed) {
        //     rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
        // }
    }

    void Jump() {
        if(IsTouchingGround()){
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void MoveRight() {
        // if(IsTouchingGround()){
            rigidBody.AddForce(Vector2.right * runningSpeed, ForceMode2D.Impulse);
        // }
    }
    void MoveLeft() {
        // if(IsTouchingGround()){
            rigidBody.AddForce(Vector2.left * runningSpeed, ForceMode2D.Impulse);
        // }
    }

    //comprueba si el jugador esta tocando el suelo
    bool IsTouchingGround() {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundMask)) {
            return true;
        } else {
            return false;
        }
    }
}
