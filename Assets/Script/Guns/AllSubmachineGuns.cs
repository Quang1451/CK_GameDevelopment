using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSubmachineGuns : AutoGuns
{
    public override void GetAmmo() {
        bulletsPertap = data_script.GetSubmachineAmmo();
    }

    public override void LoseAmmo(int number) {
        data_script.LoseSubmachineAmmo(number);
    }
}
