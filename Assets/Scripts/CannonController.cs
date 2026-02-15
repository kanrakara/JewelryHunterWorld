using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;            //発生させるPrefabデータ
    public float delayTime = 3.0f;          //遅延時間
    public float fireSpeed = 4.0f;          //発射速度
    public float length = 8.0f;             //範囲

    GameObject player;                      //プレイヤー
    Transform gateTransform;                //発射口のTransform
    float passedTimes = 0;                  //経過時間

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //発射口オブジェクトのTransformを取得
        gateTransform = transform.Find("gate");
        //プレイヤーを取得
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //待機時間加算
        passedTimes += Time.deltaTime;

        if (player != null)
        {
            //Playerとの距離チェック
            if (CheckLength(player.transform.position))
            {
                //待機時間経過
                if (passedTimes > delayTime)
                {
                    passedTimes = 0;        //時間を0にリセット
                                            //砲弾をプレハブから作る。Instantiate メソッド(どのオブジェクト, どこに, 回転）
                                            //Transformって3つありますが、position, scale はVector3 型、rotation はQuaternion 型なので注意。
                                            //Quaternion.identity で、0,0,0 を表す。
                    Vector2 pos = new Vector2(
                        gateTransform.position.x,
                        gateTransform.position.y);

                    //メソッドなので右辺だけで成り立つが、生成したオブジェクトはまだ使うことがあるので、左辺を書いて変数に格納している。2行を1行に省略している形
                    GameObject obj = Instantiate(
                        objPrefab,
                        pos,
                        Quaternion.identity);
                    //砲身が向いている方向に発射する
                    Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();

                    //AddForce はVector2(x,y)型のため、x,y 座標を出して方向とする
                    float angleZ = transform.localEulerAngles.z - 180;  //絵が左向きで基準の方向が逆のため、-180°している
                    //半径1の円（単位円）として考える。sin, cos がそのまま使えるので。
                    float x = Mathf.Cos(angleZ * Mathf.Deg2Rad); //角度に対して底辺を取得
                    float y = Mathf.Sin(angleZ * Mathf.Deg2Rad); //角度に対して垂直辺を取得
                    Vector2 v = new Vector2(x, y) * fireSpeed;
                    //ここで出した方向は、（単位円のため）距離1になるので、Impulse を掛けて速さを変更できるようにしている
                    rbody.AddForce(v, ForceMode2D.Impulse);
                }
            }
        }
    }

    //Playerとの距離チェック
    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position, targetPos);
        if (length >= d)
        {
            ret = true;
        }
        return ret;
    }

    //範囲表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
