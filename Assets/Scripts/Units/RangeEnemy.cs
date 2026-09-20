using UnityEngine;
using UnityEngine.UI;
public class RangedEnemy : Unit
{
    [SerializeField] private float maxHealth = 25f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Slider healthBar;
    [SerializeField] private AudioSource audioSource;

    private float currentHealth;
    private void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        attackCooldownTimer = attackSpeed;
    }

    private void Update()
    {
        if (target == null)
            return;

        attackCooldownTimer -= Time.deltaTime;

        float distance =
            Vector2.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            if (attackCooldownTimer <= 0f)
            {
                Attack();
                attackCooldownTimer = attackSpeed;
            }
        }
        //Same as Enemy
        LookAtTarget();
        fixHealtBar();
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        float distance =
            Vector2.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            Vector2 direction =
                (target.position - transform.position).normalized;

            rb.linearVelocity = direction * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void fixHealtBar()
    {
        healthBar.transform.rotation = Quaternion.identity;
        healthBar.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f);
    }

    private void Attack()
    {
        if (target == null)
            return;

        Vector2 direction =
            (target.position - transform.position).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            transform.rotation
        );

        audioSource.Play();
    }

    public override void TakeDamage(float damageAmount)
    {
        base.TakeDamage(damageAmount);
        currentHealth -= damageAmount;
        healthBar.value = currentHealth;
        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}