using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float movingSpeed = 4f;

    Rigidbody2D rigidBody;

    public bool facingRight = false;

    private Vector3 startPosition;

    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start() {
        this.transform.position = startPosition;
    }

    void FixedUpdate() {
        float currentMovingSpeed = movingSpeed;

        if (facingRight) {
            currentMovingSpeed = movingSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        } else {
            currentMovingSpeed = -movingSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if(GameManager.sharedInstance.currentGameState == GameState.inGame) {
            rigidBody.velocity = new Vector2(currentMovingSpeed, rigidBody.velocity.y);
        }
    }
}
