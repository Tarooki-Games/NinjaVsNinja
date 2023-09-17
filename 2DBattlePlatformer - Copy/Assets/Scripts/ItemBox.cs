using UnityEngine;

public class ItemBox : HittableFromBelow
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] GameObject _itemInBox;
    [SerializeField] Vector2 _itemLaunchVelocity;

    bool _used;

    void Start()
    {
        if (_itemInBox != null)
            _itemInBox.SetActive(false);
    }

    protected override bool CanUse => _used == false;

    protected override void Use(int playerNumber)
    {
        _itemInBox = Instantiate(_itemPrefab, transform.position + Vector3.up, Quaternion.identity, transform);

        if (_itemInBox == null)
            return;

        _used = true;
        _itemInBox.SetActive(true);
        var itemRigidBody = _itemInBox.GetComponent<Rigidbody2D>();
        if (itemRigidBody != null)
        {
            itemRigidBody.velocity = _itemLaunchVelocity;
        }
    }
}
