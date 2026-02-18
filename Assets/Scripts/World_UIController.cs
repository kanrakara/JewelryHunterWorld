using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class World_UIController : MonoBehaviour
{
    public TextMeshProUGUI keyText;
    int currentKeys;
    public TextMeshProUGUI arrowText;
    int currentArrows;

    GameObject player;

    //各EntranceのDoorNumberごとに解錠か非解錠かをまとめておく（t/f)
    public static Dictionary<int, bool> keyOpened;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //WorldMap中のEntranceオブジェクトを集めて配列objに代入（順不同に中に入れられる。宣言と同時に挿入しているので、数は固定でなくて良い）
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        //リストがない時の情報取得とセッティング
        if (keyOpened == null)
        {
            keyOpened = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクトが持っているEntranceControllerを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    //辞書に(keyOpenedディクショナリー)に変数doorNumberと変数opend の状況を記録
                    keyOpened.Add(
                        entranceController.doorNumber,
                        entranceController.opened
                    );
                }
            }
        }

        //プレイヤーの位置
        player = GameObject.FindGameObjectWithTag("Player");
        //暫定のプレイヤーの位置
        //Vector2 currentPlayerPos = new Vector2(0,0);
        Vector2 currentPlayerPos = Vector2.zero;

        //GameManagerに記録されているcurrentDoorNumberと一致するdoorNumberを持っているEntranceを探す
        for (int i = 0; i < obj.Length; i++)
        {
            //EntranceのEntranceControllerの変数doorNumberが、GameManagerの把握しているcurrentDoorNumberと同じかどうかチェック
            if (obj[i].GetComponent<EntranceController>().doorNumber == GameManager.currentDoorNumber)
            {
                //暫定プレイヤーの位置を一致したEntranceオブジェクトの位置に書き換え
                currentPlayerPos = obj[i].transform.position;
            }
        }
        //最終的に残ったcurrentPlayerPosの座標がPlayerの座標になる
        player.transform.position = currentPlayerPos;

    }

    // Update is called once per frame
    void Update()
    {
        //把握していた鍵の一人、GameManagerの鍵の数に違いが出たら、正しい数となるようUIを更新
        if (currentKeys != GameManager.keys)
        {
            currentKeys = GameManager.keys;
            keyText.text = currentKeys.ToString();
        }

        //把握していた矢野一人GameManagerの屋の数に違いが出たら、正しい数となるようUIを更新
        if (currentArrows != GameManager.arrows)
        {
            currentArrows = GameManager.arrows;
            arrowText.text = currentArrows.ToString();
        }
    }
}
