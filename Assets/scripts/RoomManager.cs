using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public void Start()
    {
        if (PublicClass.isStartedMatch)
        {
            OnMatchStart();
            StartCoroutine(match());
        }
    }

    IEnumerator match()
    {
        yield return new WaitForSeconds(0.03f);
        PublicClass.isStartedMatch = false;
    }

    public virtual void OnMatchStart() => Debug.Log("Start"); 
}
