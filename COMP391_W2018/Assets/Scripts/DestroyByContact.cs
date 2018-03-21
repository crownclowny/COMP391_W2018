using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject ExplosionAsteroid;
    public GameObject ExplosionPlayer;
    public int scorevalue = 10;


    private GameController gameControllerScript;
    // Use this for initialization
    void Start () {
        GameObject gameControllerObject; GameObject.FindWithTag("GameContoller");

        if (gameControllerObject != null)
        {
            gameControllerScript = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerScript != null)
        {
            Debug.Log("Cannot find Game Controller script");
        }
    }
	
	
	void onTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Boundary")
        {
            return;
            //Debug.Log("DestroyedByContact");
        }
        if (other.tag == "Player")
        {
            Vector3 deltaP = (this.transform.position + other.transform.position)/2;//vector between player and asteroid
            

            //creat our explosion animation
            Instantiate(ExplosionPlayer, deltaP, other.transform.rotation);

            gameControllerScript.GameOver();
        }
        else
        {
            Instantiate(ExplosionAsteroid, other.transform.position, other.transform.rotation);
            gameControllerScript.Addscore(scorevalue);
        }

        Instantiate(ExplosionAsteroid, this.transform.position, this.transform.rotation);
        Destroy(other.gameObject);//Destroy the other thing(Lazer)
        Destroy(this.gameObject);//Destroying the asteroid
        
	}
}
