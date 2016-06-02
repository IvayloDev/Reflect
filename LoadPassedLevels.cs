using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadPassedLevels : MonoBehaviour {

    [SerializeField]
    private Text[] texts;

    [SerializeField]
    private Button[] butts;

    [SerializeField]
    private Color grey;

    [SerializeField]
    private Color defaultColor;

    public static int level;

    void OnDestroy() {

        PlayerPrefs.SetInt("level",level);

    }

    void Awake() {

        level = PlayerPrefs.GetInt("level", 1);

        //foreach (Text txt in texts) {
        //    txt.color = grey;
        //}

        //foreach (Button butt in butts) {
        //    butt.interactable = false;
        // }

    }



    void Start () {

        //for (int i = 0; i <= level - 1; i++) {

        //    texts[i].color = defaultColor;
        //    butts[i].interactable = true;

        //}



    }

    void Update () {

        Debug.Log(level);

        Debug.LogWarning(AnimationController.levelIndex);

    }
}
