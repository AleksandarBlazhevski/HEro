using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Transform target;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    
    [Header("Base stats")]
    [SerializeField] protected float attackCooldownTimer;
    [SerializeField] protected float attackSpeed = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Stagger(Vector2 direction, float force)
    {
        attackCooldownTimer = 0.5f; //Refactor constant
        if(attackCooldownTimer > attackSpeed)
        {
            attackCooldownTimer = attackSpeed;
        }
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public virtual void TakeDamage(float damageAmount)
    {
        sr.color = Color.red;

        Invoke(nameof(stopDmgIndicator), 0.2f);
    }

    private void stopDmgIndicator()
    {
        sr.color = Color.white;
    }

    protected virtual void LookAtTarget()
    {
        Vector2 direction =
            target.position - transform.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }
}
