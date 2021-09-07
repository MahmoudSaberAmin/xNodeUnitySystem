using OdinSerializer.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XNode;
using XNodeEditor;


[CustomNodeEditor(typeof(SystemNode))]
public class SystemNodeEditor : NodeEditor
{
	private double _lastUpdateTime = 0;
	private float _updateDuration = 2;

	private bool _isControllerGreen = true;
	private bool _isImplementationGreen = true;
	private bool? _isEndTransformGreen = true;
    private bool _wasUnlocked = false;

    public override void OnHeaderGUI()
	{
		GUI.color = Color.white;
		SystemNode node = target as SystemNode;
		SystemsGraph graph = node.graph as SystemsGraph;
		if (graph.current.Contains(node)) GUI.color = Color.green;

		string title = "";
		if (node?.Type == SystemType.Duplicate)
		{
			title = "< Duplicate";
		}
		else
		{
			if (node != null)
			{
				title = GetNodeGuid(node); //node?.ImplentationGameOject?.name?.Replace(" Implementation", "") ?? ""; //target.name;
			}
		}

		GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
		GUI.color = Color.white;
	}

	public override void OnBodyGUI()
	{

		SystemNode node = target as SystemNode;
		SystemsGraph graph = node.graph as SystemsGraph;

		if (!graph.IsGraphEditEnabled)
		{
			return;
		}

		_wasUnlocked = node.IsUnlocked;
		node.IsUnlocked = GUILayout.Toggle(node.IsUnlocked, "Unlock");

		base.OnBodyGUI();

		var name = GetNodeGuid(node);

        if (!_wasUnlocked && node.IsUnlocked)
        {
			node.IsValid = false;
		}

		if (node.IsUnlocked)
		{
			if (EditorApplication.timeSinceStartup - _lastUpdateTime > _updateDuration)
			{
				if (!node.IsDeleted && node.IsValid == false)
				{
					_lastUpdateTime = EditorApplication.timeSinceStartup;

					CreateNodeAssets(graph);
				}
			}

			if (_isControllerGreen)
			{
				GUI.contentColor = Color.green;
			}
			else
			{
				GUI.contentColor = Color.red;
			}


			if (GUILayout.Button("Select Controller"))
			{
				var sceneName = SceneManager.GetActiveScene().name;
				Selection.objects = graph.FindObject_s_WithTag(new List<string> { GameContstants.GetControllerTag(name) }).Select(x => x.gameObject).ToArray();
			}

			if (_isImplementationGreen)
			{
				GUI.contentColor = Color.green;
			}
			else
			{
				GUI.contentColor = Color.red;
			}

			if (GUILayout.Button("Select Implementation"))
			{
				var sceneName = SceneManager.GetActiveScene().name;
				Selection.objects = graph.FindObject_s_WithTag(new List<string> { GameContstants.GetImplementationTag(name) }).Select(x => x.gameObject).ToArray();
			}

			if (_isEndTransformGreen == true)
			{
				GUI.contentColor = Color.green;
				if (GUILayout.Button("Select End Transform"))
				{
					var sceneName = SceneManager.GetActiveScene().name;
					Selection.objects = graph.FindObject_s_WithTag(new List<string> { GameContstants.GetEndTransformTag(name) }).Select(x => x.gameObject).ToArray();
				}
			}
			else if (_isEndTransformGreen == false)
			{
				GUI.contentColor = Color.red;
				if (GUILayout.Button("Select End Transform"))
				{
					var sceneName = SceneManager.GetActiveScene().name;
					Selection.objects = graph.FindObject_s_WithTag(new List<string> { GameContstants.GetEndTransformTag(name) }).Select(x => x.gameObject).ToArray();
				}
			}
			else
			{
				GUI.contentColor = Color.white;
			}



			GUI.contentColor = Color.white;


			if (!_isControllerGreen || !_isImplementationGreen)
			{
				if (GUILayout.Button("Reset Missing Components"))
				{
					if (!node.IsControllerGreen(name))
					{
						node.CreateController(name);
						node.PutScriptsOnController();
						node.AssignNodeToController();
					}

					if (!node.IsImplementationGreen(name))
					{
						node.CreateImplementation(name);
						node.PutScriptsOnImplentation();
					}

					node.RenameControllerAndImplementation(name);
				}
			}


			//if (GUILayout.Button("Continue Graph")) graph.ContinueToNextNode();
			////if (GUILayout.Button("Set as current")) { graph.current.Clear(); graph.current.Add(node); };
			//if (GUILayout.Button("Input (A)"))
			//{

			//	node.Controller?.OnControllerInputDown();
			//	node.Controller?.Invoke("OnControllerInputUp", 1f);
			//};

			//if (GUILayout.Button("Duplicate Node"))
			//{
			//	var duplicate = graph.CopyNode(node);

			//	var duplicateSystemNode = duplicate as SystemNode;
			//	duplicateSystemNode.Type = SystemType.Duplicate;

			//	duplicateSystemNode.CreateController(node.name);
			//	duplicateSystemNode.PutScriptsOnController();


			//	duplicate.position.x += 100f;
			//	duplicate.position.y += 80f;
			//};
		}
	}

	public void RefreshAllNodes(SystemsGraph sysGraph)
	{
		SystemNode node = target as SystemNode;

		if (!node.IsDeleted)
		{
			_lastUpdateTime = EditorApplication.timeSinceStartup;

			CreateNodeAssets(sysGraph);
		}
	}

	private void CreateTags(SystemsGraph graph, string name, bool isCreateEndTransform)
	{
		graph.AddTag(GameContstants.GetNodeTag(name));
		graph.AddTag(GameContstants.GetControllerTag(name));
		graph.AddTag(GameContstants.GetImplementationTag(name));

		if (isCreateEndTransform)
		{
			graph.AddTag(GameContstants.GetEndTransformTag(name));
		}
	}


	private void CreateNodeAssets(SystemsGraph sysGraph)
	{
		SystemNode node = target as SystemNode;

		if (node.IsDeleted)
		{

		}
		else
		{
			var name = GetNodeGuid(node);

			//TODO
			CreateTags(sysGraph, name, node.IsEndTransformAvailable);

			_isControllerGreen = node.IsControllerGreen(name);
			_isImplementationGreen = node.IsImplementationGreen(name);
			_isEndTransformGreen = node.IsEndTransformGreen(name);

            if (_isControllerGreen && _isImplementationGreen && _isEndTransformGreen != false)
            {
				node.IsValid = true;
                if (_wasUnlocked)
                {
					node.IsUnlocked = false;
                }
			}

			if (node != null)
			{
				//Debug.Log("2");
				node.name = name;
				if (node.ISGraphParentExist())
				{
					if (node.IsNodeGreen(name) == false)
					{
						node.CreateNodeParent(name);
						node.CreateController(name);
						node.CreateImplementation(name);
						node.PutScriptsOnController();
						node.PutScriptsOnImplentation();
						node.AssignNodeToController();
					}

					node.RenameControllerAndImplementation(name);
				}
			}
		}
	}

	private string GetNodeGuid(SystemNode node)
	{
		return node.Id + " " + node.Type.ToString();
	}


}
