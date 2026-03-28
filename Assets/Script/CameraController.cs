using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCamera;
    private CinemachineInputAxisController inputController;

    //void Start()
    //{
    //    // 마우스 커서를 숨기고 중앙에 고정
    //    Cursor.lockState = CursorLockMode.Locked;
    //    inputController = virtualCamera.GetComponent<CinemachineInputAxisController>();
    //}

    //void Update()
    //{
    //    // 마우스 우클릭(1)을 누르고 있을 때만 시네머신 입력 활성화
    //    if (Input.GetMouseButton(1))
    //    {
    //        inputController.enabled = true;
    //        Cursor.lockState = CursorLockMode.Locked; // 커서 고정
    //        Cursor.visible = false;
    //    }
    //    else
    //    {
    //        inputController.enabled = false;
    //        Cursor.lockState = CursorLockMode.None; // 커서 해제
    //        Cursor.visible = true;
    //    }
    //}
}
