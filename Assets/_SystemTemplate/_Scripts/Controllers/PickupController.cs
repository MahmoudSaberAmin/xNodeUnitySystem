using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// A subclass from system controller which enables user to hold an object and leave it to snap to an end transform.
/// </summary>
public class PickupController : SystemController, ISystemValidate, IPickup
{
    /// <summary>
    /// A sub class from system node which contain pickup paramters like snap distance and end transform.
    /// </summary>
    [SerializeField] private PickupNode _assignedNode;

    /// <summary>
    /// The node assigns itself to a controller.
    /// </summary>
    /// <param name="node"></param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as PickupNode;
    }

    /// <summary>
    /// Assigns the end transform at playtime.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    /// <summary>
    /// This method delegates Input factory which is a factory that decouples Input like VR Input from the controller, 
    /// Input is used to attach a gameobject by adding a throwable component to the hand in VR or some behaviour like this if keyboard or etc..
    /// </summary>
    /// <param name="other">Collided gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)//TODO
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            _assignedNode.IsSystemPlaying = true;
            //Andrew
            InputFactory.CreatePickupInput(this, _assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject, _assignedNode.InputType);
        }
    }

    /// <summary>
    /// Unbind Input on leaving the node.
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();
        InputFactory.UnbindVRInput(this, _assignedNode.InputType);
    }

    /// <summary>
    /// validates the end transform and the implementations.
    /// </summary>
    /// <returns>true if valid implementations and transform</returns>
    public bool IsValidted()
    {
        //other.transform.GetChild(0).gameObject  //TODO Rewrite
        if (_assignedNode.Implementations.FirstOrDefault() != null)
        {
            if (_assignedNode.EndTransform == null)
            {
                Logger.Log("Error, There is no Pickup transform on this trigger");
            }
            else
            {
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is no Snapobject " + gameObject.name);
        }

        return false;
    }

    /// <summary>
    /// On Object Pickup
    /// </summary>
    public void Pickup()
    {
        Logger.Log("Object attached");
    }

    /// <summary>
    /// On Object Detach
    /// Remove the pickup Input binding from this controller.
    /// </summary>
    public void Detach()
    {
        Logger.Log("On Detach + distance: " + Vector3.Distance(_assignedNode.Implementations.FirstOrDefault().transform.parent.position, _assignedNode.EndTransform.position));
        if (Vector3.Distance(_assignedNode.Implementations.FirstOrDefault().transform.parent.position, _assignedNode.EndTransform.position) < _assignedNode.MinDistance)
        {
            _assignedNode.Implementations.FirstOrDefault().transform.parent.position = _assignedNode.EndTransform.position;
            _assignedNode.Implementations.FirstOrDefault().transform.parent.rotation = _assignedNode.EndTransform.rotation;
            InputFactory.RemovePickupInput(this, _assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject, _assignedNode.InputType);
            EndSystem();
        }
    }

    /// <summary>
    /// Draws the end transform Gizmos as agreen sphere in the scene, N.B: not visible in VR Game mode
    /// </summary>
    void OnDrawGizmos()
    {
        if (_assignedNode.IsUnlocked)
        {
            if (_assignedNode.EndTransform==null)
            {
                _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
            }
            // Draw a yellow sphere at the transform's position
            Gizmos.color = new Color32(0, 255, 0, 50);
            Gizmos.DrawSphere(_assignedNode.EndTransform.position, _assignedNode.MinDistance);
        }
    }
}
