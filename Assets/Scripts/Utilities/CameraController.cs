using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _camera;

    public Transform GeneralPosition;
    public Transform ResolvePosition;

    [SerializeField] private float _cameraMovementDuration;

    public void MoveCameraTo(Transform target)
    {
        _camera.DOMove(target.position, _cameraMovementDuration);
        _camera.DORotateQuaternion(target.rotation, _cameraMovementDuration);
    }

}
