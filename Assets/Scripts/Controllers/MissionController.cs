using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionController : MonoBehaviour{

    public List<MainStoryMission> MainStoryMissions;

    public int CurrentMSMission = -1;
    public int _MSMissionStep = 0;
    public int MSMissionStep
    {
        get
        {
            return _MSMissionStep;
        }
        set
        {
            _MSMissionStep = value;
            CheckMissionStatus();
        }
    }
    public bool MSMissionCompleted = false;
    public void SpawnMissions(SolarSystem Sol)
    {
        if(CurrentMSMission == -1 || MSMissionCompleted)
        {
            CurrentMSMission++;

            Debug.Log(MainStoryMissions[CurrentMSMission].Name);

            MainStoryMissions[CurrentMSMission].Start(MainStoryMissions[CurrentMSMission]);

            MSMissionCompleted = false;
        }
    }
    public void CheckMissionStatus()
    {
        MainStoryMissions[CurrentMSMission].Progress(MainStoryMissions[CurrentMSMission]);
    }
    public void InitMissions()
    {
        MainStoryMissions = new List<MainStoryMission>()
        {
            new MainStoryMission(
                "Good Day to Fly.",
                "Things about stuff said here!", 
                10,
                "Earth",
                start: MainMission1Start,
                progress: MainMission1Progress,
                end: MainMission1End),
            new MainStoryMission(
                "Weeeeeeeeeee",
                "Things about stuff said here!", 
                10,
                "Earth",
                start:foo,
                progress:foo,
                end:foo),
            new MainStoryMission(
                "Looky what the womprat dragged in.",
                "Things about stuff said here!", 
                10,
                "Earth",
                start:foo,
                progress:foo,
                end:foo),
            new MainStoryMission(
                "And the Red shirt laughed...",
                "Things about stuff said here!", 
                10,
                "Earth",
                start:foo,
                progress:foo,
                end:foo)
        };


    }
    public void MainMission1Start(MainStoryMission M)
    {
        Debug.Log("MSM1:Start");
        Debug.Log(M.Info);
    }
    public void MainMission1Progress(MainStoryMission M)
    {
        
        switch (MSMissionStep)
        {
            case 1:
                Debug.Log("MSM1:Progress:Step1");
                M.End(M);
                break;
        }
    }
    public void MainMission1End(MainStoryMission M)
    {
        Debug.Log("MSM1:End");
        MSMissionCompleted = true;
        MSMissionStep = 0;
    }
    public void foo(MainStoryMission M)
    {

    }
}
