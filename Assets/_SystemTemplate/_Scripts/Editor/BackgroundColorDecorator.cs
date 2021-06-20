// BackgroundColorDecorator.cs  NOTE: need to be inside an editor folder
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(BackgroundColorAttribute))]
public class BackgroundColorDecorator : DecoratorDrawer
{
    BackgroundColorAttribute attr { get { return ((BackgroundColorAttribute)attribute); } }
    public override float GetHeight() { return 0; }

    public override void OnGUI(Rect position)
    {
        GUI.backgroundColor = attr.color;
    }
}