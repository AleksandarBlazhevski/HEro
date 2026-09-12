using System.Linq;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float timeToLive = 1f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private bool isBlokedByWalls = false;
    [SerializeField] private string target = "Player";

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float newDamage)
    {
        if(newDamage <= 0)
        {
            Debug.Log("Projectile damage can not be 0 or negative");
            return;
        }
        damage = newDamage;
    }

    public void SetSpeed(float newSpeed)
    {
        if(newSpeed <= 0)
        {
            Debug.Log("Projectile speed can not be 0 or negative");
            return;
        }
        speed = newSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(target))
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();

            if(unit != null)
            {
                unit.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("wall") && isBlokedByWalls)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}