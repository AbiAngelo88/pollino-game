using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public enum PlayerState { Idle, Run, Jump, Fall, Crash }

    private string nickname;
    private bool audio;
    private float volume;
    private Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> levels;
    private bool isFirstGame;

    public PlayerData()
    {
        this.nickname = "test";
        this.audio = true;
        this.volume = 0;
        this.isFirstGame = true;
    }

    public PlayerData(string nickname, bool audio, float volume)
    {
        this.nickname = nickname;
        this.audio = audio;
        this.volume = volume;
        this.isFirstGame = false;
    }

    public void SetIsFirstGame(bool value)
    {
        this.isFirstGame = value;
    }

    public bool GetIsFirstGame()
    {
        return this.isFirstGame;
    }

    public void SetNickname(string nickname)
    {
        this.nickname = nickname;
    }

    public string GetNickname()
    {
        return this.nickname;
    }

    public void SetAudio(bool audio)
    {
        this.audio = audio;
    }

    public bool GetAudio()
    {
        return this.audio;
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

    public float GetVolume()
    {
        return this.volume;
    }

    public void SetLevels(Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> levels)
    {
        this.levels = levels;       
    }

    public Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> GetLevels()
    {
        return this.levels;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}

[System.Serializable]
public class PlayerLevel {
    private Level.LevelID level;
    private int collectablesScore;
    private int ecoScore;
    private bool completed;
    private int score;

    public PlayerLevel() { }

    public PlayerLevel(Level.LevelID level, int collectablesScore, int ecoScore, int score, bool completed)
    {
        this.level = level;
        this.collectablesScore = collectablesScore;
        this.ecoScore = ecoScore;
        this.score = score;
        this.completed = completed;
    }

    public void SetLevel(Level.LevelID value)
    {
        this.level = value;
    }

    public Level.LevelID GetLevel()
    {
        return this.level;
    }

    public void SetCollectablesScore(int value)
    {
        this.collectablesScore = value;
    }

    public int GetCollectablesScore()
    {
        return this.collectablesScore;
    }

    public void SetEcoScore(int value)
    {
        this.ecoScore = value;
    }

    public int GetEcoScore()
    {
        return this.ecoScore;
    }

    public void SetScore(int value)
    {
        this.score = value;
    }

    public int GetScore()
    {
        return this.score;
    }

    public void SetCompleted(bool value)
    {
        this.completed = value;
    }

    public bool GetCompleted()
    {
        return this.completed;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}


