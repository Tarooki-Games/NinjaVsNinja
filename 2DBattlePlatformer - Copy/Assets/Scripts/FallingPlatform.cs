using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public bool PlayerInside;

    HashSet<Player> _playersInTrigger = new HashSet<Player>();
    Coroutine _coroutine;
    Vector3 _initialPosition;
    float _shakeTimer;
    bool _shaking;
    bool _falling;

    [Tooltip("Resets the __shakeTimer when no players are on the platform (aka the HashSet<Player> _playersInTrigger is empty)")]
    [SerializeField] bool _resetOnEmpty;
    [SerializeField] float _fallSpeed = 7f;
    [Range(0.1f, 5f)]     [SerializeField] float _fallAfterSeconds = 2f;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeX = 0.03f;
    [Range(0.005f, 0.1f)] [SerializeField] float _shakeY = 0.03f;

    void Start()
    {
        _initialPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        _playersInTrigger.Add(player);

        PlayerInside = true;

        if (_playersInTrigger.Count == 1)
            _coroutine = StartCoroutine(ShakeThenFall());
    }

    IEnumerator ShakeThenFall()
    {
        Debug.Log("Platform Waiting To Shake");
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Platform Shaking");
        _shaking = true;

        while (_shakeTimer < _fallAfterSeconds)
        {
            float randomX = UnityEngine.Random.Range(-_shakeX, _shakeX);
            float randomY = UnityEngine.Random.Range(-_shakeY, _shakeY);
            transform.position = _initialPosition + new Vector3(randomX, randomY);
            float randomDelay = UnityEngine.Random.Range(0.005f, 0.01f);
            yield return new WaitForSeconds(randomDelay);
            _shakeTimer += randomDelay;
            Debug.Log(_shakeTimer);
        }

        Debug.Log("Platform Falling");
        _falling = true;

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        float fallTimer = 0;

        // This looked a bit wierd so I changed it to the hacky way below
        //while (fallTimer < 3f)
        //{
        //    transform.position += Vector3.down * Time.deltaTime * _fallSpeed;
        //    fallTimer += Time.deltaTime;
        //    yield return null;
        //}

        // Velocity over time - hacky way.
        while (fallTimer < 3f)
        {
            fallTimer += Time.deltaTime;
            transform.position += Vector3.down * fallTimer * _fallSpeed * 0.1f;
            yield return null;
        }

        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (_falling || _shaking)
            return;

        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        _playersInTrigger.Remove(player);

        if (_playersInTrigger.Count == 0)
        {
            PlayerInside = false;
            StopCoroutine(_coroutine);

            if (_resetOnEmpty)
                _shakeTimer = 0;
        }
    }
}
