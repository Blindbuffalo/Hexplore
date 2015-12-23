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
    public MainStoryMission(string name, string info, int reward, List<Goal> missiongoals)
    {
        Name = name;
        Info = info;
        Reward = reward;

        MissionGoals = missiongoals;
        
    }


    public string LocationName { get; protected set; }
    public List<Goal> MissionGoals { get; set; }
    

}
public class Goal
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string StartLocation { get; protected set; }

    public Action<Goal> Start { get; protected set; }
    public Action<Goal> Progress { get; protected set; }
    public Action<Goal> End { get; protected set; }
}
public class FetchGoal : Goal
{
    public FetchGoal(string name, string description, string startlocation, string dropofflocation, Action<Goal> start, Action<Goal> progress, Action<Goal> end, Cargo fetchitem)
    {
        Name = name;
        Description = description;
        StartLocation = startlocation;
        FetchItem = fetchitem;
        Start = start;
        DropOffLocation = dropofflocation;
        Progress = progress;
        End = end;
    }

    Cargo FetchItem { get; set; }
    public string DropOffLocation { get; set; }
}