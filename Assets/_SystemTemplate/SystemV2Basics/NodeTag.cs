using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using XNode;
using XNodeEditor;
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class SystemV2TagAttribute : System.Attribute
{
    public string Tag;

    public SystemV2TagAttribute(string tag)
    {
        Tag = tag;
    }
}

public class NodeTag : MonoBehaviour
{
    [SystemV2TagAttribute("")]
    public string TagValue;

    public void Print()
    {
        Debug.LogError(TagValue);
    }

#if UNITY_EDITOR
    [ContextMenu("SelectInGraph")]
    void SeclectNode()
    {
        var name = Selection.activeGameObject?.GetComponent<NodeTag>()?.TagValue;
        if (!string.IsNullOrWhiteSpace(name))
        {
            name = name.Replace("Implementation", "Controller");
            name = name.Replace("End Transform", "Controller");

            if (!name.Contains("Controller"))
            {
                name += " /Controller";
            }

            var node = MonoBehaviour.FindObjectsOfType<NodeTag>().FirstOrDefault(x => x.TagValue == name)?.GetComponent<SystemController>()?.SystemNode;
            Selection.activeObject = node;

            NodeEditorWindow w = NodeEditorWindow.Open(node.graph);
            w.Home(); // Focus selected node
        }

    }
#endif
}