using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HammerUpgrade1 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        
        HammerScript blackHoleGun = ( HammerScript)targetGun;
        blackHoleGun.damage *= 2f;
        blackHoleGun.knockBackAmount *= 2f;
    }

    public override void unApply(GunData targetGun)
    {
        HammerScript blackHoleGun = ( HammerScript)targetGun;
        blackHoleGun.damage *= 1/2f;
        blackHoleGun.knockBackAmount *= 1/2f;
    }
}
