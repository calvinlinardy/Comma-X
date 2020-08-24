using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSprites : MonoBehaviour
{
    [SerializeField] Sprite[] ufoSprites = null;

    public void Start()
    {
        int arrayIdx = Random.Range(0, ufoSprites.Length);
        Sprite ufoSprite = ufoSprites[arrayIdx];
        GetComponent<SpriteRenderer>().sprite = ufoSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
