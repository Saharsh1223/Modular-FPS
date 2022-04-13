//Basic player look

using UnityEngine;

public class Look : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform orientation;

    [Header("Look Settings")]
    [SerializeField] private float sensX = 10f;
    [SerializeField] private float sensY = 10f;

    private float y;
    private float x;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * 0.1f;
        float mouseY = Input.GetAxisRaw("Mouse Y") * 0.1f;

        y += mouseX * sensX;
        x -= mouseY * sensY;

        x = Mathf.Clamp(x, -90f, 90f);

        orientation.rotation = Quaternion.Euler(0f, y, 0f);
        cameraHolder.localRotation = Quaternion.Euler(x, y, 0f);

        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(Input.GetMouseButtonDown(0) && Cursor.visible == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}