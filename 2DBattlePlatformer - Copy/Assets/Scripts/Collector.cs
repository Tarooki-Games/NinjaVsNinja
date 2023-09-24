using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour
{
    [SerializeField] List<Collectible> _collectibles;
    [SerializeField] UnityEvent _onCollectionComplete;

    TMP_Text _remainingText;
    int _countCollected;
    int _countRemaining;

    void Start()
    {
        _remainingText = GetComponentInChildren<TMP_Text>();

        foreach (var collectible in _collectibles)
        {
            collectible.OnCollected += Collectible_OnCollected;
        }

        _countRemaining = _collectibles.Count - _countCollected;

        _remainingText?.SetText(_countRemaining.ToString());
    }

    public void Collectible_OnCollected()
    {
        _countCollected++;
        _countRemaining = _collectibles.Count - _countCollected;
        _remainingText?.SetText(_countRemaining.ToString());

        if (_countRemaining > 0)
            return;

        _onCollectionComplete.Invoke();
    }

    void OnValidate()
    {
        _collectibles = _collectibles.Distinct().ToList();
    }

    void OnDrawGizmos()
    {
        foreach (var collectible in _collectibles)
        {
            Gizmos.color = UnityEditor.Selection.activeGameObject == gameObject ? Color.yellow : Color.gray;

            Gizmos.DrawLine(transform.position, collectible.transform.position);
        }
    }

    private void OnDisable()
    {
        foreach (var collectible in _collectibles)
        {
            collectible.OnCollected -= Collectible_OnCollected;
        }
    }
}
