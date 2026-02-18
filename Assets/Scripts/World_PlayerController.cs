using UnityEngine;
using UnityEngine.InputSystem;

public enum Direction { none, left, right }



public class World_PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    Vector2 moveVec = Vector2.zero;
    float angleZ = 0.0f;
    Rigidbody2D rbody;
    Animator animator;


    bool isActionButtonPressed;     //ActionぼButtonが押されたらtrue
    public bool IsActionButtonPressed       //カプセル化。privateで触れないisActionB～ に干渉するために、同名の変数とそのプロパティを設定
    {
        get { return isActionButtonPressed; }
        set { isActionButtonPressed = value; }
    }

    //On + Actionsの名前　で呼び出せる。InputValueは状態を返す。
    void OnActionButton(InputValue value)
    {
        IsActionButtonPressed = value.isPressed; // ボタンが押され続けている間はtrue
    }

    //InputSystemの入力値を変数moveVecにVector2型で代入
    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>();
    }

    //INputsystemの入力値moveVecからプレイヤーの角度を算出
    float GetAngle()
    {
        float angle = angleZ;
        if (moveVec != Vector2.zero)
        {
            float rad = Mathf.Atan2(moveVec.y, moveVec.x);
            angle = rad * Mathf.Rad2Deg;
        }
        return angle;
    }

    //その時のangleZの値に応じて右向きなのか、左向きなのか判断
    Direction AngleToDirection()
    {
        Direction dir;
        if (angleZ >= -89 && angleZ <= 89)
        {
            dir = Direction.right;
        }
        else if (angleZ >= 91 && angleZ <= 269)
        {
            dir = Direction.left;
        }
        else
        {
            dir = Direction.none;
        }
        return dir;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        angleZ = GetAngle();
        Direction dir = AngleToDirection();

        //右向きなら絵はそのまま、左向きなら絵は逆
        if (dir == Direction.right)
        {
            transform.localScale = new Vector2(1, 1);   //絵はそのまま
        }
        else if (dir == Direction.left)
        {
            transform.localScale = new Vector2(-1, 1);  //絵は逆転
        }

        //移動キーが入力されていそうならRun
        if (moveVec != Vector2.zero)
        {
            animator.SetBool("Run", true);      //Runパラメーターをfalse
        }
        else
        {
            animator.SetBool("Run", false);     //Runパラメーターをfalse
        }
    }

    //実際にPlayerを動かしている
    void FixedUpdate()
    {
        rbody.linearVelocity = moveVec * speed;
    }
}
