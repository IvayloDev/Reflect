using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovementController : MonoBehaviour {

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;
    private Vector2 firstPos, secondPos;

    private GameObject[] reflectLightsArray;

    private Vector3 mousePos;
    private Vector3 basePos = new Vector3(0, -20, 0);

    [SerializeField]
    private Material material;

    [SerializeField]
    private Animator dontCrossLineAnim;

    //Not lagging with speed set to 40k
    private float speed = 60000f;
    private float distance = 10f;

    private bool addForceBool;

    private Rigidbody2D rb;
    private TrailRenderer tr;
    private LineRenderer line;

    private RaycastHit hit;
    private Ray ray;

    private Animator ballAnim;



    IEnumerator DestroyPlayer() {

        //ScreenShake
        //Screen Flash
        //Sound

        tr.time = 0.8f;

        ballAnim.SetBool("DestroyBall",true);

        rb.isKinematic = true;

        yield return new WaitForSeconds(0.8f);

        GetComponent<SpriteRenderer>().enabled = false;

        tr.enabled = false;
        transform.position = basePos;
        rb.isKinematic = true;

    }

    void Start() {

        Application.targetFrameRate = 60;

        //Disable Object at start
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = false;
        ballAnim = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

    }

    void Update() {


        if (Input.GetMouseButtonDown(0)) {

            //Populate array with every instantiated reflect light GO
            reflectLightsArray = GameObject.FindGameObjectsWithTag("reflectedLight");

            //get coordinates of clicked space
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }

        if (Physics.Raycast(ray, out hit, 100)) {

            //Check if finger position is in the clickable panel area else dont allow to fire the ball
            if (hit.transform.CompareTag("ClickableArea")) {

                if (AnimationController.levelFinished) {
                    return;
                }

                SetBallPosition();
                DrawLine();
                GetMousePosition();

            } else if (hit.transform.CompareTag("Untagged")) {

                //Play "Show Clickable Line" Animation
                dontCrossLineAnim.SetTrigger("DontCross");
                ray.origin = new Vector3(0, 1000, 0);

            }
        }
    }

    //Disable ball and line renderer when game paused
    void OnApplicationPause() {

        //Destroy(GameObject.FindGameObjectWithTag("LINE"));
        //GetComponent<SpriteRenderer>().enabled = false;
        //tr.Clear();


    }

    void OnCollisionEnter2D(Collision2D collision) {

        if (collision.transform.CompareTag("Enemy")) {

            //Destroy Player
            StartCoroutine(DestroyPlayer());

        }
    }

    //Handle Physics
    void FixedUpdate() {

        if (addForceBool) {

            rb.AddForce((currentSwipe * Time.fixedDeltaTime) * speed);

            addForceBool = false;

        }
    }

    //Line is drawn on screen and destroyed when click released
    void DrawLine() {

        if (Input.GetMouseButtonDown(0)) {
            //check if there is no line renderer created
            if (line == null) {
                //create the line
                CreateLine();
            }

            //get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //set the z co ordinate to 0 as we only want the x,y axes
            mousePos.z = 0;
            //set the start point and end point of the line renderer
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);

            firstPos = mousePos;
        }


         //if mouse button is held clicked and line exists
         else if (Input.GetMouseButton(0) && line) {

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePos.z = 0;

            line.SetPosition(1, mousePos);

            secondPos = mousePos;

            //Set dotting texture to be tilling
            line.material.mainTextureScale = new Vector2((int)Vector2.Distance(firstPos, secondPos) * 10, 1);


        }

          //if line renderer exists and left mouse button is released
          else if (Input.GetMouseButtonUp(0) && line) {

            //Destroy Previously created line
            Destroy(GameObject.FindGameObjectWithTag("LINE"));

        }
    }

    //Setting width and material of Line
    void CreateLine() {

        //create a new empty gameobject and line renderer component
        line = new GameObject("Line").AddComponent<LineRenderer>();

        //assign the material to the line
        line.material = material;

        //set the number of points to the line
        line.SetVertexCount(2);

        //set the width
        line.SetWidth(0.08f, 0.08f);

        //
        line.material.SetTextureOffset("_MainTex", new Vector2(-0.2f, 0));

        //render line to the world origin and not to the object's position
        line.useWorldSpace = true;

        //set tag for destruction later (Destroyed in DrawLine Method)
        line.gameObject.tag = "LINE";

    }

    //Reset position,trail and rigidbody2D when clicked
    void SetBallPosition() {


        if (Input.GetMouseButtonDown(0)) {

            ballAnim.SetBool("DestroyBall", false);

            //Activate Object
            GetComponent<SpriteRenderer>().enabled = true;
            tr.enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            rb.isKinematic = false;

            //Reset Trial 
            tr.time = 15;
            tr.enabled = true;
            StopAllCoroutines();

            transform.localScale = new Vector3(1, 1, 1);

            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            //Stop Movement
            rb.isKinematic = true;
            rb.isKinematic = false;

            //Reset Trail
            tr.Clear();

            //Check if array is not null
            if (reflectLightsArray != null) {

                //Destroy every reflect Light GO
                for (int i = 0; i < reflectLightsArray.Length; i++) {

                    Destroy(reflectLightsArray[i]);

                }
            }
        }
    }

    //Create Vector for rigidbody2D
    void GetMousePosition() {

        if (Input.GetMouseButtonDown(0)) {

            //Origin point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }

        if (Input.GetMouseButtonUp(0)) {

            //Destination point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            currentSwipe.Normalize();

            addForceBool = true;


        }
    }

}
