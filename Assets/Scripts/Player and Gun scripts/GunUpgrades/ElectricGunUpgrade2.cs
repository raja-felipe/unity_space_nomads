using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElectricGunUpgrade2 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        
        ElectricGunScript ElectricGun = (ElectricGunScript)targetGun;
        ElectricGun.damage *= 3f;
        ElectricGun.fireRate *= 0.5f;
       //  throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        ElectricGunScript ElectricGun = (ElectricGunScript)targetGun;
        ElectricGun.damage *= 1/3f;
        ElectricGun.fireRate *= 1/0.5f;
        // throw new System.NotImplementedException();
    }
}
