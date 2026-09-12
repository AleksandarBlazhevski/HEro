using UnityEngine;
using UnityEngine.UI;
public class Enemy : Unit
{
    [SerializeField] private float maxHealth = 40f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Slider healthBar;
    private float currentHealth;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        //Same as RangeEnemy
        LookAtTarget();
        fixHealtBar();
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (target.position - transform.position).normalized;
    
        rb.linearVelocity = direction * moveSpeed;
    }

    private void fixHealtBar()
    {
        healthBar.transform.rotation = Quaternion.identity;
        healthBar.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f);
    }

    public void StartAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        audioSource.Play();
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    public override void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
}