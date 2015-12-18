using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {
    float lastCamPositionY;
    float lastCamPositionX;
    
    public Utilites Utility;
    public DrawHexGraphics HexG;
	// Use this for initialization
    Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        float currentCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float currentCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) //middle or right mouse button
        {
            Camera.main.transform.Translate(-(new Vector3( lastCamPositionX, 0, 0) - new Vector3( currentCamPositionX, 0, 0)));
            Camera.main.transform.Translate(new Vector3(0, lastCamPositionY, 0) - new Vector3(0, currentCamPositionY, 0));
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0) //back
        {
            Camera.main.orthographicSize += 1;
            
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //back
        {
            Camera.main.orthographicSize -= 1;
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1f, 50f);
        lastCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        lastCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;





	}
}
