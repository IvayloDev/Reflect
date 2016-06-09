using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadPassedLevels : MonoBehaviour {

    [SerializeField]
    private Text[] texts;

    [SerializeField]
    private Button[] butts;

    [SerializeField]
    private Color grey;

    [SerializeField]
    private Color defaultColor;

    private int currentlevel;
    private int nextLevel;
    public static int maxLevel = 1;

    void OnDestroy() {

        PlayerPrefs.SetInt("maxLvl",maxLevel+1);

    }

    void Awake() {

        currentlevel = SceneManager.GetActiveScene().buildIndex;

        nextLevel = currentlevel + 1;

        foreach (Text txt in texts) {
            txt.color = grey;
        }

        foreach (Button butt in butts) {
            butt.interactable = false;
        }

        StartCoroutine(PopulateCompletedLevels());

    }

    IEnumerator PopulateCompletedLevels() {

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < maxLevel; i++) {

            Debug.LogError(i);
            texts[i].color = defaultColor;
            butts[i].interactable = true;

        }
    }


    void Update () {

            if (currentlevel >= maxLevel) {

              maxLevel = currentlevel;

            }


        Debug.Log("curr " + currentlevel);
        Debug.Log("next " + nextLevel);
        Debug.LogError(Application.loadedLevel);
        Debug.LogError("MAX LEVEL " + maxLevel);
    }
}
