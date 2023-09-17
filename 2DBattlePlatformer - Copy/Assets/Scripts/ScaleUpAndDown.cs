using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpAndDown : MonoBehaviour
{
    [SerializeField] float _frequency = 10.0f;
    Vector3 _startScale;
    
    void Start()
    {
        _startScale = transform.localScale;
    }

    void Update()
    {
        // float waveFormula = Mathf.Cos(_frequency * Time.timeSinceLevelLoad + Mathf.PI + timeInvulnerable) + 1;

        //                        Cos(        x        *  frequency *    pi) * 0.5f + 0.5f
        float waveFormula = Mathf.Sin(Time.time * _frequency) * 0.5f + 0.5f;
        waveFormula *= _startScale.x / 3;
        //Debug.Log(waveFormula);

        transform.localScale =
            new Vector3(_startScale.x + waveFormula, _startScale.x + waveFormula, 0);
    }
}
