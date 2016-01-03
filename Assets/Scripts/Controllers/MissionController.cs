using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class MissionController : MonoBehaviour{
    public enum MissionProgress { notStarted, progressing, ending }
    
    public GameObject MissionMarkerPrefab;
    //public GameObject MainShip;
    private MissionController() { }
    public int CurrentMission = 0;

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
    public bool StartCurrentMainMission()
    {
        if (MissionP == MissionProgress.notStarted)
        {
            MissionP = MissionProgress.progressing;
            MainStoryMissions[CurrentMission].Start(MainStoryMissions[CurrentMission]);
            Debug.Log("mission (" + CurrentMission + ") " + MainStoryMissions[CurrentMission].Name + " started!");
            return true;
        }
        else
        {
            return false;
        }
       
    }

    private bool EndCurrentMainMission()
    {
        if (MissionP == MissionProgress.progressing)
        {

            CharController.Instance.IncreaseXP(MainStoryMissions[CurrentMission].XPreward);


            MissionP = MissionProgress.notStarted;
            CurrentMission += 1;
            
            return true;
        }
        else
        {
            return false;
        }

    }
    public void InitMissions()
    {
        MainStoryMissions = new List<MainStoryMission>()
        {
            new MainStoryMission(
                id: 0,
                prereq: -1,
                name: "Good Day to Fly.",
                info: "Things about stuff said here!",
                xpreward: 10,
                start: DeliveryMissionStart,
                missiongoals: new List<Goal>() {
                        new DeliveryGoal (
                            name: "Deliver the Hull plating to the guy!",
                            description: "get the guy some hull.",
                            startlocation: "Earth",
                            dropofflocation: "Earth",
                            deliveryitem: new ShipPart("New hull", 1f, 2f, 0, 999f, ShipPartType.HullPlate),
                            givenitem: new ShipPart("New hull", 1f, 2f, 0, 999f, ShipPartType.HullPlate),
                            goalprogress: DeliveryGoalCheck,
                            goalend: DeliveryGoalEnd,
                            status: GoalStatus.NA
                            )

                    }
                )
        };


    }
    private void DeliveryMissionStart(MainStoryMission m)
    {
        Debug.Log("MSM1:Start");
        Debug.Log(m.Info);
        MissionP = MissionProgress.progressing;
        foreach (Goal g in m.MissionGoals)
        {
            DeliveryGoal d = g as DeliveryGoal;
            if(d!= null)
            {
                CharController.Instance.MainShip.Cargohold.Hold.Add(d.GivenItem);
            }
            
        }
    }
    public void MainMissionProgress()
    {
        if (MissionP == MissionProgress.progressing)
        {

            MainStoryMission m = MainStoryMissions[CurrentMission];
            Debug.Log("MSM1:progress");
            if (MissionP == MissionProgress.progressing)
            {

                foreach (Goal g in m.MissionGoals)
                {
                    if (g as DeliveryGoal != null)
                    {
                        (g as DeliveryGoal).GoalProgress(g as DeliveryGoal);
                        if(g.Status == GoalStatus.CanTurnIn)
                        {
                            Debug.Log("TURN IT IN!!!!!");
                        }
                    }


                }
            }
        }
    }
    public void DeliveryGoalCheck(DeliveryGoal d)
    {
        Hex PLoc;
        if (d != null)
        {
            if(d.Status == GoalStatus.TurnedIn)
            {
                return;
            }
            foreach (Planet p in SolarSystem.Instance.Planets)
            {
                if (p.Name == d.DropOffLocation)
                {
                    PLoc = p.Orbit[p.CurrentPosition];
                    //List<Hex> ShipsNeighbors = Hex.Neighbors(CharController.Instance.MainShip.CurrentHexPosition);
                    //if (ShipsNeighbors.Contains(PLoc))
                    //{
                    if (Hex.Equals(PLoc, CharController.Instance.MainShip.CurrentHexPosition))
                    {
                        Debug.Log("im in same tile as " + d.DropOffLocation);
                        foreach (Cargo c in CharController.Instance.MainShip.Cargohold.Hold)
                        {
                            ShipPart sp = c as ShipPart;
                            if (sp != null)
                            {
                                ShipPart CargoNeeded = d.DeliveryItem as ShipPart;
                                if (CargoNeeded != null)
                                {
                                    if (sp.Type == CargoNeeded.Type)
                                    {
                                        Debug.Log("i have a " + CargoNeeded.Type.ToString() + " in my cargo hold.");
                                        d.Status = GoalStatus.CanTurnIn;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            d.Status = GoalStatus.NA;
        }
        
    }
    public void DeliveryGoalEnd(DeliveryGoal d)
    {
        Debug.Log("Goal has been turned in.");
        d.Status = GoalStatus.TurnedIn;

        //check to see if mission is complete?
        foreach (Goal g in MainStoryMissions[CurrentMission].MissionGoals)
        {
            if(g.Status != GoalStatus.TurnedIn)
            {
                return;
            }
        }
        EndCurrentMainMission();

    }
    public void TurnInGoals()
    {
        foreach (Goal g in MainStoryMissions[CurrentMission].MissionGoals)
        {
            if(g.Status == GoalStatus.CanTurnIn)
            {
                if ((g as DeliveryGoal) != null)
                {
                    (g as DeliveryGoal).GoalEnd((g as DeliveryGoal));
                }
            }
        }
    }



}
