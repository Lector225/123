using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public string title;
    public Sprite icon;

    public abstract void Apply(PlayerCharacter playerCharacter);
}
