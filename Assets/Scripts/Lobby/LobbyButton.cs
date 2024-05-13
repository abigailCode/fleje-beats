using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyButton : MonoBehaviour
{

    [SerializeField] GameObject BoxPool;
    [SerializeField] TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressed() {

        if (!BoxPool.activeSelf) {

            BoxPool.SetActive(true);
            text.text = "LOOK UP";
            text.fontSize = 18;

        
        }
    
    }


}
