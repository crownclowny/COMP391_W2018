using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Boundary class
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;

}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float nextFire = 0.25f;
    public Boundary boundary;
    public GameObject laser;
    public Transform laserspawn;


    /*
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    */
    //public float xMin, xMax, yMin, yMax;


    //Private variable
    private Rigidbody2D rBody;
    private float myTime = 0.0f;

    // Use this for initialization
    void Start () {
        rBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        myTime += Time.deltaTime;

        if (Input.GetButton("Fire1")&& myTime> nextFire)
        {
            Instantiate(laser, laserspawn.position, laserspawn.rotation);

            myTime = 0.0f;
        }

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

        //rbody.position = new Vector2(Mathf.Clamp(rbody.position.x, -8.5f, 3.0f),Mathf.Clamp(rbody.position.y, -4.0f, 4.0f));
        //rbody.position = new Vector2(Mathf.Clamp(rbody.position.x, xMin, xMax), Mathf.Clamp(rbody.position.y, yMin, yMax));

        rbody.position = new Vector2(Mathf.Clamp(rbody.position.x,boundary.xMin,boundary.xMax), Mathf.Clamp(rbody.position.y,boundary.yMin, boundary.yMax));

    }
}
