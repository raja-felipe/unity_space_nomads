using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShotGunUpgrade2 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        ShotgunData targetShotgun = (ShotgunData)targetGun;
        targetShotgun.fireRate *= 2f;
        targetShotgun.clipSize *= 2;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        ShotgunData targetShotgun = (ShotgunData)targetGun;
        targetShotgun.fireRate *= 0.5f;
        targetShotgun.clipSize = Mathf.CeilToInt(targetShotgun.clipSize * 0.5f);
        // throw new System.NotImplementedException();
    }
}
