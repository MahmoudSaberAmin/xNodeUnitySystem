using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// This is the node of text controller which displays text to user.
/// </summary>
[System.Serializable]
[NodeWidth(300)]
[NodeTint(0.2f, 0.2f, 0.4f)]
public class TextNode : SystemNode
{
    /// <summary>
    /// The canvas of the text to display,  
    /// Must have a textmeshpro child, 
    /// We use rtlmpro to write arabic, 
    /// so this prefab must have an rtlmpro child, 
    /// This field gets a default value of a basic canvas text with rtmlpro in PutScriptsOnImplentation method.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public GameObject WorldCanvasPrefab;
    /// <summary>
    /// Text to display to the user.
    /// </summary>
    [TextArea]
    [OnValueChanged("SetColor")]
    [GUIColor("GetColor")]
    public string TextString;
    /// <summary>
    /// Duration before stopping the text from being displayed.
    /// </summary>
    [Header("0 for Inifinity...")]
    public float ShowTime = 2.5f;

    /// <summary>
    /// True if canvas should follow the VR camera with lerp.
    /// </summary>
    [Space]
    public bool IsCameraFollow = false;
    /// <summary>
    /// Scale of the text.
    /// </summary>
    [Range(0.1f, 5f)]    
    public float Scale = 1f;

    [HideInInspector]
    public bool _isModified = false;

    public TextNode() : base()
    {
        Type = SystemType.Text;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<TextController>();
    }

    /// <summary>
    /// We assign the world canvas prefab default value here.
    /// </summary>
    public override void PutScriptsOnImplentation()
    {
        base.PutScriptsOnImplentation();

        WorldCanvasPrefab = Resources.Load<GameObject>("CanvasText");
    }


    private Color GetColor() { return this._isModified == false ? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }
}