using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPistolGuns : AutoGuns
{
    public override void GetAmmo() {
        bulletsPertap = data_script.GetPistolAmmo();
    }

    public override void LoseAmmo(int number) {
        data_script.LosePistolAmmo(number);
    }
}
