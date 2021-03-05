using UnityEngine;

public class AI
{
    // Inserire qui tutti i codici delle AI
    public enum AiCodes { AI }
    private AiCodes code;
    private string desc;
    private bool isFriend;
    private bool isEnemy;
    private int baseDamage;
    private bool canFly;
    private string prefabName;
    
    public AI() { }

    public AI(AiCodes code, string desc, bool isFriend, bool isEnemy, int baseDamage, bool canFly, string prefabName)
    {
        this.code = code;
        this.desc = desc;
        this.isFriend = isFriend;
        this.isEnemy = isEnemy;
        this.baseDamage = baseDamage;
        this.canFly = canFly;
        this.prefabName = prefabName;
    }

    public AiCodes GetCode()
    {
        return this.code;
    }

    public void SetCode(AiCodes code)
    {
        this.code = code;
    }

    public string GetDesc()
    {
        return this.desc;
    }

    public void SetDesc(string desc)
    {
        this.desc = desc;
    }

    public string GetPrefabName()
    {
        return this.prefabName;
    }

    public void SetPrefabName(string prefabName)
    {
        this.prefabName = prefabName;
    }

    public bool GetIsFriend()
    {
        return this.isFriend;
    }

    public void SetIsFriend(bool isFriend)
    {
        this.isFriend = isFriend;
    }

    public void SetIsEnemy(bool isEnemy)
    {
        this.isEnemy = isEnemy;
    }

    public bool GetIsEnemy()
    {
        return this.isEnemy;
    }

    public int GetBaseDamage()
    {
        return this.baseDamage;
    }

    public void SetBaseDamage(int baseDamage)
    {
        this.baseDamage = baseDamage;
    }

    public bool GetCanFly()
    {
        return this.canFly;
    }

    public void SetCanFly(bool canFly)
    {
        this.canFly = canFly;
    }
}
