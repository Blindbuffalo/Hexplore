using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public enum MissionType { Fetch, Puzzle }
//public enum MissionLocation { Planetary, Moon, Space }
public class MissionLocation
{
    public class t
    {
        
    }
}
public class MainStoryMission
{
    public MainStoryMission(string name, string info, int xp, Action<MainStoryMission> logic)
    {
        Name = name;
        Info = info;
        XP = xp;
        Logic = logic;

    }
    
    public string Name { get; protected set; }
    public string Info { get; protected set; }
    public int XP { get; protected set; }
    
    public Action<MainStoryMission> Logic;

}
