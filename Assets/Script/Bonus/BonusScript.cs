using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{
    public SpriteRenderer _bonusSpriteRendrer;
    public string _bonusName;

    private Sprite _bonusImage;

    void Start()
    {
        _bonusImage = _bonusSpriteRendrer.sprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == GameManager.instance._player.gameObject)
        {
            GameManager.instance._player._bonusImage.sprite = _bonusImage;
            GameManager.instance._player._bonusImage.color = Color.white;
            GameManager.instance._player._typeOfBonus = _bonusName;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
