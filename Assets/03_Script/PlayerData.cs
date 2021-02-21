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

    public void setIsFirstGame(bool value)
    {
        this.isFirstGame = value;
    }

    public bool getIsFirstGame()
    {
        return this.isFirstGame;
    }

    public void setNickname(string nickname)
    {
        this.nickname = nickname;
    }

    public string getNickname()
    {
        return this.nickname;
    }

    public void setAudio(bool audio)
    {
        this.audio = audio;
    }

    public bool getAudio()
    {
        return this.audio;
    }

    public void setVolume(float volume)
    {
        this.volume = volume;
    }

    public float getVolume()
    {
        return this.volume;
    }

    public void setLevels(Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> levels)
    {
        this.levels = levels;       
    }

    public Dictionary<Level.LevelID, Dictionary<Level.Difficulty, PlayerLevel>> getLevels()
    {
        return this.levels;
    }
}

[System.Serializable]
public class PlayerLevel {
    private Level.LevelID level;
    private int collectablesScore;
    private int ecoScore;
    private bool hasBoss;
    private int record;
    private bool completed;
    private int stars;

    public void setLevel(Level.LevelID value)
    {
        this.level = value;
    }

    public Level.LevelID getLevel()
    {
        return this.level;
    }

    public void setCollectablesScore(int value)
    {
        this.collectablesScore = value;
    }

    public int getCollectablesScore()
    {
        return this.collectablesScore;
    }

    public void setEcoScore(int value)
    {
        this.ecoScore = value;
    }

    public int getEcoScore()
    {
        return this.ecoScore;
    }


    public void setRecord(int value)
    {
        this.record = value;
    }

    public int getRecord()
    {
        return this.record;
    }

    public void setStars(int value)
    {
        this.stars = value;
    }

    public int getStars()
    {
        return this.stars;
    }

    public void setHasBoss(bool value)
    {
        this.hasBoss = value;
    }

    public bool getHasBoss()
    {
        return this.hasBoss;
    }

    public void setCompleted(bool value)
    {
        this.completed = value;
    }

    public bool getCompleted()
    {
        return this.completed;
    }
}


