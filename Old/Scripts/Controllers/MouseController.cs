using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {
    float lastCamPositionY;
    float lastCamPositionX;


    public GameObject PlanetUIwindow;
    public Text PlanetName;
	// Use this for initialization
    Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));
	void Start () {
        PlanetUIwindow.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
        float currentCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        float currentCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        if (Input.GetMouseButton(2) || Input.GetMouseButton(0)) //middle or left mouse button
        {
            Camera.main.transform.Translate((new Vector3( lastCamPositionX, 0, 0) - new Vector3( currentCamPositionX, 0, 0)));
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            PlanetUIwindow.SetActive(false);

        }

        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                foreach (Planet p in SolarSystem.Instance.Planets)
                {
                    if(p.Name + "_GO" == hit.transform.name)
                    {
                        Debug.Log(hit.transform.name);
                        PlanetUIwindow.SetActive(true);
                        PlanetName.text = p.Name;
                        break;
                    }
                }

            }else
            {
                
            }
        }


    }
}
