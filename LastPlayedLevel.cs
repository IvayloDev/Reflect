using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LastPlayedLevel : MonoBehaviour {

	void Awake () {

        //Script for first scene
        int levelToLoad = PlayerPrefs.GetInt("maxLvl", 1);

        if (levelToLoad > SceneManager.sceneCountInBuildSettings || levelToLoad == 0) {

            levelToLoad = 1;

        }

        SceneManager.LoadScene(levelToLoad);

	}

}
