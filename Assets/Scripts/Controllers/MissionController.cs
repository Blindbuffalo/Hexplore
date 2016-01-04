using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class MissionController : MonoBehaviour{
    public enum MissionProgress { notStarted, progressing, ending, noMore }

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
    public void DrawMissionIndicators()
    {
        GameObject GOmm;
        switch (MissionP)
        {
            case MissionProgress.notStarted:
                //string P = MainStoryMissions[CurrentMission].LocationName;
                //GameObject Pgo = DrawHexGraphics.Instance.GetPlanetGO(P, GameController.Instance.SunGO);
                //GOmm = (GameObject)Instantiate(MissionMarkerPrefab, Pgo.transform.position, Quaternion.identity);
                //GOmm.transform.SetParent(Pgo.transform);
                break;
            case MissionProgress.progressing:

                break;
            case MissionProgress.ending:
                break;
            case MissionProgress.noMore:
                break;
        }
    }

    public List<MainStoryMission> MainStoryMissions;
    public bool StartCurrentMainMission()
    {
        if (MissionP == MissionProgress.notStarted)
        {
            MissionP = MissionProgress.progressing;
            if(MainStoryMissions.Count <= CurrentMission)
            {
                //no more missions
                Debug.Log("No more missions.");
                MissionP = MissionProgress.noMore;
                return false;
            }
            else
            {
                Debug.Log("mission");
                MainStoryMissions[CurrentMission].Start(MainStoryMissions[CurrentMission]);
                Debug.Log("mission (" + CurrentMission + ") " + MainStoryMissions[CurrentMission].Name + " started!");
                CharController.Instance.MainShip.Cargohold.DebugListOfItemsInHold();
                return true;
            }
            
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

            GameController.Instance.IncreaseXP(MainStoryMissions[CurrentMission].XPreward);


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
                LocationName: "Earth",
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
                ),
            new MainStoryMission(
                id: 1,
                prereq: 0,
                name: "To Mars and back again.",
                info: "Bring some stuff to mars.",
                LocationName: "Mars",
                xpreward: 10,
                start: DeliveryMissionStart,
                missiongoals: new List<Goal>() {
                        new DeliveryGoal (
                            name: "Deliver the Hull plating to the guy!",
                            description: "get the guy some hull.",
                            startlocation: "Earth",
                            dropofflocation: "Mars",
                            deliveryitem: new ShipPart("New hull", 1f, 2f, 0, 999f, ShipPartType.HullPlate),
                            givenitem: new ShipPart("New hull", 1f, 2f, 0, 999f, ShipPartType.HullPlate),
                            goalprogress: DeliveryGoalCheck,
                            goalend: DeliveryGoalEnd,
                            status: GoalStatus.NA
                            ),
                        new DeliveryGoal (
                            name: "Deliver the Hull plating to the guy!",
                            description: "get the guy some hull.",
                            startlocation: "Mars",
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

            Planet p = SolarSystem.Instance.Planets[d.DropOffLocation];
            //foreach (KeyValuePair<string, Planet> p in SolarSystem.Instance.Planets)
            //{


            //    if (p.Name == d.DropOffLocation)
            //    {
                    PLoc = p.Orbit[p.CurrentPosition];
                    //List<Hex> ShipsNeighbors = Hex.Neighbors(CharController.Instance.MainShip.CurrentHexPosition);
                    //if (ShipsNeighbors.Contains(PLoc))
                    //{
                    if (Hex.Equals(PLoc, CharController.Instance.MainShip.CurrentHexPosition))
                    {
                        Debug.Log("im in same tile as " + d.DropOffLocation);
                        if (CharController.Instance.MainShip.Cargohold.ItemInCargoHold(d.DeliveryItem))//if the item is in the cargo hold
                        {
                            Debug.Log("i have the item in my cargo hold.");
                            d.Status = GoalStatus.CanTurnIn;
                            return;
                        }
                    }
            //    }
            //}
            d.Status = GoalStatus.NA;
        }
        
    }
 
    
    public void DeliveryGoalEnd(DeliveryGoal d)
    {
        Debug.Log("Goal turning in now");
        d.Status = GoalStatus.TurnedIn;
        if(CharController.Instance.MainShip.Cargohold.ItemInCargoHold(d.DeliveryItem)) //if the item is in the cargo hold
        {
            CharController.Instance.MainShip.Cargohold.RemoveFirstItemOfSameType(d.DeliveryItem); //remove the item
            CharController.Instance.MainShip.Cargohold.DebugListOfItemsInHold();
            Debug.Log("Goal has been turned in.");
            //check to see if mission is complete?
            foreach (Goal g in MainStoryMissions[CurrentMission].MissionGoals)
            {
                if (g.Status != GoalStatus.TurnedIn)
                {

                    return;
                }
            }
            EndCurrentMainMission();
        }
        else
        {
            Debug.LogError("the item is not in the hold :(");
        }


    }
    public void TurnInGoals()
    {
        if (MissionP == MissionProgress.progressing)
        {
            foreach (Goal g in MainStoryMissions[CurrentMission].MissionGoals)
            {
                if (g.Status == GoalStatus.CanTurnIn)
                {
                    if ((g as DeliveryGoal) != null)
                    {
                        (g as DeliveryGoal).GoalEnd((g as DeliveryGoal));
                    }
                }
            }
        }

    }



}
