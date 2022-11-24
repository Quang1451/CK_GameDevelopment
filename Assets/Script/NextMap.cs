using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NextMap : MonoBehaviour
{   
    
    [SerializeField] private PlayerData dataPlayer;
    [SerializeField] private LoadingScreenBarSystem load;
    [SerializeField] private GunsInventory inventory;
    [SerializeField] private PickupAndDrop pickAndDrop;
    [SerializeField] private int map;
    
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.name == "Player") {
            PlayerDataSetting.Instance.Health = dataPlayer.GetHealth();
            PlayerDataSetting.Instance.RifleAmmo = inventory.rifleAmmo;
            PlayerDataSetting.Instance.ShotgunAmmo = inventory.shotgunAmmo;
            PlayerDataSetting.Instance.SubmachineAmmo= inventory.submachineAmmo;
            PlayerDataSetting.Instance.PistolAmmo = inventory.pistolAmmo;
            PlayerDataSetting.Instance.Grenade = inventory.grenade;
            PlayerDataSetting.Instance.timePickupGun = pickAndDrop.GetTimePickup();
            
            PlayerDataSetting.Instance.Map = map;

            var path = "Guns/";
            if(inventory.GetAllGuns()[1]) {
                PlayerDataSetting.Instance.GunsHaving[1] = Resources.Load<GameObject>(path+inventory.GetAllGuns()[1].name);
            }
            if(inventory.GetAllGuns()[2]) {
                PlayerDataSetting.Instance.GunsHaving[2] = Resources.Load<GameObject>(path+inventory.GetAllGuns()[2].name);
            }
            
            SaveData save = new SaveData();
            save.Health = dataPlayer.GetHealth();
            save.RifleAmmo = inventory.rifleAmmo;
            save.ShotgunAmmo = inventory.shotgunAmmo;
            save.SubmachineAmmo= inventory.submachineAmmo;
            save.PistolAmmo = inventory.pistolAmmo;
            save.Grenade = inventory.grenade;
            save.timePickupGun = pickAndDrop.GetTimePickup();
            save.Map = map;
            save.GunsHaving = PlayerDataSetting.Instance.GunsHaving;
            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(Application.dataPath+"/Player.json",json);

            load.loadingScreen(map);
        }
    }
}
