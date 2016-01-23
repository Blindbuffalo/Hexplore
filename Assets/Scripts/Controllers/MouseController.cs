using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {
    float lastCamPositionY;
    float lastCamPositionX;

    public Vector3 point = new Vector3(0, 0, 0);
    //public GameObject PlanetUIwindow;
    //public Text PlanetName;
	// Use this for initialization
    Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));
	void Start () {
        //PlanetUIwindow.SetActive(false);

    }

        private const int LevelArea = 2000;

        private const int ScrollArea = 100;
        private const int ScrollSpeed = 50;
        private const int DragSpeed = 100;

        private const int ZoomSpeed = 25;
        private const int ZoomMin = 25;
        private const int ZoomMax = 1000;

        private const int PanSpeed = 50;
        private const int PanAngleMin = 20;
        private const int PanAngleMax = 90;

        // Update is called once per frame
        void Update()
        {
            // Init camera translation for this frame.
            var translation = Vector3.zero;

        // Zoom in or out
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.direction, Color.red);
        var zoomDelta = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed * Time.deltaTime;
        if (zoomDelta != 0)
        {
            translation -= new Vector3 (Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z * zoomDelta);

            Camera.main.transform.localPosition = translation;
            Debug.Log(translation);
        }

        // Move camera with arrow keys
        translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Move camera with mouse
            if (Input.GetMouseButton(0)) // MMB
            {
                // Hold button and drag camera around
                translation -= new Vector3(Input.GetAxis("Mouse X") * DragSpeed * Time.deltaTime, 0,
                                   Input.GetAxis("Mouse Y") * DragSpeed * Time.deltaTime);
            }


            // Keep camera within level and zoom area
            //var desiredPosition = Camera.main.transform.position + translation;
            //if (desiredPosition.x < -LevelArea || LevelArea < desiredPosition.x)
            //{
            //    translation.x = 0;
            //}
            //if (desiredPosition.y < ZoomMin || ZoomMax < desiredPosition.y)
            //{
            //    translation.y = 0;
            //}
            //if (desiredPosition.z < -LevelArea || LevelArea < desiredPosition.z)
            //{
            //    translation.z = 0;
            //}

            // Finally move camera parallel to world axis
            Camera.main.transform.position += translation;
        }
    
    // Update is called once per frame
    //void Update () {
    //    float currentCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
    //    float currentCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
    //    if (Input.GetMouseButton(2) || Input.GetMouseButton(0)) //middle or left mouse button
    //    {
    //        Camera.main.transform.Translate((new Vector3( lastCamPositionX, 0, 0) - new Vector3( currentCamPositionX, 0, 0)));
    //        Camera.main.transform.Translate(new Vector3(0, lastCamPositionY, 0) - new Vector3(0, currentCamPositionY, 0));
    //    }


    //    if (Input.GetAxis("Mouse ScrollWheel") < 0) //back
    //    {
    //        Camera.main.orthographicSize += 1;
            
    //    }
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0) //back
    //    {
    //        Camera.main.orthographicSize -= 1;
    //    }


    //    Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1f, 50f);
    //    lastCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
    //    lastCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;


    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {

    //    }

    //    if (Input.GetMouseButtonDown(0))
    //    { // if left button pressed...

    //    }


    //}
}
