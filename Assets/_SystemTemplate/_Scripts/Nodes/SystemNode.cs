using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;
using OdinSerializer;
using UnityEngine.SceneManagement;
using System;
using MyBox;
using System.Globalization;
using Sirenix.OdinInspector;

[System.Serializable]
public enum SystemType
{
    None,
    Duplicate,
    Arrow,
    Audio,
    Animation,
    Text,
    Pickup,
    Model,
    Tween,
    Visibility,
    Scene,
    Data,
    Feedback,
    Snap,
    Logic,
    Instantiation,
    Skip,
    MakeChild,
    Mouth,
    LookAt,
    Timeline,
    Hand,
}


[System.Serializable]
[NodeWidth(300)]
public abstract class SystemNode : Node
{
    /// <summary>
    /// Previous Node Port.
    /// </summary>
    [Input] public Empty Previous;

    /// <summary>
    /// Next Node Port.
    /// </summary>
    [Output] public Empty Next;


    /// <summary>
    /// Text Area for Help.
    /// </summary>
    [TextArea]
    public string Help;

    /// <summary>
    /// Time duration before playing the system.
    /// </summary>
    public float Delay;

    /// <summary>
    /// True if the object should collide with the controller's trigger and false if Node is autoplay.
    /// </summary>
    //public bool IsTrigger = false;

    /// <summary>
    /// True if Input should be colored in Nodes like Pickup.
    /// </summary>
    [HideInInspector] public bool IsColorInput = false;

    /// <summary>
    /// Text field for Pickup to show settings change like Vr Input Type and Trigger Tag.
    /// </summary>
    [BackgroundColor(0f, 1f, 0f, 1f, true)]
    [ConditionalField("IsColorInput", false, true)]
    public string _1 = "Below Settings are changed";

    /// <summary>
    /// the tag that the controller collider shoulkd check.
    /// </summary>
    [BackgroundColor()]
    [ValueDropdown("GetAllTags", NumberOfItemsBeforeEnablingSearch = 1)]
    public string TriggerTag = GameContstants.TriggerTagDefaultName;



    /// <summary>
    /// Input type rquired by controller, like VR or Keyboard.
    /// </summary>
    [ValueDropdown("GetAllInputTags", NumberOfItemsBeforeEnablingSearch = 1)]
    public string InputType = "None";

    /// <summary>
    /// true if Input is required before system Play.
    /// </summary>
    [ConditionalField("InputType", true, "None")]
    public bool IsPreInputRequired = false;

    /// <summary>
    /// true if Input is required to go to next nodes.
    /// </summary>
    [ConditionalField("InputType", true, "None")]
    public bool IsPostInputRequired = false;


    /// <summary>
    /// true if extra buttons or ui is needed to be visible, 
    /// UI like select controller, Implementation or EndTransform, 
    /// saves tons of performance when false.
    /// </summary>
    [HideInInspector]
    public bool IsUnlocked = false;   
    
    [HideInInspector]
    public bool IsValid = false;


    /// <summary>
    /// True on system end playing.
    /// </summary>
    [NonSerialized]
    public bool IsSystemEnded = false;

    /// <summary>
    /// True if system started to play.
    /// </summary>
    [NonSerialized]
    public bool IsSystemPlaying = false;

    /// <summary>
    /// Type of this system Node.
    /// </summary>
    [HideInInspector]
    public SystemType Type;

    /// <summary>
    /// True if this node is deleted, 
    /// Helps to delete nodes in Editor, 
    /// To stop recreating Controller and Implementation again.
    /// </summary>
    [HideInInspector]
    public bool IsDeleted = false;

    /// <summary>
    /// Unique Id of the node, 
    /// we use data as aunique identifier.
    /// </summary>
    [HideInInspector]
    public string Id;

    /// <summary>
    /// true if End Transform should be created in create Implementations.
    /// </summary>
    [NonSerialized]
    public bool IsEndTransformAvailable = false;

    /// <summary>
    /// The created game object for the node which holds both implmentation and controller as children.
    /// </summary>
    [HideInInspector]
    public GameObject NodeParent;

    /// <summary>
    /// AllGameObjects of the Implementations. those are the objects affected by the system or gets attached to affcted game objects in the hirearchy.
    /// </summary>
    [HideInInspector]
    public List<GameObject> Implementations = new List<GameObject>();

    /// <summary>
    /// The node Controller which executes the behaviour of the node.
    /// </summary>
    [HideInInspector]
    public SystemController Controller;

    /// <summary>
    /// The gameobject of the controller.
    /// </summary>
    protected GameObject _triggerGameOject;

    /// <summary>
    /// Refrence to the graph.
    /// </summary>
    [HideInInspector]
    public SystemsGraph Graph;

