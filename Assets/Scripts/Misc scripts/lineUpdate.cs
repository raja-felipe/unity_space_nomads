using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineUpdate : MonoBehaviour
{
    public Transform medicEnemy;
    public float healingDistance = 5.0f;

    public string healingTargetLayer = "Enemy";

    private LineRenderer line;

    private Transform healingEnemy;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        healingEnemy = null;

        int enemyLayer = LayerMask.GetMask(healingTargetLayer);
        float closestHealDistance = healingDistance;

        Collider[] objectInRange = Physics.OverlapSphere(medicEnemy.position, healingDistance, enemyLayer);

        foreach (var enemy in objectInRange)
        {
            float distance = Vector3.Distance(medicEnemy.position, enemy.transform.position);
            if (distance < healingDistance)
            {
                closestHealDistance = distance;
                healingEnemy = enemy.transform;
            }
        }

        if (healingEnemy != null)
        {
            line.enabled = true;
            line.useWorldSpace = true;
            line.SetPosition(0, medicEnemy.position);
            line.SetPosition(1, healingEnemy.position);
        }
        else
        {
            line.useWorldSpace = false;
            line.positionCount = 2;
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
            line.enabled = false;
        }
    }
    

}
