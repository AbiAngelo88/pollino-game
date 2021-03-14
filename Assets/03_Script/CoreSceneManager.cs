using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Awake()
    {
        AddSoundToBtns();   
    }

    public virtual void Start()
    {
        
    }

    private void AddSoundToBtns()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        if(buttons != null && buttons.Length > 0)
        {
            foreach(Button button in buttons)
            {
                button.onClick.AddListener(() => AudioHelper.PlayOneShotSound(AudioHelper.Sounds.ButtonClick));
            }
        }
        else
        {
            Debug.Log("Nessun pulsante UI trovato");
        }
    }

}
