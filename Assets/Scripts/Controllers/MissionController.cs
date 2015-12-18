using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionController {

    public List<MainStoryMission> MainStoryMissions;

    public int CurrentMSMission = -1;
    public bool MSMissionCompleted = false;
    public void SpawnMissions()
    {
        if(CurrentMSMission == -1 || MSMissionCompleted)
        {
            CurrentMSMission++;

            Debug.Log(MainStoryMissions[CurrentMSMission].Name);

            MSMissionCompleted = false;
        }
    }
    public void InitMissions()
    {
        MainStoryMissions = new List<MainStoryMission>()
        {
            new MainStoryMission(
                "Good Day to Fly.",
                "Things about stuff said here!", 
                10, 
                foo),
            new MainStoryMission(
                "Weeeeeeeeeee",
                "Things about stuff said here!", 
                10, 
                foo),
            new MainStoryMission(
                "Looky what the womprat dragged in.",
                "Things about stuff said here!", 
                10, 
                foo),
            new MainStoryMission(
                "And the Red shirt laughed...",
                "Things about stuff said here!", 
                10,
                foo)
        };


    }
    public void foo(MainStoryMission M)
    {

    }
}
