using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace XNodeEditor.Examples
{
	[CustomNodeEditor(typeof(LogicNode))]
	public class LogicNodeEditor : NodeEditor
	{
		private float _lastUpdateTime = 0;
		private float _updateDuration= 1;

		public override void OnHeaderGUI()
		{
			GUI.color = Color.white;
			LogicNode node = target as LogicNode;
			SystemsGraph graph = node.graph as SystemsGraph;

			GUI.color = Color.cyan;

			string title = "Switch ("+ node.Name.ToString() + ")";
			
			GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
			GUI.color = Color.white;
		}

		public override void OnBodyGUI()
		{
			base.OnBodyGUI();

			LogicNode node = target as LogicNode;
			SystemsGraph graph = node.graph as SystemsGraph;


            if (EditorApplication.timeSinceStartup - _lastUpdateTime> _updateDuration)
            {
				_lastUpdateTime = Time.time;

				node.CreateOutputPorts();
            }
		}
	}
}