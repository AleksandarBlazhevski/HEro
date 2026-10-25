using UnityEngine;
using UnityEngine.UI;

public class HealthbarManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    public float curPercentage = 1;

    void Awake()
    {
        slider.maxValue = maxHealth;
        slider.minValue = 0f;
        slider.value = currentHealth;
    }

    void Update()
    {
        // fixHealtBar();

        //
        curPercentage = currentHealth / maxHealth;
        slider.value = currentHealth;
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
