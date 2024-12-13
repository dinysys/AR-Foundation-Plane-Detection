using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

public class CreateUlat : MonoBehaviour
{
    [Header("Referensi Objek")]
    //public GameObject penandaBidang;  
    public GameObject objek3D;       

    [Header("Komponen AR")]
    public ARRaycastManager manajerRaycast;

    private List<ARRaycastHit> hasilRaycast = new List<ARRaycastHit>();
    private Camera arCamera;
    private bool createBangun;

    private void Start()
    {
        arCamera = Camera.main;

        //if (penandaBidang != null)
        //    penandaBidang.SetActive(false);
    }

    private void Update()
    {
        Vector2 layarTengah = new Vector2(Screen.width / 2, Screen.height / 2);

        if (manajerRaycast.Raycast(layarTengah, hasilRaycast, TrackableType.PlaneWithinPolygon))
        {
            Pose poseBidang = hasilRaycast[0].pose;
            //if (penandaBidang != null)
            //{
            //    penandaBidang.SetActive(true);
            //    penandaBidang.transform.position = poseBidang.position;
            //    penandaBidang.transform.rotation = poseBidang.rotation;
            //}
        }
        //else
        //{
        //    if (penandaBidang != null)
        //        penandaBidang.SetActive(false);
        //}
    }

    public void OnTouchInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (createBangun == false)
            {
                Vector2 posisiSentuh = Pointer.current.position.ReadValue();
                if (manajerRaycast.Raycast(posisiSentuh, hasilRaycast, TrackableType.PlaneWithinPolygon))
                {
                    Pose poseKetukan = hasilRaycast[0].pose;
                    Instantiate(objek3D, poseKetukan.position, poseKetukan.rotation);
                }
                createBangun = true;
            }

        }
    }
}
