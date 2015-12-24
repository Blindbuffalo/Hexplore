using UnityEngine;
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
                FetchMissionStart,
                FetchMissionProgress,
                FetchMissionEnd,
                new List<Goal>() {
                        new FetchGoal (
                            "Get Me the Things!", 
                            "get the guy an engine.", 
                            "Earth", 
                            "Earth",
                            new ShipPart("Engine", 1f, 2f, 0, 999f, ShipPartType.Engine))

                    }
                )
        };


    }
    public void FetchMissionStart(MainStoryMission m)
    {
        Debug.Log("MSM1:Start");
        Debug.Log(m.Info);
        MissionP = MissionProgress.progressing;

    }
    public void FetchMissionProgress(MainStoryMission m)
    {
        Debug.Log("MSM1:progress");
        Debug.Log(m.Info);
        if(MissionP == MissionProgress.progressing)
        {
            Hex PLoc;
            foreach (Goal g in m.MissionGoals)
            {
                FetchGoal f = g as FetchGoal;
                if (f != null)
                {
                    foreach (Planet p  in SolarSystem.Instance.Planets)
                    {
                        if(p.Name == f.DropOffLocation)
                        {
                            PLoc = p.Orbit[p.CurrentPosition];
                            List<Hex> ShipsNeighbors = Hex.Neighbors(CharController.Instance.MainShip.CurrentHexPosition);
                            if (ShipsNeighbors.Contains(PLoc))
                            {
                                Debug.Log("im near " + f.DropOffLocation);
                                foreach (Cargo c in CharController.Instance.MainShip.Cargohold.Hold)
                                {
                                    ShipPart sp = c as ShipPart;
                                    if(sp != null)
                                    {
                                        ShipPart CargoNeeded = f.FetchItem as ShipPart;
                                        if (CargoNeeded != null) {
                                            if (sp.Type == CargoNeeded.Type)
                                            {
                                                Debug.Log("i have a " + CargoNeeded.Type.ToString() + " in my cargo hold.");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
    }
    public void FetchMissionEnd(MainStoryMission m)
    {
        Debug.Log("MSM1:end");
        Debug.Log(m.Info);
    }

    public void foo(MainStoryMission G)
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
