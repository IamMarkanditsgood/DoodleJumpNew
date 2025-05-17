using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEnemy : BasicEnemyController
{
    private MovableEnemyConfig _movableEnemyConfig;

    private float _cameraLeftBound;
    private float _cameraRightBound;
    private float _leftLimit;
    private float _rightLimit;
    private float _currentDiraction;
    private float _currentSpeed;

    public override void Init(BasicEnemyConfig basicEnemyConfig)
    {
        base.Init(basicEnemyConfig);

        _movableEnemyConfig = basicEnemyConfig as MovableEnemyConfig;

        if (_movableEnemyConfig == null)
        {
            throw new InvalidCastException($"[Init] _movableEnemyConfig is not of type MovableEnemyConfig in {gameObject.name}");
        }
        else
        {
            _currentDiraction = _movableEnemyConfig.StartDirection;
        }

        SetSpeedValue();
        SetBounds();
    }

    public override void UpdateEnemy()
    {
        base.UpdateEnemy();
        MoveEnemy();
    }
    private void MoveEnemy()
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

    private void SetSpeedValue()
    {
        _currentSpeed = _movableEnemyConfig.BasicSpeed;
    }

    private void SetBounds()
    {
        if (!_movableEnemyConfig.UseCustomMovementLimits)
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
            _leftLimit = _movableEnemyConfig.LeftLimit;
            _rightLimit = _movableEnemyConfig.RightLimit;
        }
    }    
}
