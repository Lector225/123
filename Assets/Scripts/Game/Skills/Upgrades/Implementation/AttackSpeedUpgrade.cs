using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedUpgrade : Upgrade
{
    [SerializeField] private AttackSpeed attackSpeedPrefab;

    public override void Apply(PlayerCharacter playerCharacter)
    {
       var attackSpeed = Instantiate(attackSpeedPrefab, playerCharacter.transform.position, Quaternion.identity);
        attackSpeed.transform.SetParent(playerCharacter.transform, true);
    }
}
