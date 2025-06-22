using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RewardEffectManager : GlobalReference<RewardEffectManager>
{
    [SerializeField] private RewardEffect rewardEffectPrefab;
    public void SpawnRewardEffect(Vector2 spawnPos, ResourceType resourceType, int amount)
    {
        var eff = Instantiate(rewardEffectPrefab, transform);
        eff.PlayEffect(spawnPos, resourceType, amount);
    }
}
