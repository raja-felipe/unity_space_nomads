using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyCanHit
{
    /// <summary>
    /// This interface is put on anything the enemies can damage.
    /// "damage" returns true if damage was done.
    /// </summary>
    public float damage(float amount, EnemyControlScript source);

}
