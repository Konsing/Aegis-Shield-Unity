using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class ShieldFactory : MonoBehaviour
{
    [SerializeField] private GameObject _spherePrefab;
    private GameObject shieldSphere;
    private GameObject shieldPoint;
    private ShieldController shieldController;

    void Awake()
    {
        shieldSphere = GameObject.Find("Sphere");
        shieldController = shieldSphere.GetComponent<ShieldController>();

        shieldPoint = GameObject.Find("ShieldPoint");
    }

    public void Build(ShieldSpec spec)
    {
        float rating = EvaluateShieldSpecifications(spec);

        if (rating > 300)
        {
            spec = AdjustSpecifications(spec);
        }

        switch (spec.ShieldType)
        {
            case "Kinetic":
                shieldController.InitializeShield(spec.MaxCapacity, spec.DelayBeforeRecharging, spec.RateOfRecharge, EffectTypes.Kinetic);
                break;
            case "Energy":
                shieldController.InitializeShield(spec.MaxCapacity, spec.DelayBeforeRecharging, spec.RateOfRecharge, EffectTypes.Energy);
                break;
            case "Arcane":
                shieldController.InitializeShield(spec.MaxCapacity, spec.DelayBeforeRecharging, spec.RateOfRecharge, EffectTypes.Arcane);
                break;
        }
    }

    private ShieldSpec AdjustSpecifications(ShieldSpec spec)
    {
        spec.MaxCapacity /= 2;
        spec.RateOfRecharge /= 2;

        return spec;
    }

    private float EvaluateShieldSpecifications(ShieldSpec spec)
    {
        return spec.MaxCapacity + (5 - spec.DelayBeforeRecharging) * 5 + spec.RateOfRecharge * (spec.RateOfRecharge / 2);
    }

    public ShieldSpec GenerateShieldSpec()
    {
        ShieldSpec spec = new ShieldSpec
        {
            MaxCapacity = Random.Range(50f, 250f),
            DelayBeforeRecharging = Random.Range(0.5f, 5f),
            RateOfRecharge = Random.Range(1f, 25f),
            ShieldType = SelectRandomShieldType()
        };

        return spec;
    }

    private string SelectRandomShieldType()
    {
        string[] types = { "Kinetic", "Energy", "Arcane" };
        return types[Random.Range(0, types.Length)];
    }
}