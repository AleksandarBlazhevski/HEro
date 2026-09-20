using UnityEngine;

public class Attack : MonoBehaviour
{
    public float damage = 10f;
    public Vector2 direction = Vector2.zero;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Unit player = collision.GetComponent<PlayerInputs>();
            player.Stagger(direction, damage / 2f); //Todo constanst
            player.TakeDamage(damage);
        }
    }
}