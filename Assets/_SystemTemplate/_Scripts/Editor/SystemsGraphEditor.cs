using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XNode;
using XNodeEditor;


[CustomNodeGraphEditor(typeof(SystemsGraph))]
public class SystemsGraphEditor : NodeGraphEditor
{
	private string _activeName;
	private int _searchIndex = 0;
	private float _scrollValueX = 0;
	private float _scrollValueY = 0;
	public static bool IsShouldScroll = false;


	public override void OnOpen()
    {
		//      base.OnOpen();
		//var path = (target as SystemsGraph).ScenePath;
		//      if (string.IsNullOrWhiteSpace(path))
		//      {
		//	Debug.Log("No defined scene path for this graph");
		//      }
		//else
		//      {
		//	EditorSceneManager.OpenScene(path);
		//	var parent = GameObject.Find((target as SystemsGraph).SceneAssetName + " Graph").transform;
		//	Selection.activeObject = parent;
		//}

		_scrollValueX = -NodeEditorWindow.current.panOffset.x;
		_scrollValueY = -NodeEditorWindow.current.panOffset.y;

		Debug.LogError("Started");

		var gMenu = new GenericMenu();
		var gContent = new GUIContent("Pack");

		gMenu.AddItem(gContent, true, () => {
			Debug.LogError("Pressed");
			var selection = Selection.objects;
			var isAllNodesAreSystemNodes = true;
            foreach (var item in selection)
            {
                if (item is SystemNode)
                {

                }
				else
                {
					isAllNodesAreSystemNodes = false;
					Debug.LogError("A selected node is not a system node, did you select start node by mistake?");
					break;

				}
            }

            if (isAllNodesAreSystemNodes)
            {
				if(IsValidNodeSequencing(selection))
                {
					var guid = System.Guid.NewGuid().ToString();
                    foreach (var node in selection)
                    {
						var systemNode = node as SystemNode;
						systemNode.TryAssignBlock(guid);
					}
                }
            }

		});

		gMenu.AddCustomContextMenuItems(Selection.objects);
	}

    private bool IsValidNodeSequencing(Object[] selection)
    {
		var areAllNodesFreeOfBlockId = true;
		foreach (var node in selection)
		{
			var systemNode = node as SystemNode;
			if(systemNode.IsInBlock())
            {
				areAllNodesFreeOfBlockId = false;
				break;
			}
		}

		return areAllNodesFreeOfBlockId;
	}

