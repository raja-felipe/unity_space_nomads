using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToSprite : MonoBehaviour
{
    [SerializeField] private string[] enemyNames;
    [SerializeField] private Sprite[] enemySprites;
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalSceneManager.EnemyImageDict = new Dictionary<string, Sprite>();
        for (int i = 0; i < enemyNames.Length; i++)
        {
            GlobalSceneManager.EnemyImageDict.Add(enemyNames[i], enemySprites[i]);
        }
    }
}
