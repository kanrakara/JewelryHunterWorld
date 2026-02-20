using UnityEngine;
using UnityEngine.SceneManagement;

public class Advent_ItemBox : MonoBehaviour
{
    public Sprite openImage;
    public GameObject itemPrefab;
    public bool isClosed = true;
    public AdventItemType type = AdventItemType.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //もしも自分のタイプがKeyだったら
        if (type == AdventItemType.Key)
        {
            //もしもGameManagerのkeyGotディクショナリーの該当シーンの記録がtrue（取得済み）だったら
            if (GameManager.keyGot[SceneManager.GetActiveScene().name])
            {
                //close状態をfalse
                isClosed = false;

                //見た目をオープンの絵にする
                GetComponent<SpriteRenderer>().sprite = openImage;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isClosed && collision.gameObject.tag == "Player")
        {
            //宝箱の絵をOpenの絵に変更
            GetComponent<SpriteRenderer>().sprite = openImage;

            //closeのフラグを解除
            isClosed = false;

            //その場に変数に指定したプレハブオブジェクトを生成
            //（もし変数にプレハブオブジェクトが指定されていれば）
            if (itemPrefab != null)
            {
                Instantiate(
                    itemPrefab,     //生成する対象オブジェクト
                    transform.position,     //生成位置、その場
                    Quaternion.identity     //生成時の回転、無回転
                    );
            }

        }
    }
}
