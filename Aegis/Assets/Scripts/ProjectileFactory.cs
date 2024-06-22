using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private GameObject projPrefab;
    private GameObject projLaunchPoint;

    void Awake()
    {
        projLaunchPoint = GameObject.Find("ProjLaunchPoint");
    }

    public void Build(ProjectileSpec spec)
    {
        float efficacy = EvaluateProjectile(spec);

        if (efficacy > 100)
        {
            spec = ReduceSpecs(spec);
        }

        GameObject projInstance = Instantiate(projPrefab, projLaunchPoint.transform.position, Quaternion.identity);
        ProjectileController projectileManager = projInstance.GetComponent<ProjectileController>();

        EffectTypes effectType = spec.Category switch
        {
            "Kinetic" => EffectTypes.Kinetic,
            "Energy" => EffectTypes.Energy,
            "Arcane" => EffectTypes.Arcane,
            _ => EffectTypes.Kinetic,
        };

        projectileManager.InitializeProjectile(spec.ImpactForce, spec.PreparationDuration, effectType);
    }

    private float EvaluateProjectile(ProjectileSpec specs)
    {
        return specs.ImpactForce * 2 + Mathf.Pow(3 - specs.PreparationDuration, 4);
    }

    public ProjectileSpec CraftProjectileSpec()
    {
        return new ProjectileSpec
        {
            ImpactForce = Random.Range(1f, 50f),
            PreparationDuration = Random.Range(0.5f, 3f),
            Category = SelectProjectileType()
        };
    }

    private ProjectileSpec ReduceSpecs(ProjectileSpec specs)
    {
        specs.ImpactForce *= 0.75f;
        specs.PreparationDuration = Mathf.Min(specs.PreparationDuration * 1.25f, 3f);
        return specs;
    }

    private string SelectProjectileType()
    {
        string[] projectileTypes = { "Kinetic", "Energy", "Arcane" };
        int index = Random.Range(0, projectileTypes.Length);
        return projectileTypes[index];
    }
}
