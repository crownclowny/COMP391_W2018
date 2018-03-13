using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour {

	public Ball theBall;

	public float speed = 30;

	public float lerpTweak = 2f;

	private Rigidbody2D rigitBody;

	// Use this for initialization
	void Start () {
		rigitBody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (theBall.transform.position.y > transform.position.y) {
			Vector2 dir = new Vector2 (0, 1).normalized;

			rigitBody.velocity = Vector2.Lerp (
				rigitBody.velocity, 
				dir * speed, 
				lerpTweak * Time.deltaTime
			);
		} else if (theBall.transform.position.y < transform.position.y) {
			Vector2 dir = new Vector2 (0, -1).normalized;

			rigitBody.velocity = Vector2.Lerp (
				rigitBody.velocity, 
				dir * speed, 
				lerpTweak * Time.deltaTime
			);
		} else {
			Vector2 dir = new Vector2 (0, 0).normalized;
			rigitBody.velocity = Vector2.Lerp (
				rigitBody.velocity, 
				dir * speed, 
				lerpTweak * Time.deltaTime
			);
		}
		
	}
}
