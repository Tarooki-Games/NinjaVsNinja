using System;
using UnityEngine;
using UnityEngine.Events;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] Sprite _leftSwitchSprite;
    [SerializeField] Sprite _rightSwitchSprite;
    [SerializeField] UnityEvent _onSwitchEntered;
    [SerializeField] UnityEvent _onToggledLeft;
    [SerializeField] UnityEvent _onToggledRight;

    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;
    Sprite _middleSwitchSprite;

    enum SwitchDirection
    {
        Left,
        Middle,
        Right,
    }

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _middleSwitchSprite = _spriteRenderer.sprite;
        _audioSource = GetComponent<AudioSource>();
    }

    // Could probably lose these next 9 lines but I like that the player can see their interaction with the switch without having to exit the collider.
    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        SwitchEntered();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        if (player.transform.position.x < transform.position.x)
            ToggleSwitchLeft();
        else
            TogglSwitchRight();

    }

    void SwitchEntered()
    {
        _spriteRenderer.sprite = _middleSwitchSprite;
        _onSwitchEntered.Invoke();
    }

    void ToggleSwitchLeft()
    {
        _spriteRenderer.sprite = _leftSwitchSprite;
        _onToggledLeft.Invoke();
        if (_audioSource != null)
            _audioSource.Play();
    }

    void TogglSwitchRight()
    {
        _spriteRenderer.sprite = _rightSwitchSprite;
        _onToggledRight.Invoke();
        if (_audioSource != null)
            _audioSource.Play();
    }
}
