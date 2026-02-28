using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;


public class TitleManager : MonoBehaviour
{
    public string sceneName; //スタートボタンを押して読み込むシーン名

    public GameObject startButton; //スタートボタンオブジェクト
    public GameObject continueButton; //コンテニューボタンオブジェクト

    //public InputAction submitAction; //決定のInputAction;

    //void OnEnable()
    //{
    //    submitAction.Enable(); //InputActionを有効化
    //}
    //void OnDisable()
    //{
    //    submitAction.Disable(); //InputActionを無効化
    //}

    //InputSystem?Actionsで決めたUIマップのSubmitアクションが押されたとき
    void OnSubmit(InputValue valuse)
    {
        Load();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PlayerPrefsからJSON文字列をロード
        string jsonData = PlayerPrefs.GetString("SaveData");

        // JSONデータが存在しない場合、エラーを回避し処理を中断
        if (string.IsNullOrEmpty(jsonData))
        {
            continueButton.GetComponent<Button>().interactable = false; //ボタン機能を無効
        }

        //コルーチン
        StartCoroutine(TitleBGMStartCol());
    }

    // Update is called once per frame
    void Update()
    {
        ////インスペクター上で登録したキーのいずれかがおされていたら成立
        //if (submitAction.WasPressedThisFrame())
        //{
        //    Load();
        //}

        ////列挙型のKeyboard型の値を変数kbに代入
        //Keyboard kb = Keyboard.current;
        //if(kb != null) //キーボードがつながっていれば
        //{
        //    //エンターキーがおされた状態なら
        //    if (kb.enterKey.wasPressedThisFrame)
        //    {
        //        Load();
        //    }
        //}
    }

    //シーンを読み込むメソッド作成
    public void Load()
    {
        SaveDataManager.Initialize(); //セーブデータを初期化
        //GameManager.totalScore = 0; //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }

    //セーブデータを読み込んでから始める
    public void ContinueLoad()
    {
        SaveDataManager.LoadGameData(); //セーブデータを読み込む
        SceneManager.LoadScene(sceneName);
    }

    //コルーチン
    IEnumerator TitleBGMStartCol()
    {
        yield return new WaitForSeconds(1.0f);
        //SoundManager.currentSoundManager.StopBGM();
        SoundManager.currentSoundManager.PlayBGM(BGMType.Title);
    }
}