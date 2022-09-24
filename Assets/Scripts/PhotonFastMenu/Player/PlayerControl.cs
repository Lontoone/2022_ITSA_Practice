using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
public class PlayerControl : MonoBehaviour
{
    private PhotonView pv;
    public Text nameInput;
    public Player player;
    private PlayerManager playerManager;

    public Collider2D checkRange;
    public LayerMask eatableLayer;
    private Collider2D[] res = new Collider2D[2];
    private ContactFilter2D filter2D;

    int point = 0;
    
    public void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();

        player = (Player)pv.InstantiationData[0];
        nameInput.text = player.NickName;

        //Manager產生player時，順便告訴player產生它的manager是誰
        //用尋找該view ID的PlayerManager
        playerManager = PhotonView.Find((int)pv.InstantiationData[1]).GetComponent<PlayerManager>();

        ScoreBoardManager.sbm_instance.SetupPlayer(player);

        //刪除不是自己的鏡頭
        if (!pv.IsMine)
        {
            Destroy(transform.GetComponentInChildren<Camera>());
        }
        //是自己的玩家物件=> 做數值設定
        else {
            filter2D.layerMask = eatableLayer;
            player.CustomProperties[PlayerManager.PLAYER_SCORE] = 0;
        }
    }

    private void Update()
    {
        if (!pv.IsMine) return;
        //TODO... player control
        // 要移動pv物件要再掛pv transform

        Vector2 _input = new Vector2( Input.GetAxis("Horizontal") , Input.GetAxis("Vertical"));
        transform.position = (Vector2)transform.position + _input * 5 * Time.deltaTime;

        //吃物品
        if (Input.GetKeyDown(KeyCode.Space)) {
            checkRange.OverlapCollider(filter2D, res);

            //吃掉物品 並加分
            if (res.Length > 0) {
                point++;
                //注意設定玩家參數時要用SetCustomProperties才會出發callback
                var data =  MyPhotonExtension.WrapToHash(new object[]{ PlayerManager.PLAYER_SCORE,point});
                player.SetCustomProperties(data);

                //注意此Destory只會發生在本地，不會同步，要用RPC發送
                //Destroy(res[0].gameObject);
                EatableObject _obj = res[0].GetComponent<EatableObject>();
                _obj.GotEat();
            }
        }
    }


}
