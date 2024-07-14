using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBars : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image shieldBar;
    // [SerializeField] private Image engineBar;
    private const float DECREMENT = 0.01f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount -= DECREMENT;
        shieldBar.fillAmount -= DECREMENT;
        // engineBar.fillAmount -= DECREMENT;
    }
}
