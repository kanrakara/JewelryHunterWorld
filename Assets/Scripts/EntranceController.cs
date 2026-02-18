using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntranceController : MonoBehaviour
{
    public int doorNumber;      //ドア番号
    public string sceneName;    //移行したいシーン名
    public bool opened;         //開場状況（初期化していない。folse が勝手に入る）

    bool isPlayerTouch;         //プレイヤーとの接触状態

    bool announcement;          //アナウンス中かどうか

    GameObject worldUI;         //Cannvasオブジェクト
    GameObject talkPanel;       //TalkPanelオブジェクト
    //MessageTextオブジェクトのTextMeshProUGUIコンポーネント
    TextMeshProUGUI messageText;    
    //World_PlayerオブジェクトのWorld_PlayerControllerコンポーネント
    World_PlayerController worldPlayerCnt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初期化していく。public化して、Unityでアタッチしていくのもあるが、今回は自動化している。
        //目的は、TMProUGUI内の情報が欲しい
        //左側はオブジェクト、右側がコンポーネント。型がコンポーネントなので（上記）
        worldPlayerCnt = GameObject.FindGameObjectWithTag("Player").GetComponent<World_PlayerController>();
        //オブジェクトなので、そのまま
        worldUI = GameObject.FindGameObjectWithTag("WorldUI");
        //talkPanelはゲームオブジェクトだが、とってきた情報がコンポーネントなので、その中のgameObjectと指定
        talkPanel = worldUI.transform.Find("TalkPanel").gameObject;
        //（.gameObjectまで）gameOnject型の、TMProUGUI型を取ってくる
        messageText = talkPanel.transform.Find("MessageText").gameObject.GetComponent<TextMeshProUGUI>();

        if (World_UIController.keyOpened != null)
        {
            //Dictionaryのもの
            opened = World_UIController.keyOpened[doorNumber];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーと接触 and ActionButtonが押されている
        if (isPlayerTouch && worldPlayerCnt.IsActionButtonPressed)
        {
            //アナウンス中じゃなければ
            if (!announcement)
            {
                //ゲーム進行をストップ
                Time.timeScale = 0;

                //解錠済みなら
                if (opened)
                {
                    //ゲーム進行を再開し
                    Time.timeScale = 1;

                    //該当ドア番号をGameManagerに管理してもらう
                    GameManager.currentDoorNumber = doorNumber;
                    //シーン移行
                    SceneManager.LoadScene(sceneName);
                    return;
                }
                else if (GameManager.keys > 0)      //鍵を持っている
                {
                    messageText.text = "新たなステージへの扉を開けた！";
                    GameManager.keys--;     //鍵を減らす
                    opened = true;          //解錠フラグを立てる
                    //World_UIControllerが所持している解錠の辞書（keyOpenedディクショナリー）に解錠したという情報を記録
                    World_UIController.keyOpened[doorNumber] = true;
                    announcement = true;        //アナウンス中フラグを立てる（text出したまま戻らせ、このフラグで内容を選ぶ）
                }
                else
                {
                    messageText.text = "鍵が足りません！";
                    announcement = true;        //アナウンス中
                }
            }
            else    //すでにアナウンス中なら announcement == true
            {
                Time.timeScale = 1;     //ゲーム進行を戻す。下でやってることと同じ
                string s = "";
                if (!opened)
                {
                    s = "(ロック)";
                }
                messageText.text = sceneName + s;
                announcement = false;   //アナウンス中フラグを解除
            }

            //連続入力にならないように一度リセット　※次にボタンが押されるまではfalse
            worldPlayerCnt.IsActionButtonPressed = false;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触判定をtrueにしてパネルを表示
            isPlayerTouch = true;
            talkPanel.SetActive(true);

            //パネルのメッセージに行き先となるシーン名
            //未解錠の場合は（ロック）と書き加える
            string s = "";
            if (!opened)
            {
                s = "(ロック)";
            }
            //Start の中で色々やっているのは、このテキストを出すため
            messageText.text = sceneName + s;
        }
    }

    //Playerが離れたとき
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触判定をfalseに戻して
            //パネルを非常時
            isPlayerTouch = false;
            if (messageText != null) // NullReferenceExceptionを防ぐ
            {
                talkPanel.SetActive(false);
                Time.timeScale = 1f; // ゲーム進行を再開。事故防止のための記述
            }
        }
    }
}
