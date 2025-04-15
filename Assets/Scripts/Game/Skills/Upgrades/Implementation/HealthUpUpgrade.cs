using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpUpgrade : Upgrade
{
    [SerializeField] private HealthUp healthUpPrefab;

    public override void Apply(PlayerCharacter playerCharacter)
    {
        var healthUp = Instantiate(healthUpPrefab, playerCharacter.transform.position, Quaternion.identity);
        healthUp.transform.SetParent(playerCharacter.transform, true);
    }
}
