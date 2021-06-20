using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ScenePhysicsTool : EditorWindow
{
    private bool _toggleValue = false;
    private void OnGUI()
    {
        _toggleValue = GUILayout.Toggle(_toggleValue, "Run Physics");
    }

    private void Update()
    {
        if (_toggleValue)
        {
            StepPhysics();
        }
    }

    private void StepPhysics()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;
    }

    [MenuItem("Tools/Scene Physics")]
    private static void OpenWindow()
    {
        GetWindow<ScenePhysicsTool>(false, "Physics", true);
    }
}