using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlackHoleUpgrade1 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        BlackHoleGunData blackHoleGun = ( BlackHoleGunData)targetGun;
        blackHoleGun.attractionStrength *= 3f;
        blackHoleGun.attractionRange *= 2f;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        BlackHoleGunData blackHoleGun = ( BlackHoleGunData)targetGun;
        blackHoleGun.attractionStrength *= 1/3f;
        blackHoleGun.attractionRange *= 1/2f;
        // throw new System.NotImplementedException();
    }
}
