using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawGraphics : MonoBehaviour {

    public GameObject PlanetPrefab;
    public GameObject HexPrefab;
    public GameObject Rings;
    public GameObject AIship;

    private DrawGraphics() { }

    private bool SystemDrawn = false;

    public static DrawGraphics Instance;
    void Awake()
    {
        Debug.Log("Drawing System awake()");
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
        Debug.Log("Drawing System start()");
        GalaxyController.Instance.RegisterNextTurnCycled(OnNextTurn);
        GalaxyController.Instance.RegisterSolarSystemChanged(OnSolarSystemChanged);
    }
    void Update()
    {
        if (SystemDrawn == false)
        {
            DrawSolarSystem(GalaxyController.Instance.GetCurrentSolarSystem());
            SystemDrawn = true;
        }
    }
    void OnDestroy()
    {
        Debug.Log("Drawing System destroy()");
        GalaxyController.Instance.UnregisterNextTurnCycled(OnNextTurn);
        GalaxyController.Instance.UnregisterSolarSystemChanged(OnSolarSystemChanged);
    }
    public Dictionary<string, GameObject> HexGraphics = null;

    private Layout L = new Layout(Layout.pointy, new Vector3(1f, 1f), new Vector3(0f, 0f));

    #region CallBacks
    private void OnNextTurn(SolarSystem Sol)
    {
        foreach (KeyValuePair<string, Planet> p in Sol.Planets)
        {
            MovePlanetObject(p.Value);
            MovePlanetHex(p.Value);

        }
        
    }
    private void OnSolarSystemCreated(SolarSystem Sol)
    {
        Debug.Log("SolarSys Created CB fired");
        DrawSolarSystem(Sol);
    }
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

        foreach ( KeyValuePair<string,Planet> p in Sol.Planets)
        {
            DrawPlanetObject(p.Value);
            DrawHex(p.Value.Orbit[p.Value.CurrentPosition], p.Value.Name, p.Value.Col);

            
        }
    }

    public void EraseSolarSystem()
    {
        var children = new List<GameObject>();
        foreach (Transform child in DrawGraphics.Instance.transform) children.Add(child.gameObject);

        foreach (GameObject c in children)
        {
            if (c.name.Contains("~DND~") == false)
            {
                Destroy(c);
            }
        }
    }



    public void GenerateGraphics(List<Hex> Hexes, GameObject Prefab)
    {
        //this function lays out all of the hex grid data as unity sprites on screen
        HexGraphics = new Dictionary<string, GameObject>();
        foreach (Hex h in Hexes)
        {

            if (HexGraphics.ContainsKey(Utilites.Instance.HexNameStr(h)))
            {

            }
            else
            {

                GameObject g = (GameObject)Instantiate(Prefab, Layout.HexToPixel(L, h, 0), Quaternion.identity);
                g.name = Utilites.Instance.HexNameStr(h);
                g.transform.SetParent(this.transform);
                HexGraphics.Add(Utilites.Instance.HexNameStr(h), g);

            }

        }
    }

    public void ChangeHexesColor(Hex h, Color color)
    {
        string HexName = Utilites.Instance.HexNameStr(h);
        if (HexGraphics.ContainsKey(HexName))
        {
            GameObject HexGO = HexGraphics[HexName];
            HexGO.GetComponent<SpriteRenderer>().color = color;
            HexGraphics[HexName] = HexGO;
        }
    }
    public void DrawOrbit(Planet Planet, Color OrbitColor)
    {
        //changes the grid hex color for a certain planet so that the
        //orbit is visible
        foreach (Hex h in Planet.Orbit)
        {
            ChangeHexesColor(h, OrbitColor);
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

    public void DrawHex(Hex h, string ObjectName, Color C)
    {
        GameObject g = (GameObject)Instantiate(HexPrefab, Layout.HexToPixel(L, h, 8), Quaternion.identity);

        g.name = ObjectName + "_Hex";
        //g.GetComponent<SpriteRenderer>().color = Planet.Col;
        g.GetComponentInChildren<Renderer>().material.color = C;
        g.transform.SetParent(this.transform);
    }
    public void MovePlanetHex(Planet planet)
    {
        GameObject pHex = GetPlanetHex(planet);
        if(pHex != null)
            pHex.transform.position = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], 8f);

    }

    public void MovePlanetObject(Planet planet)
    {       
        GameObject pGO = GetPlanetGO(planet);
        if(pGO != null)
            pGO.transform.position = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], 10f);

    }
    public GameObject GetPlanetGO(Planet planet)
    {
        string planetNameCheck = planet.Name + "_GO";
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject pGO = this.transform.GetChild(i).gameObject;
            if (pGO.name == planetNameCheck)
            {
                return pGO;
            }
        }
        return null;
    }
    public GameObject GetPlanetHex(Planet planet)
    {
        string planetNameCheck = planet.Name + "_Hex";
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject pHex = this.transform.GetChild(i).gameObject;
            if (pHex.name == planetNameCheck)
            {
                return pHex;
            }
        }
        return null;
    }
}
