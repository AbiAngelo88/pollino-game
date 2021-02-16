using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeManager : MonoBehaviour
{

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Loader.OnLoadScene += OnLoadScene;
    }

    private void OnLoadScene(Loader.Scene scene)
    {
        if (anim)
        {
            anim.SetTrigger("crossfade");
        } else
        {
            Debug.Log("No animator found");
        }
    }

    private void OnDestroy()
    {
        Loader.OnLoadScene -= OnLoadScene;
    }
}
