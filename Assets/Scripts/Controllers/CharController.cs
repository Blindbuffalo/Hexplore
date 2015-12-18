using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
public class CharController : MonoBehaviour {

    public DrawHexGraphics HexG;
    public Utilites Utility;
    public GameObject Prefab;
    Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));
    public Hex Center = new Hex(0, 0, 0);
    public Ship MainShip;
    public bool shipMoving = false;
    public Action<int> Test;
    public Hex ClickedHex;
    // Use this for initialization
    void Start () {
        MainShip = new Ship(3, new Hex(5, 0, -5));
        MainShip.RegisterMovesLeftCB(RedrawMovementHexes);

        placeShipOnHex(MainShip.CurrentHexPosition);

        MainShip.MovesLeft = MainShip.Movement;
        //List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);
        //HexG.CreateMovementHexGraphics(Reachable, Prefab, this.transform.gameObject);


    }
	
	// Update is called once per frame
	void Update () {

        //if (shipMoving)
        //{
            
        //    moveShipToHex(ClickedHex);

        //    if(this.transform.position == Utility.HexToVector3(ClickedHex))
        //    {
        //        shipMoving = false;
                
        //    }
        //}
        //else
        //{
            float currentCamPositionY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            float currentCamPositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if (Input.GetMouseButton(0)) //left mouse button
            {
                Point P = new Point(currentCamPositionX, currentCamPositionY);
                FractionalHex FH = Layout.PixelToHex(L, P);
                ClickedHex = FractionalHex.HexRound(FH);

                int d = Hex.Distance(MainShip.CurrentHexPosition, ClickedHex);

                if (d > MainShip.MovesLeft)
                {
                    //Debug.LogError("you clicked outside of the allowed movement area.");
                }
                else
                {
                    
                    shipMoving = true;
                    MainShip.ShipMoved(ClickedHex);

                    placeShipOnHex(ClickedHex);

                }
            }
        //}
    }

    //private void moveShipToHex(Hex h)
    //{
    //    Point P2 = Layout.HexToPixel(L, h);
    //    this.transform.position = new Vector3((float)P2.x, (float)P2.y, 10f);

    //    this.transform.Translate(new Vector3((float)P2.x, (float)P2.y, 10f).normalized * Time.deltaTime);
    //}
    private void placeShipOnHex(Hex h)
    {



        Point P2 = Layout.HexToPixel(L, h);

        //Vector3 dir = Utility.HexToVector3(h) -this.transform.position;

        //float speed = 1;

        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.left);
        //transform.rotation = q;

        this.transform.position = new Vector3((float)P2.x, (float)P2.y, 10f);
        
    }
    void RedrawMovementHexes()
    {
        List<Hex> Reachable = Hex.Reachable(MainShip.CurrentHexPosition, MainShip.MovesLeft);

        foreach (Transform child in transform)
        {
            if (!child.name.StartsWith("ship"))
            {
                GameObject.Destroy(child.gameObject);
            }
            
        }

        HexG.CreateMovementHexGraphics(Reachable, Prefab, this.transform.gameObject);
    }
}
