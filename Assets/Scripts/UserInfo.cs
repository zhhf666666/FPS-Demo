using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    private static UserInfo Instance;
    public string UserName;
    public string GameTimes;
    public string MaxLevelRecord;

    private UserInfo() {}

    public static UserInfo GetInstance()
    {
        if(Instance == null)
            Instance = new UserInfo();
        return Instance;
    }

    public static bool DoesExist()
    {
        if(Instance == null)
            return false;
        else
            return true;
    }

    public void DeleteInstance()
    {
        Instance = null;
    }
}
