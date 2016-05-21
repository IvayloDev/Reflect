using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour {

    private GameObject reflectedLight;

    private Vector3 curPos;

    private float offSet = 0.05f;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private CircleCollider2D BulbCol;

    public static bool levelFinished;

    private GameObject[] mirrorsArray;

    private Rigidbody2D rb;

    private TrailRenderer tr;

    private Animator BallAnim;

    [SerializeField]
    private Animator BulbAnim;

    [SerializeField]
    private Animator MirrorAnim;

    [SerializeField]
    private Animator ObstacleAnim;


    IEnumerator StartAnimations() {

        yield return new WaitForSeconds(0.2f);

        //Bulb Animation
        BulbAnim.GetComponent<Image>().enabled = true;
        BulbAnim.SetBool("Start", true);

        yield return new WaitForSeconds(0.4f);

        //Obstacle Animation
        ObstacleAnim.GetComponent<Image>().enabled = true;
        ObstacleAnim.SetBool("Start", true);

        yield return new WaitForSeconds(0.3f);

        //Mirrors Animation
        foreach (GameObject go in mirrorsArray) {
            go.GetComponent<Image>().enabled = true;
            go.GetComponent<Animator>().SetBool("Start", true);
        }

    }

    IEnumerator EndAnimations() {

        yield return new WaitForSeconds(0.1f);

        //Bulb Animation
        BulbAnim.SetBool("End", true);

        yield return new WaitForSeconds(0.2f);

        //Obstacle Animation
        ObstacleAnim.SetBool("End", true);

        yield return new WaitForSeconds(0.2f);

        //Mirrors Animation
        foreach (GameObject go in mirrorsArray) {
            go.GetComponent<Animator>().SetBool("End", true);
        }


    }

    IEnumerator NextLevel() {

        //Set public bool to true can see from other scripts if the next level is currently being loaded
        levelFinished = true;

        //Set bigger radius so that the player cannot sligshot past the end point
        BulbCol.radius = 150;

        //Make trail retrieve faster
        tr.time = 2.5f;

        yield return new WaitForSeconds(2f);

        BallAnim.SetBool("ExpandBallBool", true);

        rb.isKinematic = true;

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(EndAnimations());

        yield return new WaitForSeconds(3f);

        //Load next level
        //SceneManager.LoadScene(Application.loadedLevel+ 1);
        SceneManager.LoadScene(Application.loadedLevel);
    }

    void Awake() {

        //Tag is RightHit because all the mirrors are with this tag
        mirrorsArray = GameObject.FindGameObjectsWithTag("RightHit");

    }

    // Use this for initialization
    void Start() {

        //Make UI invisible for the animations to be right
        ObstacleAnim.GetComponent<Image>().enabled = false;
        //MirrorAnim.GetComponent<Image>().enabled = false;
        BulbAnim.GetComponent<Image>().enabled = false;

        foreach(GameObject go in mirrorsArray) {
            go.GetComponent<Image>().enabled = false;
        }

        levelFinished = false;

        //Get trail component
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        BallAnim = GetComponent<Animator>();

        //Reset end point radius
        BulbCol.radius = 48;

        rb.isKinematic = false;


        //Set animations

        StartCoroutine(StartAnimations());




    }

    void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "RightHit"|| collision.tag == ("LeftHit")) { 
        //Play MirrorHitAnimation
        collision.GetComponent<Animator>().SetTrigger("MirrorHitTrigger");
    }

        if (collision.tag == "End") {

            StartCoroutine(NextLevel());
        
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {


        if (coll.transform.tag == "LeftHit") {
            //Mathf.Clamp(curPos.y, coll.collider.bounds.min.y, coll.collider.bounds.max.y);

            curPos = coll.transform.position;

            SpawnReflection();


        } else if (coll.transform.tag == "RightHit") {

            curPos = coll.transform.position;

            SpawnReflection();

        }
    }

    void SpawnReflection() {

        curPos.y = transform.position.y;

        if (curPos.x <= 0) {
            curPos.x += offSet;
        } else {
            curPos.x -= offSet;
        }

        reflectedLight = Instantiate(Resources.Load("ReflectionLightOnMirror"), curPos, Quaternion.identity) as GameObject;
        reflectedLight.transform.tag = "reflectedLight";

    }

}