    public override void OnGUI()
	{
		// Repaint each frame
		var sysGraph = target as SystemsGraph;

		sysGraph.IsGraphEditEnabled = GUILayout.Toggle(sysGraph.IsGraphEditEnabled, "Is Edit Enabled");

		var e = Event.current;


		//Debug.Log("panOffset = " + NodeEditorWindow.current.panOffset);
		


		_scrollValueX = -NodeEditorWindow.current.panOffset.x;
		_scrollValueY = -NodeEditorWindow.current.panOffset.y;

		if (e.mousePosition.y > Screen.height - 75f || e.mousePosition.x > Screen.width - 75f)
		{

			GUI.backgroundColor = Color.black;
			GUI.Box(new Rect(0, Screen.height - 75, Screen.width, 75), "");
			GUI.Box(new Rect(Screen.width - 75, 0, 75, Screen.height), "");
			GUI.backgroundColor = Color.white;

            //Debug.Log("Screen.width = " + Screen.width+""+ e.mousePosition.x);

			//Debug.Log("_scrollValueX =" + _scrollValueX + ", _scrollValueY = " + _scrollValueY);


			_scrollValueX = GUI.HorizontalScrollbar(new Rect(0, Screen.height - 50, Screen.width, 50), _scrollValueX, 100, NodeEditorWindow.current.graph.nodes.Min(node=> node.position.x), NodeEditorWindow.current.graph.nodes.Max(node => node.position.x)+320);
			_scrollValueY = GUI.VerticalScrollbar(new Rect(Screen.width- 50, 0, 50, Screen.height), _scrollValueY, 100, NodeEditorWindow.current.graph.nodes.Min(node=> node.position.y), NodeEditorWindow.current.graph.nodes.Max(node => node.position.y)+960);
			//Debug.Log("_scrollValue = " + _scrollValue);
			NodeEditorWindow.current.panOffset = -new Vector2(_scrollValueX, _scrollValueY);

		}



		var color = GUI.color;
		GUI.color = Color.black;


		GUI.Box(new Rect(0, 20, Screen.width, 55), "");

        foreach (var block in sysGraph.Blocks)
        {
			GUI.Box(new Rect(0, 20, Screen.width, 55), "");
		}


		GUI.color = color;

		//// define click area
		//Event e = Event.current;
		//windoClickArea = GUI.Window(0, windoClickArea, drawWindow, "MyWindow");
		//if (e.type == EventType.MouseDown && windoClickArea.Contains(e.mousePosition))
		//{
		//	Debug.Log("click");
		//	GUI.FocusControl(null);
		//}

		GUI.Label(new Rect((Screen.width - 200) / 2f, 0, 200, 20), sysGraph.SceneAssetName);


		if (Application.loadedLevelName != _activeName)
		{
			_activeName = Application.loadedLevelName;
			SceneChanged();
		}

		GUI.color = Color.red;
		if (GUI.Button(new Rect(Screen.width - 150, 30, 140, 35), "Clear All Graph Tags"))
		{
			ClearAllGraphTags();
			sysGraph.IsGraphEditEnabled = false;
		}
		GUI.color = Color.white;

		if (GUI.Button(new Rect(Screen.width - 150 - 120 - 90 - 10, 30, 90, 35), "MigrateToV2"))
		{
			MigrateToV2(sysGraph);
		}

		if (!sysGraph.IsGraphEditEnabled)
		{
			return;
		}

		if (sysGraph.IsGraphAceneAssetSet)
		{

			//sysGraph.OnGUIUpdate();
			GUI.color = Color.green;
			if (GUI.Button(new Rect(Screen.width - 150 - 120 , 30, 90, 35), "Save"))
			{
				sysGraph.SaveTags(true);
			}
			GUI.color = Color.white;


			GUILayout.Space(11.5f);
		GUI.SetNextControlName("Search");
		sysGraph.SearchQuery = GUILayout.TextField(sysGraph.SearchQuery, GUILayout.MaxWidth(140), GUILayout.Height(35));

		if (GUI.Button(new Rect(150, 30, 55, 35), "Focus"))
		{
			sysGraph.SearchQuery = "a";
			GUI.FocusControl("Search");
		}

		if (GUI.Button(new Rect(208, 30, 60, 35), "Search"))
		{
			Search(sysGraph.SearchQuery);
		}

		if (GUI.Button(new Rect(270, 30, 50, 35), "Next"))
		{
			FindNext(sysGraph.SearchQuery, _searchIndex);
			_searchIndex++;
		}

		if (GUI.Button(new Rect(500, 30, 70, 35), "Refresh"))
		{
			Refresh();
		}		
		
		if (GUI.Button(new Rect(600, 35, 250, 35), "Reorder Nodes (Please avoid node loops)"))
		{
			ReorderGraphNodes();
		}


		}
        else { 
			GUI.SetNextControlName("AssetSceneName");
			sysGraph.SceneAssetName = GUILayout.TextField(sysGraph.SceneAssetName, GUILayout.MaxWidth(140), GUILayout.Height(35));

			if (GUI.Button(new Rect(150, 20, 55, 35), "Focus"))
			{
				sysGraph.SearchQuery = "a";
				GUI.FocusControl("AssetSceneName");
			}

			if (GUI.Button(new Rect(210, 20, 180, 35), "Create Scene Asset"))
			{
				if (Application.isPlaying)
				{
					return;
				}

				if (IsCurrentGraphSceneAssetExists())
				{
				}
				else
				{
					if (string.IsNullOrWhiteSpace(sysGraph.SceneAssetName))
					{
						Logger.LogError("Please type Scene Asset Name for " + SceneManager.GetActiveScene() + " Scene");
					}
					else
					{
						var sceneName = SceneManager.GetActiveScene().name;
						sysGraph.SceneAssetName += " [" + sceneName + "] ";
						sysGraph.ScenePath = SceneManager.GetActiveScene().path;
						CreateGraphSceneAsset(sysGraph.SceneAssetName);
						sysGraph.IsGraphAceneAssetSet = true;
					}
				}
			}
		}


		//void drawWindow(int aID)
		//{
		//	// it's a dummy proc
		//}




		//if (Application.isPlaying)
		//{
		//	return;
		//}

		//if (EditorApplication.timeSinceStartup - _lastFrame> _checkDuration)
		//         {
		//	if (Event.current.type == EventType.Repaint)
		//	{
		//                 if (target.name.ToLower().Replace(" ", "").Contains(SceneManager.GetActiveScene().name.ToLower().Replace(" ", "")))
		//                 {
		//			if (IsCurrentGraphSceneAssetExists())
		//			{
		//				LinkStartNextNodes();
		//			}
		//			else
		//			{
		//				CreateGraphSceneAsset();
		//			}
		//                 }
		//	}

		//	_lastFrame = EditorApplication.timeSinceStartup;
		//}

	}



