using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllRifleGuns : AutoGuns
{
    public override void GetAmmo() {
        bulletsPertap = data_script.GetRifleAmmo();
    }

    public override void LoseAmmo(int number) {
        data_script.LoseRifleAmmo(number);
    }
}
