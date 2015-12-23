using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawHexGraphics : MonoBehaviour {

    private static DrawHexGraphics instance;

    private DrawHexGraphics() { }

    public static DrawHexGraphics Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (DrawHexGraphics)FindObjectOfType(typeof(DrawHexGraphics));
                if (instance == null)
                    instance = (new GameObject("DrawHexGraphics")).AddComponent<DrawHexGraphics>();
            }
            return instance;
        }
    }


    public Dictionary<string, GameObject> HexGraphics = null;

    private Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));
    public void GenerateGraphics(List<Hex> Hexes, GameObject Prefab)
    {
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
    public void CreateMovementHexGraphics(List<Hex> Hexes, GameObject Prefab, GameObject parent)
    {
        
        foreach (Hex h in Hexes)
        {
            GameObject g = (GameObject)Instantiate(Prefab, Layout.HexToPixel(L, h, 0), Quaternion.identity);
            g.name = Utilites.Instance.HexNameStr(h);
            g.transform.SetParent(parent.transform);
                

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
        foreach (Hex h in Planet.Orbit)
        {
            ChangeHexesColor(h, OrbitColor);
        }

        //return Hexes;
    }

    public void DrawPlanetHex(Planet Planet, Color OrbitColor)
    {
        if(Planet.OrbitDirection == OrbitDir.CCW)
        {
            int LP = Planet.CurrentPosition;
            Planet.CurrentPosition = Planet.CurrentPosition - Planet.NumberOfMoves;

            if (Planet.CurrentPosition < 0)
            {
                Planet.CurrentPosition = (Planet.Orbit.Count ) + (LP - Planet.NumberOfMoves);

            }
            ChangeHexesColor(Planet.Orbit[Planet.LastPosition], OrbitColor);
            ChangeHexesColor(Planet.Orbit[Planet.CurrentPosition], Planet.Col);

            Planet.LastPosition = Planet.CurrentPosition;
        }
        else
        {
            Planet.CurrentPosition = Planet.CurrentPosition + Planet.NumberOfMoves;

            if (Planet.CurrentPosition >= Planet.Orbit.Count)
            {
                Planet.CurrentPosition = Planet.CurrentPosition - Planet.Orbit.Count;

            }
            ChangeHexesColor(Planet.Orbit[Planet.LastPosition], OrbitColor);
            ChangeHexesColor(Planet.Orbit[Planet.CurrentPosition], Planet.Col);

            Planet.LastPosition = Planet.CurrentPosition;
        }

    }
    public void DrawPlanetObject(Planet planet, GameObject PlanetPrefab, GameObject Sun, GameObject Rings)
    {
        Vector3 p = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], -10f);
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

        
        GO.transform.SetParent(Sun.transform);
    }
    public GameObject GetPlanetGO(Planet planet, GameObject Sun)
    {
        string planetNameCheck = planet.Name + "_GO";
        for (int i = 0; i < Sun.transform.childCount; i++)
        {
            GameObject pGO = Sun.transform.GetChild(i).gameObject;
            if (pGO.name == planetNameCheck)
            {
                return pGO;
            }
        }
        return null;
    }
    public void MovePlanetObject(Planet planet, GameObject Sun)
    {       
        GameObject pGO = GetPlanetGO(planet, Sun);
        pGO.transform.position = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], -10f);

    }
}
