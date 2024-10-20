using System;
using UnityEngine;

public class ScoreManager
{
    private const string SCORE_MAX = "save_score_max";


    public event Action<int> OnScoreUpdated;
    
    
    private int score;
    private int scoreMax;

    public int Score => score;
    public int ScoreMax => scoreMax;
    public bool IsNewScoreRecord { get; private set; }


    public ScoreManager()
    {
        score = 0;
        scoreMax = PlayerPrefs.GetInt(SCORE_MAX, 0);
        IsNewScoreRecord = false;
    }

    public void StartGame()
    {
        score = 0;
        IsNewScoreRecord = false;
    }
    
    public void CharacterDeathHandler(Character character)
    {
        score++;
        OnScoreUpdated?.Invoke(score);
        if (score <= scoreMax)
            return;

        scoreMax = Score;
        PlayerPrefs.SetInt(SCORE_MAX, scoreMax);
        IsNewScoreRecord = true;
    }

    public void EndGame()
    {
        score = 0;
    }
}
