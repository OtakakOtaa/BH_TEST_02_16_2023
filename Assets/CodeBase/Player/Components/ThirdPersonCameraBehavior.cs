using System;
using UnityEngine;

namespace CodeBase.Player.Components
{
    public class ThirdPersonCameraBehavior
    {
        private readonly Settings _settings;

        const int SensitivityMultiplier = 300;

        private readonly Transform _person;
        private readonly Camera _camera;
        private readonly PlayerInput _playerInput;

        private Vector3 _remoteVector;
        private float _originVerticalCameraAngle;
        private float _lastVerticalPosition;
        
        public ThirdPersonCameraBehavior(Transform person, Camera camera, PlayerInput playerInput, Settings settings)
        {
            _person = person;
            _camera = camera;
            _playerInput = playerInput;
            _settings = settings;

            PrepareCameraRotation();
            CalculateRemoteCameraVector();
            FollowTheTarget();
            CaptureTarget();
            CashOriginCameraRotation();
        }

        public void UpdateState()
        {
            TryRotatePerson();
        }

        public void UpdateLastState()
        {
            UpdateCameraVerticalRotation();
            FollowTheTarget(); 
        }
 
        private void FollowTheTarget()
            => _camera.transform.position = _person.position + _person.rotation * _remoteVector;

        private void CaptureTarget()
            => _camera.transform.LookAt(_person);

        private void UpdateCameraVerticalRotation()
        {
            float x = -_settings.Sensitivity * SensitivityMultiplier * _playerInput.MousePosition.y * Time.deltaTime;
            x = ClampRotation(_lastVerticalPosition + x);
            
            _camera.transform.rotation = Quaternion.Euler(x, _person.rotation.eulerAngles.y, 0);
            _lastVerticalPosition = x;
        }

        private void TryRotatePerson()
        {
            float y = _settings.Sensitivity * SensitivityMultiplier * _playerInput.MousePosition.x * Time.deltaTime;
            var rotation = _person.rotation.eulerAngles;
            _person.transform.rotation = Quaternion.Euler(rotation.x, rotation.y + y, rotation.z);
        }
        
        private void PrepareCameraRotation()
            => _camera.transform.rotation = Quaternion.Euler(0, 0, 0);

        private void CalculateRemoteCameraVector()
        {
            _remoteVector = new Vector3
            {
                x = 0,
                y = _settings.RemoteHeight,
                z = -(float)Math.Sqrt(Math.Pow(_settings.RemoteDistance, 2) - Math.Pow(_settings.RemoteHeight, 2))
            };
        }

        private float ClampRotation(float angle)
        {
            return Math.Clamp(
                angle,
                _originVerticalCameraAngle - _settings.MaxVerticalAngleOffset,
                _originVerticalCameraAngle + _settings.MaxVerticalAngleOffset
            );
        }

        private void CashOriginCameraRotation() 
            => _originVerticalCameraAngle = _camera.transform.rotation.eulerAngles.x;

        [Serializable] public class Settings
        {
            [SerializeField, Range(0, 5f)] private float _remoteDistance;
            [SerializeField] private float _remoteHeight;
            [SerializeField, Range(0, 30)] private int _maxVerticalAngleOffset;
            [SerializeField, Range(0.1f, 1f)] private float _sensitivity;

            public float RemoteDistance => _remoteDistance;
            public float RemoteHeight => _remoteHeight;
            public int MaxVerticalAngleOffset => _maxVerticalAngleOffset;
            public float Sensitivity => _sensitivity;
        }
    }
}