using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shotgun : MonoBehaviour
{
    [Header("Animation")]
    //Animator
    private Animator animator;
    
    [Header("Reference")]
    public Camera fpsCam;
    public Transform bulletSpawnPoint;
    public GunsInventory data_script;
    //Đạn
    public GameObject bullet;

    [Header("Graphic")]
    //Graphíc
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    ParticleSystem[] childrenParticleSytems;

    [Header("Audio")]
    public AudioClip[] gunSounds;
	public AudioSource audioSource;
    public AudioSource audioSourceReadyToShot;
    
    [Header("Guns data")]
    //Lực bắn và lực hướng lên
    public float shootForce;
    //Các thông số: Thời gian giữ các phát bắng, độ rộng, thời gian nạp đạn, số lượng đạn tối đa và số lượng đạn trong 1 băng đạn
    public float timeBetweenShooting, spread, reloadTimeOpen, reloadTimeInsert;
    public int magazineSize;
    public int bulletsPertap;
    public bool allowButtonHold;
    public int pellets;
    
    private int bulletsLeft;
    private bool shooting, readyToShoot, reloading;
    //bug fixxing
    private bool allowInvoke = true;
    
    public void Awake() {
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        childrenParticleSytems = muzzleFlash.GetComponentsInChildren<ParticleSystem>();
    }

    public void Update() {  
        //Lấy số đạn tỏng dữ liệu người chơi
        GetAmmo();
        
        GunsInput();

        //Hiển thị số lượng đạn trên màng hinh
        if(ammunitionDisplay != null) {
            ammunitionDisplay.SetText(bulletsLeft + " / " + bulletsPertap);
        }
    }    

    private void GunsInput() {
        //Kiểm tra cho phép player được nhấn giử phím bắn hay không
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Kiểm tra player có nhấn chuột phải không
        animator.SetBool("Aim", Input.GetKey(KeyCode.Mouse1));
        
        //Kiểm tra player bấm nut nạp đạn
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && bulletsPertap > 0){
            reloading = true;
            animator.SetBool("Finish",false);
            animator.Play("Reload Open");
            AudioManager.Instance.PlayAudio(audioSource, gunSounds[1]);
            /* audioSource.clip = 
            audioSource.Play(); */
            Invoke("Reload", reloadTimeOpen);
        }
           
        
        //Tự động nạp đạn khi hết đạn
        if(readyToShoot && !reloading && bulletsLeft <= 0 && bulletsPertap > 0) {
            reloading = true;
            animator.SetBool("Finish",false);
            animator.Play("Reload Open");
            AudioManager.Instance.PlayAudio(audioSource, gunSounds[1]);
            /* audioSource.clip = gunSounds[1];
            audioSource.Play(); */
            Invoke("Reload", reloadTimeOpen);
        }
            
        //Kiểm tra player có nhấn phím bấn và sẵn sàng bắn
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            Shoot();
            AudioManager.Instance.PlayAudio(audioSource, gunSounds[0]);
            /* audioSource.clip = gunSounds[0];
            audioSource.Play(); */
            if(Input.GetKey(KeyCode.Mouse1))
                animator.Play("Aim Fire",0,0.0f);
            else
                animator.Play("Fire",0,0.0f);

            AudioManager.Instance.PlayAudio(audioSourceReadyToShot, gunSounds[3]);
            /* audioSourceReadyToShot.clip = gunSounds[3];
            audioSourceReadyToShot.Play(); */
        }
    }

    private void Shoot() {
        readyToShoot = false;
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f ,0.5f,0));

        //Kiểm tra nếu tâm ngấm trúng gì đó
        Vector3 targetPoint;
        //Điểm đến là vào không khí
        targetPoint = ray.GetPoint(10);
        
        //Tính khoản cách từ đầu ra của súng đến mục tiêu
        Vector3 direction = targetPoint - bulletSpawnPoint.position;
        for (int i = 1; i <= pellets; i++) {
            //Tính độ lệch của đường đạn
            Vector2 randomPoint = Random.insideUnitCircle * spread;

            //Tính khoản cách giữa điểm mục tiêu và đường đạn lệch
            Vector3 directionWithSpread = direction + new Vector3(0,randomPoint.x,randomPoint.y);

            //Tạo dấu đạn và đường đạn
            GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
            //Thêm lực đẩy cho đạn đi
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        }
        
        //Thêm hiệu ứng flash
        if (muzzleFlash != null) {
            foreach(ParticleSystem childPS in childrenParticleSytems) {
                childPS.Play();
            }
        }
        
        //Trừ đi số đạn trong băng đạn
        bulletsLeft--;
        
        //Timer nlại function resetShot (nếu chưa gọi)
        if(allowInvoke) {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot() {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() {
        animator.Play("Insert");
        AudioManager.Instance.PlayAudio(audioSource, gunSounds[2]);
        /* audioSource.clip = gunSounds[2];
        audioSource.Play(); */
        Invoke("ReloadFinsihed", 0.55f);
    }

    //Hoàn tất thay đạn và trừ số đạn trong băng
    private void ReloadFinsihed() {
        bulletsLeft +=1;
        LoseAmmo(1);
        
        GetAmmo();
        if (bulletsPertap ==0 || bulletsLeft == magazineSize || Input.GetKey(KeyCode.Mouse0)) {
            animator.SetBool("Finish",true);
            AudioManager.Instance.PlayAudio(audioSource, gunSounds[3]);
            /* audioSource.clip = gunSounds[3];
            audioSource.Play(); */
            Invoke("ReloadAllFinish",1);
        }
        else { 
            Reload();
        }
    }
    
    public void ReloadAllFinish() {
        reloading = false;
    }

    public void GetAmmo() {
        bulletsPertap = data_script.GetShotgunAmmo();
    }

    public void LoseAmmo(int number) {
        data_script.LoseShotgunAmmo(number);
    }

    //Lấy số đạn còn lại 
    public int GetBulletLeft() {
        return bulletsLeft;
    }

    //Đặt số lượng đạn có sãn trong súng
    public void SetBulletLeft(int amount) {
        bulletsLeft = amount;
    }
}
