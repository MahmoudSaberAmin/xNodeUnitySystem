using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeChildController : SystemController
{
    /// <summary>
    /// Instance of a MakeChild node which is system node subclass
    /// </summary>
    [SerializeField] 
    private MakeChildNode _assignedNode;

    /// <summary>
    /// MakeChild Node assigns itself to this controller.
    /// </summary>
    /// <param name="node"></param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as MakeChildNode;
    }

    /// <summary>
    /// Assigns the end transform in runtime.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }


    /// <summary>
    /// assign the implementation parent position and rotation of the end transform
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);


        SystemNode.IsSystemPlaying = true;

        Transform implementationParent = _assignedNode.Implementations.FirstOrDefault().transform.parent;
        implementationParent.parent = _assignedNode.EndTransform;

        if (_assignedNode.IsFollowPos)
            _assignedNode.Implementations.FirstOrDefault().transform.parent.localPosition = Vector3.zero;

        if (_assignedNode.IsFollowRot)
            _assignedNode.Implementations.FirstOrDefault().transform.parent.localRotation = Quaternion.identity;
      

        EndSystem();

    }


}
