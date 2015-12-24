using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum MissionType { Fetch, Puzzle }
//public enum MissionLocation { Planetary, Moon, Space }
public class Mission
{
    public string Name { get; protected set; }
    public string Info { get; protected set; }
    public int Reward { get; protected set; } //maybe change so that this can be xp, or items, or $$, or all of them
}
public class MainStoryMission : Mission
{
    public MainStoryMission(string name, string info, int reward, Action<MainStoryMission> start, Action<MainStoryMission> progress, Action<MainStoryMission> end, List<Goal> missiongoals)
    {
        Name = name;
        Info = info;
        Reward = reward;

        MissionGoals = missiongoals;

        Start = start;
        
        Progress = progress;
        End = end;
    }


    public string LocationName { get; protected set; }
    public List<Goal> MissionGoals { get; set; }

    public Action<MainStoryMission> Start { get; protected set; }
    public Action<MainStoryMission> Progress { get; protected set; }
    public Action<MainStoryMission> End { get; protected set; }

}
public class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartLocation { get; set; }
}
public class FetchGoal : Goal
{
    public FetchGoal(string name, string description, string startlocation, string dropofflocation,  Cargo fetchitem)
    {
        Name = name;
        Description = description;
        StartLocation = startlocation;

        FetchItem = fetchitem;
        DropOffLocation = dropofflocation;
    }
    public string DropOffLocation { get; set; }
    public Cargo FetchItem { get; set; }
    
}