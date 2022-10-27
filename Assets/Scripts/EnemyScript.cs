using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float movingSpeed = 1.5f;

    Rigidbody2D rigidBody;

    public bool facingRight = false;

    private Vector3 startPosition;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start() {
        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
