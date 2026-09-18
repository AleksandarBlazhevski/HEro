using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : Unit
{
    [SerializeField] private float maxHealth = 40f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Slider healthBar;

    [Header("Attack")]
    [SerializeField] private float attackDelay = 0.2f;
    [SerializeField] private float attackCooldownTimer = 0;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject attackPrefab;

    private float currentHealth;
    [SerializeField] private AudioSource audioSource;
    private NavMeshAgent agent;
    private bool playerInRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            playerInRange = false;

        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        if (agent)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = moveSpeed;
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
        attackCooldownTimer -= Time.deltaTime;
        if (target == null)
            return;

        if (playerInRange && attackCooldownTimer <= 0f)
        {
            StartCoroutine(StartToAttack());
        }

        agent.SetDestination(target.position);
    }

    private IEnumerator StartToAttack()
    {
        agent.speed = 0;
        attackCooldownTimer = attackCooldown;
        //todo rework so it does not spawn on top of player
        Vector3 pos = target.position;

        GameObject attack =
            Instantiate(
                attackPrefab,
                pos,
                transform.rotation
            );

        audioSource.Play();
        Destroy(attack, attackDelay);

        yield return new WaitForSeconds(attackCooldown);

        agent.speed = moveSpeed;
    }

    private void fixHealtBar()
    {
        healthBar.transform.rotation = Quaternion.identity;
        healthBar.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.7f);
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

    public void SetPlayerInRange(bool isInRange)
    {
        playerInRange = isInRange;
    }
}