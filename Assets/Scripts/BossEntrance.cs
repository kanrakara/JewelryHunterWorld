using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEntrance : MonoBehaviour
{
    // 各エントランスtのクリア状況を管理
    public static Dictionary<int, bool> stagesClear;
    public string sceneName;     // シーン切り替え先
    bool isOpened;      // 空いているかの状況



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Entrance");

        // リストがないときの情報取得とセッティング
        if (stagesClear == null)
        {
            stagesClear = new Dictionary<int, bool>(); // 最初に初期化が必要

            //集めてきたEntranceを全点検
            for (int i = 0; i < obj.Length; i++)
            {
                //Entranceオブジェクトが持っているEntranceControllerを取得
                EntranceController entranceController = obj[i].GetComponent<EntranceController>();
                if (entranceController != null)
                {
                    //辞書に(keyOpenedディクショナリー)に変数doorNumberと変数opend の状況を記録
                    //最初は必ずfalseのため
                    stagesClear.Add(
                        entranceController.doorNumber,
                        false
                    );
                }
            }
        }
        else
        {
            int sum = 0;        // クリアがどのくらいあるのかカウント用
            // Entranceの数だけ繰り返す
            for(int i  = 0; i < obj.Length; i++)
            {
                if (stagesClear[i])     // stagesClearディクショナリの中身を順にチェック
                {
                    sum++;      // もしtrue（クリア済み）ならカウント
                }
            }
            if(sum >= obj.Length)       // もしクリアの数（trueの数）とEntranceの数が一致していたら
            {
                // 全部クリアしたので扉を開ける
                GetComponent<SpriteRenderer>().enabled = false;     // 見た目をなくす（コライダーは残るように）
                isOpened = true;        //  扉が開いたという状態にする
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 触れた相手がPlayerかつ扉が空いていれば
        if(collision.gameObject.tag == "Player" && isOpened)
        {
            SceneManager.LoadScene(sceneName);      // Boss部屋に行く
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