	private void MigrateToV2(SystemsGraph sysGraph)
	{
		var oldTags = EditorUtils.ListAllTags() ?? new string[] { };

		var sceneName = sysGraph.ScenePath.Split('/').LastOrDefault()?.Replace(".unity", "") ?? "     ";

		Debug.LogError(sceneName);

		foreach (var t in oldTags)
		{
			if (t.StartsWith(sceneName))
			{
				Debug.LogError(t);
				var gameObjects = GameObject.FindGameObjectsWithTag(t);

				if (gameObjects != null && gameObjects.Length > 0)
				{
					Debug.LogError("Found**************");
					foreach (var go in gameObjects)
					{
						if (go != null && go.GetComponent<NodeTag>() == null)
						{
							go.AddComponent<NodeTag>().TagValue = t;
						}
					}

					sysGraph.AddTag(t);
				}
			}
		}
	}

	private void LinkGraphStart()
    {
		if (IsCurrentGraphSceneAssetExists())
		{
			LinkStartNextNodes();
		}
	}

	private void FindNext(string query, int index)

	{
		Logger.LogError(index.ToString());
		var nodes = target.nodes.FindAll(x => x.name.ToLower().Contains(query.ToLower())
		|| (((x as SystemNode)?.Help) ?? "").ToLower().Contains(query.ToLower())
		).ToArray();

		if (nodes != null && nodes.Length > 0)
		{
			var node = nodes[index % nodes.Length];
			Logger.LogError(node.name);
			Logger.LogError(index.ToString());
			Selection.activeObject = node;
			Vector2 nodeDimension = NodeEditorWindow.current.nodeSizes.ContainsKey(node) ?
							NodeEditorWindow.current.nodeSizes[node] / 2 : Vector2.zero;
			Vector2 pos = -(node.position + nodeDimension);//this is important
			NodeEditorWindow.current.panOffset = pos;
		}
	}

	private void Search(string query)
	{
		Logger.LogError(query);
		var nodes = target.nodes.FindAll(x => x.name.ToLower().Contains(query.ToLower())
		|| (((x as SystemNode)?.Help) ?? "").ToLower().Contains(query.ToLower())

		).ToArray();
		foreach (var n in nodes)
		{
			Logger.LogError(n.name);
		}
		Selection.objects = nodes;
	}

	private void SceneChanged()
	{
		var sysGraph = target as SystemsGraph;
		sysGraph.IsGraphEditEnabled = Application.isPlaying;

		LinkGraphStart();
	}

	private void ClearAllGraphTags()
	{
		var parentName = "Deleted Game with Tags";

		var parent = GameObject.Find(parentName);

		var sysGRaph = target as SystemsGraph;

		if (parent == null)
		{
			parent = new GameObject(parentName);
		}

		var deletedGosParent = parent.transform;
		var tags = sysGRaph.GetAllTagsFromTextFile();


		if (tags.Count > 0)
		{

			sysGRaph.FindObject_s_WithTag(tags).Select(x => x.gameObject).ToList().ForEach(x => {
				x.transform.parent = deletedGosParent;
				x.tag = "Untagged";
				x.name += " [deleted]";

				var controller = x.GetComponent<SystemController>();
				if (controller != null)
				{
					controller.SystemNode.Controller = null;
					controller.SystemNode.Implementations.Clear();
					controller.SystemNode = null;
				}


			});


			(target as SystemsGraph).ResetTags();


			AssetDatabase.Refresh();
		}
	}

	private void LinkStartNextNodes()
	{
		var systemsGraph = target as SystemsGraph;
		systemsGraph.current.Clear();

		var startNodes = target.nodes.Where(x => {
			var start = x as StartNode;
			return start != null && start.IsEnabled == true;
			});

		if (startNodes != null)
		{
            foreach (var startNode in startNodes)
            {
				var nextNodes = startNode.GetOutputPort("Next")?.GetConnections().Select(x => x.node as SystemNode);

				if (nextNodes == null || nextNodes.Count() == 0)
				{
					Logger.Log("Start is not connected to a system node");
				}
				else
				{
					systemsGraph.current.Clear();
					systemsGraph.current.AddRange(nextNodes);
				}
            }
		}
	}



