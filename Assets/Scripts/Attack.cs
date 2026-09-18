using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerInputs>().TakeDamage(damage);
        }
    }
}