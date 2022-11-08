using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField]
    private Animator animator;

    [Header("Reference")]
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject flashlight;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    public LayerMask groundMask;

    private CharacterController controller;
    private PlayerData dataPlayer;
    private Vector3 velogity;
    private bool isGrounded;
    private float speed;
    private AudioSource moveSounds;
    private float xRotation, yRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        dataPlayer = gameObject.GetComponent<PlayerData>();
        speed = dataPlayer.moveSpeed;
        moveSounds = GetComponent<AudioSource>();

        //Đảm bảo con trỏ chuột bị khóa ở giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
        //Không hiển thị con trỏ chuột
        Cursor.visible = false;
    }

    void Update()
    {   
        RotateCamera();
        Flashlight();
        GameObject inventory = GameObject.Find("GunsInventory");
        for(int i = 0; i < inventory.transform.childCount; i++ ){
            if(inventory.transform.GetChild(i).gameObject.activeSelf == true) {
                animator = inventory.transform.GetChild(i).GetComponent<Animator>();
                break;
            }
        }
    }

    void FixedUpdate()
    {      
        Movement();
        SitAndStand();
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
            if(!moveSounds.isPlaying)
                moveSounds.Play();
        }
        else {
            if(animator.GetBool("Walk") == true)
                animator.SetBool("Walk", false);
            moveSounds.Stop();
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
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            controller.height = dataPlayer.normalHeight;
            speed = dataPlayer.moveSpeed;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
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
}
