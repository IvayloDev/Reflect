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

    [SerializeField]
    private Animator whiteFlashAnim;

    [SerializeField]
    private Animator levelSelectAnim, topPanelAnim;

    //Not lagging with speed set to 40k
    private float speed = 100000f;
    private float distance = 10f;

    private bool addForceBool;

    private Rigidbody2D rb;
    private TrailRenderer tr;
    private LineRenderer line;

    private RaycastHit hit;
    private Ray ray;

    private Animator ballAnim;

    private AudioSource ballDragSound;

    private bool boolDrag1, boolDrag2, boolDrag3, boolDrag4, boolDrag5, boolDrag6, boolDrag7, boolDrag8, boolDrag9;

    IEnumerator DestroyPlayer() {

        AudioManager.Main.PlayNewSound("Destruction");

        //Screen Flash
        whiteFlashAnim.SetTrigger("WhiteFlash");

        TopPanelManager.hintCount++;
        //Sound

        GameObject particle = Instantiate(Resources.Load("Particles"), transform.position, Quaternion.identity) as GameObject;


        tr.time = 0.8f;

        ballAnim.SetTrigger("DestroyBall");

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
        ballDragSound = GetComponent<AudioSource>();

        boolDrag1 = true;
        boolDrag2 = true;
        boolDrag3 = true;
        boolDrag4 = true;
        boolDrag5 = true;
        boolDrag6 = true;
        boolDrag7 = true;
        boolDrag8 = true;
        boolDrag9 = true;


    }


    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            //Populate array with every instantiated reflect light GO
            reflectLightsArray = GameObject.FindGameObjectsWithTag("Destroy");

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

                levelSelectAnim.SetBool("LevelSelectIn", false);
                levelSelectAnim.SetBool("LevelSelectOut", true);

                topPanelAnim.SetBool("PanelOut", true);
                topPanelAnim.SetBool("PanelIn", false);

                TopPanelManager.menuClicked = 1;



            } else if (hit.transform.CompareTag("Untagged") && !TopPanelManager.isPanelActive) {

                    //Play "Show Clickable Line" Animation
                    dontCrossLineAnim.SetTrigger("DontCross");
                     ray.origin = new Vector3(0, 1000, 0);

            }else if (hit.transform.CompareTag("TopPanelUI")) {

                return;

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
            if (Vector2.Distance(firstPos,secondPos) < 2f) {

                line.material.mainTextureScale = new Vector2(10, 1);

            } else
            
            line.material.mainTextureScale = new Vector2((int)Vector2.Distance(firstPos, secondPos) * 10, 1);

            DragSounds();
        


        }

          //if line renderer exists and left mouse button is released
          else if (Input.GetMouseButtonUp(0) && line) {

            //Destroy Previously created line
            Destroy(GameObject.FindGameObjectWithTag("LINE"));

        }
    }

    void DragSounds() {
        
        if (line.material.mainTextureScale.x == 10 && boolDrag1) {

            ballDragSound.Play();

            boolDrag1 = false;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;
        }

        if (line.material.mainTextureScale.x == 20 && boolDrag2) {

            ballDragSound.Play();
            boolDrag1 = true;
            boolDrag2 = false;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;
        }

        if (line.material.mainTextureScale.x == 30 && boolDrag3) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = false;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;
        }

        if (line.material.mainTextureScale.x == 40 && boolDrag4) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = false;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;
        }

        if (line.material.mainTextureScale.x == 50 && boolDrag5) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = false;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;

        }

        if (line.material.mainTextureScale.x == 60 && boolDrag6) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = false;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = true;

        }

        if (line.material.mainTextureScale.x == 70 && boolDrag7) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = false;
            boolDrag8 = true;
            boolDrag9 = true;

        }

        if (line.material.mainTextureScale.x == 80 && boolDrag8) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = false;
            boolDrag9 = true;

        }

        if (line.material.mainTextureScale.x == 90 && boolDrag9) {

            ballDragSound.Play();

            boolDrag1 = true;
            boolDrag2 = true;
            boolDrag3 = true;
            boolDrag4 = true;
            boolDrag5 = true;
            boolDrag6 = true;
            boolDrag7 = true;
            boolDrag8 = true;
            boolDrag9 = false;

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

        line.material.SetTextureOffset("_MainTex", new Vector2(-0.2f, 0));

        //render line to the world origin and not to the object's position
        line.useWorldSpace = true;

        //set tag for destruction later (Destroyed in DrawLine Method)
        line.gameObject.tag = "LINE";

    }

    //Reset position,trail and rigidbody2D when clicked
    void SetBallPosition() {

        if (Input.GetMouseButtonDown(0)) {

            TopPanelManager.isPanelActive = false;

            ballAnim.SetBool("Default",true);

            GetComponent<CircleCollider2D>().enabled = true;
            
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
