using MyBox;
using System.Diagnostics;
using XNode;

/// <summary>
/// First node in the system, there can be multiple start nodes, the node connected is considered to start as first node.
/// </summary>
[NodeTint(150,0, 0)]
[NodeWidth(140)]
[System.Serializable]
public class StartNode : Node
{
    [Output] public Empty Next;


    [ConditionalField("IsEnabled", false, false)]
    [BackgroundColor(0, 0, 1, 1, true)]
    public string _ = "Disabled";
    /// <summary>
    /// If the node is disabled, it skips starting the next node, 
    /// Used for debugging purposes.
    /// </summary>
    [BackgroundColor()]
    public bool IsEnabled = true;

    public override object GetValue(NodePort port)
    {
        return null; 
    }
}
