using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSchedule : MonoBehaviour
{
    [SerializeField] private GameObject projLaunchArea;
    private ProjectileFactory projFactory;

    void Awake()
    {
        projFactory = projLaunchArea.GetComponent<ProjectileFactory>();
    }

    public void InitiateEvaluation(List<ProjectileSpec> projSpecs)
    {
        StartCoroutine(ExecuteEvaluation(projSpecs));
    }

    private IEnumerator ExecuteEvaluation(List<ProjectileSpec> projSpecs)
    {
        foreach (var spec in projSpecs)
        {
            var preparationTime = spec.PreparationDuration;
            try
            {
                projFactory.Build(spec);
            }
            catch
            {
                Debug.LogError("Evaluation failed");
                yield break;
            }
            yield return new WaitForSeconds(preparationTime + 1);
        }
        Debug.Log("Evaluation successful");
    }
}
