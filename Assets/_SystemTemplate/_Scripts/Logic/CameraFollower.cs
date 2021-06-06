using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public float CanvasOffsetZ = 1;
    [SerializeField] private float _motionSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 8f;
    public Transform _followed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _followed.position /*+ Vector3.forward* CanvasOffsetZ*/, Time.deltaTime* _motionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _followed.rotation, Time.deltaTime* _rotateSpeed);
    }
}
