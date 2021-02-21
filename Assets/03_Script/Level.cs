using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public enum Difficulty { Easy, Medium, Hard }
    public enum LevelID { First, Second }

    private LevelID code;
    private string desc;
    private Difficulty difficulty;
    private int collectables;
    private int ecoPoints;
    private bool hasBoss;

    public Level() { }

    public void setCode(LevelID value)
    {
        this.code = value;
    }

    public LevelID getCode()
    {
        return this.code;
    }

    public void setDesc(string value)
    {
        this.desc = value;
    }

    public string getDesc()
    {
        return this.desc;
    }

    public void setDifficulty(Difficulty value)
    {
        this.difficulty = value;
    }

    public Difficulty getDifficulty()
    {
        return this.difficulty;
    }

    public void setCollectables(int value)
    {
        this.collectables = value;
    }

    public int getCollectables()
    {
        return this.collectables;
    }

    public void setEcoPoints(int value)
    {
        this.ecoPoints = value;
    }

    public int getEcoPoints()
    {
        return this.ecoPoints;
    }

    public void setHasBoss(bool value)
    {
        this.hasBoss = value;
    }

    public bool getHasBoss()
    {
        return this.hasBoss;
    }


}





