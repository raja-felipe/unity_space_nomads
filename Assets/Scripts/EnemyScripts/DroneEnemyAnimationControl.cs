using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneEnemyAnimationControl : MonoBehaviour
{
    private Animator enemyAnimations;
    private string enemyOpen = "isTrigger";

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimations = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAnimations.SetBool(enemyOpen, true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAnimations.SetBool(enemyOpen, true);
        }
    }
}
