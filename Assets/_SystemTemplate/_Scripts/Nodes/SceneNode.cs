using UnityEngine;
/// <summary>
/// This node is used by Scene controller to Load another scene.
/// </summary>
[System.Serializable]
public class SceneNode : SystemNode
{
    /// <summary>
    /// The name of the scene to load.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public string NextSceneName;
    /// <summary>
    /// True to load the scene in the background.
    /// </summary>
    public bool IsLoadInBackground;
    /// <summary>
    /// additve scenes doest not remove original scenes, 
    /// Very useful when there are performance bottleneck where you can divide the scene into chunks and load them over time, 
    /// Follow this link for more info 
    /// https://www.youtube.com/watch?v=zObWVOv1GlEhttps://www.youtube.com/watch?v=zObWVOv1GlE
    /// </summary>
    public bool IsAdditive;

    public SceneNode() : base()
    {
        Type = SystemType.Scene;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<SceneController>();
    }
}