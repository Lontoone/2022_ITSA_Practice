using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{

    PhotonView pv;
    GameObject controller;

    [HideInInspector]
    public Player player;
    public GameObject playerCameraObject;

    [HideInInspector]
    public Camera _playerCamera;

    public const string PLAYER_SCORE= "PLAYER_SCORE";

    public void Awake()
    {
        pv = GetComponent<PhotonView>();


        CreateController();
        if (pv.IsMine)
        {
            player = PhotonNetwork.LocalPlayer;
            //GameSceneManager.instance.localPlayer = controller;
        }
    }
    void Start()
    {
    }

    void CreateController()
    {
        Debug.Log("Instantiated Player Controller");


        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"),
            Vector2.zero,
            Quaternion.identity,
            0,
            new object[] { PhotonNetwork.LocalPlayer, pv.ViewID });
            //new object[] { pv.ViewID });

        /*
        //玩家攝影機
        if (_playerCamera == null)
            _playerCamera = Instantiate(playerCameraObject).GetComponent<Camera>();
        */

    }

    private void FixedUpdate()
    {
        /*
        if (pv.IsMine)
            _playerCamera.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, -3);
        */
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
    }
}
