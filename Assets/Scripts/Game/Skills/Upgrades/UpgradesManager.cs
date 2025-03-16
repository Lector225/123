using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private UpgradesUIManager uiManager;
    [SerializeField] private Upgrade[] upgrades;
    [SerializeField] private PlayerCharacter playerCharacter;

    private List<Upgrade> availableUpgrades;

    private void Awake()
    {
        availableUpgrades = upgrades.ToList();
    }

    public void Suggest()
    {
        if (availableUpgrades.Count > 0)
        {
            Time.timeScale = 0;
            uiManager.Show(availableUpgrades, playerCharacter);
        }
    }
    public void OnUpgradeApplied(Upgrade appliedUpgrade)
    {
        uiManager.Hide();
        availableUpgrades.Remove(appliedUpgrade);
        Time.timeScale = 1;
    }
}
