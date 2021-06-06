using OdinSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using XNode;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu]
public class SystemsGraph : NodeGraph
{
    // The current "active" node
    public List<SystemNode> current = new List<SystemNode>();
    [NonSerialized] public bool IsGraphEditEnabled = true;
    [SerializeField] public bool IsSystemV1 = false;

    public List<string> ListOfSystemTags { get; set; }

    public string SearchQuery = "a";
    public string SceneAssetName = "";
    public string ScenePath = "";
    public bool IsGraphAceneAssetSet = false;

    private List<string> _tagsList;

    private List<string> TagsList
    {
        get
        {
            if (_tagsList == null)
            {
                LoadTags();
            }
            return _tagsList;
        }

        set
        {
            _tagsList = value;
        }
    }

    private bool _isDirty = false;

    private double _lastSaveTime = 0;
    private double _saveDuration = 0.5d;

    private TextAsset _textAsset;


    public void ContinueToNextNode()
    {
        List<SystemNode> nextList = new List<SystemNode>();
        for (int i = 0; i < current.Count; i++)
        {
            NodePort exitPort = current[i].GetOutputPort("Next");
            for (int j = 0; j < exitPort.ConnectionCount; j++)
            {
                var nextNode = exitPort.GetConnection(j).node as SystemNode;
                if (!nextList.Contains(nextNode))
                {
                    nextList.Add(nextNode);
                }
            }
        }

        current = nextList;
    }

    internal void SkipWithCurrentNode(SystemNode systemNode)
    {
        foreach (var node in current)
        {
            if (node!=systemNode)
            {
                node.Controller?.StopAllCoroutines();
                node.IsSystemEnded = true;
            }
        }

        current.Clear();
        current.Add(systemNode);
    }

    public bool IsAllSystemsEnded()
    {
        return current.All(x => x.IsSystemEnded);
    }

    public bool IsAnySystemAuto()
    {
        return current.Any(x => x.TriggerTag== GameContstants.TriggerTagDefaultName);
    }

    public bool? IsCurrent(SystemNode systemNode)
    {
        return current?.Contains(systemNode);
    }

    public void ExecuteAllNodes()
    {
        current.ForEach(x => x.Controller?.StartCoroutine(x.Controller?.GetComponent<SystemController>()?.PlaySystem(null)));
    }

    

    public override void RemoveNode(Node node)
    {
        base.RemoveNode(node);


        var systemNode = node as SystemNode;
        if (systemNode != null)
        {
            systemNode.IsDeleted = true;
            var nodeTypeString = systemNode.Type.ToString();



            if (systemNode!= null && systemNode.Type!=SystemType.Duplicate)
            {
                systemNode.Controller = FindObjectWithTag(GameContstants.GetControllerTag(node.name))?.GetComponent<SystemController>();
                systemNode.Implementations = FindObject_s_WithTag(new List<string> { GameContstants.GetImplementationTag(node.name) }).Select(x => x.gameObject).ToList();
                systemNode.NodeParent = FindObjectWithTag(GameContstants.GetNodeTag(node.name));

                for (int i = 0; i < systemNode?.Implementations?.FirstOrDefault()?.transform.childCount; i++)
                {
                    var t = systemNode.Implementations.FirstOrDefault().transform.GetChild(i);
                    t.transform.parent = null;
                }

                //systemNode.Controller.tag = "Untagged";
                //systemNode.NodeParent.tag = "Untagged";

                if (systemNode.Controller)
                {
                    DestroyImmediate(systemNode.Controller.gameObject);
                    systemNode.Controller = null;
                }

                if (systemNode.Implementations!=null)
                {
                    foreach (var imp in systemNode.Implementations)
                    {
                        imp.tag = "Untagged";
                        DestroyImmediate(imp);
                    }
                    systemNode.Implementations.Clear();
                }


                if (systemNode.Type == SystemType.Tween)
                {
                    var tweenNode = systemNode as TweenNode;

                    if (tweenNode==null)
                    {
                        Logger.LogError("Error, TweenNode node is null");
                    }

                    tweenNode.EndAnimationTransform = FindObjectWithTag(GameContstants.GetEndTransformTag(systemNode.name))?.transform;
                    DestroyImmediate(tweenNode.EndAnimationTransform.gameObject);
                }

                if (systemNode.NodeParent)
                {
                    DestroyImmediate(systemNode.NodeParent);
                    systemNode.NodeParent = null;
                }
                

                Logger.Log(nodeTypeString + " Node Controller and Implementation was removed");
            }
        }

    }



    public void AddTag(string tag)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Debug.LogError(tag);
            if (TagsList.Contains(tag))
            {
                Debug.LogError("1");
            }
            else
            {
                Debug.LogError("2");
                TagsList.Add(tag);
                _lastSaveTime = EditorApplication.timeSinceStartup;
                _isDirty = true;
            }
        }
#endif
    }

    public void LoadTags()
    {
        if (_tagsList == null || _textAsset == null)
        {
            _lastSaveTime = EditorApplication.timeSinceStartup;
            _textAsset = Resources.Load<TextAsset>("Tags");
            _tagsList = _textAsset.text.Split('\n').ToList();
        }
    }


    public bool IsDirty()
    {
        return _isDirty;
    }


    public IEnumerable<NodeTag> FindObject_s_WithTag(List<string> queries)
    {
        return GameObject.FindObjectsOfType<NodeTag>().Where(x => queries.All(y => x.TagValue.Split(',').Contains(y)));
    }


    public bool IsTagExist(string tag)
    {
        return TagsList.Contains(tag);
    }


    public GameObject FindObjectWithTag(string tag)
    {
        return FindObject_s_WithTag(new List<string> { tag }).FirstOrDefault()?.gameObject;
    }


    public void OnGUIUpdate()
    {
        SaveTags(false);
    }




    public void RemoveTag(string tag)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            TagsList.Remove(tag);
            _lastSaveTime = EditorApplication.timeSinceStartup;
            _isDirty = true;
        }
#endif
    }

    public List<string> GetAllTagsFromTextFile()
    {
        return TagsList;
    }

    public void AddRangeOfTags(string[] tags)
    {
        foreach (var t in tags)
        {
            AddTag(t);
        }
    }


    public void SaveTags(bool forceSave)
    {
#if UNITY_EDITOR
        if ((!Application.isPlaying && _isDirty && EditorApplication.timeSinceStartup - _lastSaveTime > _saveDuration) || forceSave)
        {
            _lastSaveTime = EditorApplication.timeSinceStartup;
            _isDirty = false;
            LoadTags();
            File.WriteAllText(AssetDatabase.GetAssetPath(_textAsset), string.Join("\n", TagsList));
            EditorUtility.SetDirty(_textAsset);
            AssetDatabase.Refresh();
        }
#endif
    }

    public void ResetTags()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            TagsList.Clear();
            File.WriteAllText(AssetDatabase.GetAssetPath(_textAsset), "");
            EditorUtility.SetDirty(_textAsset);
            AssetDatabase.Refresh();
        }
#endif
    }
}