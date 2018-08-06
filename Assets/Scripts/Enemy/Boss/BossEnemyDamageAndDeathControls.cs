using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemyDamageAndDeathControls : MonoBehaviour
{

    [Header("Boss Stats")]
    public float maxBossHealth = 5000;
    public GameObject deathEffect;

    [Header("Boss Health")]
    public Color minHealthColor = Color.red;
    public Color halfHealthColor = Color.yellow;
    public Color maxHealthColor = Color.green;
    public Slider bossHealthSlider;
    public Image bossHealthFiller;

    [HideInInspector]
    public float currentBossHealth;

    // Use this for initialization
    void Start()
    {
        currentBossHealth = maxBossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float maxHealth = maxBossHealth;
        float currentHealthLeft = currentBossHealth;
        float healthRatio = currentHealthLeft / maxHealth;

        if (healthRatio <= 0.5)
            bossHealthFiller.color = Color.Lerp(minHealthColor, halfHealthColor, healthRatio * 2);
        else
            bossHealthFiller.color = Color.Lerp(halfHealthColor, maxHealthColor, (healthRatio - 0.5f) * 2);
        bossHealthSlider.value = healthRatio;

        if (currentHealthLeft <= 0)
        {
            Instantiate(deathEffect, gameObject.transform.position, deathEffect.transform.rotation);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsManager.SwordContact))
            currentBossHealth -= DamageRateManager.GetSwordContactDamage();

        if (other.CompareTag(TagsManager.FootContact))
            currentBossHealth -= DamageRateManager.GetFootContactDamage();

        if (other.CompareTag(TagsManager.SphereSpawn))
            currentBossHealth -= DamageRateManager.GetArcContactDamage();
    }
}
