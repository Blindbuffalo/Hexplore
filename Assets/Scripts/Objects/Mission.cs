using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum MissionType { Fetch, Puzzle }
public enum GoalStatus { NA, CanTurnIn, TurnedIn}
//public enum MissionLocation { Planetary, Moon, Space }
public class Mission
{
    public string Name { get; protected set; }
    public string Info { get; protected set; }
    public int XPreward { get; protected set; } //maybe change so that this can be xp, or items, or $$, or all of them
}
public class MainStoryMission : Mission
{
    public MainStoryMission(int id, int prereq, string name, string info, int xpreward, 
        Action<MainStoryMission> start, 
        List<Goal> missiongoals)
    {
        ID = id;
        Prerequisite = prereq;
        Name = name;
        Info = info;
        XPreward = xpreward;

        MissionGoals = missiongoals;

        Start = start;
    }


    public string LocationName { get; protected set; }
    public List<Goal> MissionGoals { get; set; }

    public Action<MainStoryMission> Start { get; protected set; }


    public int Prerequisite { get; protected set; }
    public int ID { get; protected set; }

}
public class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartLocation { get; set; }

    public GoalStatus Status { get; set; }
}
public class DeliveryGoal : Goal
{
    public DeliveryGoal(string name, string description, string startlocation, string dropofflocation,  Cargo deliveryitem, Cargo givenitem,
        Action<DeliveryGoal> goalprogress, Action<DeliveryGoal> goalend, GoalStatus status)
    {
        Name = name;
        Description = description;
        StartLocation = startlocation;

        DeliveryItem = deliveryitem;
        GivenItem = givenitem;

        DropOffLocation = dropofflocation;

        GoalProgress = goalprogress;
        GoalEnd = goalend;
        Status = status;
    }
    public string DropOffLocation { get; set; }
    public Cargo DeliveryItem { get; set; }
    public Cargo GivenItem { get; set; }

    public Action<DeliveryGoal> GoalProgress { get; protected set; }
    public Action<DeliveryGoal> GoalEnd { get; protected set; }
}