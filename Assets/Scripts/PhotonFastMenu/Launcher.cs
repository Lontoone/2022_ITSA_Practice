using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;
    private List<RoomListItem> _roomItems = new List<RoomListItem>();
    [SerializeField] UnityEngine.UI.InputField roomNameInputField;
    [SerializeField] UnityEngine.UI.Text errorText;
    [SerializeField] UnityEngine.UI.Text roomNameText;
    [SerializeField] Transform roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject StartGameButton;
    [SerializeField] UnityEngine.UI.InputField nameInput;


    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //connect to server
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connect to lobby");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("title");
        Debug.Log("Join lobby");

        PhotonNetwork.NickName = nameInput == null ? "Player" + Random.Range(0, 1000).ToString("0000") : nameInput.text;
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) { return; }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
        //PhotonNetwork.CreateRoom(roomNameInputField.text);
        PhotonNetwork.JoinOrCreateRoom(roomNameInputField.text, roomOptions, TypedLobby.Default);

        Debug.Log("room create name: " + roomNameInputField.text);

        //避免玩家在等待server時亂點其他按鈕s
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnJoinedRoom() //called when create or join a room
    {
        MenuManager.instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        //只有host可以開始遊戲按鈕
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //switch host時讓下個host可以開始遊戲
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room create Failed " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Create room list items:

        //  Check is Delete or Create
        foreach (RoomInfo _room in roomList)
        {
            Debug.Log("room list update " + _room.Name + " " + _room.RemovedFromList);
            //      Delete
            if (_room.RemovedFromList)
            {
                //Debug.Log("delete room list " + (_roomItems.Find(x => x.info.Name == _room.Name).name));
                Destroy(_roomItems.Find(x => x.info.Name == _room.Name)?.gameObject);
            }
            //      Create:
            else if (!_roomItems.Exists(x => x.info.Name == _room.Name))
            {
                RoomListItem _newItem = Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>();
                _newItem.SetUp(_room);
                _roomItems.Add(_newItem);

                //Debug.Log("create room list " + _newItem.name);
            }

        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
