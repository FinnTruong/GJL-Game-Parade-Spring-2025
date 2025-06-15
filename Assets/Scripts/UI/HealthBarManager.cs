using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance;
    [SerializeField] List<HealthBar> healthBars;
    [SerializeField] HealthBar healthBarPrefab;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    public void BindToHealthBar(BreakableObject target)
    {
        for (int i = 0; i < healthBars.Count; i++)
        {
            if (healthBars[i].IsAvailable)
            {
                healthBars[i].Bind(target);
                return;
            }
        }

        var newBar = Instantiate(healthBarPrefab, transform);
        healthBars.Add(newBar);
        newBar.Bind(target);
    }
}
