using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeGunUpgrade1 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        GrenadeLauncherData targetLauncher= (GrenadeLauncherData)targetGun;
        targetLauncher.AOE *= 1.5f;
        targetLauncher.damage *= 1.5f;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        GrenadeLauncherData targetLauncher= (GrenadeLauncherData)targetGun;
        targetLauncher.AOE *= 2/3f;
        targetLauncher.damage *= 2/3f;
        // throw new System.NotImplementedException();
    }
}
