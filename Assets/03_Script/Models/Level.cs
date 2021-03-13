using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public enum Difficulty { Easy, Medium, Hard }
    public enum LevelID { Level_01, Second } // Devono essere gli stessi nomi delle scene di gioco

    private LevelID code;
    private bool hasBoss;
    private List<AI> enemies;
    private List<AI> friends;

    public Level() { }

    public Level(LevelID code, bool hasBoss, List<AI> enemies, List<AI> friends)
    {
        this.code = code;
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

    public void SetHasBoss(bool value)
    {
        this.hasBoss = value;
    }

    public bool GetHasBoss()
    {
        return this.hasBoss;
    }
}





