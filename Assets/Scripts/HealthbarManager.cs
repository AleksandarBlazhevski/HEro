using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth  = 100f;
    public float CurrentHealth { get => currentHealth; protected set => currentHealth = value; }

    void Awake()
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0f;
        slider.value = currentHealth;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O))
        // {
        //     takeDamage(15);
        // }
    }

    public void takeDamage(float value)
    {
        currentHealth -= value;
        if(currentHealth < 0) currentHealth = 0;
        slider.value = currentHealth;
    }

    private void fixHealtBar()
    {
        slider.transform.rotation = Quaternion.identity;
        slider.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f);
    }



}
