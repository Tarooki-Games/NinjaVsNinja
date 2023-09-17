using System.Collections;
using UnityEngine;

public class Fly : MonoBehaviour, ITakeDamage
{
    //[SerializeField] Sprite _deadSprite;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField] float _maxDistance = 2;
    [SerializeField] float _speed = 2;
    [SerializeField] float _rotateSpeed = 1.0f;

    Vector2 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _direction.normalized);
        var distance = Vector2.Distance(_startPosition, transform.position);
        if (distance >= _maxDistance)
        {
            transform.position = _startPosition + (_direction.normalized * _maxDistance);
            _direction *= -1;
        }
    }

    public void TakeDamage(int playerFired, Player player)
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        //spriteRenderer.sprite = _deadSprite;
        //spriteRenderer.flipY = true;
        GetComponent<Animator>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
        enabled = false; // this script.
        //GetComponent<Rigidbody2D>().simulated = false;

        float alpha = 1;

        while (alpha > 0)
        {
            yield return null;
            if(transform.rotation.z <= 180)
                transform.Rotate(_rotateSpeed * 180 * Time.deltaTime * Vector3.forward);
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }
}