using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] ActionSequencer _sequencer;

    public Transform GeneralPosition;
    public Transform ResolvePosition;

    [SerializeField] private float _cameraMovementDuration;

    public async void MoveCameraTo(Transform target)
    {
        CameraMoveAction moveAction = new CameraMoveAction(_camera, target, _cameraMovementDuration, this.GetCancellationTokenOnDestroy());
        await _sequencer.InsertAction(moveAction);


        //_camera.DOMove(target.position, _cameraMovementDuration);
        //_camera.DORotateQuaternion(target.rotation, _cameraMovementDuration);
    }

}
