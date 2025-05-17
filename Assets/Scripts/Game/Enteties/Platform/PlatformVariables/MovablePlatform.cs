using System;
using UnityEngine;

public class MovablePlatform : BasicPlatformController
{
    private MovablePlatformConfig _movablePlatformConfig;

    private float _cameraLeftBound;
    private float _cameraRightBound;
    private float _leftLimit;
    private float _rightLimit;
    private float _currentDiraction;
    private float _currentSpeed;

    public override void Init(BasicPlatformConfig basicPlatformConfig)
    {
        base.Init(basicPlatformConfig);

        _movablePlatformConfig = basicPlatformConfig as MovablePlatformConfig;

        if (_movablePlatformConfig == null)
        {
            throw new InvalidCastException($"[Init] _movablePlatformConfig is not of type MovablePlatformConfig in {gameObject.name}");
        }
        else
        {
            _currentDiraction = _movablePlatformConfig.StartDirection;
        }

        SetSpeedValue();
        SetBounds();
    }

    public override void UpdatePlatform()
    {
        base.UpdatePlatform();
        MovePlatform();
    }

    private void SetSpeedValue()
    {
        _currentSpeed = _movablePlatformConfig.BasicSpeed;
    }

    private void SetBounds()
    {
        if (!_movablePlatformConfig.UseCustomMovementLimits)
        {
            Camera mainCam = Camera.main;
            float camHeight = 2f * mainCam.orthographicSize;
            float camWidth = camHeight * mainCam.aspect;

            Vector3 camPos = mainCam.transform.position;
            _cameraLeftBound = camPos.x - camWidth / 2f;
            _cameraRightBound = camPos.x + camWidth / 2f;

            _leftLimit = _cameraLeftBound;
            _rightLimit = _cameraRightBound;
        }
        else
        {
            _leftLimit = _movablePlatformConfig.LeftLimit;
            _rightLimit = _movablePlatformConfig.RightLimit;
        }
    }

    private void MovePlatform()
    {
        transform.Translate(Vector3.right * _currentDiraction * _currentSpeed * Time.deltaTime);

        if (transform.position.x > _rightLimit)
        {
            transform.position = new Vector3(_rightLimit, transform.position.y, transform.position.z);
            _currentDiraction *= -1;
        }
        else if (transform.position.x < _leftLimit)
        {
            transform.position = new Vector3(_leftLimit, transform.position.y, transform.position.z);
            _currentDiraction *= -1;
        }
    }
}