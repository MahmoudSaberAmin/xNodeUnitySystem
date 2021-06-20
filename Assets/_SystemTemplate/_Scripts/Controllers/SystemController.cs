using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;
using OdinSerializer.Utilities;
using System;

/// <summary>
/// SystemController.cs is an abstract class uses a SystemNode class instace. 
/// SystemNode.cs contains data used to control each node.
/// Nodes like AudioNode and VisibilityNode inherits from SystemNode.
/// Custom behaviours and fields are added to each new [Node]Controller based on their responsiblities 
///  
/// xNode Graph deals with nodes which create controllers that do custom behaviour
/// Nodes are just data and controllers are just behaviour
/// This is called Model View Controller design pattern where this is the controller, node and graph are data with parmeters and flow while the scene objects affected are the view
/// </summary>

public abstract class SystemController : MonoBehaviour, IInputHandler
{
    /// <summary>
    /// The composed base node which holds basic data that are the same for every system.
    /// </summary>
    [HideInInspector]
    public SystemNode SystemNode;

    /// <summary>
    /// Checks wether the system needs an input before it runs.
    /// </summary>
    [NonSerialized]
    public bool IsPreInputRecieved = false;   
    
    /// <summary>
    /// Checks wether the system needs Input before running the next node.
    /// </summary>
    [NonSerialized]
    public bool IsPostInputRecieved = false;

    /// <summary>
    /// Checks if a tagged object lies within this controller gameobject which has a trigger collider.
    /// </summary>
    private bool _isObjectWithinTrigger = false;

    /// <summary>
    /// Checks if Input is down, this is serialized in the controller to simulate Input.
    /// </summary>
    [SerializeField] private bool _isInputDown = false;


    SystemsGraph _graph;

    /// <summary>
    /// This is the graph property cached for performance.
    /// </summary>
    SystemsGraph Graph
    {
        get
        {
            if (_graph == null)
            {
                //FindObjectsOfType<SceneGraph>().FirstOrDefault(x => x.graph.nodes.Contains(SystemNode))?.graph as SystemsGraph;
                _graph = SystemNode.graph as SystemsGraph;
            }

            return _graph;
        }
    }


    /// <summary>
    /// Checks whether the user pressed the Input required for PreInput, 
    /// PreInput is Input before playing the node.
    /// </summary>
    private bool _isPreInputDone = false;

    /// <summary>
    /// Checks whether the user pressed the Input required for PostInput, 
    /// Post Input is Input after playing the node before moving to the Next node.
    /// </summary>
    private bool _isPostInputDone = false;


    protected virtual void Awake()
    {
        InitializeNodeParameters();
    }

    protected virtual void Start()
    {
        SystemNode.IsSystemEnded = false;
        ExecuteOnStartup();
    }

    /// <summary>
    /// Node Initialization, 
    /// Setting Controller and Implementations at Play Mode, 
    /// Gets overriden by some sub controllers to add End transforms.
    /// </summary>
    protected virtual void InitializeNodeParameters()
    {
        if (SystemNode != null)
        {
            SystemNode.Controller = this;
            SystemNode.Implementations = Graph.FindObject_s_WithTag(new List<string> { GameContstants.GetImplementationTag(SystemNode.name) }).Select(x => x.gameObject).ToList();
        }
    }

