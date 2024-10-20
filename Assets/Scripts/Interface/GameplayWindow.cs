using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWindow : Window
{
    [SerializeField]
    private TMP_Text healthText;
    [SerializeField]
    private Slider healthSlider;

    [Space][SerializeField]
    private Slider experienceSlider;

    [Space] [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text coinsText;
    
    
    public override void Initialize()
    {

    }

    protected override void OpenStart()
    {
        base.OpenStart();
        var player = GameManager.Instance.CharacterFactory.PlayerCharacter;

        UpdateHealthVisual(player);
        player.HealthComponent.OnCharacterHealthChange += UpdateHealthVisual;

        UpdateScore(GameManager.Instance.ScoreManager.Score);
        GameManager.Instance.ScoreManager.OnScoreUpdated += UpdateScore;
    }

    protected override void CloseStart()
    {
        base.CloseStart();
        
        var player = GameManager.Instance.CharacterFactory.PlayerCharacter;
        if (player == null)
            return;
        
        player.HealthComponent.OnCharacterHealthChange -= UpdateHealthVisual;
        GameManager.Instance.ScoreManager.OnScoreUpdated -= UpdateScore;
    }

    private void UpdateHealthVisual(Character character)
    {
        int health = (int)character.HealthComponent.Health;
        int healthMax = character.HealthComponent.HealthMax;
        
        healthText.text = health + "/" + healthMax;
        healthSlider.maxValue = healthMax;
        healthSlider.value = health;
    }

    private void UpdateScore(int scoreCount)
    {
        coinsText.text = scoreCount.ToString();
    }

    private void Update()
    {
        float gameSeconds = GameManager.Instance.GameTimeSeconds;
        int minutes = (int)(gameSeconds / 60);
        int seconds = (int)(gameSeconds % 60);
        string zero = "0";

        timerText.text = minutes + ":" + ((seconds < 10) ? zero : "") + seconds;
    }
}
