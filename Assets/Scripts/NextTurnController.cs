using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
public class NextTurnController : MonoBehaviour {
    private Func<bool> GalaxysNextTurnDataGenerated;
    private Func<bool> GalaxysNextTurnsGraphicsDrawn;

    private NextTurnController() { }
    public static NextTurnController Instance;
    void Awake()
    {
        Debug.Log("NextTurnController awake()");
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //generate galaxy data for the next turn
            if (GalaxysNextTurnDataGenerated != null)
            {
                if (GalaxysNextTurnDataGenerated() != true)
                {
                    return;
                }
            }
            //generate AI ship data for the next turn

            //generate Main ship data for the next turn

            //generate Enemy AI ship data for the next turn

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
}
