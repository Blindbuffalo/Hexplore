using UnityEngine;
using System.Collections;
using System;

public class DrawPlanetGraphics : MonoBehaviour {

    private bool PlanetDrawn = false;
    private bool AdvanceTurn = false;

    public static DrawPlanetGraphics Instance;
    void Awake()
    {
        Debug.Log("DrawPlanetGraphics awake()");
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }

    }
    // Use this for initialization
    void Start () {
        NextTurnController.Instance.RegisterGalaxyNextTurnsGraphicsDrawn(OnNextTurn);
    }
	void Destroy()
    {
        Debug.Log("DrawPlanetGraphics destroy()");
        NextTurnController.Instance.UnregisterGalaxyNextTurnsGraphicsDrawn(OnNextTurn);
    }
	// Update is called once per frame
	void Update () {
        if (PlanetDrawn == false)
        {
            PlanetDrawn = true;
        }
        if (AdvanceTurn == true)
        {




            if (true)
            {

                AdvanceTurn = false;
                NextTurnController.Instance.DrawingComplete();
            }
        }
    }

    #region CallBacks
    private bool OnNextTurn()
    {
        try
        {
            Debug.Log("Draw Planet Next turn data");


            AdvanceTurn = true;
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }

    }
    #endregion
}
