using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionlessSides : MonoBehaviour
{
    [SerializeField] LayerMask _playerLayerMask;
    [SerializeField] PhysicsMaterial2D _physicsMaterial2D;
    [SerializeField] BoxCollider2D _boxCollider2D;

    Vector2 _transform2D;
    Vector2 _leftBounds;
    Vector2 _rightBounds;

    void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _transform2D = new Vector2(transform.position.x, transform.position.y);
        _leftBounds = new Vector2(+_boxCollider2D.bounds.extents.x + 0.05f, 0.5f);
        _rightBounds = new Vector2(-_boxCollider2D.bounds.extents.x - 0.05f, 0.5f);
    }

    void Update()
    {
        Debug.DrawRay(_transform2D + _leftBounds, Vector2.down, Color.cyan);
        Debug.DrawRay(_transform2D + _rightBounds, Vector2.down, Color.red);

        if (Physics2D.Raycast(_transform2D + _leftBounds, Vector2.down, 1f, _playerLayerMask)
            || Physics2D.Raycast(_transform2D + _rightBounds, Vector2.down, 1f, _playerLayerMask))
        {
            _boxCollider2D.sharedMaterial = _physicsMaterial2D;
            // Debug.Log($"true: {_physicsMaterial2D.friction}");
        }
        else
        {
            _boxCollider2D.sharedMaterial = null;
            // Debug.Log($"false: {_physicsMaterial2D.friction}");
        }
    }
}

//void Awake()
//{
//    _boxCollider2D = GetComponent<BoxCollider2D>();
//    _transform2D = new Vector2(transform.position.x, transform.position.y);
//    _rightBounds = new Vector2(+_boxCollider2D.bounds.extents.x - 0.2f, 0.55f);
//    _leftBounds = new Vector2(-_boxCollider2D.bounds.extents.x + 0.2f, 0.55f);
//}

//void Update()
//{
//    Debug.DrawRay(_transform2D + _leftBounds, Vector2.right, Color.red);
//    Debug.DrawRay(_transform2D + _rightBounds, Vector2.left, Color.cyan);
//    if (Physics2D.Raycast(_transform2D + _leftBounds, Vector2.right, +_boxCollider2D.bounds.extents.x, _playerLayerMask)
//        || Physics2D.Raycast(_transform2D + _rightBounds, Vector2.left, +_boxCollider2D.bounds.extents.x, _playerLayerMask))
//    {
//        _boxCollider2D.sharedMaterial = null;
//        Debug.Log("false: " + _physicsMaterial2D.friction);
//    }
//    else
//    {
//        _boxCollider2D.sharedMaterial = _physicsMaterial2D;
//        Debug.Log("true: " + _physicsMaterial2D.friction);
//    }
//}
