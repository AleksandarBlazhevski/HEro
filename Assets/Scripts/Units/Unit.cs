using UnityEngine;

public class Unit : MonoBehaviour
{
    protected Transform target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TakeDamage(float damageAmount)
    {
        Debug.Log("BASE CALLED TAKE DAMAGE");
        // currentHealth -= damageAmount;
        // healthBar.value = currentHealth;

        // if (currentHealth <= 0f)
        // {
        //     Destroy(gameObject);
        // }
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
