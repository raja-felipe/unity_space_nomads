using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BlackHoleUpgrade2 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        BlackHoleGunData blackHoleGun = ( BlackHoleGunData)targetGun;
        blackHoleGun.damage *= 3f;
    }

    public override void unApply(GunData targetGun)
    {
        BlackHoleGunData blackHoleGun = ( BlackHoleGunData)targetGun;
        blackHoleGun.damage *= 1/3f;
    }
}

