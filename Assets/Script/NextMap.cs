using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMap : MonoBehaviour
{   
    [SerializeField] private PlayerData dataPlayer;
    [SerializeField] private LoadingScreenBarSystem load;
    [SerializeField] private GunsInventory inventory;
    [SerializeField] private int map;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Player") {
            PlayerDataSetting.Instance.Health = dataPlayer.GetHealth();
            PlayerDataSetting.Instance.RifleAmmo = inventory.rifleAmmo;
            PlayerDataSetting.Instance.ShotgunAmmo = inventory.shotgunAmmo;
            PlayerDataSetting.Instance.SubmachineAmmo= inventory.submachineAmmo;
            PlayerDataSetting.Instance.PistolAmmo = inventory.pistolAmmo;
            PlayerDataSetting.Instance.Grenade = inventory.grenade;
            load.loadingScreen(map);
        }
    }
}
