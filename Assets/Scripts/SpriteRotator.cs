using Unity.VisualScripting;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    [SerializeField] float offset = -90;


    public void lookAtDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
    }
}
