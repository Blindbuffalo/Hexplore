﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawGraphics : MonoBehaviour {

    private static DrawGraphics instance;

    public GameObject PlanetPrefab;
    public GameObject HexPrefab;
    public GameObject Rings;

    private DrawGraphics() { }

    

    public static DrawGraphics Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (DrawGraphics)FindObjectOfType(typeof(DrawGraphics));
                if (instance == null)
                    instance = (new GameObject("DrawGraphics")).AddComponent<DrawGraphics>();
            }
            return instance;
        }
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
        GalaxyController.Instance.RegisterOnNextTurn(OnNextTurn);
    }
    
    public Dictionary<string, GameObject> HexGraphics = null;

    private Layout L = new Layout(Layout.pointy, new Vector3(1f, 1f), new Vector3(0f, 0f));

    private void OnNextTurn(SolarSystem Sol)
    {
        foreach (KeyValuePair<string, Planet> p in Sol.Planets)
        {
            MovePlanetObject(p.Value);
            MovePlanetHex(p.Value);

        }
        
    }

    public void DrawSolarSystem(SolarSystem Sol)
    {
        this.transform.localScale = new Vector3(Sol.SunRadius, Sol.SunRadius, Sol.SunRadius);
        foreach( KeyValuePair<string,Planet> p in Sol.Planets)
        {
            DrawPlanetObject(p.Value);
            DrawHex(p.Value.Orbit[p.Value.CurrentPosition], p.Value.Name, p.Value.Col);
            Hex CenterSunHex = new Hex(0, 0, 0);
            List<Hex> SunHexes = Hex.Neighbors(CenterSunHex);
            DrawHex(CenterSunHex, "Sun_HEX", Sol.OrbitColor);
            foreach (Hex h in SunHexes)
            {
                DrawHex(h, "Sun_HEX", Sol.OrbitColor);
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
        pHex.transform.position = Layout.HexToPixel(L, planet.Orbit[planet.CurrentPosition], 8f);

    }

    public void MovePlanetObject(Planet planet)
    {       
        GameObject pGO = GetPlanetGO(planet);
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
