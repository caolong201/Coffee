using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkin : MonoBehaviour
{
    public Sprite[] Skin;

    public bool Optional = false;

    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();

        RandomImage();
    }

    public void RandomImage()
    {
        if(_sprite == null)
            _sprite = GetComponent<SpriteRenderer>();

        if (Optional && Random.value <= 0.33f)
        {
            _sprite.sprite = null;
        }
        else if(Skin.Length > 0)
        {
            _sprite.sprite = Skin[Random.Range(0, Skin.Length)];
        }
    }
}
