using UnityEngine;
using System.Collections.Generic;

public class TestFactory : MonoBehaviour
{
    [SerializeField] private GameObject shieldLocation;
    [SerializeField] private GameObject projLaunch;
    private ShieldFactory shieldFactory;
    private ProjectileFactory projFactory;
    private TestSchedule testSchedule;

    [SerializeField] private float evaluationPeriod;
    private float elapsedDuration;

    void Awake()
    {
        shieldFactory = shieldLocation.GetComponent<ShieldFactory>();
        projFactory = projLaunch.GetComponent<ProjectileFactory>();
        testSchedule = GetComponent<TestSchedule>();
        elapsedDuration = evaluationPeriod;
    }

    void Update()
    {
        if (elapsedDuration > evaluationPeriod)
        {
            ConductTrial();
            elapsedDuration = 0;
        }
        else
        {
            elapsedDuration += Time.deltaTime;
        }
    }

    private void ConductTrial()
    {
        var shieldSpec = shieldFactory.GenerateShieldSpec();
        shieldFactory.Build(shieldSpec);

        var projSpecs = CraftProjectileSpec();
        testSchedule.InitiateEvaluation(projSpecs);
    }

    private List<ProjectileSpec> CraftProjectileSpec()
    {
        var projSpecs = new List<ProjectileSpec>();
        var missileCount = Random.Range(5, 11);
        for (int i = 0; i < missileCount; i++)
        {
            projSpecs.Add(projFactory.CraftProjectileSpec());
        }
        return projSpecs;
    }
}
