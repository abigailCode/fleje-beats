using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject rightHand, leftHand;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.position = transform.position + (Vector3.up) + (transform.forward * -50);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
