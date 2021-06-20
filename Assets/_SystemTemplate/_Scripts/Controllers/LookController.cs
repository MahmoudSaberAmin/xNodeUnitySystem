using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : SystemController
{
    [SerializeField] private LookNode _assignedNode;
    public LookAtSomething Looker = new LookAtSomething();

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Audio Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as LookNode;
    }

    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;


        Looker.LookAtTransform = SystemNode.GetEndTransform();
        Looker.LookAtWithGameobject(SystemNode.Implementations?.FirstOrDefault()?.transform.parent?.gameObject, _assignedNode.lookAmount);

        yield return new WaitForSeconds(_assignedNode.lookDuration);

        Looker.LookAtTransform = null;
        Looker.LookAtWithGameobject(SystemNode.Implementations?.FirstOrDefault()?.transform.parent?.gameObject, 1f);

        EndSystem();
    }
}
