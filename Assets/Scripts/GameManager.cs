using UnityEngine;
using UnityEngine.SceneManagement;      //シーンの切り替えに必要

public enum GameState           // GameStateという名前の型を自作する。4つのゲームの状態しか持てないものにする（列挙型）
{
    InGame,                     // ゲーム中
    GameClear,                  // ゲームクリア
    GameOver,                   // ゲームオーバー
    GameEnd,                    // ゲーム終了
}

public class GameManager : MonoBehaviour
{
    // ゲームの状態
    public static GameState gameState;
    public string nextSceneName;            // 次のシーン名


    //スコア追加
    public static int totalScore;           //合計スコア。各シーンをまたいで同じものを使い続けたいので、static を使用。

    //サウンド関連
    public AudioClip meGameClear;           //ゲームクリアの音
    public AudioClip meGameOver;            //ゲームオーバーの音
    AudioSource soundPlayer;                //AudioSource型の変数

    public bool isGameClear = false;        //ゲームクリア判定
    public bool isGameOver = false;         //ゲームオーバー判定

    //ワールドマップで最後に入ったエントランスのドア番号
    public static int currentDoorNumber = 0;

    //所持アイテム　鍵の管理
    public static int keys = 1;

    //どのステージの鍵が入手済みかを管理
    public static Dictionary<string, bool> keyGot;      //シーン名, true/galse 

    //所持アイテム　矢の管理
    public static int allows = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameState = GameState.InGame;       //ステータスをゲーム中にする
        soundPlayer = GetComponent<AudioSource>();      //AudioSourceを参照する

        //keyGotが何もない状態だったときのみ初期化
        if (keyGot == null)
        {
            keyGot = new Dictionary<string, bool>();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(gameState == GameState.GameClear)
        {
            soundPlayer.Stop();             //ステージ曲を止める
            soundPlayer.PlayOneShot(meGameClear);       //ゲームクリアの音を1回だけ鳴らす
            isGameClear = true;             //クリアフラグ
            gameState = GameState.GameEnd;      //ゲームの状態を更新
        }
        else if(gameState == GameState.GameOver)
        {
            soundPlayer.Stop();             //ステージ曲を止める
            soundPlayer.PlayOneShot(meGameOver);       //ゲームオーバーの音を1回だけ鳴らす
            isGameOver = true;              //ゲームオーバーフラグ
            gameState = GameState.GameEnd;      //ゲームの状態を更新
        }
    }

    //リスタート
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    //PlayerController経由で「UIマップのSubmit」が押されたとき呼び出される
    public void GameEnd()
    {
        //UI表示が終わって最後の状態であれば
        if(gameState == GameState.GameEnd)
        {
            //ゲームクリアの状態なら
            if (isGameClear)
            {
                Next();
            }

            //ゲームオーバーの状況なら
            else if (isGameOver)
            {
                Restart();
            }
        }
    }
}
