using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpUpgrade : Upgrade
{
    [SerializeField] private DamageUp damageUpPrefab;

    public override void Apply(PlayerCharacter playerCharacter)
    {
        var damageUp = Instantiate(damageUpPrefab, playerCharacter.transform.position, Quaternion.identity);
        damageUp.transform.SetParent(playerCharacter.transform, true);
    }
}
