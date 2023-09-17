using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasWarningLights : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _warningLights;
    [SerializeField] protected Sprite _redLights;

    protected Sprite _startLights;

    protected bool _warningCoroutineRunning = false;

    protected virtual void Start()
    {
        _startLights = _warningLights.sprite;
    }

    protected IEnumerator WarningLights()
    {
        _warningCoroutineRunning = true;
        _warningLights.sprite = _redLights;
        yield return new WaitForSeconds(0.45f);
        _warningLights.sprite = _startLights;
        yield return new WaitForSeconds(0.45f);
        _warningLights.sprite = _redLights;
        yield return new WaitForSeconds(0.45f);
        _warningLights.sprite = _startLights;
        yield return new WaitForSeconds(0.45f);
        _warningLights.sprite = _redLights;
        yield return new WaitForSeconds(0.2f);
        _warningCoroutineRunning = false;
    }
}
