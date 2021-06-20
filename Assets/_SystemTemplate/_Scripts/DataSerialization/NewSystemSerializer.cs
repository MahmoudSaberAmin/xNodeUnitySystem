using OdinSerializer;
using System.Collections.Generic;
using System.IO;

public static class NewSystemSerializer
{
    public static void SaveNode(List<SystemNode> data, string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.JSON);
        File.WriteAllBytes(filePath, bytes);
    }

    public static List<SystemNode> LoadNode(string filePath, int count)
    {
        byte[] bytes = File.ReadAllBytes(filePath);

        //var listOfSystemNodes = new List<UnityEngine.Object>();

        //for (int i = 0; i < count; i++)
        //{
        //    listOfSystemNodes.Add(SystemNode.CreateInstance<SystemNode>());
        //}



        return SerializationUtility.DeserializeValue<List<SystemNode>>(bytes, DataFormat.JSON

            //, listOfSystemNodes

            );
    }    
    
    
    
    public static void SaveGraph(SystemsGraph data, string filePath)
    {
        byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.JSON);
        File.WriteAllBytes(filePath, bytes);
    }

    public static SystemsGraph LoadGraph(string filePath)
    {
        byte[] bytes = File.ReadAllBytes(filePath);
        return SerializationUtility.DeserializeValue<SystemsGraph>(bytes, DataFormat.JSON);
    }
}
