using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunUpgrade : ScriptableObject
{
    // Start is called before the first frame update
    public new string name  = " missing name";
    public string description = "no description";
    public abstract void apply(GunData targetGun);
    public abstract void unApply(GunData targetGun);
    public override bool Equals(object other)
    {
        var item = other as GunUpgrade;

        if (item == null) return false;
        
        return this.name.Equals(item.name);
    }

    public override int GetHashCode()
    {
        return this.name.GetHashCode();
    }
}