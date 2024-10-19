using UnityEngine;
using UnityEngine.UI;

public class DefeatWindow : Window
{
    [SerializeField]
    private Button restartButton;
    [SerializeField]
    private Button returnToMainMenuButton;
    
    
    public override void Initialize()
    {
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        returnToMainMenuButton.onClick.AddListener(OnReturnToMainMenuButtonClicked);
    }

    private void OnReturnToMainMenuButtonClicked()
    {
    }

    private void OnRestartButtonClicked()
    {
    }
}
