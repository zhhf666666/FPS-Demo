using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public string UserName;
    public int GameTimes;
    public int MaxLevelRecord;

    public UserInfo(string name)
    {
        UserName = name;
        GameTimes = 0;
        MaxLevelRecord = 0;
    }
}
