using UnityEngine;
using System.Linq;
using System;

[System.Serializable]
public class SkipNode : SystemNode
{
    ///// <summary>
    ///// It is required
    ///// </summary>
    //[Header("Special Data...")]
    //[Space]

    public SkipNode() : base()
    {
        Type = SystemType.Skip;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<SkipController>();
    }

    internal void Skip()
    {
        Graph.SkipWithCurrentNode(this);
    }

}