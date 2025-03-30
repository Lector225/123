using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpUpgrade : Upgrade
{
    [SerializeField] private SpeedUp speedUpPrefab;

    public override void Apply(PlayerCharacter playerCharacter)
    {
        var speedUp = Instantiate(speedUpPrefab, playerCharacter.transform.position, Quaternion.identity);
        speedUp.transform.SetParent(playerCharacter.transform, true);
    }
}
