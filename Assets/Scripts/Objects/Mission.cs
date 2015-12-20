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
    public int XP { get; protected set; }
}
public class MainStoryMission : Mission
{
    public MainStoryMission(string name, string info, int xp, string locationname, Action<MainStoryMission> start = null, Action<MainStoryMission> progress = null, Action<MainStoryMission> end = null)
    {
        Name = name;
        Info = info;
        XP = xp;
        LocationName = locationname;

        Start = start;
        Progress = progress;
        End = end;
        
    }
    

    public string LocationName { get; protected set; }

    public Action<MainStoryMission> Start { get; protected set; }
    public Action<MainStoryMission> Progress { get; protected set; }
    public Action<MainStoryMission> End { get; protected set; }

}
