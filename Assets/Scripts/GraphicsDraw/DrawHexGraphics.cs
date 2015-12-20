using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawHexGraphics : MonoBehaviour {
    public Dictionary<string, GameObject> HexGraphics = null;
    public Utilites Utility;
    private Layout L = new Layout(Layout.pointy, new Point(.52, .52), new Point(0, 0));
    void Start()
    {
        Debug.Log("hexgraphics Start");
        
    }
    public void GenerateGraphics(List<Hex> Hexes, GameObject Prefab)
    {
        HexGraphics = new Dictionary<string, GameObject>();
        foreach (Hex h in Hexes)
        {
            Point p = Layout.HexToPixel(L, h);

            if (HexGraphics.ContainsKey(Utility.HexNameStr(h)))
            {

            }
            else
            {

                GameObject g = (GameObject)Instantiate(Prefab, new Vector3((float)p.x, (float)p.y, 0), Quaternion.identity);
                g.name = Utility.HexNameStr(h);
                g.transform.SetParent(this.transform);
                HexGraphics.Add(Utility.HexNameStr(h), g);

            }

        }
    }
    public void CreateMovementHexGraphics(List<Hex> Hexes, GameObject Prefab, GameObject parent)
    {
        
        foreach (Hex h in Hexes)
        {
            Point p = Layout.HexToPixel(L, h);

            GameObject g = (GameObject)Instantiate(Prefab, new Vector3((float)p.x, (float)p.y, 0), Quaternion.identity);
            g.name = Utility.HexNameStr(h);
            g.transform.SetParent(parent.transform);
                

        }
    }
    public void ChangeHexesColor(Hex h, Color color)
    {
        string HexName = Utility.HexNameStr(h);
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
        Planet.CurrentPosition = Planet.CurrentPosition + Planet.NumberOfMoves;

        if (Planet.CurrentPosition >= Planet.Orbit.Count)
        {
            Planet.CurrentPosition = Planet.CurrentPosition - Planet.Orbit.Count;

        }
        ChangeHexesColor(Planet.Orbit[Planet.LastPosition], OrbitColor);
        ChangeHexesColor(Planet.Orbit[Planet.CurrentPosition], Planet.Col);

        Planet.LastPosition = Planet.CurrentPosition;
    }
    public void DrawPlanetObject(Planet planet, GameObject PlanetPrefab, GameObject Sun, GameObject Rings)
    {
        Point p = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition]);

        GameObject GO = (GameObject)Instantiate(PlanetPrefab, new Vector3((float)p.x, (float)p.y, 10f), Quaternion.identity);
        GO.name = planet.Name + "_GO";
        GO.transform.localScale *= planet.Size;

        if (planet.Rings != null)
        {
            GameObject RingsGO = (GameObject)Instantiate(Rings, new Vector3((float)p.x, (float)p.y, 10f), Quaternion.identity);
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
        Point p = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition]);
        
        GameObject pGO = GetPlanetGO(planet, Sun);
        pGO.transform.position = new Vector3((float)p.x, (float)p.y, 10f);

    }
}
