using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Photon.Pun;


public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject XRRigP1, XRRigP2;
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsMasterClient)
        {

            Debug.Log("Eres J1, te pillas la rana");
            PhotonNetwork.Instantiate("XRRigP1", new Vector3(-10, 0, 0), Quaternion.identity);


        }

        else
        {
            Debug.Log("Eres J2");
            PhotonNetwork.Instantiate("XRRigP2", new Vector3(10, 0, 0), Quaternion.identity);
        }
    }

    
}
