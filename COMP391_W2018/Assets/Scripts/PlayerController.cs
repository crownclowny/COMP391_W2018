using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Used when performing physics calculations 
    void FixedUpdate()
    {
        float moveHorizontal=Input.GetAxis("Horizontal");// Return a value between -1 and 1 when ever left, right,a, or d is pushed
        float moveVertical=Input.GetAxis("Vertical");// Return a value between -1 and d1 whenever up, down,w or s is pushed
       
        //Debug.Log("H=" + moveHorizontal + "v="+ moveVertical + );

        Vector2 movement=new Vector2(moveHorizontal, moveVertical);

        Rigidbody2D rbody = this.gameObject.GetComponent<Rigidbody2D>();// Establishes a "connection" to the Rigigdbody2D component
        rbody.velocity = movement * speed;
    }
}
