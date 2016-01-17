using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DrawSolarSystemGraphics : MonoBehaviour {

    public GameObject PlanetPrefab;
    public GameObject HexPrefab;
    public GameObject Rings;
    public GameObject AIship;

    private DrawSolarSystemGraphics() { }

    private bool SystemDrawn = false;
    private bool AdvanceTurn = false;
    private bool shipsAnimated = false;

    public static DrawSolarSystemGraphics Instance;
    void Awake()
    {
        Debug.Log("DrawSolarSystemGraphics awake()");
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }
    void Start()
    {
        Debug.Log("DrawSolarSystemGraphics start()");
        NextTurnController.Instance.RegisterGalaxyNextTurnsGraphicsDrawn(OnNextTurn);
        GalaxyController.Instance.RegisterSolarSystemChanged(OnSolarSystemChanged);
    }
    void Update()
    {
        if (SystemDrawn == false)
        {
            DrawSolarSystem(GalaxyController.Instance.GetCurrentSolarSystem());
            SystemDrawn = true;
        }
        if(AdvanceTurn == true)
        {




            if (true)
            {
                //if all of the Orbiting objects (maybe ships?) have moved then set advanced turn to false

                SolarSystem Sol = GalaxyController.Instance.GetCurrentSolarSystem();

                //move all planets
                foreach (KeyValuePair<string, Planet> p in Sol.Planets)
                {
                    MovePlanetObject(p.Value);
                    MovePlanetHex(p.Value);

                }
                //move or spawn all ships
                shipsAnimated = true;
                foreach (KeyValuePair<string, Ship> sKV in Sol.Ships)
                {
                    Ship s = sKV.Value;
                    if(GetGOHex(s.Name ) == null)
                    {

                        //ship has not been spawned yet, so we will need to do that
                        DrawHex(s.CurrentHexPosition, s.Name, Color.yellow);
                    }
                    else
                    {

                        //ship has been spawned so just update its position
                        //MoveShipHex(s);

                        if (s.PathToTarget == null)
                        {


                        }
                        else
                        {
                            Vector3 dir = Layout.HexToPixel(L, s.PathToTarget[s.Movement + s.PositionOnPath], 8f) - Layout.HexToPixel(L, s.PathToTarget[s.PositionOnPath], 8f);
                            Vector3 vel = dir.normalized * s.Movement * Time.deltaTime;
                            vel = Vector3.ClampMagnitude(vel, dir.magnitude);
                            GetGOHex(s.Name).transform.Translate(vel);
                            if (GetGOHex(s.Name).transform.position != Layout.HexToPixel(L, s.PathToTarget[s.Movement + s.PositionOnPath], 8f))
                            {
                                shipsAnimated = false;
                            }
                            else
                            {

                            }
                        }
                    }
                }

                if (shipsAnimated)
                {
                    AdvanceTurn = false;
                    NextTurnController.Instance.DrawingComplete();
                }
            }
        }


    }
    void OnDestroy()
    {
        Debug.Log("Drawing System destroy()");
        NextTurnController.Instance.UnregisterGalaxyNextTurnsGraphicsDrawn(OnNextTurn);
        GalaxyController.Instance.UnregisterSolarSystemChanged(OnSolarSystemChanged);
    }
    public Dictionary<string, GameObject> HexGraphics = null;

    private Layout L = new Layout(Layout.pointy, new Vector3(1f, 1f), new Vector3(0f, 0f));

    #region CallBacks
    private bool OnNextTurn()
    {
        try
        {
            Debug.Log("Draw Next turn data");


            AdvanceTurn = true;
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }

    }
    //private void OnSolarSystemCreated(SolarSystem Sol)
    //{
    //    Debug.Log("SolarSys Created CB fired");
    //    DrawSolarSystem(Sol);
    //}
    private void OnSolarSystemChanged()
    {
        Debug.Log("SolarSys Changed CB fired");
        EraseSolarSystem();
        SystemDrawn = false;
    }
    #endregion

    public void DrawSolarSystem(SolarSystem Sol)
    {
        Debug.Log("Drawing Solar System");

        //size the sun object (the script is on the sun)
        this.transform.localScale = new Vector3(Sol.SunRadius, Sol.SunRadius, Sol.SunRadius);
        //lay down some hexes under the sun!
        Hex CenterSunHex = new Hex(0, 0, 0);
        List<Hex> SunHexes = Hex.Neighbors(CenterSunHex);
        DrawHex(CenterSunHex, "Sun_HEX", Color.red);
        foreach (Hex h in SunHexes)
        {
            DrawHex(h, "Sun_HEX_n", Color.red);
        }

        //draw initial planets
        foreach ( KeyValuePair<string,Planet> p in Sol.Planets)
        {
            DrawPlanetObject(p.Value);
            DrawHex(p.Value.Orbit[p.Value.CurrentPosition], p.Value.Name, p.Value.Col);

            
        }
        //draw some ships
        foreach (KeyValuePair<string, Ship> s in Sol.Ships)
        {

            if (GetGOHex(s.Value.Name) == null)
            {
                Debug.Log("Drawing ships: Ship needs to be rendered");
                //ship has not been spawned yet, so we will need to do that
                DrawHex(s.Value.CurrentHexPosition, s.Value.Name, Color.yellow);
            }

        }
    }

    public void EraseSolarSystem()
    {
        var children = new List<GameObject>();
        foreach (Transform child in DrawSolarSystemGraphics.Instance.transform) children.Add(child.gameObject);

        foreach (GameObject c in children)
        {
            if (c.name.Contains("~DND~") == false)
            {
                Destroy(c);
            }
        }
    }






    private void DrawPlanetObject(Planet planet)
    {
        Vector3 p = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], 10f);
        GameObject GO = (GameObject)Instantiate(PlanetPrefab, p, Quaternion.identity);
        GO.name = planet.Name + "_GO";
        GO.transform.localScale *= planet.Size;

        if (planet.Rings != null)
        {
            GameObject RingsGO = (GameObject)Instantiate(Rings, p, Quaternion.identity);
            RingsGO.name = planet.Name + "_Rings";
            RingsGO.GetComponent<SpriteRenderer>().color = planet.Rings.RingColor;
            RingsGO.transform.SetParent(GO.transform);
            RingsGO.transform.localScale = new Vector3( planet.Rings.RingsScale, planet.Rings.RingsScale, planet.Rings.RingsScale);
        }

        
        GO.transform.SetParent(this.transform);
    }

    public void DrawHex(Hex h, string ObjectName, Color C, int offset = 0)
    {
        GameObject g = (GameObject)Instantiate(HexPrefab, Layout.HexToPixel(L, h, 8), Quaternion.identity);

        g.name = ObjectName + "_Hex";
        //g.GetComponent<SpriteRenderer>().color = Planet.Col;
        g.GetComponentInChildren<Renderer>().material.color = C;
        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y - offset, g.transform.position.z);
        g.transform.SetParent(this.transform);
    }
    public void MovePlanetHex(Planet planet)
    {
        GameObject pHex = GetGOHex(planet.Name);
        if(pHex != null)
            pHex.transform.position = Layout.HexToPixel(L, planet.GetCurrentHexPosition(), 8f);

    }

    public void MovePlanetObject(Planet planet)
    {       
        GameObject pGO = GetGO(planet.Name);
        if(pGO != null)
            pGO.transform.position = Layout.HexToPixel(L, planet.GetCurrentHexPosition(), 10f);

    }

    public void MoveShipHex(Ship ship)
    {
        GameObject pGO = GetGOHex(ship.Name);
        
        if (pGO != null)
        {
            Debug.Log("!!!!");
            pGO.transform.position = Layout.HexToPixel(L, ship.CurrentHexPosition, 8f);
        }
            

    }
    public GameObject GetGO(string GOname)
    {
        string NameCheck = GOname + "_GO";
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject pGO = this.transform.GetChild(i).gameObject;
            if (pGO.name == NameCheck)
            {
                return pGO;
            }
        }
        return null;
    }

    public GameObject GetGOHex(string name)
    {
        string NameCheck = name + "_Hex";
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject pHex = this.transform.GetChild(i).gameObject;
            if (pHex.name == NameCheck)
            {
                return pHex;
            }
        }
        return null;
    }


}
