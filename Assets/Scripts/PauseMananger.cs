using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PauseMananger : Singleton<PauseMananger>
{

    public static bool isOnPause => instance._isOnPause;
    private bool _isOnPause;
    private List<Guid> _pauseGuids;

    void Start()
    {
        _pauseGuids =  new List<Guid>(10);
    }

    public static Guid Pause()
    {
        var guid = Guid.NewGuid();
        instance._pauseGuids.Add(guid);
        instance._isOnPause = true;
        return guid;
    }

    public static void Resume(Guid guid)
    {
        
        instance._pauseGuids.Remove(guid);
        if (!instance._pauseGuids.Any())
        {
            instance._isOnPause = false;
        }
    }

    void Update()
    {
        Time.timeScale = _isOnPause ? 0.0f : 1.0f;
    }
}
