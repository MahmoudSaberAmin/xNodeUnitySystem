using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumName
{
    DataTag,
    DataKey,
    DataValue,
    Condition,
}


public enum TriggerTag
{
    Player = 10,
    RightHand = 20,
    Interactable = 30,
    LazerDot = 40,
}


public enum DataTag
{
    None,
    MenuScene,
    MallScene,
}

public enum DataKey
{
    None,
    Sex,
    Language,
    Places,
    Levels,
}

public enum LevelsValue
{
    Level_1,
    Level_2,
    Level_3,
    Level_4,
    Level_5,
    Level_6,
    Level_7,
    Level_8,
}

public enum PlacesValue
{
    Places_Mall,
    Places_Shop,
    Places_Mosque,
    Places_Menu,
}

public enum DataValue
{
    None,
    Sex_Man,
    Sex_Woman,
    Langauage_Arabic,
    Langauage_English,
}

public enum Condition
{
    True,
    False,
}

public static class GameContstants
{
    private static Dictionary<EnumName, System.Type> _enumDictionary;
    internal static string ControllerDefaultAnimationName = "Default";
    internal static string AnimatorControllerName = "SystemAnimatorController";

    public static Dictionary<EnumName, System.Type> EnumDictionary
    {
        get
        {
            if (_enumDictionary==null)
            {
                _enumDictionary = new Dictionary<EnumName, System.Type>();

                _enumDictionary[EnumName.DataTag] = typeof(DataTag);
                _enumDictionary[EnumName.DataKey] = typeof(DataKey);
                _enumDictionary[EnumName.DataValue] = typeof(DataValue);
                _enumDictionary[EnumName.Condition] = typeof(Condition);
            }

            return _enumDictionary;
        }
    }

    public const string MecanimMultiplierParameter = "Multiplier";
    public const string MecanimMouthTalkingParameter = "IsTalking";
    public const string MecanimMouthSpeedParameter = "TalkingMultiplier";

    public static string GetControllerTag(string nodeName)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return sceneName + "/" + nodeName + " /Controller";
    }


    public static string GetImplementationTag(string nodeName)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return sceneName + "/" + nodeName + " /Implementation";
    }

    public static string GetEndTransformTag(string nodeName)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return sceneName + "/" + nodeName + " /End Transform";
    }



    public static string GetNodeTag(string nodeName)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        return sceneName + "/" + nodeName;
    }

       public const string EndTransformName = "End Transform";
       public const string TriggerName = "Trigger1";
       public const string ImplementationObjectDefaultName  = "Implementation GameObject.";
       public const string ControllerDefaultName  = "Controller GameObject.";
       public const string TriggerTagDefaultName  = "None";
       public const string RightHandName  = "RightHand";
    
}
