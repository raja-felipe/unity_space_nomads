using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HammerUpgrade2 : GunUpgrade{
    
    public override void apply(GunData targetGun)
    {
        HammerScript blackHoleGun = ( HammerScript)targetGun;
        blackHoleGun.hitDimensions *= 1.5f;
        blackHoleGun.forwardOffset *= 1.5f;
        blackHoleGun.fireRate *= 1.5f;
        blackHoleGun.knockBackAmount *= 1/2f;
    }

    public override void unApply(GunData targetGun)
    {
        HammerScript blackHoleGun = ( HammerScript)targetGun;
        blackHoleGun.hitDimensions *= 1/2f;
        blackHoleGun.forwardOffset *= 1/2f;
        blackHoleGun.fireRate *= 2/3f;
        blackHoleGun.knockBackAmount *= 2f;
    }
}
