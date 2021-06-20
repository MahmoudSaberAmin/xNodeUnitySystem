using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Text controller is a system controller subclass which shows text to user with customization.
/// </summary>
public class TextController : SystemController, ISystemValidate
{
    /// <summary>
    /// The text node which is a system node subclass.
    /// </summary>
    [SerializeField] private TextNode _assignedNode;

    /// <summary>
    /// The prefab of the canvas to instantiate. this canvas holds the text or text mesh pro component.
    /// </summary>
    private GameObject _worldCanvas;


    /// <summary>
    /// The text node assigns it self to the system controller.
    /// </summary>
    /// <param name="node">Text node as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as TextNode;
    }

    /// <summary>
    /// Validates the text and the world canvas prefab.
    /// </summary>
    /// <returns>true if valid text and canvas prefab</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.TextString != null)
        {
            Logger.Log(_assignedNode?.TextString);
            if (_assignedNode.WorldCanvasPrefab == null)
            {
                Logger.Log("Error, Please assign a world canvas");
            }
            else
            {
                return true;

            }
        }
        else
        {
            Logger.Log("Error, There is no txt on this trigger");
        }

        return false;
    }

    /// <summary>
    /// Destroys the world canvas on leaving the node.
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();
        if (_worldCanvas!=null)
        {
            Destroy(_worldCanvas);
        }
    }

    /// <summary>
    /// Instantiates an instance from world canvas prefab,
    /// if is camera follow true it is parented to the implementation gameobject,
    /// its local position and rotation is reseted, 
    /// We get the text component(Rtl support component), assigns text to it and hides after show time
    /// 
    /// here is a link for the arabic text mesh pro compoenent
    /// https://github.com/mnarimani/RTLTMPro
    /// 
    /// Here is the Readme of the GitHub
    /// You need to have TextMeshPro plugin in your project. You can install TMPro via Package Manager. DO NOT Install Text Mesh Pro from Asset Store.
    /// 
    //    Go to release page and download latest unitypackage file(or copy RTLTMPro folder from source to your project.)
    //  Open one of the range files in Assets/RTLTMPro/Ranges/ folder using your favorite text editor.
    //  RTL Letters are in LetterRanges.txt file
    //  English, Arabic and Farsi numbers are in NumberRanges.txt file
    //  Arabic tashil are in TashkilRanges.txt file.
    //  Make sure you have copied ranges that you want to use
    //  Open Window/TextMeshPro/Font Asset Creator window.
    //  Assign your font in Font Source field (Your font must support RTL characters)
    //  Set Character Set to Unicode Range
    //  Paste copied ranges inside Character Sequence (Hex)
    //  Press Generate Font Atlas button and wait for it to generate the atlas file.
    //  Press Save TextMeshPro Font Asset and save the asset.
    //  Use GameObject/UI/* - RTLTMP menu to create RTL UI elements. (Alternatively you can replace Text Mesh Pro UGUI components with RTL Text Mesh Pro)
    //  Assign your font asset Font Asset property in RTL Text Mesh Pro component
    //  Enter text in RTL TEXT INPUT BOX secion.
    /// 
    /// We use Unity 2019.4.8
    /// 
    /// In case of unity update make sure to follow the link and update the plugin, or use a different rtl plugin
    /// all what is needed is replacing this line
    /// var txt = _worldCanvas.GetComponentInChildren<RTLTMPro.RTLTextMeshPro>();
    /// by
    /// var txt = _worldCanvas.GetComponentInChildren<SomeNewScript>();
    /// </summary>
    /// <param name="other">Collided gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;

        if (_assignedNode.IsCameraFollow == false)
        {
            _worldCanvas = Instantiate(_assignedNode.WorldCanvasPrefab,  _assignedNode.Implementations.FirstOrDefault().transform);
        }
        else
        {
            _worldCanvas = Instantiate(_assignedNode.WorldCanvasPrefab);
        }

        _worldCanvas.transform.localPosition = Vector3.zero;
        _worldCanvas.transform.localRotation = Quaternion.identity;
        _worldCanvas.transform.localScale = Vector3.one * _assignedNode.Scale;



        var txt = _worldCanvas.GetComponentInChildren<RTLTMPro.RTLTextMeshPro>();
        if (txt == null)
        {
            Logger.Log("Error, There is no text component");
        }
        else
        {
            //var follower = _worldCanvas.GetComponentInChildren<CameraFollower>();
            //follower.CanvasOffsetZ = _assignedNode.DistanceFromCamera;
            //follower._followed = _assignedNode.Implementations.FirstOrDefault().transform;
            //follower.enabled = _assignedNode.IsCameraFollow;

            txt.text = _assignedNode?.TextString;
            if (_assignedNode.ShowTime > 0)
            {
                yield return new WaitForSeconds(_assignedNode.ShowTime);
                Destroy(_worldCanvas);
                yield return null;
            }
        }

        EndSystem();
    }

}
