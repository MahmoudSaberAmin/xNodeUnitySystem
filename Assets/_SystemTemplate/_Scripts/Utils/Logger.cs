using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Logger
{
    private static bool IsShouldLog = true;

    private static bool IsShouldLogVerbose = true;
    private static bool IsShouldLogWarning = true;
    private static bool IsShouldLogError = true;

    public static void Log(string message)
    {
        if (IsShouldLog && IsShouldLogVerbose)
        {
            Debug.Log(message);
        }
    }    
    
    public static void LogWarning(string message)
    {
        if (IsShouldLog && IsShouldLogWarning)
        {
            Debug.LogWarning(message);
        }
    }    
    
    public static void LogError(string message)
    {
        if (IsShouldLog && IsShouldLogError)
        {
            Debug.LogError(message);
        }
    }
}
