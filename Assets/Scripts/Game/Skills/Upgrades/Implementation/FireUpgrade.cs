using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireUpgrade : Upgrade
{
    [SerializeField] private Fire firePrefab;

    public override void Apply(PlayerCharacter playerCharacter)
    {
       var fire = Instantiate(firePrefab, playerCharacter.transform.position, Quaternion.identity);
        fire.transform.SetParent(playerCharacter.transform, true);
    }
}
