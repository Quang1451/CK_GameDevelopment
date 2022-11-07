using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAndDrop : MonoBehaviour
{
    [Header("Guns throw")]
    public GameObject AKPickup;
    public GameObject M416Pickup;
    public GameObject ShotgunPickup;
    public GameObject MP5Pickup;

    [Header("Reference")]
    public Camera fpsCam;
    public PlayerData healthPlayer;
    public Transform pointThrowItem;
    [Header("Option")]
    public float distance;
    public LayerMask layerMask;

    private GunsInventory inventory;
    private bool canPickup;
    private GameObject item;
    private int timePickupGun;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = gameObject.GetComponent<GunsInventory>();
        canPickup = false;        
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();

        if(canPickup && Input.GetKeyDown(KeyCode.E) && item !=null) {
            Pickup();
        }
    }

    //Kiểm tra điểm nhấm có bắt được vật phẩm có thể nhặt hay không;
    void CheckItem() {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f ,0.5f,0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, layerMask)) {
            item = hit.transform.gameObject;
            item.GetComponent<Outline>().enabled = true;
            canPickup = true;
        }
        else if(item != null) {
            item.GetComponent<Outline>().enabled = false;
            canPickup = false;
            item = null;
        }
    }

    //Nhặt súng và các vật phẩm hỗ trợ
    void Pickup() {
        if(item.tag == "Guns") {
            GunItem gun = item.GetComponent<GunItem>();
            switch (timePickupGun) {
                case 0:
                    inventory.SetGuns(1,gun.nameObject, gun.AmmoLeft);
                    break;
                case 1:
                    inventory.SetGuns(2,gun.nameObject, gun.AmmoLeft);
                    break;
                case >=2:
                    PickAndDrop(inventory.GetSlotCurrentWeapon());
                    break;
            }
        timePickupGun++;
        }
        else {
            UseItem use = item.GetComponent<UseItem>();
            switch(use.kind) {
                case "Medkit":
                    healthPlayer.Heal(use.amount);
                    break;
                case "RifleAmmo":
                    inventory.AddRifleAmmo(use.amount);
                    break;
                case "ShotgunAmmo":
                    inventory.AddShotgunAmmo(use.amount);
                    break;
                case "SubmachineAmmo":
                    inventory.AddSubmachineAmmo(use.amount);
                    break;
                case "PistolAmmo":
                    inventory.AddPistolAmmo(use.amount);
                    break;
                case "Grenade":
                    inventory.AddGrenade(use.amount);
                    break;
            }
        }
        Destroy(item);
        item = null;
    }

    //Nhặt súng mới và ném súng cũ
    void PickAndDrop(int slot) {
        GunItem gun = item.GetComponent<GunItem>();
        switch (slot) {
            case 1:
                Drop(slot);
                inventory.SetGuns(1,gun.nameObject, gun.AmmoLeft);
                break;
            case 2:
                Drop(slot);
                inventory.SetGuns(2,gun.nameObject, gun.AmmoLeft);
                break;
        }
    }

    //Ném súng cũ
    void Drop(int slot) {
        GameObject throwGun;
        switch (inventory.GetGunName(slot)) {
            case "AK":
                throwGun = Instantiate(AKPickup, pointThrowItem.position, pointThrowItem.rotation * Quaternion.Euler(0f,0f,70f));
                throwGun.GetComponent<GunItem>().AmmoLeft = inventory.GetBulletLeft(slot);
                throwGun.GetComponent<Rigidbody>().AddForce(pointThrowItem.forward * 10, ForceMode.Impulse);
                break;
            case "M416":
                throwGun = Instantiate(M416Pickup, pointThrowItem.position, pointThrowItem.rotation * Quaternion.Euler(0f,0f,70f));
                throwGun.GetComponent<GunItem>().AmmoLeft = inventory.GetBulletLeft(slot);
                throwGun.GetComponent<Rigidbody>().AddForce(pointThrowItem.forward * 10, ForceMode.Impulse);
                break;
            case "Shotgun":
                throwGun = Instantiate(ShotgunPickup, pointThrowItem.position, pointThrowItem.rotation * Quaternion.Euler(0f,0f,70f));
                throwGun.GetComponent<GunItem>().AmmoLeft = inventory.GetBulletLeft(slot);
                throwGun.GetComponent<Rigidbody>().AddForce(pointThrowItem.forward * 10, ForceMode.Impulse);
                break;
            case "MP5":
                throwGun = Instantiate(MP5Pickup, pointThrowItem.position, pointThrowItem.rotation * Quaternion.Euler(0f,0f,70f));
                throwGun.GetComponent<GunItem>().AmmoLeft = inventory.GetBulletLeft(slot);
                throwGun.GetComponent<Rigidbody>().AddForce(pointThrowItem.forward * 10, ForceMode.Impulse);
                break;
        }
        
    }
}
