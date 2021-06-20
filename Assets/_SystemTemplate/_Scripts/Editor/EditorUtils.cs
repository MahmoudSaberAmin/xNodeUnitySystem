using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorUtils 
{
	//public static UnityEditor.SerializedObject CreateNewTag(this UnityEditor.SerializedObject tagManager, string tag = "")
	//{
 //       if (tagManager==null)
 //       {

	//		tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
 //       }

 //       // Open tag manager
	//	UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

	//	// Adding a Tag
	//	string s = tag;

	//	// First check if it is not already present
	//	bool found = false;
	//	for (int i = 0; i < tagsProp.arraySize; i++)
	//	{
	//		UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
	//		if (t.stringValue.Equals(s)) { found = true; break; }
	//	}

	//	// if not found, add it
	//	if (!found)
	//	{
	//		var index = tagsProp.arraySize;
	//		tagsProp.InsertArrayElementAtIndex(index);
	//		UnityEditor.SerializedProperty n = tagsProp.GetArrayElementAtIndex(index);
	//		n.stringValue = s;
	//	}

	//	// and to save the changes
	//	return tagManager;
	//}

	//public static void ApplyModifications(this UnityEditor.SerializedObject tagManager)
 //   {
	//	tagManager.ApplyModifiedProperties();
	//}

	//public static void RemoveTag(string tag)
 //   {
	//	// Open tag manager
	//	UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
	//	UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

	//	// Adding a Tag
	//	string s = tag;

	//	// First check if it is not already present
	//	for (int i = 0; i < tagsProp.arraySize; i++)
	//	{
	//		UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
	//		if (t.stringValue.Equals(s)) {
	//			t.stringValue = "";
	//			break; 
	//		}
	//	}

	//	// and to save the changes
	//	tagManager.ApplyModifiedProperties();
	//}

 //   public static void RemoveTags(List<string> tags)
 //   {
	//	// Open tag manager
	//	UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
	//	UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

	//	// Adding a Tag

	//	// First check if it is not already present
	//	for (int i = 0; i < tagsProp.arraySize; i++)
	//	{
	//		UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
	//		if (tags.Contains(t.stringValue))
	//		{
	//			Debug.LogError(t.stringValue + " deleted");
	//			t.stringValue = "";
	//		}
	//	}

	//	// and to save the changes
	//	tagManager.ApplyModifiedProperties();
	//}

 //   public static string[] ListTags()
 //   {
	//	// Open tag manager
	//	UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
	//	UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

	//	var tagsList = new List<string>();

	//	// First check if it is not already present
	//	for (int i = 0; i < tagsProp.arraySize; i++)
	//	{
	//		UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
 //           if (!t.stringValue.Contains("/"))
 //           {
	//			tagsList.Add(t.stringValue);
 //           }
	//	}

	//	return tagsList.ToArray();
	//}    
	
	public static string[] ListAllTags()
    {
		// Open tag manager
		UnityEditor.SerializedObject tagManager = new UnityEditor.SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
		UnityEditor.SerializedProperty tagsProp = tagManager.FindProperty("tags");

		var tagsList = new List<string>();

		// First check if it is not already present
		for (int i = 0; i < tagsProp.arraySize; i++)
		{
			UnityEditor.SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
			tagsList.Add(t.stringValue);
		}

		return tagsList.ToArray();
	}
}
