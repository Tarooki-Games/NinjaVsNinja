using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] Transform _clockHandTransform;
    [SerializeField] TMP_Text _dayCounterText;

    const float REAL_SECONDS_PER_INGAME_DAY = 24f;

    float _dayValue;
    int _dayCount = 0;
    bool _nightTime = false;

    void Start()
    {
        _dayCounterText.SetText($"Days: {_dayCount}");
    }
    void Update()
    {
        _dayValue += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;
        float dayValueNormalized = _dayValue % 1f;
        float rotationDegreesPerDay = 360f;

        _clockHandTransform.eulerAngles = new Vector3(0, 0, -dayValueNormalized * rotationDegreesPerDay);

        if (!_nightTime && _dayValue > 0.75f)
        {
            _nightTime = true;
            Debug.Log("Do Night Time Stuff ONCE!!!");
        }

        if (_dayValue > 1)
        {
            _dayValue = 0;
            _nightTime = false;
            _dayCount++;
            _dayCounterText.SetText($"Days: {_dayCount}");
            if (_dayCount % 1 == 0)
                Debug.Log("Do End Of Day Thing ONCE!!!");
            if (_dayCount % 2 == 0)
                Debug.Log("Do End Of Second Day Thing ONCE!!!");
            if (_dayCount % 2 != 0)
                Debug.Log("Do End Of First and Every Second Day Thing ONCE!!!");
        }
    }
}
