using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AutoGuns : MonoBehaviour
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
    public AudioSource audioSource2 ;
    
    [Header("Guns data")]
    //Lực bắn và lực hướng lên
    public float shootForce;
    //Các thông số: Thời gian giữ các phát bắng, độ rộng, thời gian nạp đạn, số lượng đạn tối đa và số lượng đạn trong 1 băng đạn
    public float timeBetweenShooting, spread, reloadTime, reloadTimeOutAmmo;
    public int magazineSize;
    public int bulletsPertap;
    public bool allowButtonHold;
    
    private int bulletsLeft, bulletsShot;
    private bool shooting, readyToShoot, reloading;
    //bug fixxing
    private bool allowInvoke = true;

    void OnEnable() {
        if(audioSource2 != null)
            AudioManager.instance.PlayAudio(audioSource2, audioSource2.clip);
    }
    
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
            animator.Play("Reload Ammo Left");
            AudioManager.instance.PlayAudio(audioSource, gunSounds[1]);
            /* audioSource.clip = gunSounds[1];
            audioSource.Play(); */
            Reload();
        }
           
        
        //Tự động nạp đạn khi hết đạn
        if(readyToShoot && !reloading && bulletsLeft <= 0 && bulletsPertap > 0) {
            animator.Play("Reload Out Of Ammo");
            AudioManager.instance.PlayAudio(audioSource, gunSounds[2]);
            /* audioSource.clip = gunSounds[2];
            audioSource.Play(); */
            Reload();
        }
            
        //Kiểm tra player có nhấn phím bấn và sẵn sàng bắn
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0) {
            Shoot();
            AudioManager.instance.PlayAudio(audioSource, gunSounds[0]);
            /* audioSource.clip = gunSounds[0];
            audioSource.Play(); */
            if(Input.GetKey(KeyCode.Mouse1))
                animator.Play("Aim Fire",0,0.0f);
            else
                animator.Play("Fire",0,0.0f);
        }

        //Reset khi tất cả đạn đã hết
        if(bulletsLeft == 0 && bulletsPertap == 0) {
            bulletsShot = 0;
        }

        //Khi nạp đạn mới sau khi đã hết toàn bộ đạn
        if(bulletsShot == 0) {
            bulletsShot = magazineSize - bulletsLeft;
        }
    }

    private void Shoot() {
        readyToShoot = false;
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f ,0.5f,0));
        RaycastHit hit;

        //Kiểm tra nếu tâm ngấm trúng gì đó
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)){
            //Điểm đến là vị trí của mục tiêu
            targetPoint = hit.point;
        }
        else
        {
            //Điểm đến là vào không khí
            targetPoint = ray.GetPoint(75);
        }

        //Tính khoản cách từ đầu ra của súng đến mục tiêu
        Vector3 direction = targetPoint - bulletSpawnPoint.position;
        
        //Tính độ lệch của đường đạn
        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);

        //Tính khoản cách giữa điểm mục tiêu và đường đạn lệch
        Vector3 directionWithSpread = direction + new Vector3(xSpread,ySpread,0);
        
        //Tạo dấu đạn và đường đạn
        GameObject currentBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation * Quaternion.Euler(90f, 0f, 0f));
        //Thêm lực đẩy cho đạn đi
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        //Thêm hiệu ứng flash
        if (muzzleFlash != null) {
            foreach(ParticleSystem childPS in childrenParticleSytems) {
                childPS.Play();
            }
        }
        
        //Trừ đi số đạn trong băng đạn
        bulletsLeft--;
        //Đếm số đạn đã bắn
        bulletsShot++;
        
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
        reloading = true;
        if(bulletsLeft == 0)
            Invoke("ReloadFinsihed", reloadTimeOutAmmo);
        else
            Invoke("ReloadFinsihed", reloadTime);
    }

    //Hoàn tất thay đạn và trừ số đạn trong băng
    private void ReloadFinsihed() {
        if(bulletsLeft > 0) {
            if(bulletsPertap + bulletsLeft >= magazineSize) {
                bulletsLeft = magazineSize;
                //bulletsPertap -=bulletsShot;
                LoseAmmo(bulletsShot);
            }
            else {
                bulletsLeft += bulletsPertap;
                //bulletsPertap = 0;
                LoseAmmo(bulletsPertap);
            }
        }
        else {
            if(bulletsPertap >= magazineSize) {
                bulletsLeft = magazineSize;
                //bulletsPertap -= magazineSize;
                LoseAmmo(magazineSize);
            }
            else {
                bulletsLeft = bulletsPertap;
                //bulletsPertap = 0;
                LoseAmmo(bulletsPertap);
            }
        }
        //data_script.rilfeAmmo = bulletsPertap;
        bulletsShot = 0;
        reloading = false;
    }

    public abstract void GetAmmo();

    public abstract void LoseAmmo(int number);

    //Lấy số đạn còn lại 
    public int GetBulletLeft() {
        return bulletsLeft;
    }

    //Đặt số lượng đạn có sãn trong súng
    public void SetBulletLeft(int amount) {
        bulletsLeft = amount;
        bulletsShot = 0;
    }
}
