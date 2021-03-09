using UnityEngine;

public class AI
{

    public enum AiState { Idle, Run, Jump, Save }

    // Inserire qui tutti i codici delle AI
    public enum AiCodes { Fox, Boar, Pyromaniac }
    private AiCodes code;
    private bool isFriend;
    private bool isEnemy;
    private int baseDamage;
    private bool canFly;
    private bool canMove;
    private string prefabName;
    
    public AI() { }

    public AI(AiCodes code, bool isFriend, bool isEnemy, int baseDamage, bool canFly, bool canMove, string prefabName)
    {
        this.code = code;
        this.isFriend = isFriend;
        this.isEnemy = isEnemy;
        this.baseDamage = baseDamage;
        this.canFly = canFly;
        this.canMove = canMove;
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

    public bool GetCanMove()
    {
        return this.canMove;
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }
}
