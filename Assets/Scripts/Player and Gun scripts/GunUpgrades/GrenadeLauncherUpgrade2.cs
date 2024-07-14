using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeLauncherUpgrade2 : GunUpgrade
{ 
    public override void apply(GunData targetGun)
    {
        GrenadeLauncherData targetLauncher= (GrenadeLauncherData)targetGun;
        targetLauncher.reloadSpeed *= 2f;
        targetLauncher.fireRate *= 1.5f;
        targetLauncher.reloadPerReload *= 2;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        GrenadeLauncherData targetLauncher= (GrenadeLauncherData)targetGun;
        targetLauncher.reloadSpeed *= 0.5f;
        targetLauncher.fireRate *= 0.75f;
        targetLauncher.reloadPerReload = Mathf.CeilToInt(targetLauncher.reloadPerReload * 0.5f);
        // throw new System.NotImplementedException();
    }
}
