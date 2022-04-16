using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public string UserName;
    public string GameTimes;
    public string MaxLevelRecord;

    public UserInfo(string name)
    {
        UserName = name;
        GameTimes = "0";
        MaxLevelRecord = "0";
    }

    public UserInfo(string name, string times, string record)
    {
        UserName = name;
        GameTimes = times;
        MaxLevelRecord = record;
    }
}
