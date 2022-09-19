using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del movimiento
    public float jumpForce = 6f;
    Rigidbody2D rigidBody;
    Animator animator;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_GROUND = "isOnGround";

    public LayerMask groundMask;
    void Awake ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

        animator.SetBool(STATE_ON_GROUND, IsTouchingGround());

        Debug.DrawRay(this.transform.position, Vector2.down*1.5f, Color.red);
    }

    void Jump() {
        if(IsTouchingGround()){
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    //comprueba si el jugador esta tocando el suelo
    bool IsTouchingGround() {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, groundMask)) {
            animator.enabled = true;
            return true;
        } else {
            animator.enabled = false;
            return false;
        }
    }
}
