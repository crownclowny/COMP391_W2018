using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpashScreen : MonoBehaviour {

	public string sceneToLoad;

	public int secTillSceenLoad;

	// Use this for initialization
	void Start () {

		Invoke ("OpenNextScene", secTillSceenLoad);

	}

	void OpenNextScene(){
		SceneManager.LoadScene (sceneToLoad);
	}
}
