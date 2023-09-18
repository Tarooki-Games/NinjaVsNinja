using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Image _uiFill;
    [SerializeField] TMP_Text _uiText;
    public bool _mainTimer;

    public int _duration;
    float _remainingDuration;

    [SerializeField] float _intervalTime;
    
    public event Action OnRoundTimeUp;
    public event Action OnIntervalAction;

    void Start()
    {
        _uiText.text = $"{_duration}";
    }

    public void Begin(int second)
    {
        _remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    float timePassed = 0f;
    
    IEnumerator UpdateTimer()
    {
        if (BattleManager.GetInstance().IsBattling)
        {
            while (_remainingDuration >= 0)
            {
                timePassed += Time.deltaTime;
                if (timePassed > _intervalTime)
                {
                    OnIntervalAction?.Invoke();

                    timePassed = 0f;
                }

                _uiText.text = $"{Mathf.CeilToInt(_remainingDuration)}";
                _uiFill.fillAmount = Mathf.InverseLerp(0, _duration, _remainingDuration);
                _remainingDuration -= Time.deltaTime;
                yield return new WaitForSeconds(0f);
            }
        }

        OnEnd();
    }

    private void OnEnd()
    {
        //Do End of Round Stuff
        Debug.Log(this + " ----- END OF ROUND -----");

        if (_mainTimer)
        {
            OnRoundTimeUp?.Invoke();
        }
    }
}