using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Variables del movimiento
    public float jumpForce = 6f, runningSpeed = 2f;

    Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;
    
    private const string STATE_ALIVE = "isAlive", 
        STATE_ON_GROUND = "isOnGround";


    [SerializeField]
    private int healthPoints, manaPoints;

    public const int INITAL_HEALTH = 100, INITAL_MANA = 15,
        MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;    

    public float jumpRaycastDistance = 1.5f;

    public LayerMask groundMask;

    void Awake ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    void Start() {
        startPosition = this.transform.position;
    }

    public void StartGame(){
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_GROUND, false);

        healthPoints = INITAL_HEALTH;
        manaPoints = INITAL_MANA;
        Invoke("RestartPosition", 0.2f);
    }

    void RestartPosition(){
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
        
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CamaraFollow>().ResetCameraPosition();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetButtonDown("Jump")){
            Jump(false);
        }
        if(Input.GetButtonDown("Super Jump")){
            Jump(true);
        }
        
        animator.SetBool(STATE_ON_GROUND, IsTouchingGround());

        Debug.DrawRay(this.transform.position, Vector2.down * jumpRaycastDistance, Color.red);
    }

    void FixedUpdate() {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame){
            if(rigidBody.velocity.x < runningSpeed) {
                rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
            }
        } else { //Si no esta en estado inGame
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    void Jump(bool superjump) {
        float jumpForceFactor = jumpForce;

        if(superjump && manaPoints >= SUPERJUMP_COST && IsTouchingGround()) {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if(GameManager.sharedInstance.currentGameState == GameState.inGame){
            if(IsTouchingGround()){
                GetComponent<AudioSource>().Play();
                rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            }    
        }
    }

    //comprueba si el jugador esta tocando el suelo
    bool IsTouchingGround() {
        if(Physics2D.Raycast(this.transform.position, Vector2.down, jumpRaycastDistance, groundMask)) {
            return true;
        } else {
            return false;
        }
    }

    public void Death(){

        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", defaultValue: 0f);
        if(travelledDistance > previousMaxDistance) {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points){
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH){
            this.healthPoints = MAX_HEALTH;
        }

        if(this.healthPoints <= 0){
            Death();
        }
    }

    public void CollectMana(int points){
        this.manaPoints += points;
        if(this.manaPoints >= MAX_MANA){
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth(){
        return healthPoints;
    }

    public int GetMana(){
        return manaPoints;
    }

    public float GetTravelledDistance(){
        return this.transform.position.x - startPosition.x;
    }

}
