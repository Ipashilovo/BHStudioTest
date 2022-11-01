using Balance.Data;
using UnityEngine;

namespace Core.PlayerSystems.Movement
{
    public class CameraMovement : IUpdatable
    {
        private readonly Camera _camera;
        private readonly CameraData _cameraData;
        private float _rotationgAngle;
        private readonly float _startRotatingX;

        public CameraMovement(Camera _camera, CameraData cameraData)
        {
            _startRotatingX = _camera.transform.localRotation.x;
            this._camera = _camera;
            _cameraData = cameraData;
        }

        public void Update()
        {
            _rotationgAngle -= Input.GetAxis("Mouse Y") * _cameraData.Speed * Time.deltaTime;
            _rotationgAngle = Mathf.Clamp(_rotationgAngle, _cameraData.MinYAngle, _cameraData.MaxYAngle);
            _camera.transform.localRotation = Quaternion.Euler(_rotationgAngle + _startRotatingX, 0, 0);
        }
    }
}