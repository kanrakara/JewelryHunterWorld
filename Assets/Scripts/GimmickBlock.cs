using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f;     // 自動落下検知距離
    public bool isDelete = false;   // 何かに触れたら削除するフラグ
    bool fade = false;            // 消滅開始フラグ
    float fadeTime = 0.5f;          // フェードアウト時間

    GameObject deadObj;             // 死亡当たり

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Rigidbody2Dの物理挙動を停止
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;            //物理計算を止める。Unityの表示上で、Body Type にあるやつ。基本は Dynamic（計算する）
        deadObj = transform.Find("DeadObject").gameObject;  //死亡あたり取得
        deadObj.SetActive(false);                           //死亡あたりを非表示。空中にある間は、下のdeadは消しておく。

        player = GameObject.FindGameObjectWithTag("Player"); // プレイヤーを探す
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // プレイヤーとの距離計測
            float d = Vector2.Distance(transform.position, player.transform.position);
            if (length >= d)
            {
                Rigidbody2D rbody = GetComponent<Rigidbody2D>();
                if (rbody.bodyType == RigidbodyType2D.Static)
                {
                    // Rigidbody2Dの物理挙動を開始
                    rbody.bodyType = RigidbodyType2D.Dynamic;
                    deadObj.SetActive(true);    //死亡あたりを表示
                }
            }
        }

        if (fade) //消滅開始フラグがオンになってから
        {
            // 透明値を変更してフェードアウトさせる
            fadeTime -= Time.deltaTime; // 前フレームの差分秒マイナス
            Color col = GetComponent<SpriteRenderer>().color;   // カラーを取り出す
            col.a = fadeTime;   // 透明値（アルファー値）を変更
            GetComponent<SpriteRenderer>().color = col; // カラーを再設定する

            //１度目の更新の際にアルファー値が0.5となるので、もし「最初は不透明（1.0）から始めて、0.5秒かけて消したい」という意図であれば、コードを以下のように調整するのもある
            //float duration = 0.5f; // 0.5秒で消したい
            //timer += Time.deltaTime; // 経過時間を足していく
            //float alpha = 1.0f - (timer / duration); // 1.0から割合を引く


            if (fadeTime <= 0.0f)
            {
                // 0以下(透明)になったら消す
                Destroy(gameObject);
            }
        }
    }

    // 接触開始。OnTriggerEnter2D 片方がisTrigger（すり抜け物体）。OnCollisionEnter2D 両方がisTriggerじゃないとき。
    // 引数注意。OnTriggerEnter2D → Collider2D。OnCollisionEnter2D → Collision2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete) //何かに触れたら消える設定の場合
        {
            fade = true; // 消滅開始フラグオン
        }
    }

    //範囲表示。パターンなので、覚えよう
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
