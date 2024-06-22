using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ShieldController : MonoBehaviour   
{
    [SerializeField] private float capacity = 100.0f;
    [SerializeField] private float rechargeRate = 1.0f;
    [SerializeField] private float rechargeDelay = 1.0f;
    [SerializeField] private EffectTypes type = EffectTypes.Kinetic;
    [SerializeField] private EffectTypeColors effectTypeColors;
    [SerializeField] private GameObject scrollingText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private float currentCapacity = 0.0f;
    private HealthBarController healthBarController;

    private bool rc = false;
    private float rcTimer = 0.0f;

    void Awake()
    {
        this.currentCapacity = this.capacity;  

        if (!this.healthBar.TryGetComponent<HealthBarController>(out this.healthBarController))
        {
            Debug.Log("ShieldController expects a health bar.");
        }

        this.healthBarController.ChangeValue(this.currentCapacity / this.capacity);

        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(this.type));     
    }

    private void TakeDamage(float damage)
    {
        float oldCapacity = this.currentCapacity;
        this.currentCapacity -= damage;
        
        if (currentCapacity < 0.0f)
        {
            currentCapacity = 0.0f;
        }
        if (currentCapacity <= 0.0f && oldCapacity > 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Explode");
        }
        else if (currentCapacity > 0)
        {
            FindObjectOfType<SoundManager>().PlaySoundEffect("Shrink");
        }

        this.healthBarController.ChangeValue(currentCapacity / capacity);
        
        if(this.scrollingText && oldCapacity > 0)
        {
            this.DisplayMarqueeText(damage.ToString());
        }

        rcTimer = 0.0f;
        rc = false;
    }

    private void DisplayMarqueeText(string text)
    {
        var marqueeTextInstance = Instantiate(this.scrollingText, this.transform.position, Quaternion.identity);
        marqueeTextInstance.GetComponent<TextMesh>().text = text;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Projectile"))
        {
            var projectileController = collider.GetComponent<ProjectileController>();
            float damageAmount = projectileController.GetDamage();
            var typeOfProjectile = projectileController.GetEffectType();
            var typeOfShield = this.type;

            float finalDamage = DamageEngine.ComputeDamage(typeOfProjectile, typeOfShield, damageAmount);
            TakeDamage(finalDamage);
        }
    }

    void Update()
    {
        var capacityRatio = currentCapacity / capacity;
        this.transform.localScale = new Vector3(capacityRatio, capacityRatio, capacityRatio);
        this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", effectTypeColors.GetColorByEffectType(this.type));

        if (!rc)
        {
            rcTimer += Time.deltaTime;

            if (rcTimer >= rechargeDelay)
            {
                rc = true;
            }
        }
        else
        {
            currentCapacity += rechargeRate * Time.deltaTime;
            currentCapacity = Mathf.Clamp(currentCapacity, 0.0f, capacity);
            this.healthBarController.ChangeValue(currentCapacity / capacity);
        }
    }

    public void InitializeShield(float maxCapacity, float delayBeforeRecharging, float rateOfRecharge, EffectTypes effect)
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(this.type));
        this.currentCapacity = maxCapacity;

        this.capacity = maxCapacity;
        this.rechargeDelay = delayBeforeRecharging;
        this.rechargeRate = rateOfRecharge;
        this.type = effect;
    }
}
