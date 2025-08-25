using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerControler : MonoBehaviour, IPunObservable
{
    [Header("Player Components")]
    [SerializeField] CharacterController controler;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera playerWeponCamer;
    [SerializeField] Camera playerCameraMap;
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject playerModel;

    [Header("Player Settings")]
    [SerializeField] public float playerSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityScale;
    [SerializeField] float mouseSensitivity = 2f;
    [SerializeField] public float playerHealth = 100f;

    [Header("Player Network")]
    [SerializeField] public PhotonView photonView;

    [Header("Player Mash")]
    [SerializeField] GameObject playerHeade;
    [SerializeField] GameObject playerBody;
    [SerializeField] GameObject playerLegs;
    [SerializeField] GameObject playerMarck;

    [Header("Player UI")]
    [SerializeField] Canvas playerUI;
    [SerializeField] TextMeshProUGUI playerPing;
    [SerializeField] TextMeshPro playerHealthText;

    Vector3 moveDirection;
    Vector3 netWorkTransformPosition;
    Quaternion netWorkTransformRotation;
    Vector3 savePosition;
    Quaternion NetworCamerRotetion;

    float rotX, rotY;
    float lerpMovmentControl = 100f;
    [HideInInspector] public float helth;
    [HideInInspector] public float normaleSpeed;
    [HideInInspector] public float croushSpeed;

    bool isJumping = false;
    [HideInInspector] public bool isCrawshing = false;
    [HideInInspector] public bool canDameg;
    [HideInInspector] public bool canMove;

    [HideInInspector] public string playerTeem;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (photonView.IsMine == true)
        {
            playerHeade.SetActive(false);
            playerBody.SetActive(false);
            playerLegs.SetActive(false);

            savePosition = transform.position;
            helth = playerHealth;

            normaleSpeed = playerSpeed;
            croushSpeed = playerSpeed / 2f;

            canMove = true;
            canDameg = true;

            photonView.RPC("ChangeColor", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            playerCamera.enabled = false;
            playerWeponCamer.enabled = false;
            playerCameraMap.enabled = false;
            playerMarck.SetActive(false);

            if (playerUI != null)
            {
                playerUI.enabled = false;
            }
        }
    }
                                                                                                                       
    void Update()
    {
        if(photonView.IsMine == true)
        {
            if(canMove == true)
                MovePlayer(playerSpeed);

            MouseControle(mouseSensitivity);
            CrawshSystem();
            playerHealthText.text = playerHealth.ToString();

            playerPing.text = "Ping: " + PhotonNetwork.GetPing() + "ms";

            if (playerHealth <= 0f)
            {
                GameObject gameManagemnt = GameObject.Find("GameManagemnt");
                PhotonView gameManagemntView = gameManagemnt.GetComponent<PhotonView>();

                if (gameManagemnt != null)
                {
                    gameManagemntView.RPC("PlayerRespown", RpcTarget.AllBuffered, photonView.ViewID, playerTeem);
                    gameManagemntView.RPC("ScoreTeemSystem", RpcTarget.AllBuffered, playerTeem);
                }
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, netWorkTransformPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, netWorkTransformRotation, Time.deltaTime * 10f);
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, NetworCamerRotetion, Time.deltaTime * 10f);
        }
    }

    public void MovePlayer(float speed)
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveZ = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(MoveX, 0,MoveZ);

        Vector3 moveLerp = transform.TransformDirection(direction * speed);

        moveDirection.x = Mathf.Lerp(moveDirection.x, moveLerp.x, lerpMovmentControl);
        moveDirection.z = Mathf.Lerp(moveDirection.z, moveLerp.z, lerpMovmentControl);

        if (controler.isGrounded)
        {
            lerpMovmentControl = 10f;
            isJumping = true;

            if (Input.GetButtonDown("Jump") && isCrawshing == false)
            {
                moveDirection.y = jumpForce;
            }
        }

        if(controler.isGrounded == false && isJumping == true)
        {
            lerpMovmentControl = 0.03f;
            isJumping = false; 
        }

        moveDirection.y -= gravityScale * Time.deltaTime; 
        controler.Move(moveDirection * Time.deltaTime);
    }

    public void MouseControle(float Sensitivity)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotX += mouseY * Sensitivity;
        rotY += mouseX * Sensitivity;

        rotX = Mathf.Clamp(rotX, -60f, 60f); 

        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);
        cameraTransform.localRotation = Quaternion.Euler(-rotX, 0f, 0f);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(playerCamera.transform.rotation);
            stream.SendNext(playerHealth);
            stream.SendNext(canMove);
            stream.SendNext(playerHealthText.text);
        }
        else
        {
            netWorkTransformPosition = (Vector3)stream.ReceiveNext();
            netWorkTransformRotation = (Quaternion)stream.ReceiveNext();
            NetworCamerRotetion = (Quaternion)stream.ReceiveNext();
            playerHealth = (float)stream.ReceiveNext();
            canMove = (bool)stream.ReceiveNext();
            playerHealthText.text = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void ChangeColor(int player)
    {
        if(player == 1 || player == 3)
        {
            playerBody.GetComponent<Renderer>().material.color = Color.red; 
            playerTeem = "red";
        }
        else if (player == 2 || player == 4)
        { 
            playerBody.GetComponent<Renderer>().material.color = Color.blue; 
            playerTeem = "blue"; 
        }
    }

    public void CrawshSystem()
    {
        if (Input.GetButtonDown("Crawsh"))
        {
            isCrawshing = true;
            playerAnimator.SetInteger("done", 1);
            playerSpeed /= 2f;
        }
        else if (Input.GetButtonUp("Crawsh"))
        {
            isCrawshing = true;
            playerAnimator.SetInteger("done", 2);
            playerSpeed *= 2f;
        }
        else if(isCrawshing == true && Input.GetButton("Crawsh") == false)
        {
            playerSpeed = normaleSpeed;
            isCrawshing = false;
        }
    }

    public IEnumerator Respown()
    {
        canDameg = false;
        canMove = false;
        yield return new WaitForSeconds(0.1f);

        playerHealth = helth;
        canMove = true;

        yield return new WaitForSeconds(5f);
        canDameg = true;
    }
}