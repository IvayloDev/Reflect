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

    public static int menuClicked;

    private Sound clickSound;


    public void OnPauseClick() {

        isPanelActive = true;

        topPanelAnim.SetBool("PanelIn", true);
        topPanelAnim.SetBool("PanelOut", false);

        AudioManager.Main.PlayNewSound("Click");

    }   

    public void OnResumeClick() {

        ReturnPanel();

        AudioManager.Main.PlayNewSound("Click");

    }

    public void OnWatchAdClick() {

        AudioManager.Main.PlayNewSound("Click");

        //if successful show hint GO

        ReturnPanel();


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

        if (menuClicked == 1) {

            AudioManager.Main.PlayNewSound("Click");

            levelSelectAnim.SetBool("LevelSelectOut", false);
            levelSelectAnim.SetBool("LevelSelectIn", true);

            menuClicked = 2;

            return;

        }

        if (menuClicked == 2){
            AudioManager.Main.PlayNewSound("Click");

            levelSelectAnim.SetBool("LevelSelectOut", true);
            levelSelectAnim.SetBool("LevelSelectIn", false);

            menuClicked = 1;

        }

    }

    void Start() {

        menuClicked = 1;

        GameObject.FindGameObjectWithTag("Hint").GetComponent<Image>().enabled = false;

        topPanelAnim.SetBool("PanelOut", false);
        levelSelectAnim.SetBool("LevelSelectOut", false);

        clickSound = AudioManager.Main.PlayNewSound("Click");

    }

    void ReturnPanel() {

        menuClicked = 1;

        isPanelActive = false;

        //getcomp scroll rect enalbed false
        levelSelectAnim.SetBool("LevelSelectIn", false);
        levelSelectAnim.SetBool("LevelSelectOut", true);

        topPanelAnim.SetBool("PanelOut", true);
        topPanelAnim.SetBool("PanelIn", false);

     

    }

    // Update is called once per frame
    void Update () {
        Debug.Log(menuClicked);
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

            if (hintCount == 4) {

            hintCount = 0;
            HintPopUp.SetTrigger("HintPopUp");

        }

	}

}
