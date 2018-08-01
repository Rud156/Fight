using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionEnemyDamageAndDeathControls : MonoBehaviour
{

    [Header("Minion Stats")]
    public float maxMinionHealth = 30;
    public GameObject deathEffect;

    [Header("Minion Health")]
    public Color minHealthColor = Color.red;
    public Color halfHealthColor = Color.yellow;
    public Color maxHealthColor = Color.green;
    public Slider minionHealthSlider;
    public Image minionHealthFiller;

    private Animator minionAnimator;
    private float currentMinionHealth;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        minionAnimator = gameObject.GetComponent<Animator>();
        currentMinionHealth = maxMinionHealth;
    }

    // Update is called once per frame
    void Update()
    {
        float maxHealth = maxMinionHealth;
        float currentHealthLeft = currentMinionHealth;
        float healthRatio = currentHealthLeft / maxHealth;

        if (healthRatio <= 0.5)
            minionHealthFiller.color = Color.Lerp(minHealthColor, halfHealthColor, healthRatio * 2);
        else
            minionHealthFiller.color = Color.Lerp(halfHealthColor, maxHealthColor, (healthRatio - 0.5f) * 2);
        minionHealthSlider.value = healthRatio;

        if (currentHealthLeft <= 0)
        {
            minionAnimator.SetBool(EnemyControlsManager.EnemyDead, true);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsManager.SwordContact))
        {
            currentMinionHealth -= Random.Range(20, 40);
            minionAnimator.SetTrigger(EnemyControlsManager.MinionHit);
        }

        if (other.CompareTag(TagsManager.FootContact))
        {
            currentMinionHealth -= Random.Range(10, 20);
            minionAnimator.SetTrigger(EnemyControlsManager.MinionHit);
        }
    }

    void Dead()
    {
        Destroy(gameObject);
    }
}
