using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public class NextTurnController : MonoBehaviour {
    private Func<bool> GalaxysNextTurnDataGenerated;
    private Func<bool> GalaxysNextTurnsGraphicsDrawn;

    private Func<bool> AIshipsNextTurnDataGenerated;
    private Func<bool> AIshipsNextTurnsGraphicsDrawn;

    private bool WaitingForSystemToBeDrawn = false;


    private int PlanetTurnCount = 0;
    private int TurnCountMax = 10;


    private NextTurnController() { }
    public static NextTurnController Instance;
    void Awake()
    {
        //Debug.Log("NextTurnController awake()");
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        NextTurnController.Instance.PlanetTurnCount = 0;
    }
    void Start () {
        //Debug.Log("NextturnController Start()");
        
	}
	
	// Update is called once per frame
	void Update () {

        Scene CurrentScene = SceneManager.GetActiveScene();
        
        if(Input.GetKeyDown(KeyCode.Space) && WaitingForSystemToBeDrawn == false && CurrentScene.name == "Planet")
        {
            WaitingForSystemToBeDrawn = true;
            PlanetTurnCount++;
            Debug.Log("Planet: " + PlanetTurnCount);

            if(PlanetTurnCount == TurnCountMax)
            {
                if (AdvanceGalaxyNextTurnData())
                {
                    PlanetTurnCount = 0;
                }
                else
                {
                    PlanetTurnCount = -1;
                }
                
            }
            DrawingComplete();
        }

        if (Input.GetKeyDown(KeyCode.Space) && WaitingForSystemToBeDrawn == false && CurrentScene.name == "SolarSystem")
        {
            WaitingForSystemToBeDrawn = true;
            //need to set ui to show that a turn is being processed


            if (AdvanceGalaxyNextTurnData() == false)
                return;

            //Draw all the things
            if (GalaxysNextTurnsGraphicsDrawn != null)
            {
                if (GalaxysNextTurnsGraphicsDrawn() != true)
                {
                    return;
                }
            }
        }

        
    }

    public bool AdvanceGalaxyNextTurnData()
    {
        //generate galaxy data for the next turn
        if (GalaxysNextTurnDataGenerated != null)
        {
            if (GalaxysNextTurnDataGenerated() != true)
            {
                return false;
            }
        }
        //generate AI ship data for the next turn

        //generate Main ship data for the next turn
        if (AIshipsNextTurnDataGenerated != null)
        {
            if (AIshipsNextTurnDataGenerated() != true)
            {
                return false;
            }
        }
        //generate Enemy AI ship data for the next turn


        return true;
    }
    public void DrawingComplete()
    {

        //setup the ui to show that a turn can be triggered

        Debug.Log("DrawingComplete CB");

        WaitingForSystemToBeDrawn = false;
    }
    public void RegisterGalaxysNextTurnData(Func<bool> func)
    {
        GalaxysNextTurnDataGenerated += func;
    }
    public void UnregisterGalaxysNextTurnData(Func<bool> func)
    {
        GalaxysNextTurnDataGenerated -= func;
    }

    public void RegisterGalaxyNextTurnsGraphicsDrawn(Func<bool> func)
    {
        GalaxysNextTurnsGraphicsDrawn += func;
    }
    public void UnregisterGalaxyNextTurnsGraphicsDrawn(Func<bool> func)
    {
        GalaxysNextTurnsGraphicsDrawn -= func;
    }

    public void RegisterAIshipsNextTurnData(Func<bool> func)
    {
        AIshipsNextTurnDataGenerated += func;
    }
    public void UnregisterAIshipsNextTurnData(Func<bool> func)
    {
        AIshipsNextTurnDataGenerated -= func;
    }
}
