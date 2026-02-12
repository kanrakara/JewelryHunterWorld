using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemData itemdata;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ゲームスタートと同時に、コンテナに書いてあるスプライトに上書きする。元からその見た目にしているので、今回は意味のない行になっている。
        GetComponent<SpriteRenderer>().sprite = itemdata.itemSprite;
    }


}