    private void Refresh()
	{
		LinkStartNextNodes();
		RefreshNodes();
	}

	private void RefreshNodes()
	{
		var systemsGraph = target as SystemsGraph;
		systemsGraph.nodes.ForEach(x => (x as SystemNode)?.Refresh());
	}

	private bool IsCurrentGraphSceneAssetExists()
	{
		var currentScene = SceneManager.GetActiveScene();
		return currentScene.GetRootGameObjects().FirstOrDefault(x => x.name == ((target as SystemsGraph).SceneAssetName + " Graph")) != null;
	}

	private void CreateGraphSceneAsset(string graphName)
	{
		var currentScene = SceneManager.GetActiveScene();
		var go = new GameObject(graphName + " Graph");
		//target.name = go.name;

		CreateStartNode();
		go.AddComponent<GraphController>().AssignGraphToController(target as SystemsGraph);

		
		AssetDatabase.Refresh();
	}


	private void CreateStartNode()
	{
		//string[] guids = AssetDatabase.FindAssets("xNode_NodeTemplate.cs");
		//if (guids.Length == 0)
		//{
		//	Debug.LogWarning("xNode_NodeTemplate.cs.txt not found in asset database");
		//	return;
		//}
		//string path = AssetDatabase.GUIDToAssetPath(guids[0]);
		//NodeEditorUtilities.CreateFromTemplate(
		//	"StartNode.cs",
		//	path
		//);

		//var startNode = Node.CreateInstance<StartNode>();
		//startNode.graph = target;

		//var systemsGraph = target as SystemsGraph;
		//systemsGraph.AddNode<StartNode>();
		if (!target.nodes.Find(x => x is StartNode))
		{
			CreateNode(typeof(StartNode), Vector2.zero);
		}
	}


	public override void RemoveNode(Node node)
	{
		var nodeName = node.name;
		base.RemoveNode(node);
		var sysGraph = target as SystemsGraph;

		//var controllerTag = SceneManager.GetActiveScene().name + "/" + node.name + "/Controller";
		//var implementationTag = SceneManager.GetActiveScene().name + "/" + node.name + "/Implementation";
		//var endTransformTag = SceneManager.GetActiveScene().name + "/" + node.name + "/End Transform";
		//var nodeParentTag = SceneManager.GetActiveScene().name + "/" + node.name;

		sysGraph.RemoveTag(GameContstants.GetControllerTag(nodeName));
		sysGraph.RemoveTag(GameContstants.GetImplementationTag(nodeName));
		sysGraph.RemoveTag(GameContstants.GetEndTransformTag(nodeName));
		sysGraph.RemoveTag(GameContstants.GetNodeTag(nodeName));
	}

	//  public override Node CopyNode(Node original)
	//  {
	//Undo.undoRedoPerformed +=


	//return base.CopyNode(original);
	//  }

	private void UndoRedoPerformed()
    {

    }

    public override bool CanRemove(Node node)
    {
        return !Application.isPlaying && base.CanRemove(node);
    }

	private void ReorderGraphNodes()
    {
		var systemsGraph = target as SystemsGraph;
		systemsGraph.current.Clear();

		var startNodes = target.nodes.Where(x => {
			var start = x as StartNode;
			return start != null && start.IsEnabled == true;
		});

		if (startNodes != null)
		{
			foreach (var startNode in startNodes)
			{
				var nextNodes = startNode.GetOutputPort("Next")?.GetConnections().Select(x => x.node as SystemNode);

				if (nextNodes == null || nextNodes.Count() == 0)
				{
					Logger.Log("Start is not connected to a system node");
				}
				else
				{
					foreach (var n in nextNodes)
					{
						RecursiveReorder(n, 1);

					}
				}
			}
		}
	}


	private void RecursiveReorder(SystemNode node, int depth)
    {
		var nextNodes = node.GetOutputPort("Next")?.GetConnections().Select(x => x.node as SystemNode);

		if (nextNodes == null || nextNodes.Count() == 0)
		{
		}
		else
		{
			depth++;
            foreach (var n in nextNodes)
            {
				RecursiveReorder(n, depth);
				n.position.x = depth * 400f;
			}
		}
	}
}
