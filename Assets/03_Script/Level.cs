using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public enum Difficulty { Easy, Medium, Hard }
    public enum LevelID { Level_01, Second } // Devono essere gli stessi nomi delle scene di gioco

    private LevelID code;
    private string desc;
    private int collectables;
    private int ecoPoints;
    private bool hasBoss;
    private List<AI> enemies;
    private List<AI> friends;

    public Level() { }

    public Level(LevelID code, string desc, int collectables, int ecoPoints, bool hasBoss, List<AI> enemies, List<AI> friends)
    {
        this.code = code;
        this.desc = desc;
        this.collectables = collectables;
        this.ecoPoints = ecoPoints;
        this.hasBoss = hasBoss;
        this.enemies = enemies;
        this.friends = friends;
    }

    public List<AI> GetEnemies()
    {
        return this.enemies;
    }

    public void SetEnemies(List<AI> enemies)
    {
        this.enemies = enemies;
    }

    public List<AI> GetFriends()
    {
        return this.friends;
    }

    public void SetFriends(List<AI> friends)
    {
        this.friends = friends;
    }

    public void SetCode(LevelID value)
    {
        this.code = value;
    }

    public LevelID GetCode()
    {
        return this.code;
    }

    public void SetDesc(string value)
    {
        this.desc = value;
    }

    public string GetDesc()
    {
        return this.desc;
    }

    public void SetCollectables(int value)
    {
        this.collectables = value;
    }

    public int GetCollectables()
    {
        return this.collectables;
    }

    public void SetEcoPoints(int value)
    {
        this.ecoPoints = value;
    }

    public int GetEcoPoints()
    {
        return this.ecoPoints;
    }

    public void SetHasBoss(bool value)
    {
        this.hasBoss = value;
    }

    public bool GetHasBoss()
    {
        return this.hasBoss;
    }
}





