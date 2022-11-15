using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] bool useHud = false;

    int currentHealth = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        if (useHud) HealthHUD.instance.SetHealth(currentHealth);

    }

    public void DealDamage(int amount)
    {
        //if(currentHealth >= amount)
        {
            currentHealth -= amount;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (useHud)
            {
                HealthHUD.instance.SetHealth(currentHealth);
                CinemachineShake.Instance.ShakeCamera(2f, .1f);

            }
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
                currentHealth = 0;
                // death effect
            }

        }
    }




}
