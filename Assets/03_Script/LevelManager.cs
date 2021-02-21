using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : UIManager
{

    [SerializeField] private TextMeshProUGUI pickedCollectablesText;
    private int pickedCollectables = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.PickedCollectableEmitter += OnPickedCollectable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPickedCollectable(GameObject collectable)
    {
        pickedCollectables += 1;
        Destroy(collectable);
        pickedCollectablesText.text = pickedCollectables.ToString();
    }

    private void OnDestroy()
    {
        PlayerMovement.PickedCollectableEmitter -= OnPickedCollectable;
    }
}
