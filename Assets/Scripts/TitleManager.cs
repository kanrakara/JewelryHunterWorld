using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;      //シーンの切り替えに必要

public class TitleManager : MonoBehaviour
{
    public string sceneName;        //読み込むシーン名

    public GameObject startButton;      // スタートボタンオブジェクト
    public GameObject continueButton;       // コンティニューボタンオブジェクト


    //入力方法③
    //On + [アクション名](InputValue) でメソッドを定義する
    //InputSystem_Actionsで決めたUIマップのSubmitアクションが押されたとき
        void OnSubmit(InputValue value)
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
            continueButton.GetComponent<Button>().interactable = false;     // ボタン機能を無効
        }

        // SoundManagerクラス自体をstaticしているから、このように指定できる
        SoundManager.currentSoundManager.StopBGM();     // BGMをストップ
        SoundManager.currentSoundManager.PlayBGM(BGMType.Title);        // タイトルのBGMを再生
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーンを読み込む
    public void Load()
    {
        //セーブデータを初期化
        SaveDataManager.Initialize();
        GameManager.totalScore = 0;     //新しくゲームを始めるにあたってスコアをリセット
        SceneManager.LoadScene(sceneName);
    }

    // シーンを読み込む
    public void ContinueLoad()
    {
        SaveDataManager.LoadGameData(); //セーブデータを読み込む
        SceneManager.LoadScene(sceneName);
    }
}
