using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShotGunUpgrade1 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        ShotgunData targetShotgun = (ShotgunData)targetGun;
        targetShotgun.hspread *= 0.5f;
        targetShotgun.vspread *= 0.5f;
        targetShotgun.damage *= 1.5f;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        
        ShotgunData targetShotgun = (ShotgunData)targetGun;
        targetShotgun.hspread *= 2f;
        targetShotgun.vspread *= 2f;
        targetShotgun.damage *= 2/3f;
        // throw new System.NotImplementedException();
    }
}
