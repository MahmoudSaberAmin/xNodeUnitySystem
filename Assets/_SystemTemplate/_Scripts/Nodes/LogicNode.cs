using System;
using System.Collections.Generic;
using System.Linq;
using XNode;
/// <summary>
/// This is still in developmen, 
/// 28 October 2020, 
/// Same behaviour can be obtsained by send and recieve.
/// </summary>
public class LogicNode : SystemNode
{
    public EnumName Name = EnumName.DataTag;

    [NonSerialized] private EnumName _lastName = EnumName.DataKey;


    public string VariableName;
    // Use this for initialization
    public LogicNode() : base()
    {
        Type = SystemType.Logic;
    }

    // Return the correct value of an output port when requested
    public override object GetValue(NodePort port)
    {
        return null;
    }


    public void CreateOutputPorts()
    {
        if (_lastName != Name)
        {
            var type = GameContstants.EnumDictionary[Name];

            ClearDynamicPorts();
            var values = System.Enum.GetValues(type);
            System.Array.Reverse(values);

            foreach (var value in values)
            {
                AddDynamicOutput(type, ConnectionType.Multiple, TypeConstraint.None, value.ToString());
            }

        }
        _lastName = Name;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<LogicController>();
    }

  

}