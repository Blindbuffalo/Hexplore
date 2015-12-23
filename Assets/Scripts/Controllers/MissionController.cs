﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class MissionController : MonoBehaviour{
    public enum MissionProgress { notStarted, progressing, ending }
    
    public GameObject MissionMarkerPrefab;
    public GameObject MainShip;
    private MissionController() { }
    public int CurrentMission = 0;
    public int CurrentGoal = 0;
    public MissionProgress MissionP = MissionProgress.notStarted;

    private static MissionController instance;
    public static MissionController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (MissionController)FindObjectOfType(typeof(MissionController));
                if (instance == null)
                    instance = (new GameObject("MissionController")).AddComponent<MissionController>();
            }
            return instance;
        }
    }


    public List<MainStoryMission> MainStoryMissions;


    public void InitMissions()
    {
        MainStoryMissions = new List<MainStoryMission>()
        {
            new MainStoryMission(
                "Good Day to Fly.",
                "Things about stuff said here!",
                10,
                new List<Goal>() {
                        new FetchGoal("Get Me the Things!", "get the guy an engine.", "Earth", "Earth", FetchMissionStart, FetchMissionProgress, FetchMissionEnd, new ShipParts("Engine", 1f, 2f, 0, 999f, ShipPartType.Engine))

                    }
                )
        };


    }
    public void FetchMissionStart(Goal g)
    {
        Debug.Log("MSM1:Start");
        Debug.Log(g.Description);
        MissionP = MissionProgress.progressing;

    }
    public void FetchMissionProgress(Goal g)
    {
        Debug.Log("MSM1:progress");
        Debug.Log(g.Description);
        if(MissionP == MissionProgress.progressing)
        {
            //TODO: check ships neigbhors to see if the location is in one of the hexes



            if(CharController.Instance.MainShip.Cargohold.Hold == null)
            {
                Debug.Log("null");
            }
            else
            {
                Debug.Log("is a shippart");
                foreach(Cargo s in CharController.Instance.MainShip.Cargohold.Hold)
                {

                }
                
            }
        }
    }
    public void FetchMissionEnd(Goal g)
    {
        Debug.Log("MSM1:end");
        Debug.Log(g.Description);
    }

    public void foo(Goal G)
    {

    }
    //private Layout L = new Layout(Layout.pointy, new Vector3(.52f, .52f), new Vector3(0f, 0f));


    //public int CurrentMSMission = -1;
    //public int _MSMissionStep = 0;
    //public int MSMissionStep
    //{
    //    get
    //    {
    //        return _MSMissionStep;
    //    }
    //    set
    //    {
    //        _MSMissionStep = value;
    //        CheckMissionStatus();
    //    }
    //}
    //public bool MSMissionCompleted = false;
    //public void SpawnMissions(SolarSystem Sol, GameObject Sun)
    //{
    //    if(CurrentMSMission == -1 || MSMissionCompleted)
    //    {
    //        CurrentMSMission++;

    //        Debug.Log(MainStoryMissions[CurrentMSMission].Name);

    //        MainStoryMissions[CurrentMSMission].Start(MainStoryMissions[CurrentMSMission]);

    //        Planet P = (from s in Sol.Planets
    //                    where s.Name.ToLower() == MainStoryMissions[CurrentMSMission].LocationName.ToLower()
    //                    select s).FirstOrDefault();
    //        if (P == null)
    //        {
    //            Debug.Log("oops");
    //        }
    //        else
    //        {

    //            GameObject MissionMarker = (GameObject)Instantiate(MissionMarkerPrefab, Layout.HexToPixel(L, P.Orbit[P.CurrentPosition], -10.2f), Quaternion.identity);

    //            MissionMarker.transform.SetParent(DrawHexGraphics.Instance.GetPlanetGO(P, Sun).transform);
    //            MissionMarker.transform.localScale = new Vector3(.5f, .5f, .5f);
    //            MissionMarker.transform.localPosition = new Vector3(0f, .46f, -1f); ;
    //        }


    //        MSMissionCompleted = false;
    //    }
    //}


}
