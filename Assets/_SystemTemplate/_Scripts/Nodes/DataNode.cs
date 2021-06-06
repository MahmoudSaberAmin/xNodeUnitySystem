using UnityEngine;

[System.Serializable]
public class DataNode : SystemNode
{
    /// <summary>
    /// Data node is still in development, 
    /// 28 Octobe 2020
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public string VariableName;
    public string VariableValue;
    public DataNode() : base()
    {
        Type = SystemType.Data;
    }
    

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<DataController>();
    }
}