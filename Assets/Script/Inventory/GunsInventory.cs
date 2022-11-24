using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunsInventory : MonoBehaviour
{
    [Header("Graphic")]
    public TextMeshProUGUI rifleAmmoDisplay;
    public TextMeshProUGUI shotgunAmmoDisplay;
    public TextMeshProUGUI submachineAmmoDisplay;
    public TextMeshProUGUI pistolAmmoDisplay;
    public TextMeshProUGUI grenadeDisplay;

    private GameObject[] gunsHaving;
    private int previousWeapon;
    private int currentWeapon;
    public int rifleAmmo, shotgunAmmo, submachineAmmo, pistolAmmo, grenade;
    void Awake()
    {
        rifleAmmo = PlayerDataSetting.Instance.RifleAmmo;
        shotgunAmmo = PlayerDataSetting.Instance.ShotgunAmmo;
        submachineAmmo = PlayerDataSetting.Instance.SubmachineAmmo;
        pistolAmmo = PlayerDataSetting.Instance.PistolAmmo;
        grenade = PlayerDataSetting.Instance.Grenade;
        //Đặt súng lục làm súng mặc định
        gunsHaving = PlayerDataSetting.Instance.GunsHaving;
        
        currentWeapon = 0;

        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {   
        SelectInput();
        ShowAmmoDisplay();
    }

    //Hiển thị súng được chọn
    void SelectWeapon() {
        foreach (Transform child in transform){
            if(child.gameObject.name == gunsHaving[currentWeapon].name)
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
    }

    void SelectInput() {
        int oldWeapon = currentWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && gunsHaving[1] != null) {
            if(currentWeapon >= 2 || (gunsHaving[2] == null && currentWeapon >= 1))
                currentWeapon = 0;
            else
                currentWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && gunsHaving[1] != null) {
            if(currentWeapon <= 0)
                if(gunsHaving[2] == null)
                    currentWeapon = 1;
                else
                    currentWeapon = 2;
            else
                currentWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && gunsHaving[1] != null) {
            currentWeapon = 1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2) && gunsHaving[2] != null) {
            currentWeapon = 2;
        }

        if(Input.GetKeyDown(KeyCode.Alpha3)) {
            currentWeapon = 0;
        }

        if(oldWeapon != currentWeapon) {
            previousWeapon = oldWeapon;
            SelectWeapon();
        }
    }

    void ShowAmmoDisplay(){
        if(rifleAmmoDisplay != null)    
            rifleAmmoDisplay.SetText(rifleAmmo.ToString());
        if(shotgunAmmoDisplay != null)    
            shotgunAmmoDisplay.SetText(shotgunAmmo.ToString());
        if(submachineAmmoDisplay != null)    
            submachineAmmoDisplay.SetText(submachineAmmo.ToString());
        if(pistolAmmoDisplay != null)    
            pistolAmmoDisplay.SetText(pistolAmmo.ToString());
        if(grenadeDisplay != null)    
            grenadeDisplay.SetText(grenade.ToString());
    }

    public void CheckAmmo() {
        if(rifleAmmo <= 0)
            rifleAmmo = 0;
        if(shotgunAmmo <= 0)
            shotgunAmmo = 0;
        if(submachineAmmo <= 0)
            submachineAmmo = 0;
        if(pistolAmmo <= 0)
            pistolAmmo = 0;
        if(grenade <= 0)
            grenade = 0;
    }

    //Các phương thức thay đổi số lượng đạn rifle
    public void LoseRifleAmmo(int number) {
        rifleAmmo -= number;
        CheckAmmo();
    }

    public void AddRifleAmmo(int number) {
        rifleAmmo += number;
    }

    public int GetRifleAmmo() {
        return rifleAmmo;
    }

    //Các phương thức thay đổi số lượng đạn Shotgun
    public void LoseShotgunAmmo(int number) {
        shotgunAmmo -= number;
        CheckAmmo();
    }

    public void AddShotgunAmmo(int number) {
        shotgunAmmo += number;
    }

    public int GetShotgunAmmo() {
        return shotgunAmmo;
    }

    //Các phương thức thay đổi số lượng đạn Submachine
    public void LoseSubmachineAmmo(int number) {
        submachineAmmo -= number;
        CheckAmmo();
    }

    public void AddSubmachineAmmo(int number) {
        submachineAmmo += number;
    }

    public int GetSubmachineAmmo() {
        return submachineAmmo;
    }

    //Các phương thức thay đổi số lượng đạn Pistol
    public void LosePistolAmmo(int number) {
        pistolAmmo -= number;
        CheckAmmo();
    }

    public void AddPistolAmmo(int number) {
        pistolAmmo += number;
    }

    public int GetPistolAmmo() {
        return pistolAmmo;
    }

    //Các phương thức thay đổi số lượng grenade
    public void LoseGrenade() {
        grenade -= 1;
        CheckAmmo();
    }

    public void AddGrenade(int number) {
        grenade += number;
    }

    public bool CheckHasGrenade() {
        if(grenade > 0)
            return true;
        return false;
    }

    public int GetSlotCurrentWeapon() {
        if (currentWeapon == 0)
            return previousWeapon;
        return currentWeapon;
    }

    //Cho súng vào slot
    public void SetGuns(int slot, string name, int amount) {
        gunsHaving[slot] = transform.Find(name).gameObject;
        if (name == "Shotgun") {
            gunsHaving[slot].GetComponent<Shotgun>().SetBulletLeft(amount);
        }
        else {
            gunsHaving[slot].GetComponent<AutoGuns>().SetBulletLeft(amount);
        }
        previousWeapon = currentWeapon;
        currentWeapon = slot;
        SelectWeapon();
    }

    //Lấy tên của súng đang dùng
    public string GetGunName(int slot) {
        return gunsHaving[slot].name;
    }

    //Lấy số đạn còn lại trong súng hiện tại 
    public int GetBulletLeft(int slot) {
        if (gunsHaving[slot].name == "Shotgun") {
            return gunsHaving[slot].GetComponent<Shotgun>().GetBulletLeft();
        }
        return gunsHaving[slot].GetComponent<AutoGuns>().GetBulletLeft();
    }

    public GameObject[] GetAllGuns() {
        return gunsHaving;
    }
}
