using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    public GameObject hazard; // what are we spawning?
    public Vector2 spawnValue;// where do we spawn our hazard?
    public int hazardCount;//hazard per wave
    public float startWait;//how long first wave
    public float spawnWait;//how long between each wave
    public float waveWait;//how long between each wave of enemies

    private bool gameover;
    private bool restart;
    private int score;

	// Use this for initialization
	void Start () {



        StartCoroutine(spawnWaves());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator spawnWaves()
    {
        yield return new WaitForSeconds(startWait);//pause 
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector2 spawnposition = new Vector2(spawnValue.x, Random.Range(-spawnValue.y, spawnValue.y));
                //                                    12                            -3.5          3.5
                Quaternion spawnRotation = Quaternion.identity;// deafult rotation.
                Instantiate(hazard, spawnposition, spawnRotation);

                yield return new WaitForSeconds(spawnWait);//wait time between spawning each asteroid
            }
            yield return new WaitForSeconds(waveWait);

            if (gameover)
            {
                break;
            }
        }

    }

    public void Addscore(int newscorevalue)
    {
        score += newscorevalue;
        //score+score value
        Debug.Log("Score is" + score);
    }

    public void GameOver()
    {
        Debug.Log("GAME IS OVER");
        gameover = true;
    }
}
