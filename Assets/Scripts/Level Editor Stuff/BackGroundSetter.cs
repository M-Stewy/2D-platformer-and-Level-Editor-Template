using UnityEngine;

public class BackGroundSetter : MonoBehaviour
{
    SpriteRenderer BG;

    public void SetBackGround(Sprite img)
    {

        BG = GetComponentInChildren<SpriteRenderer>();
        if (img == null) BG.enabled = false;
        else BG.sprite = img;
    }
}
