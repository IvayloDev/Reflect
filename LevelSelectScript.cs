using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour {

    private GameObject[] texts;

    public static int currLevel;


    void Start() {

        StartCoroutine(MakeCurrLevelRed());

        texts = GameObject.FindGameObjectsWithTag("Text");

        currLevel = SceneManager.GetActiveScene().buildIndex;

    }
    IEnumerator MakeCurrLevelRed() {

        yield return new WaitForSeconds(0.15f);

        foreach (GameObject go in texts) {

            if (go.GetComponent<Text>().text == currLevel.ToString()) {

                go.GetComponent<Text>().color = Color.red;

            }

        }

    }

    public void OnLevelSelected() {

        AudioManager.Main.PlayNewSound("Click");

        //Get text (int) and Play that level
        int levelSelected = int.Parse(GetComponent<Text>().text);
        SceneManager.LoadScene(levelSelected);

    }
}
