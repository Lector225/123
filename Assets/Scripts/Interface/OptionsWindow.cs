using UnityEngine;
using UnityEngine.UI;

public class OptionsWindow : Window
{
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundsToggle;
    [SerializeField] private Button closeButton;


    public override void Initialize()
    {
        musicToggle.onValueChanged.AddListener(MusicToggleHandler);
        soundsToggle.onValueChanged.AddListener(SoundsToggleHandler);
        closeButton.onClick.AddListener(CloseOptionsHandler);
    }

    private void CloseOptionsHandler()
    {
        Hide(true);
        GameManager.Instance.WindowsService.ShowWindow<MainMenuWindow>(false);
    }

    private void SoundsToggleHandler(bool arg0)
    {
    }

    private void MusicToggleHandler(bool arg0)
    {
    }
}
