using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesUIManager : MonoBehaviour
{
    [SerializeField] private UpgradeUI upgradeUIPrefab;
    [SerializeField] private UpgradesManager upgradesManager;

    public void Show(List<Upgrade> upgrades, PlayerCharacter playerCharacter)
    {
        gameObject.SetActive(true);

        foreach (var upgrade in upgrades)
        {
            var ui = Instantiate(upgradeUIPrefab, transform);
            ui.Setup(upgrade.title, upgrade.icon, () => OnClickApply(upgrade, playerCharacter));
        }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnClickApply(Upgrade upgrade, PlayerCharacter playerCharacter)
    {
        upgrade.Apply(playerCharacter);
        upgradesManager.OnUpgradeApplied(upgrade);
    }
}
