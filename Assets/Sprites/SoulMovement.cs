using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulMovement : MonoBehaviour {
    // Start is called before the first frame update
    [Range(0, 12)]
    public int speed = 1;
    public int xDir = -1;
    public int yDir = 1;
    public Vector3 direction = new Vector3(.5f, .5f, 0f);

    public bool isMoving = true;
    void Start() {
        Debug.Log("-+ Soul spawned in! +-");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space")) {
            isMoving = false;
        }
        if (isMoving) {
            transform.position += Vector3.up * speed * yDir * Time.deltaTime;
            transform.position += Vector3.right * speed * xDir * Time.deltaTime;
            
            // force bouncing based on renders -- this... might be different based on window size?
            // absolutely disgusting
            if (transform.position.y >= 0.42 || transform.position.y <= -5.21) {
                yDir *= -1;
            }

            if (transform.position.x <= -2.9 || transform.position.x >= 10.1) {
                xDir *= -1;
            }
        }
        
    }
}
