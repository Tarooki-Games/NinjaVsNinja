using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : HasWarningLights
{
    [SerializeField] Transform _target;
    [SerializeField] float _moveSpeed;

    float _direction = -1;

    Vector3 _startPos;
    Vector3 _endPos;
    float timer = 0.0f;
    float waitingTime = 5.0f;
    bool _waiting = true;

    protected override void Start()
    {
        base.Start();
        _startPos = transform.position;
        _endPos = _target.position;
    }

    protected void Update()
    {
        if (_waiting)
        {
            timer += Time.deltaTime;
            if (!_warningCoroutineRunning && timer > waitingTime - 2.0f)
            {
                StartCoroutine(WarningLights());
            }
            if (timer > waitingTime)
            {
                timer = 0f;
                _waiting = false;
            }
        }
        else
        {
            if (_direction == -1 && transform.position.y > _endPos.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, _endPos, _moveSpeed * Time.deltaTime);
            }
            else
            {
                _direction = 1;

                transform.position = Vector2.MoveTowards(transform.position, _startPos, _moveSpeed * Time.deltaTime);
                if (transform.position.y == _startPos.y)
                {
                    _direction = -1;
                    _waiting = true;
                    _warningLights.sprite = _startLights;
                }
            }
        }
        //Debug.Log(_direction);
    }
}
