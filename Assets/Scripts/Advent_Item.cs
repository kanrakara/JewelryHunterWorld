using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

//どのアイテムタイプかの分類を、自作の列挙型で。クラスの外なのでどこからでも使える。
public enum AdventItemType { None, Arrow, Key, Life };


public class Advent_Item : MonoBehaviour
{
    //オブジェクトのアイテムタイプ
    public AdventItemType type = AdventItemType.None;
    //タイプがArrowだった場合の増加量
    public int numberOfArrow = 10;
    //タイプがLifeだった場合の回復量
    public int recoveryValue = 1;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Playerタグと接触したらタイプごとの処理
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case AdventItemType.Key:
                    GameManager.keys ++;
                    //keyGotに登録
                    GameManager.keyGot[SceneManager.GetActiveScene().name] = true;
                    break;

                case AdventItemType.Arrow:
                    GameManager.arrows += numberOfArrow;
                    break;

                case AdventItemType.Life:
                    //これでも良いが
                    //PlayerController.playerLife += recoveryValue;

                    //PlayerControllerのstaticメソッドを使っても良い
                    PlayerController.PlayerRecovery(recoveryValue);
                    break;
            }

            // アイテムゲット演出
            GetComponent<CircleCollider2D>().enabled = false;      // 当たりを消す
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            rbody.gravityScale = 1.0f; //重力を戻す
            rbody.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); // 上に少し跳ね上げる
            Destroy(gameObject, 0.5f); // 0.5秒後にヒエラルキーからオブジェクトを抹消
        }
    }
}
