using UnityEngine;
/// <summary>
/// This is  a legacy system Node used to instantiate a model and color it.
/// </summary>
[System.Serializable]
public class ModelNode : SystemNode
{
    /// <summary>
    /// Prefab of the model in assets
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public GameObject ModelPrefab;

    /// <summary>
    /// Color to be assigned to the first material of the object.
    /// </summary>
    public Color Color;

    public ModelNode() : base()
    {
        Type = SystemType.Model;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<ModelController>();
    }
}