    /// <summary>
    /// Checks if data is  sent.
    /// </summary>
    [HideInInspector]
    public bool IsSent = false;

    /// <summary>
    /// Checks if data is recieved.
    /// </summary>
    [NonSerialized]
    public bool IsRecieved = false;

    /// <summary>
    /// Recieve port for the node, 
    /// Used to send data to another node.
    /// </summary>
    [Input] public Empty Recieve;

    /// <summary>
    /// Send port for the node, 
    /// Used to recieve data sent from another node.
    /// </summary>
    [Output] public Empty Send;

    


    /// <summary>
    /// Initialization, 
    /// Node assign itself to the controller and generate an ID.
    /// </summary>
    protected override void Init()
    {
        base.Init();
        AssignNodeToController();
        Graph = graph as SystemsGraph;

        if (Id == null)
        {
            //Id = DateTime.Now..ToString().Replace("/", " ");


            Id = GetEnglishTime().Replace("/", " ");// Remove slashes because it creates sub tags

            IsUnlocked = true;
        }
    }




    /// <summary>
    /// Gets time in English to avoid ص (AM) in the date.
    /// </summary>
    /// <returns>English Time formatted string</returns>
    private string GetEnglishTime()
    {
        CultureInfo ci = new CultureInfo("en-US");
        DateTime dt = DateTime.Now;
        return dt.ToString("d MMM yyyy h m", ci) + " :" + dt.Second + "s";
    }


    public override object GetValue(NodePort port)
    {
        //return base.GetValue(port);
        return null;
    }


    public void AssignNodeToController()
    {
        Controller?.AssignSystemNode(this);
    }

    /// <summary>
    /// Change the name of controller and Implementations to  "SceneName / Node name / Impelemntation or controller".
    /// </summary>
    /// <param name="name">node name</param>
    public virtual void RenameControllerAndImplementation(string name)
    {
        if (Controller != null)
        {
            Controller.name = name + " Controller";
        }

        if (Implementations != null)
        {
            foreach (var imp in Implementations)
            {
                if (imp != null && imp.name == GameContstants.ImplementationObjectDefaultName)
                {
                    imp.name = name + " Implementation";
                }
            }
        }
    }

    /// <summary>
    /// True if GameObject with Tag exists.
    /// </summary>
    /// <param name="name">Tag</param>
    /// <returns>true if gameobject with tag exists</returns>
    private bool IsGreen(string name)
    {
        return Graph.FindObjectWithTag(name) != null;
    }

    /// <summary>
    /// Checks if node gameobject exists.
    /// </summary>
    /// <param name="name">node name</param>
    /// <returns>true if gameobject with tag exists</returns>
    public bool IsNodeGreen(string name)
    {
        return IsGreen(GameContstants.GetNodeTag(name));
    }

    /// <summary>
    /// Checks if Controller gameobject exists.
    /// </summary>
    /// <param name="name">node name</param>
    /// <returns>true if gameobject with controller tag exists</returns>
    public bool IsControllerGreen(string name)
    {
        return IsGreen(GameContstants.GetControllerTag(name));
    }


    /// <summary>
    /// Checks if Implemntations gameobjects exists.
    /// </summary>
    /// <param name="name">node name</param>
    /// <returns>true if gameobject with implemntation tag exists</returns>
    public bool IsImplementationGreen(string name)
    {
        return IsGreen(GameContstants.GetImplementationTag(name));
    }

    /// <summary>
    /// Checks if End Transform gameobject exists.
    /// </summary>
    /// <param name="name">node name</param>
    /// <returns>true if end transform gameobject exists, false if not, null if end transform is available</returns>
    public bool? IsEndTransformGreen(string name)
    {
        if (IsEndTransformAvailable == false)
        {
            return null;
        }

        return IsGreen(GameContstants.GetEndTransformTag(name));
    }

    /// <summary>
    /// Checks for graph parent.
    /// </summary>
    /// <returns>true if graph parent gameobject exists</returns>
    public bool ISGraphParentExist()
    {
        var sceneName = Graph.SceneAssetName;
        return GameObject.Find(sceneName + " Graph") != null;
    }


    /// <summary>
    /// Refreshes the graph, when called from systems graph editor.
    /// </summary>
    public void Refresh()
    {
        var nodeName = Id + " " + Type.ToString();
        name = nodeName;

#if UNITY_EDITOR
        CreateTags(nodeName, IsEndTransformAvailable);
#endif

        if (ISGraphParentExist())
        {
            if (IsNodeGreen(nodeName) == false)
            {
                CreateNodeParent(nodeName);
                CreateController(nodeName);
                CreateImplementation(nodeName);
                PutScriptsOnController();
                PutScriptsOnImplentation();
                AssignNodeToController();
            }

            RenameControllerAndImplementation(nodeName);
        }
    }


