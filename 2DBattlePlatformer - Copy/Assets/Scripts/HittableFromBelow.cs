using UnityEngine;

public abstract class HittableFromBelow : MonoBehaviour
{
    [SerializeField] protected Sprite _usedSprite;
    Animator _animator;

    protected virtual bool CanUse => true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (CanUse == false)
            return;

        Player player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        float yVelocity = player.GetComponent<Rigidbody2D>().velocity.y;
        if (collision.contacts[0].normal.y > 0 && yVelocity >= 0)
        {
            PlayAnimation(player.PlayerNumber);
            Use(player.PlayerNumber);

            if (CanUse == false)
                GetComponent<SpriteRenderer>().sprite = _usedSprite;
        }
    }

    void PlayAnimation(int playerNumber)
    {
        if (_animator != null)
            _animator.SetTrigger($"BoxHitP{playerNumber}");
    }

    protected abstract void Use(int playerNumber = 0);
}