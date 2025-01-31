﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

[RequireComponent(typeof(ProjectileMotion))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private float chargeDelay = 0.5f;
    [SerializeField] private EffectTypes type = EffectTypes.Kinetic;
    [SerializeField] private EffectTypeColors effectTypeColors;

    private float ChargeTimer = 0.0f;
    private bool Fired = false;

    void Start()
    {
        this.ChargeTimer = 0f;
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        FindObjectOfType<SoundManager>().PlaySoundEffect("Charge");
        this.gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(this.type));
    }

    public float GetDamage()
    {
        return this.damage;
    }

    public EffectTypes GetEffectType()
    {
        return this.type;
    }

    void Update()
    {
        if(this.ChargeTimer > this.chargeDelay && !this.Fired)
        {
            FindObjectOfType<SoundManager>().StopSoundEffect("Charge");
            FindObjectOfType<SoundManager>().PlaySoundEffect("Fire");
            Debug.Log("fired!");
            this.GetComponent<ProjectileMotion>().Fire();
            this.Fired = true;
        }
        else if(!this.Fired)
        {
            this.ChargeTimer += Time.deltaTime;
            var scale = this.ChargeTimer / this.chargeDelay;
            //var scale = this.transform.localScale * scale;
            this.transform.localScale = new Vector3(scale, scale, scale);
        }

        this.gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(this.type));
    }

    public void InitializeProjectile(float impactPower, float preparationTime, EffectTypes projectileType)
    {
        Renderer projectileRenderer = this.gameObject.GetComponentInChildren<Renderer>();
        projectileRenderer.material.SetColor("_Color", this.effectTypeColors.GetColorByEffectType(projectileType));
        this.damage = impactPower;
        this.chargeDelay = preparationTime;
        this.type = projectileType;
    }
}
