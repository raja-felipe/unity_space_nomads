using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ElectricGunUpgrade1 : GunUpgrade
{
    public override void apply(GunData targetGun)
    {
        ElectricGunScript ElectricGun = (ElectricGunScript)targetGun;
        ElectricGun.clipSize *= 2;
        ElectricGun.chainCount *= 2;
        ElectricGun.chainRange *= 2f;
        // throw new System.NotImplementedException();
    }

    public override void unApply(GunData targetGun)
    {
        ElectricGunScript ElectricGun = (ElectricGunScript)targetGun;
        ElectricGun.clipSize = (int)(ElectricGun.clipSize / 2f); // *= Mathf.CeilToInt(ElectricGun.clipSize  * 0.5f);
        ElectricGun.chainCount = (int)(ElectricGun.chainCount / 2f); // *= Mathf.CeilToInt(ElectricGun.chainCount  * 0.5f);
        ElectricGun.chainRange *= 0.5f;
        // throw new System.NotImplementedException();
    }
}
