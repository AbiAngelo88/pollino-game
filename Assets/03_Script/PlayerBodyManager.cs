using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBodyManager : MonoBehaviour
{

    public delegate void PickedCollectable(GameObject collectable);
    public static event PickedCollectable PickedCollectableEmitter;

    public delegate void WinLevel();
    public static event WinLevel WinLevelEmitter;

    public delegate void DefeatLevel();
    public static event DefeatLevel DefeatLevelEmitter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tagName = collision.gameObject.tag;
        switch (tagName)
        {
            case "Collectable":
                PickedCollectableEmitter?.Invoke(collision.gameObject);
                break;
            case "Defeat":
                DefeatLevelEmitter?.Invoke();
                break;
            case "Win":
                WinLevelEmitter?.Invoke();
                break;
            default:
                break;
        }
    }
}
