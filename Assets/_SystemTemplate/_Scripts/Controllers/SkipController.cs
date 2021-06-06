//TODO:Andrew
//Node looping would stop the execution of the Audio node 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Audio controller is a subclass of system controller which plays an audio clip.
/// </summary>
public class SkipController : SystemController
{
    /// <summary>
    /// The Skip node which is a subclass of System Node.
    /// </summary>
    [SerializeField] private SkipNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Audio Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as SkipNode;
    }


    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;

        _assignedNode.Skip();

        EndSystem();
    }
}
