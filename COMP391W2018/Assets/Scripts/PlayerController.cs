using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boundary class
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float nextFire = 0.1f;
    public Boundary boundary;
    public GameObject laser;
    public Transform laserSpawn;

    /*
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    */

    // public float xMin, xMax, yMin, yMax;

    // Private variables
    private Rigidbody2D rBody;
    private float myTime = 0.0f;

	// Use this for initialization
	void Start () {
        rBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        myTime += Time.deltaTime;

		if(Input.GetButton("Fire1") && myTime > nextFire)
        {
            Instantiate(laser, laserSpawn.position, laserSpawn.rotation);

            myTime = 0.0f;
        }
	}

    // Used when performing physics calculations
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Returns a value between -1 and 1 whenever left, right, a, or d is pushed
        float moveVertical = Input.GetAxis("Vertical"); // Reutn a value between -1 an d1 whenever up, down, w, or s is pushed

        // Debug.Log("H= " + moveHorizontal + " V= " + moveVertical);

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Rigidbody2D rBody = this.gameObject.GetComponent<Rigidbody2D>(); // Establishes a "connection" to the rigidbody2D component
        rBody.velocity = movement * speed;

        // rBody.position = new Vector2(Mathf.Clamp(rBody.position.x, -8.5f, 3.0f), Mathf.Clamp(rBody.position.y, -4.0f, 4.0f));
        // rBody.position = new Vector2(Mathf.Clamp(rBody.position.x, xMin, xMax), Mathf.Clamp(rBody.position.y, yMin, yMax));
        rBody.position = new Vector2(Mathf.Clamp(rBody.position.x, boundary.xMin, boundary.xMax), Mathf.Clamp(rBody.position.y, boundary.yMin, boundary.yMax));
    }
}
