using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NetworkConnection : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(new TypedLobby("default", LobbyType.Default));
    }
    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("LobbyPqge");
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        SceneManager.LoadScene("close");
    }
}
