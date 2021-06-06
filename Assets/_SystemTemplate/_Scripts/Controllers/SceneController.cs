using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Scene Controller is A sub system controller which loads a new scene.
/// </summary>
public class SceneController : SystemController
{
    /// <summary>
    /// The scene node which holds data like Next Scene Name, should the scene load in background, Load mode: Is addittive or single.
    /// </summary>
    [SerializeField] private SceneNode _assignedNode;

    /// <summary>
    /// Assigns Scene node as a system node to the scene controller.
    /// </summary>
    /// <param name="node">Scene node as a system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as SceneNode;
    }


    /// <summary>
    /// Load the scene with the node parameters.
    /// </summary>
    /// <param name="other">Collided GameObject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (!string.IsNullOrWhiteSpace(_assignedNode?.NextSceneName))
        {
            SystemNode.IsSystemPlaying = true;
            var mode = _assignedNode.IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            if (_assignedNode.IsLoadInBackground)
            {
                SceneManager.LoadSceneAsync(_assignedNode.NextSceneName, mode);
            }
            else
            {
                SceneManager.LoadScene(_assignedNode.NextSceneName, mode);
            }
        }

        EndSystem();
    }


   
}
