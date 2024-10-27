using System;
using UnityEngine;

public class ScoreManager
{
    private const string SCORE_MAX = "save_score_max";
    
    
    public event Action<int> OnScoreChanged;
    
    
    private int score;
    private int scoreMax;

    public int Score => score;
    public int ScoreMax => scoreMax;


    public ScoreManager()
    {
        score = 0;
        scoreMax = PlayerPrefs.GetInt(SCORE_MAX, 0);
    }
    
    public void CharacterDeathHandler(Character character)
    {
        score += character.CharacterData.ScoreCost;
        OnScoreChanged?.Invoke(score);
        
        if (score <= scoreMax)
            return;

        scoreMax = Score;
        PlayerPrefs.SetInt(SCORE_MAX, scoreMax);
    }

    public void EndGame()
    {
        score = 0;
    }
}
