using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Remember to wrap your custom attribute drawer within a #if UNITY_EDITOR condition, or locate the file inside an Editor folder.
public class CustomRangeAttributeDrawer : OdinAttributeDrawer<SystemV2TagAttribute, String>
{
    private string currentSelectedType;
    private static List<string> currentSource = new List<string>();

    protected override void DrawPropertyLayout(GUIContent label)
    {
        currentSource = OdinSelector<string>.DrawSelectorDropdown(label, string.IsNullOrWhiteSpace(currentSelectedType) ? this.ValueEntry.SmartValue : currentSelectedType, (rect) =>
        {
            MySelector selector = new MySelector(currentSource, true);
            selector.SetSelection(currentSelectedType?.Split(','));
            selector.ShowInPopup(rect);
            return selector;
        }, null)?.ToList();

        currentSource?.Sort();

        if (currentSource!=null)
        {
            foreach (var item in currentSource)
            {
                Debug.LogError(item);
            }

            currentSelectedType = this.ValueEntry.SmartValue = currentSource?.Count()>1? currentSource.Aggregate((x, y)=> x+","+y): currentSource?.FirstOrDefault()?? "";
        }

    }
}


public class MySelector : OdinSelector<string>
{
    private readonly List<string> source;

    [OdinSerializer.OdinSerialize]
    private static string[] extraSource;
    private readonly bool supportsMultiSelect;

    public MySelector(List<string> source, bool supportsMultiSelect)
    {
        this.source = source;
        this.supportsMultiSelect = supportsMultiSelect;
        extraSource = Resources.Load<TextAsset>("Tags").text.Replace("\n", "").Split(',');
    }


    protected override void BuildSelectionTree(OdinMenuTree tree)
    {
        tree.Config.DrawSearchToolbar = true;
        tree.Selection.SupportsMultiSelect = this.supportsMultiSelect;

        tree.Add("Defaults/None", null);
        tree.Add("Defaults/Player", "Player");
        tree.Add("Defaults/RightHand", "RightHand");

        tree.Add("Custom/Controller", "Controller");
        tree.Add("Custom/Implementation", "Implementation");
        tree.Add("Custom/Date","Date,"+ DateTime.Now.ToString());
        tree.Add("Custom/Animation", "Animation");
        tree.Add("Custom/Audio", "Audio");
        tree.Add("Custom/Tween", "Tween");

        tree.AddRange(extraSource, x => "Custom2/"+x);
    }

    [OnInspectorGUI]
    private void DrawInfoAboutSelectedItem()
    {
        string selected = this.GetCurrentSelection().Count()> 1? this.GetCurrentSelection().Aggregate((x, y) => x + "," + y) : this.GetCurrentSelection().FirstOrDefault()??"";

        if (selected != null)
        {
            GUILayout.Label("Tag: " + selected);
        }
    }
}

