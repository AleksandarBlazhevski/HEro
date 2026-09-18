using System.Collections;
using UnityEngine;

public class InAttackRange : MonoBehaviour
{
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] GameObject target = null;
    
    private Enemy unit;

    private void Start()
    {
        unit = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            unit.SetPlayerInRange(true);
            target = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            unit.SetPlayerInRange(false);
            target = null;
        }
    }
}
