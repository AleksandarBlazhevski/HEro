using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInputs : Unit
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Attack")]
    [SerializeField] private float attRange = 5f;
    [SerializeField] private float arrowDamage = 10f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private AudioSource audioSource;

    private Vector3 movement;
    

    private void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        attackCooldownTimer = 0f;

        UpdateHealthUI();
    }

    private void Update()
    {
        movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movement.y += 1;

        if (Input.GetKey(KeyCode.S))
            movement.y -= 1;

        if (Input.GetKey(KeyCode.A))
            movement.x -= 1;

        if (Input.GetKey(KeyCode.D))
            movement.x += 1;

        movement = movement.normalized;

        attackCooldownTimer -= Time.deltaTime;

        if (movement == Vector3.zero)
        {
            FindClosestVisibleEnemy();
            if (target != null && attackCooldownTimer <= 0f)
            {
                Attack();
                attackCooldownTimer = 1f / attackSpeed;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(
            movement * moveSpeed,
            ForceMode2D.Impulse
        );

        if (movement != Vector3.zero)
        {
            float angle =
                Mathf.Atan2(movement.y, movement.x)
                * Mathf.Rad2Deg - 90f;

            transform.rotation =
                Quaternion.Euler(0f, 0f, angle);
        }
        else if (target != null)
        {
            LookAtTarget();
        }
    }

    private void FindClosestVisibleEnemy()
    {
        target = null;
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = attRange;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, enemy.transform.position, LayerMask.GetMask("wall"));
            if(hit.collider != null)
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if(distance < closestDistance)
            {
                closestEnemy = enemy.transform;
                closestDistance = distance;
            }
        }

        target = closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attRange);
        if(target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }

    }

    private void Attack()
    {
        if (target == null)
            return;

        LookAtTarget();

        GameObject arrowObject = Instantiate(
            arrowPrefab,
            firePoint.position,
            transform.rotation
        );

        RangedProjectile arrow = arrowObject.GetComponent<RangedProjectile>();

        if (arrow != null)
        {
            arrow.SetDamage(arrowDamage);
        }
        audioSource.pitch = UnityEngine.Random.Range(1f, 1.3f);
        audioSource.Play();
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0f)
            currentHealth = 0f;

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Destroy(gameObject);
            GameManager.Instance.GameOver();
        }
        base.TakeDamage(damage);
    }

    private void UpdateHealthUI()
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void IncreaseDamage(float amount)
    {
        arrowDamage += amount;
    }

    //Heal % max hp
    public void Heal(float amount)
    {
        float amoutToHeal = maxHealth * (amount/100);
        currentHealth += amoutToHeal;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attackSpeed += amount;
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        UpdateHealthUI();
    }
}