    /// <summary>
    ///  Check if this controller node is current, 
    ///  If the tag matches collider tag, 
    ///  We check if we can handle the situation or the system and if true we play the system.
    /// </summary>
    /// <param name="other">other collider</param>
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Graph.IsCurrent(SystemNode)==true)
        {
            if (other.tag == SystemNode.TriggerTag.ToString())
            {
                _isObjectWithinTrigger = true;
                if (IsHandleSituation())
                {
                    StartCoroutine(PlaySystem(other.gameObject));
                }
            }
        }
    }

    /// <summary>
    /// Start execution at startup if connected to a start node.
    /// </summary>
    private void ExecuteOnStartup()
    {
        if (Graph.IsCurrent(SystemNode) == true)
        {
            print(SystemNode.TriggerTag);
            print(GameContstants.TriggerTagDefaultName);
            print(SystemNode.TriggerTag == GameContstants.TriggerTagDefaultName);

            if (SystemNode?.GetInputPort("Previous")?.Connection?.node is StartNode)
            {
                if ((SystemNode.InputType==InputsType.None.ToString()||SystemNode.IsPreInputRequired == false) && SystemNode.TriggerTag==GameContstants.TriggerTagDefaultName)
                {
                    StartCoroutine(PlaySystem(null));
                }

                OnEnter();
            }
        }
    }

    /// <summary>
    /// Called when node is entered as current, 
    /// has no relation to play system.
    /// </summary>
    protected virtual void OnEnter()
    {
        InputFactory.BindGenericInput(this, SystemNode.InputType);
    }

    /// <summary>
    /// Called when next node is about to be current, 
    /// Just before next node becomes current.
    /// </summary>
    protected virtual void OnExit()
    {

    }

    /// <summary>
    ///  If node is current we set _isObjectWithinTrigger boolean to false.
    /// </summary>
    /// <param name="other">Other collider</param>
    protected virtual void OnTriggerExit(Collider other)
    {
        if (Graph.IsCurrent(SystemNode) == true)
        {
            if (other.tag == SystemNode.TriggerTag.ToString())
            {
                _isObjectWithinTrigger = false;
            }
        }
    }


    /// <summary>
    /// If node is current we set _isObjectWithinTrigger to true.
    /// </summary>
    /// <param name="other">Other collider</param>
    protected virtual void OnTriggerStay(Collider other)
    {
        if (Graph.IsCurrent(SystemNode) == true)
        {
            if (other.tag == SystemNode.TriggerTag.ToString())
            {
                _isObjectWithinTrigger = true;

            }
        }


    }


    /// <summary>
    /// If node is current we set _isObjectWithinTrigger boolean to false .
    /// </summary>
    protected virtual void LateUpdate()
    {
        if (Graph.IsCurrent(SystemNode) == true)
        {
            _isObjectWithinTrigger = false;
        }
    }

    /// <summary>
    /// If node is current and system has not been played and there is a required Input, 
    /// Execute Input which is a method that checks for pre and post Input.
    /// </summary>
    protected virtual void Update()
    {
        if (Graph.IsCurrent(SystemNode) == true && SystemNode.IsSystemPlaying == false && SystemNode.IsSystemEnded==false && (SystemNode.IsPreInputRequired == true|| SystemNode.IsPostInputRequired == true))
        {
            if (SystemNode.InputType!=InputsType.None.ToString())
            {
                ExecuteInput();
            }
        }
    }


    /// <summary>
    /// IInputHandler inetrface Implemnted Methods, 
    /// IInputHandler is a bridge between Input and Every Controller.
    /// </summary>
    public void OnControllerInputDown()
    {
        _isInputDown = true;
    }

    /// <summary>
    ///  IInputHandler inetrface Implemnted Methods, 
    ///  IInputHandler is a bridge between Input and Every Controller.
    /// </summary>
    public void OnControllerInputUp()
    {
        _isInputDown = false;
    }


    /// <summary>
    /// Checks for PreInput and Post Input, 
    /// 
    /// if Pre Input Done play the system, 
    /// 
    /// if Post Input done then continue to next node, 
    /// 
    /// PreInput and Post Input are separated from each other, Either or both or none can be required.
    /// </summary>
    private void ExecuteInput()
    {
        if (_isPreInputDone == false && IsPreInputApplied())
        {
            IsPreInputRecieved = true;
            Logger.LogError("IsPreInputRecieved = true");
        }

        if (_isPreInputDone == false && IsPreInputRecieved == true && _isInputDown == false)
        {
            _isPreInputDone = true;
            Logger.LogError("EndWithInputRecieved");
            EndWithInputRecieved();
        }

        if (_isPostInputDone == false && ((SystemNode.IsPreInputRequired && IsPreInputRecieved) || !SystemNode.IsPreInputRequired) && IsPostInputApplied())
        {
            IsPostInputRecieved = true;
            Logger.LogError("IsPostInputRecieved = true");
        }


        if (_isPostInputDone == false && IsPostInputRecieved == true && _isInputDown == false)
        {
            _isPostInputDone = true;
            Logger.LogError("ContinueToNextNodes");
            ContinueToNextNodes();
        }
    }

    /// <summary>
    /// The system Node calls this method to assign itself to the controller, 
    /// This method get overriden by every subcontroller (every sub class of this class) to assign the proper node.
    /// </summary>
    /// <param name="node">System node to be assigned</param>
    public virtual void AssignSystemNode(SystemNode node)
    {
        SystemNode = node;
    }


    /// <summary>
    /// Called after the system play is finished or on system validation error, 
    /// Checks for Input, 
    /// Sets IsSystemPlaying to false, 
    /// Continues to next connected nodes.
    /// </summary>
    protected void EndSystem()
    {
        if (SystemNode == null)
        {
            Logger.Log("Situation is null");
        }
        else
        {
            var isInputOk = (IsPreInputOk() && IsPostInputOk())|| SystemNode.InputType==InputsType.None.ToString();
            SystemNode.IsSystemEnded = isInputOk;
            SystemNode.IsSystemPlaying = false;
        }

        if(SystemNode.IsSystemEnded)
        {
            ContinueToNextNodes();
        }
    }

    /// <summary>
    /// Called when System is played to disable this controller's collider.
    /// </summary>
    private void DisableTriggerCollider()
    {
        GetComponents<Collider>()?.ForEach(x=> x.enabled=false);
    }

    /// <summary>
    /// Loops through connected nodes to the next port, 
    /// If the node is auto which means it is not trigger it checks for pre Input then executes it.
    /// </summary>
    protected void ContinueToNextNodes()
    {

        var graph = SystemNode.graph as SystemsGraph;


        //if (graph.IsAllSystemsEnded())
        //{
        //    graph.ContinueToNextNode();

        //    if (graph.IsAnySystemAuto())
        //    {
        //        graph.ExecuteAllNodes();
        //    }
        //}

        var nextNode = SystemNode?.GetOutputPort("Next")?.Connection?.node as SystemNode;
        if (nextNode!=null)
        {
            var listOfNodes = ContinueToNextNode();

            foreach (var n in listOfNodes)
            {
                EnterNode(n);
                if (IsNextNodeAuto(n))
                {
                    ExecuteNextNode(n);
                }
            }
        }
    }

    /// <summary>
    /// Called immediately after continuing to next nodes.
    /// </summary>
    /// <param name="n">entered node</param>
    private void EnterNode(SystemNode n)
    {
        n.Controller?.OnEnter();
    }


    /// <summary>
    /// Go to the next nodes in the graph.
    /// </summary>
    /// <returns>List of current nodes</returns>
    private List<SystemNode> ContinueToNextNode()
    {
        List<SystemNode> nextList = new List<SystemNode>();
        NodePort exitPort = SystemNode.GetOutputPort("Next");
        for (int j = 0; j < exitPort.ConnectionCount; j++)
        {
            var nextNode = exitPort.GetConnection(j).node as SystemNode;
            nextList.Add(nextNode);
        }

        OnExit();
        Graph.current.Remove(SystemNode);

        var removedList = new List<SystemNode>();
        for (int i = 0; i < nextList.Count; i++)
        {
            var node = nextList[i];



            if (SystemNode.GetOutputPort("Send").IsConnected)
            {
                var port = SystemNode.GetOutputPort("Send");
                for (int j = 0; j < port.ConnectionCount; j++)
                {
                    var nextNode = port.GetConnection(j).node as SystemNode;
                    nextNode.IsRecieved = true;
                }
            }

            var isSkipExecution = false;
            //Logger.LogError("0" + node.name);
            if (node.GetInputPort("Recieve").IsConnected)
            {
                isSkipExecution = true;
                //Logger.LogError("1" + node.name);
                var prevNode = node.GetInputPort("Recieve").Connection.node as SystemNode;
                if (prevNode != null && node.IsRecieved)
                {
                    isSkipExecution = false;
                    Logger.LogError("2" + prevNode.name);
                }
            }




            if (Graph.IsCurrent(node) == true || isSkipExecution)
            {
                removedList.Add(node);
            }
            else
            {
                if (isSkipExecution == false)
                {
                    Graph.current.Add(node);
                }
            }
        }

        for (int i = 0; i < removedList.Count; i++)
        {
            var node = removedList[i];
            nextList.Remove(node);
        }

        return nextList;
    }


    /// <summary>
    /// Checks if the next node IsTrigger is false, 
    /// IsTrigger requires the player to enter the collider of the controller.
    /// </summary>
    /// <param name="node">Next node</param>
    /// <returns>true if next node IsTrigger is false.</returns>
    protected bool IsNextNodeAuto(SystemNode  node)
    {
        return node.TriggerTag == GameContstants.TriggerTagDefaultName;
    }

    /// <summary>
    /// Executes next node if no pre Input required and system was not played through any other path.
    /// </summary>
    /// <param name="node">Next node to execute.</param>
    protected void ExecuteNextNode(SystemNode node)
    {
        if (node?.Controller != null && (node?.IsPreInputRequired == false || node?.InputType==InputsType.None.ToString()) && node?.IsSystemPlaying == false && node?.IsSystemEnded==false)
        {
            //var isSkipExecution = false;
            //    Logger.LogError("0"+ node.name);
            //if (SystemNode.GetInputPort("Recieve").IsConnected)
            //{
            //    isSkipExecution = true;
            //    Logger.LogError("1"+ node.name);
            //    var prevNode = SystemNode.GetInputPort("Recieve").Connection.node as SystemNode;
            //    if (prevNode != null && SystemNode.IsRecieved)
            //    {
            //        isSkipExecution = false;
            //    Logger.LogError("2"+ prevNode.name);
            //    }
            //}

            //if (isSkipExecution==false)
            //{
                node.Controller.StartCoroutine(node.Controller.PlaySystem(null));
            //}

            //if (SystemNode.GetOutputPort("Send").IsConnected)
            //{
            //    var nextNode = SystemNode.GetOutputPort("Send").Connection.node as SystemNode;
            //    if (nextNode!=null)
            //    {
            //    Logger.LogError("3" + nextNode.name);
            //        nextNode.IsRecieved = true;
            //    }
            //}
        }
    }

    /// <summary>
    /// Plays Next System.
    /// </summary>
    public void EndWithInputRecieved()
    {
        StartCoroutine(PlaySystem(null));
    }

    /// <summary>
    /// Checks if system is not ended and Pre Input is applied.
    /// </summary>
    /// <returns>true if applied Pre Input.</returns>
    private bool IsPreInputApplied()
    {
        return SystemNode != null &&  SystemNode.IsSystemEnded == false && SystemNode.IsPreInputRequired && _isInputDown && IsTriggerOk();
    }

    /// <summary>
    /// Checks if system is not ended and Post Input is applied.
    /// </summary>
    /// <returns>true if applied Post Input.</returns>
    private bool IsPostInputApplied()
    {
        return SystemNode != null && SystemNode.IsSystemEnded == false && SystemNode.IsPostInputRequired && _isInputDown; //&& IsTriggerOk();
    }

    /// <summary>
    /// Checks if the object is within trigger or is trigger is false.
    /// </summary>
    /// <returns>true if object is withing trigger or IsTrigger is false.</returns>
    private bool IsTriggerOk()
    {
        return (SystemNode.TriggerTag!= GameContstants.TriggerTagDefaultName && _isObjectWithinTrigger) || SystemNode.TriggerTag == GameContstants.TriggerTagDefaultName;
    }

    /// <summary>
    /// Checks if Pre Input is required and recieved or PreInput was not required.
    /// </summary>
    /// <returns>true if Pre Input is satisfied or not required.</returns>
    private bool IsPreInputOk()
    {
        return (SystemNode.IsPreInputRequired && IsPreInputRecieved) || !SystemNode.IsPreInputRequired;
    }


    /// <summary>
    /// Checks if Post Input is required and recieved or Post Input was not required.
    /// </summary>
    /// <returns>true if Post Input is satisfied or not required.</returns>
    private bool IsPostInputOk()
    {
        return (SystemNode.IsPreInputRequired && IsPostInputRecieved) || !SystemNode.IsPostInputRequired;
    }


    /// <summary>
    /// Checks if the system can be handeled in order to proceed to next nodes.
    /// </summary>
    /// <returns>true if system is ok to be played.</returns>
    private bool IsHandleSituation()
    {
        if (SystemNode != null && SystemNode.IsSystemEnded)
        {
            Logger.Log("Audio Sitiuation Ended In Child");
        }
        else
        {
            
        }

        
        var isThisNodeIsACurrentGraphNode = Graph != null && Graph.current != null && Graph.IsCurrent(SystemNode) == true;

        var isInputOk = IsPreInputOk() && IsPostInputOk();
        var IsSituationOpen = SystemNode != null && SystemNode.IsSystemEnded == false;

        return 
            IsSituationOpen
            &&
            isInputOk
            && 
            isThisNodeIsACurrentGraphNode;
    }

    /// <summary>
    /// Plays the current system of this controller after waiting for delay.
    /// </summary>
    /// <param name="other">Refrence to the collided gameobject</param>
    /// <returns>Coroutine.</returns>
    public virtual IEnumerator PlaySystem(GameObject other)
    {
        DisableTriggerCollider();

        yield return new WaitForSeconds(SystemNode.Delay);

        if (SystemNode == null)
        {
            Logger.Log("System Node is null");
        }
        else
        {
            if (SystemNode.IsSystemEnded)
            {
                Logger.Log(SystemNode.ToString() + " Situation Ended");
            }
            else
            {
                Logger.Log("Playing System: " + SystemNode.ToString());
            }
        }
    }
}
