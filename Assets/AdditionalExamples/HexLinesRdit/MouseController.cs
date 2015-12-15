using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {
    Vector3 lastCamPosition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currentCamPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Camera.main.transform.Translate(lastCamPosition - currentCamPosition);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //back
        {
            Camera.main.orthographicSize += 10;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //back
        {
            Camera.main.orthographicSize -= 10;
        }

        lastCamPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
