using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    private Vector3 mousePos;

    public Material material;

    private float speed = 40000f;
    private float distance = 500f;

    private bool addForceBool;

    private Rigidbody2D rb;
    private TrailRenderer tr;
    private LineRenderer line;

    void Start() {

        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();

    }


    void Update() {

        GetMousePosition();
        SetBallPos();
        DrawLine();

    }


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
                createLine();
            }
            //get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //set the z co ordinate to 0 as we are only want the x,y axes
            mousePos.z = 0;
            //set the start point and end point of the line renderer
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        }

          //if mouse button is held clicked and line exists
          else if (Input.GetMouseButton(0) && line) {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;
                    //set the end position as current position but dont set line as null
                    line.SetPosition(1, mousePos);
                }

          //if line renderer exists and left mouse button is released
          else if (Input.GetMouseButtonUp(0) && line) {

              //Destroy Previously created line
              Destroy(GameObject.FindGameObjectWithTag("LINE"));

            }
    }

    //Setting width and material of Line
    void createLine() {
    //create a new empty gameobject and line renderer component
    line = new GameObject("Line").AddComponent<LineRenderer>();
    //assign the material to the line
    line.material = material;
    //set the number of points to the line
    line.SetVertexCount(2);
    //set the width
    line.SetWidth(0.02f, 0.02f);
    //render line to the world origin and not to the object's position
    line.useWorldSpace = true;

    //set tag for destruction later (Destroyed in DrawLine Method)
    line.gameObject.tag = "LINE";

}
    

    void SetBallPos() {

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y,distance);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);

            //Stop Movement
            rb.isKinematic = true;
            rb.isKinematic = false;

            //Reset Trail
            tr.Clear();

        }

    }


    void GetMousePosition() {

        if (Input.GetMouseButtonDown(0)) {

            //save began touch 2d point
            //Origin point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }

        if (Input.GetMouseButtonUp(0)) {
            //Destination point
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            currentSwipe.Normalize();

            addForceBool = true;


        }

    }


}
