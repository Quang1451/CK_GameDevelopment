using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [Header("Pause UI")]
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject optionUI;
    [Header("Reference")]
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject flashlight;
    [SerializeField]
    private GunsInventory inventory;
    [SerializeField]
    private GameObject throwPoint;
    [SerializeField]
    private GameObject knifeAttackPoint;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float timeResetThorw, throwForce;
    [SerializeField]
    private float timeResetAttack1, timeResetAttack2, timeInCombo;


    [Header("Prefab Object")]
    [SerializeField]
    private GameObject grenade;

    private CharacterController controller;
    private PlayerData dataPlayer;
    private Vector3 velogity;
    private bool isGrounded;
    private float speed;
    private AudioSource moveSource, throwSourse, knifeSource;
    private float xRotation, yRotation;
    private float _resetThrow, _resetAttack, _resetCombo;
    
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        dataPlayer = GetComponent<PlayerData>();
        moveSource = GetComponent<AudioSource>();
        speed = dataPlayer.moveSpeed;

        //Lấy nguồn âm thanh của điểm ném lựu đạn     
        if(throwPoint)
            throwSourse = throwPoint.GetComponent<AudioSource>();
        
        if(knifeAttackPoint)
            knifeSource = knifeAttackPoint.GetComponent<AudioSource>();

        _resetAttack = _resetCombo = _resetThrow = Time.time;

        //Đảm bảo con trỏ chuột bị khóa ở giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
        //Không hiển thị con trỏ chuột
        Cursor.visible = false;
        Resume();
    }

    void Update()
    {   
        CheckGunAnimation();
        RotateCamera();
        Flashlight();
        KnifeAttack();
        ThrowGrenade();
        SitAndStand();
        CheckPause();
    }

    void FixedUpdate()
    {      
        Movement();
        Jump();     
    }

    void RotateCamera() {
        //Lấy dữ liệu di chuyển chột
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * dataPlayer.sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * dataPlayer.sensY;

        xRotation -= mouseY;
       
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Movement() {
        //Tạo ra một hình cầu có tâm là vị trí của groundCheck và bán kính là groundDistance 
        isGrounded = Physics.CheckSphere(groundCheck.position, dataPlayer.groundDistance, groundMask);

        //Kiểm tra xem player có chạm đất hay không và gia tốc có nhỏ hơn 0 hay không
        if (isGrounded && velogity.y < 0) {
            velogity.y = -2f;
        }

        //Lấy giá trị duy chuyển từ các nút bấm
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Chạy animation di chuyển
        if(horizontalInput!= 0 || verticalInput!=0) {
            animator.SetBool("Walk", true);   
            if(!moveSource.isPlaying)
                moveSource.Play();
        }
        else {
            if(animator.GetBool("Walk") == true)
                animator.SetBool("Walk", false);
            moveSource.Stop();
        }

        //Tính tốc độ duy chuyển 
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        controller.Move(moveDirection * speed * Time.deltaTime);
        
    }

    void Jump() {
         //Kiểm tra người choi có nhấn phím cách và có chạm đất hay không
        if(Input.GetButton("Jump") && isGrounded) {
        //Tính vận tốc nhảy bằng công thúc sqrt(h * -2 * g), h là chiều cao muốn nhảy, g là trọng lực
            velogity.y = Mathf.Sqrt(dataPlayer.jumgHeight * -2f * dataPlayer.gravity);
        }       
        //Tính tốc độ rơi
        velogity.y += dataPlayer.gravity * Time.deltaTime;
        controller.Move(velogity * Time.deltaTime);
    }

    void SitAndStand() {
        //Thay đổi tốc độ đứng và ngồi
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            controller.height = dataPlayer.normalHeight;
            speed = dataPlayer.moveSpeed;
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            controller.height = dataPlayer.crouchHeight;
            speed = speed / 4;
        }
    }

    void Flashlight() {
        //Bật tắt đèn
        if(Input.GetKeyDown(KeyCode.T)) {
            flashlight.SetActive(!flashlight.active);
        }
    }

    void CheckGunAnimation() {
        GameObject inventory = GameObject.Find("GunsInventory");
        for(int i = 0; i < inventory.transform.childCount; i++ ){
            if(inventory.transform.GetChild(i).gameObject.activeSelf == true) {
                animator = inventory.transform.GetChild(i).GetComponent<Animator>();
                break;
            }
        }
    }

    void KnifeAttack() {
        if(Input.GetKeyDown(KeyCode.F) && Time.time > _resetAttack) {
            if(Time.time < _resetCombo) {
                animator.Play("Knife Attack 1");
                AudioManager.instance.PlayAudio(knifeSource, dataPlayer.knifeSounds[1]);
                
                _resetAttack = Time.time + timeResetAttack2;
            }
            else{
                animator.Play("Knife Attack 2");
                AudioManager.instance.PlayAudio(knifeSource, dataPlayer.knifeSounds[0]);
                
                _resetCombo = Time.time + timeInCombo;
                _resetAttack = Time.time + timeResetAttack1;
            }
            Collider[] hit = Physics.OverlapSphere(knifeAttackPoint.transform.position,0.15f);
            foreach(Collider col in hit) {
                if(col.gameObject.tag == "Enemy") {
                    EnemyHealth enemy = col.GetComponent<EnemyHealth>();
                    enemy.LoseHeal(Random.Range(40,70));
                }
            }
        }
    }
    
    void ThrowGrenade() {
        if (Input.GetKeyDown(KeyCode.G) && Time.time > _resetThrow && inventory.CheckHasGrenade()) {
            AudioManager.instance.PlayAudio(throwSourse, (dataPlayer.throwSounds)[Random.Range(0,(dataPlayer.throwSounds).Length)]);
          
            Invoke("Throw",0.5f);
            _resetThrow = Time.time + timeResetThorw;
        } 
    }

    void Throw() {
        animator.Play("Grenade Throw");
        GameObject currentGrenade = Instantiate(grenade, throwPoint.transform.position, throwPoint.transform.rotation);
        currentGrenade.GetComponent<Rigidbody>().AddForce(throwPoint.transform.forward * throwForce, ForceMode.VelocityChange);
        inventory.LoseGrenade();
    }

    void CheckPause() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Time.timeScale > 0){
                Pause();
            }
            else {
                Resume();
            }
        }
    }

    public void Pause() {
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume() {
        if(!optionUI.activeSelf) {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            optionUI.SetActive(false);
        }
    }
}
