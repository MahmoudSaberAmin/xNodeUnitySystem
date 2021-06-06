using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves a data in the player prefs, still in devevlopment.
/// </summary>
public class DataController : SystemController, ISystemValidate
{
    [SerializeField] private DataNode _assignedNode;

    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);
        _assignedNode = node as DataNode;
    }


    /// <summary>
    /// Writes the value into the key in Player prefs.
    /// </summary>
    /// <param name="other">Collided  gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;
            //PlayerPrefs.SetString(_assignedNode.DataTag.ToString()+ _assignedNode.Key.ToString() + _assignedNode.VariableName, _assignedNode.Value.ToString());
            PlayerPrefs.SetString(_assignedNode.VariableName, _assignedNode.VariableValue.ToString());
            EndSystem();
            Logger.Log("Success, Data Saved Successfully");
        }
    }

    /// <summary>
    /// Validates variable name.
    /// </summary>
    /// <returns>true if valid data values</returns>
    public bool IsValidted()
    {
        if (_assignedNode  !=null)
        {
            if (
                string.IsNullOrWhiteSpace(_assignedNode.VariableValue))
            {
                Logger.LogError("Error, Data is not valid, Please Assign data");
            }
            else
            {
                Logger.Log("Success, Saving Data ");
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is no Data Node " + gameObject.name);
        }

        return false;
    }

   
}
