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
    [SerializeField] private float attSpeed = 1f;
    [SerializeField] private float attRange = 5f;
    [SerializeField] private float arrowDamage = 10f;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private AudioSource audioSource;

    private Rigidbody2D rb;
    private Vector3 movement;
    private float attTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        attTimer = 0f;

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

        attTimer -= Time.deltaTime;

        if (movement == Vector3.zero)
        {
            FindClosestEnemy();
            if (target != null && attTimer <= 0f)
            {
                Attack();
                attTimer = 1f / attSpeed;
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

    private void FindClosestEnemy()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = attRange;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
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
    }

    private void UpdateHealthUI()
    {
        healthBar.value = currentHealth;
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void IncreaseDamage(float amount)
    {
        arrowDamage += amount;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void IncreaseAttackSpeed(float amount)
    {
        attSpeed += amount;
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