    private void CreateTags(string name, bool isCreateEndTransform)
    {
#if UNITY_EDITOR
        Graph.AddTag(GameContstants.GetNodeTag(name));
        Graph.AddTag(GameContstants.GetControllerTag(name));
        Graph.AddTag(GameContstants.GetImplementationTag(name));

        if (isCreateEndTransform)
        {
            Graph.AddTag(GameContstants.GetEndTransformTag(name));
        }
#endif
    }


    /// <summary>
    /// Create Node parent gameobject.
    /// </summary>
    /// <param name="name">node name</param>
    public void CreateNodeParent(string name)
    {
        NodeParent = new GameObject(name);

        var parent = GameObject.Find(Graph.SceneAssetName + " Graph").transform;
        NodeParent.transform.SetParent(parent);

        NodeParent.gameObject.AddComponent<NodeTag>().TagValue = GameContstants.GetNodeTag(name);
    }


    /// <summary>
    /// Create Node Controller.
    /// </summary>
    /// <param name="name">node name</param>
    public void CreateController(string name)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        _triggerGameOject = new GameObject(GameContstants.ControllerDefaultName);

        var parent = Graph.FindObjectWithTag(GameContstants.GetNodeTag(name)).transform;
        _triggerGameOject.transform.SetParent(parent);

        _triggerGameOject.gameObject.AddComponent<NodeTag>().TagValue = GameContstants.GetControllerTag(name);
    }

    /// <summary>
    /// Create Node Implementation.
    /// </summary>
    /// <param name="name">node name</param>
    public virtual void CreateImplementation(string name)
    {
        var sceneName = SceneManager.GetActiveScene().name;

        if (Implementations != null)
        {
            Implementations.Clear();
            Implementations.Add(new GameObject(GameContstants.ImplementationObjectDefaultName));

            var parent = Graph.FindObjectWithTag(GameContstants.GetNodeTag(name)).transform;

            if (Implementations.Count > 0 && Implementations.FirstOrDefault() != null)
            {
                Implementations.FirstOrDefault().transform.SetParent(parent);
                Implementations.FirstOrDefault().gameObject.AddComponent<NodeTag>().TagValue = GameContstants.GetImplementationTag(name);
            }
            else
            {
                Logger.LogError("Implementations list is empty");
            }
        }
        else
        {
            Logger.LogError("Implementations list is null");
        }
    }

    /// <summary>
    /// Put basic scripts for controller like trigger collider.
    /// </summary>
    public virtual void PutScriptsOnController()
    {
        if (_triggerGameOject != null)
        {
            var trigger = _triggerGameOject.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = Vector3.one * 0.1f;
        }
    }

    /// <summary>
    /// Put basic scripts on Implementation.
    /// </summary>
    public virtual void PutScriptsOnImplentation()
    {

    }

    /// <summary>
    /// Gets overriden by sub nodes to get end transform.
    /// </summary>
    /// <returns>End Transform</returns>
    public virtual Transform GetEndTransform()
    {
        return null;
    }

    public override void OnPastedFromSourceNode(Node srcNode)
    {
        base.OnPastedFromSourceNode(srcNode);
        Id = GetEnglishTime().Replace("/", " "); // new Id
        position.x += 40;
        position.y += 30;
        IsUnlocked = true;
        Refresh();
    }


    private List<string> GetAllTags()
    {
#if UNITY_EDITOR
            return ListTags();
#else
        return null;
#endif
    }


    private List<string> GetAllInputTags()
    {
#if UNITY_EDITOR
        if (Type==SystemType.Pickup)
        {
            return new List<string> { "VRInput" };
        }
        else
        {
            return Enum.GetValues(typeof(InputsType)).Cast<InputsType>().Select(x=> x.ToString()).ToList();
        }
#else
        return null;
#endif
    }

    private List<string> GetAllSystemTags()
    {
#if UNITY_EDITOR
        if (Graph.ListOfSystemTags == null)
        {
            Graph.ListOfSystemTags = ListTags();
        }
        return Graph.ListOfSystemTags;
#else
        return null;
#endif
    }


    public static List<string> ListTags()
    {
#if UNITY_EDITOR
        // Open tag manager
        UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

        var tagsList = new List<string>() { GameContstants.TriggerTagDefaultName, "Player", "MainCamera" };

        // First check if it is not already present
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (!t.stringValue.Contains("/") && !string.IsNullOrWhiteSpace(t.stringValue))
            {
                tagsList.Add(t.stringValue);
            }
        }

        return tagsList;
#endif
        return null;
    }


}



/// <summary>
/// This is an empty class used for node ports.
/// </summary>
[System.Serializable]
public class Empty { }