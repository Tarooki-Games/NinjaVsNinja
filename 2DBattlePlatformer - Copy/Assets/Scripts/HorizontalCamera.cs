using UnityEngine;

public class HorizontalCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_target.position.x, transform.position.y, transform.position.z);
    }
}
