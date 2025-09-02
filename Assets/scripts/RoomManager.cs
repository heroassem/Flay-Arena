using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public GameManagemnt gameManagemnt;

    public void Start()
    {
        if (PublicClass.isStartedMatch)
        {
            OnMatchStart();
            StartCoroutine(match());
        }
    }

    public void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && !PublicClass.isStartedMatch)
            {
                OnAllPlayersReady(PhotonNetwork.LocalPlayer);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers && PublicClass.isStartedMatch)
            {
                OnAllPlayersNotReady(PhotonNetwork.LocalPlayer);
            }
        }
    }

    IEnumerator match()
    {
        yield return new WaitForSeconds(0.03f);
        PublicClass.isStartedMatch = false;
    }


    //On match start
    public virtual void OnMatchStart() => Debug.Log("Start");

    //When all players are ready
    public virtual void OnAllPlayersReady(Player playerCount) => Debug.Log("All players are ready");

    //When all players are not ready anymore
    public virtual void OnAllPlayersNotReady(Player playerCount) => Debug.Log("All players not ready anymore");
}
