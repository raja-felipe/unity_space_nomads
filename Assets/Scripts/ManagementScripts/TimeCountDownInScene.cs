using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCountDownInScene : MonoBehaviour
{
    // Start is called before the first frame update
    public float howManyTime = 300f;
    private TextMeshPro showText;
    public Transform player;
    void Start()
    {
        showText = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {

        howManyTime -= Time.deltaTime;
        int sec = Mathf.FloorToInt(howManyTime % 60f);
        int min = Mathf.FloorToInt(howManyTime / 60f);

        showText.text = min.ToString("00") + ":" + sec.ToString("00");

        transform.LookAt(player);
        transform.Rotate(0,180,0);
    }
}
