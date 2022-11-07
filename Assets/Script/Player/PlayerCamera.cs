using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{   
    public GameObject obj;
    private PlayerData dataPlayer;
    public Transform playerbody;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        dataPlayer = obj.GetComponent<PlayerData>();
        //Đảm bảo con trỏ chuột bị khóa ở giữa màn hình
        Cursor.lockState = CursorLockMode.Locked;
        //Không hiển thị con trỏ chuột
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Lấy dữ liệu di chuyển chột x cho ngang và y cho dọc
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * dataPlayer.sensX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * dataPlayer.sensY;

        xRotation -= mouseY;
       
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
        playerbody.Rotate(Vector3.up * mouseX);
    }
}
