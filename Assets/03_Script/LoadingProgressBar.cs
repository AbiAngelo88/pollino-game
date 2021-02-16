using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadingProgressBar : MonoBehaviour
{
    private Slider slider;
    void Awake()
    {
        slider = transform.GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = Loader.getLoadingProgress();
    }

}
