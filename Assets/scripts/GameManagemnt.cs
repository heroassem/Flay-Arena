using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagemnt : MonoBehaviour, IPunObservable
{
    [Header("Game Settings")]
    [SerializeField] public float gameTime; // 5 minutes

    [Header("NetWork Settings")]
    [SerializeField] public PhotonView photonView;

    [Header("Show Matsh Settings")]
    [SerializeField] TextMeshProUGUI redTeemScore;
    [SerializeField] TextMeshProUGUI blueTeemScore;
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Spowne Point")]
    [SerializeField] Transform redCorner;
    [SerializeField] Transform blueCorner;

    int scoreTeemOne = 0;
    int scoreTeemTwo = 0;

    private void Awake()
    {
        PhotonNetwork.SendRate = 15;
        PhotonNetwork.SerializationRate = 7;

        SpownPlayer();
    }

    public void Update()
    {
        if(photonView.IsMine)
        {
            Timer(ref gameTime);
        }
    }

    public void Timer(ref float timeSeconds)
    {
        if (timeSeconds > 0)
        {
            timeSeconds -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(timeSeconds / 60);
            float seconds = Mathf.FloorToInt(timeSeconds % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "00:00";
        }
    }

    [PunRPC]
    public void ScoreTeemSystem(string teemName)
    {
        if (teemName == "red")
        {
            scoreTeemOne += 1;

            string scoreText = string.Format("{0:00}", scoreTeemOne);

            blueTeemScore.text = scoreText;
            Debug.Log("Red team scored! Current score: " + scoreText);
        }
        else if (teemName == "blue")
        {
            scoreTeemTwo += 1;

            string scoreText = string.Format("{0:00}", scoreTeemTwo);

            redTeemScore.text = scoreText.ToString();
            Debug.Log("Blue team scored! Current score: " + scoreText);
        }

        Debug.Log("Current scores - Red: " + scoreTeemOne + ", Blue: " + scoreTeemTwo);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameTime);
            stream.SendNext(timerText.text);
        }
        else
        {
            gameTime = (float)stream.ReceiveNext();
            timerText.text = (string)stream.ReceiveNext();
        }
    }

    public void SpownPlayer()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1 || PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(redCorner.position.x - 3, redCorner.position.x + 3), redCorner.position.y + 2, redCorner.position.z), redCorner.rotation);
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 2 || PhotonNetwork.LocalPlayer.ActorNumber == 4)
            {
                PhotonNetwork.Instantiate("Player 1", new Vector3(Random.Range(blueCorner.position.x - 3, blueCorner.position.x + 2), blueCorner.position.y + 2, blueCorner.position.z), blueCorner.rotation);
            }
        }
    }

    [PunRPC]
    public void PlayerRespown(int palyerID, string playerTeem)
    {
        GameObject player = PhotonView.Find(palyerID).gameObject;

        if (player != null)
        {
            if (playerTeem == "red")
            {
                player.transform.position = new Vector3(Random.Range(redCorner.position.x - 3, redCorner.position.x + 3), redCorner.position.y + 2, redCorner.position.z);

                PlayerControler playerControler = player.GetComponent<PlayerControler>();

                if (playerControler != null)
                {
                    StartCoroutine(playerControler.Respown());
                }
            }
            else if (playerTeem == "blue")
            {
                player.transform.position = new Vector3(Random.Range(blueCorner.position.x - 3, blueCorner.position.x + 2), blueCorner.position.y + 2, blueCorner.position.z);

                PlayerControler playerControler = player.GetComponent<PlayerControler>();

                if (playerControler != null)
                {
                    StartCoroutine(playerControler.Respown());
                }
            }
        }
    }
}
