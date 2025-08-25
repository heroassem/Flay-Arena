using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class LoobyUiManagemnt : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject RM_panal;
    [SerializeField] GameObject S_panal;
    [SerializeField] GameObject RL_panal;
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] TMP_Dropdown roomPlayerMaxPlayerOptin;

    int maxPlayer;

    private void Start()
    {
        ShowRMPanel();
    }

    public int PlayerNumberManagemnt()
    {
        if(roomPlayerMaxPlayerOptin.value == 0)
            maxPlayer = 2;
        else if (roomPlayerMaxPlayerOptin.value == 1)
            maxPlayer = 4;

        return maxPlayer;
    }

    public void ShowRMPanel()
    {
        RM_panal.SetActive(true);
        S_panal.SetActive(false);
        RL_panal.SetActive(false);
    }

    public void ShowSPanel()
    {
        RM_panal.SetActive(false);
        S_panal.SetActive(true);
        RL_panal.SetActive(false);
    }

    public void ShowRLPanel()
    {
        RM_panal.SetActive(false);
        S_panal.SetActive(false);
        RL_panal.SetActive(true);
    }

    public void CreateToRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            char[] chars = new char[8];
            for (int i = 0; i < chars.Length; i++)
            {
                int rand = Random.Range(65, 121);
                chars[i] = (char)(rand);
            }

            string s = roomNameInput.text;

            foreach (char c in chars)
                s += c;

            roomNameInput.text = s;

            return;
        }
        RoomOptions roomOptions = new RoomOptions() { IsOpen = true, MaxPlayers = PlayerNumberManagemnt()};
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);

        Debug.Log(PlayerNumberManagemnt());
    }

    public void JoinToRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            char[] chars = new char[8];
            for (int i = 0; i < chars.Length; i++)
            {
                int rand = Random.Range(65, 122);
                chars[i] = (char)(rand);
            }

            string s = roomNameInput.text;

            foreach (char c in chars)
                s += c;

            roomNameInput.text = s;

            return;
        }

        PhotonNetwork.JoinRoom(roomNameInput.text);
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Match");
        Debug.Log("Room created successfully: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Room Players : " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Match");
        Debug.Log("Room joinet successfully: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Room Players : " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
}
