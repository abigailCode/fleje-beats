using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Connection : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

   override
   public void OnConnectedToMaster()
    {
        print("Conectado al máster. Dale tus respetos");
    }


    private void OnConnectedToServer()
     {
        print("Conectado al servidor");
     }
    public void ButtonConnect() {

        

        Photon.Realtime.RoomOptions options = new Photon.Realtime.RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("Sala1",options,Photon.Realtime.TypedLobby.Default);
        

    }
        private void FixedUpdate() {

        
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1) {

            SCManager.instance.LoadScene("MultiplayerTest");
        
        
        }
        
        
    
    }
    override
        public void OnJoinedRoom()
    {

        Debug.Log("Conectado a la sala "+PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Hay un total de " + PhotonNetwork.CurrentRoom.PlayerCount+" jugadores actualmente");
        //Destroy(this);

    }

   
}
