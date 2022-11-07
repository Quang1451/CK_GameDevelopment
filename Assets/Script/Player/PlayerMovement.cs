using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [Header("Animation")]
    public Animator animator;

    [Header("Reference")]
    public GameObject obj;
    public CharacterController controller;
    private PlayerData dataPlayer;
    public Transform groundCheck;
    public LayerMask groundMask;

    Vector3 velogity;
    bool isGrounded;
    float speed;
    AudioSource moveSounds;
    
    // Start is called before the first frame update
    void Start()
    {
        dataPlayer = obj.GetComponent<PlayerData>();
        speed = dataPlayer.moveSpeed;
        moveSounds = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameObject inventory = GameObject.Find("GunsInventory");
        for(int i = 0; i < inventory.transform.childCount; i++ ){
            if(inventory.transform.GetChild(i).gameObject.activeSelf == true) {
                animator = inventory.transform.GetChild(i).GetComponent<Animator>();
                break;
            }
        }
        if(Input.GetKeyUp(KeyCode.LeftShift)){
            controller.height = dataPlayer.normalHeight;
            speed = dataPlayer.moveSpeed;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            controller.height = dataPlayer.crouchHeight;
            speed = speed / 4;
        }
    }

    void FixedUpdate()
    {
        //Tạo ra một hình cầu có tâm là vị trí của groundCheck và bán kính là groundDistance 
        isGrounded = Physics.CheckSphere(groundCheck.position, dataPlayer.groundDistance, groundMask);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Kiểm tra xem player có chạm đất hay không và gia tốc có nhỏ hơn 0 hay không
        if (isGrounded && velogity.y < 0) {
            velogity.y = -2f;
        }
        
        //Tính tốc độ duy chuy?n 
        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        controller.Move(moveDirection * speed * Time.deltaTime);
        
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            animator.SetBool("Walk", true);   
            if(!moveSounds.isPlaying)
                moveSounds.Play();
        }
        else {
            if(animator.GetBool("Walk") == true)
                animator.SetBool("Walk", false);
            moveSounds.Stop();
        }

        //Kiểm tra người choi có nhấn phím cách và có chạm đất hay không
        if(Input.GetButton("Jump") && isGrounded) {
            //Tính vận tốc nhảy bằng công thúc sqrt(h * -2 * g), h là chiều cao muốn nhảy, g là trọng lực
            velogity.y = Mathf.Sqrt(dataPlayer.jumgHeight * -2f * dataPlayer.gravity);
        }       

        //Tính tốc độ rơi
        velogity.y += dataPlayer.gravity * Time.deltaTime;
        controller.Move(velogity * Time.deltaTime);     
    }
}
