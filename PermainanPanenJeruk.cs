using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using System.Runtime.ConstrainedExecution;

public class PermainanPanenJeruk : MonoBehaviour
{
    public ARRaycastManager manajerRaycast;
    public GameObject pohonJeruk;
    public GameObject[] jerukMatang;
    public GameObject[] jerukMentah;
    public Text teksPeringatan;
    public AudioSource panen, warning;

    private Vector3 posisiTerakhir;
    private List<Vector3> posisiJerukTerpakai = new List<Vector3>();
    private List<ARRaycastHit> hasilRaycast = new List<ARRaycastHit>();
    private bool bidangTerdeteksi = false;
    private int skor = 0;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnTouch(InputValue value)
    {
        Vector2 posisiSentuh = value.Get<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(posisiSentuh);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject jerukTerdekat = hit.collider.gameObject;
            PeriksaJeruk(jerukTerdekat);
        }
    }

    void Update()
    {
        if (bidangTerdeteksi) return;
        Vector2 layarTengah = new Vector2(Screen.width / 2, Screen.height / 2);

        if (manajerRaycast.Raycast(layarTengah, hasilRaycast, TrackableType.PlaneWithinPolygon))
        {
            Pose poseBidang = hasilRaycast[0].pose;

            posisiTerakhir = poseBidang.position;
            pohonJeruk.transform.position = posisiTerakhir;
            pohonJeruk.SetActive(true);

            bidangTerdeteksi = true;
            teksPeringatan.text = "Pohon muncul";
        }
    }

    void PeriksaJeruk(GameObject jerukTerdekat)
    {
        if (System.Array.Exists(jerukMatang, jeruk => jeruk.Equals(jerukTerdekat)))
        {
            skor++;
            teksPeringatan.text = "Berhasil dipanen! Skor: " + skor;
            panen.Play();
            jerukTerdekat.SetActive(false);
            Invoke("HapusPeringatan", 5f);
        }
        else if (System.Array.Exists(jerukMentah, jeruk => jeruk.Equals(jerukTerdekat)))
        {
            teksPeringatan.text = "Jeruk tidak boleh dipanen!";
            warning.Play();
            Invoke("HapusPeringatan", 5f);
        }
        else
        {
            teksPeringatan.text = "Bukan jeruk yang valid!";
            warning.Play();
            Invoke("HapusPeringatan", 5f);
        }
    }

    void HapusPeringatan()
    {
        teksPeringatan.text = "Cari Jeruk..";
    }
}
