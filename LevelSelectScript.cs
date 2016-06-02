using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour {

    private GameObject[] texts;

    private int currLevel;

    void Awake() {

        texts = GameObject.FindGameObjectsWithTag("Text");

    }

    void Start() {

        currLevel = Application.loadedLevel + 1;

        foreach (GameObject go in texts) {

            if(go.GetComponent<Text>().text == currLevel.ToString()) {

                go.GetComponent<Text>().color = Color.red;

            }

        }

    }

    public void OnLevelSelected() {

        //Get text (int) and Play that level
        int levelSelected = int.Parse(GetComponent<Text>().text);
        Application.LoadLevel(levelSelected - 1);

    }
}
