using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour {

    private GameObject reflectedLight;

    private Vector3 curPos;
    private Quaternion curRot;

    private float offSet;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private CircleCollider2D BulbCol;

    public static bool levelFinished;

    private GameObject[] mirrorsArray;

    private GameObject[] ObstaclesArray;

    private Rigidbody2D rb;

    private TrailRenderer tr;

    private Animator BallAnim;

    [SerializeField]
    private Animator BulbAnim;

    [SerializeField]
    private Animator ObstacleAnim;

    [SerializeField]
    private Animator LevelTextAnim;

    [SerializeField]
    private Color mainColor, transitionColor;

    private int lvlIndex;

    private bool endReachedFirst;

    private GameObject[] reflectLightsArray;
    private bool boolDrag1, boolDrag2, boolDrag3, boolDrag4, boolDrag5, boolDrag6;

    IEnumerator StartAnimations() {

        levelFinished = true;

        yield return new WaitForSeconds(0.1f);

        AudioManager.Main.PlayNewSound("NewLevel");
        
        //transform.localScale = new Vector3(500, 500);

        LevelTextAnim.GetComponent<Text>().text = "" + LevelSelectScript.currLevel;
        LevelTextAnim.SetTrigger("text");

        yield return new WaitForSeconds(1.3f);

        //Ball shrinking animation
        tr.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.position = BulbAnim.GetComponent<Transform>().position;
        transform.localScale = new Vector3(500, 500);
        GetComponent<SpriteRenderer>().enabled = true;
        BallAnim.SetBool("ShrinkBall",true);

        yield return new WaitForSeconds(0.5f);

        Camera.main.backgroundColor = mainColor;

        yield return new WaitForSeconds(1f);

        //Bulb Animation
        BulbAnim.GetComponent<Image>().enabled = true;
        BulbAnim.SetBool("Start", true);

        yield return new WaitForSeconds(0.15f);
       
        //Obstacle Animation
        foreach (GameObject go in ObstaclesArray) {
            go.GetComponent<Image>().enabled = true;
            go.GetComponent<Animator>().SetBool("Start",true);
        }

        yield return new WaitForSeconds(0.15f);

        //Mirrors Animation
        foreach (GameObject go in mirrorsArray) {
            go.GetComponent<Image>().enabled = true;
            go.GetComponent<Animator>().SetTrigger("Start");
        }

       

        BulbAnim.SetBool("Start", false);

        yield return new WaitForSeconds(0.7f);
        GetComponent<SpriteRenderer>().enabled = false;

        levelFinished = false;
        BallAnim.SetBool("ShrinkBall", false);

        foreach (GameObject go in ObstaclesArray) {
            go.GetComponent<Animator>().SetBool("Start",false);
        }


    }

    IEnumerator EndAnimations() {

        reflectLightsArray = GameObject.FindGameObjectsWithTag("Destroy");

        tr.Clear();

        //Destroy every reflect Light GO
        for (int i = 0; i < reflectLightsArray.Length; i++) {

            Destroy(reflectLightsArray[i]);

        }

        yield return new WaitForSeconds(0.1f);

        //Bulb Animation
        BulbAnim.SetBool("End", true);

        yield return new WaitForSeconds(0.2f);

        //Obstacle Animation
        foreach (GameObject go in ObstaclesArray) {
            go.GetComponent<Animator>().SetTrigger("End");
        }

        yield return new WaitForSeconds(0.2f);

        //Mirrors Animation
        foreach (GameObject go in mirrorsArray) {
            go.GetComponent<Animator>().SetTrigger("End");
        }

        //Display text and anim for it


        yield return new WaitForSeconds(0.8f);

        Camera.main.backgroundColor = transitionColor;

      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    IEnumerator NextLevel() {
        Debug.Log("LOL");
        BallAnim.SetBool("Default", false);

        //Set public bool to true can see from other scripts if the next level is currently being loaded
        levelFinished = true;

        //Set bigger radius so that the player cannot sligshot past the end point
        BulbCol.radius = 150;

        //Make trail retrieve faster
        tr.time = 2.5f;

        yield return new WaitForSeconds(2f);

        rb.isKinematic = true;

        BallAnim.SetBool("ExpandBallBool",true);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(EndAnimations());


    }


    void Awake() {

        mirrorsArray = GameObject.FindGameObjectsWithTag("Mirror");

        ObstaclesArray = GameObject.FindGameObjectsWithTag("Enemy");

    }

    // Use this for initialization
    void Start() {

        //Make UI invisible for the animations to be right

        BulbAnim.GetComponent<Image>().enabled = false;

        foreach (GameObject go in ObstaclesArray) {
            go.GetComponent<Image>().enabled = false;
        }

        foreach (GameObject go in mirrorsArray) {
            go.GetComponent<Image>().enabled = false;
        }

        levelFinished = false;

        //Get trail component
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        BallAnim = GetComponent<Animator>();

        BallAnim.SetBool("Default", false);

        //Reset end point radius
        BulbCol.radius = 48;

        rb.isKinematic = false;

        StartCoroutine(StartAnimations());

        endReachedFirst = true;

    }



    void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Mirror") { 
        //Play MirrorHitAnimation
        collision.GetComponent<Animator>().SetTrigger("MirrorHitTrigger");

        }

        if (collision.tag == "End" && endReachedFirst) {

            endReachedFirst = false;

            StartCoroutine(NextLevel());
            AudioManager.Main.PlayNewSound("reachedEnd");

        }
    }

    void OnCollisionEnter2D(Collision2D coll) {

        if (coll.transform.tag == "Mirror") {

            curPos = transform.position;

            SpawnReflection();
            AudioManager.Main.PlayNewSound("MirrorHit");

        }
    }

    void Update() {


    }

    void SpawnReflection() {

        reflectedLight = Instantiate(Resources.Load("ReflectionLightOnMirror"), curPos, Quaternion.identity) as GameObject;

    }

}
