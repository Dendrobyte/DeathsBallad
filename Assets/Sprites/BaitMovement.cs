using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitMovement : MonoBehaviour {
    private Vector3 mousePos;
    public Vector3 originalPos;
    // Start is called before the first frame update
    void Start() {
        Debug.Log("-+ Bait loaded! +-");
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update() {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 1;

        transform.position = mousePos;

        // lock the position into the bounds
        if (transform.position.y >= 0.42 ){
            transform.position = new Vector3(transform.position.x, 0.42f, transform.position.z);
        } else if (transform.position.y <= -5.21) {
            transform.position = new Vector3(transform.position.x, -5.21f, transform.position.z);
        }

        if (transform.position.x >= 10.1){
            transform.position = new Vector3(10.1f, transform.position.y, transform.position.z);
        } else if (transform.position.x <= -2.9) {
            transform.position = new Vector3(-2.9f,transform.position.y, transform.position.z);
        }

        // Check for collision with a soul
    }
}
