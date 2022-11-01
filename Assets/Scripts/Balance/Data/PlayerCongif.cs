using System;
using Balance.Data;
using UnityEngine;

namespace Balance.Configs
{
    [UnityEngine.CreateAssetMenu(fileName = "PlayerCongif", menuName = "ScriptableObject/PlayerCongif", order = 0)]
    public class PlayerCongif : ScriptableObject
    {
        [SerializeField] private CameraConfig _cameraConfig;
        [SerializeField] private MovementConfig _movementConfig;
        [SerializeField] private float _time;
        [SerializeField] private Color _color;

    

        [Serializable]
        private class MovementConfig
        {
            public float Speed;
            public float RotateSpeed;
            public float DashSpeed;
            public float DashDistance;
        }
        [Serializable]
        private class CameraConfig
        {
            public float Speed;
            public float MaxAngle;
            public float MinAngle;
        }
        
        
        public float Time => _time;

        public Color Color => _color;

        public CameraData GetCameraData()
        {
            return new CameraData()
            {
                Speed = _cameraConfig.Speed,
                MinYAngle = _cameraConfig.MinAngle,
                MaxYAngle = _cameraConfig.MaxAngle
            };
        }
        
        

        public PlayerMovementData GetMovementData()
        {
            return new PlayerMovementData()
            {
                Speed = _movementConfig.Speed,
                DashSpeed = _movementConfig.DashSpeed,
                DashDistance = _movementConfig.DashDistance,
                RotateSpeed = _movementConfig.RotateSpeed
            };
        }
    }
}