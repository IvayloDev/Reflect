using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class TopPanelManager : MonoBehaviour {

    [SerializeField]
    private Animator topPanelAnim;

    [SerializeField]
    private Animator levelSelectAnim,HintPopUp;

    public static int hintCount = 0;

    public static bool isPanelActive;

    private GameObject hint;

    public void OnPauseClick() {

        isPanelActive = true;

        topPanelAnim.SetBool("PanelIn", true);
        topPanelAnim.SetBool("PanelOut", false);

    }

    public void OnResumeClick() {

        isPanelActive = false;

    }

    public void OnWatchAdClick() {

        //if successful show hint GO

        isPanelActive = false;

        if (Advertisement.IsReady("rewardedVideo")) {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }

    }


    private void HandleShowResult(ShowResult result) {
        switch (result) {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                GameObject.FindGameObjectWithTag("Hint").GetComponent<Image>().enabled = true;

                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }

    public void OnLevelSelectClick() {

        levelSelectAnim.SetBool("LevelSelectOut", false);
        levelSelectAnim.SetBool("LevelSelectIn", true);


    }

    void Start() {

        GameObject.FindGameObjectWithTag("Hint").GetComponent<Image>().enabled = false;

        topPanelAnim.SetBool("PanelOut", false);
        levelSelectAnim.SetBool("LevelSelectOut", false);


    }

    void ReturnPanel() {

        isPanelActive = false;

        //getcomp scroll rect enalbed false
        levelSelectAnim.SetBool("LevelSelectIn", false);
        levelSelectAnim.SetBool("LevelSelectOut", true);

        topPanelAnim.SetBool("PanelOut", true);
        topPanelAnim.SetBool("PanelIn", false);

     

    }

    // Update is called once per frame
    void Update () {

        if (!isPanelActive) {

            ReturnPanel();

        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

            if (hintCount == 4) {

            hintCount = 0;
            HintPopUp.SetTrigger("HintPopUp");

        }

	}

}
