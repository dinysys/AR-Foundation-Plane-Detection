using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DeteksiBidangAR : MonoBehaviour
{
    [Header("Referensi Objek 3D")]
    public GameObject kubusPrefab;
    public GameObject balokPrefab;
    public GameObject bolaPrefab;
    public Text cek;

    [Header("Komponen AR")]
    public ARRaycastManager manajerRaycast;

    private List<ARRaycastHit> hasilRaycast = new List<ARRaycastHit>();
    private GameObject objekAktif;
    private bool bidangTerdeteksi = false;

    private Vector3 posisiTerakhir;
    private float derajatTerakhir = 0f;

    void Update()
    {
        if (bidangTerdeteksi) return;

        // Deteksi bidang pertama kali
        Vector2 layarTengah = new Vector2(Screen.width / 2, Screen.height / 2);

        if (manajerRaycast.Raycast(layarTengah, hasilRaycast, TrackableType.PlaneWithinPolygon))
        {
            Pose poseBidang = hasilRaycast[0].pose;

            // Set posisi awal
            posisiTerakhir = poseBidang.position;

            // Tampilkan kubus sebagai objek awal
            objekAktif = kubusPrefab;
            SetObjekAktif(kubusPrefab, true);

            bidangTerdeteksi = true;
            cek.text = "Bidang terdeteksi, Kubus muncul!";
        }
    }

    // Fungsi untuk mengubah bentuk objek
    public void UbahBentuk(string bentuk)
    {
        if (!bidangTerdeteksi) return;

        // Nonaktifkan semua objek
        kubusPrefab.SetActive(false);
        balokPrefab.SetActive(false);
        bolaPrefab.SetActive(false);

        // Reset rotasi terakhir
        derajatTerakhir = 0f;

        switch (bentuk)
        {
            case "Balok":
                objekAktif = balokPrefab;
                break;
            case "Kubus":
                objekAktif = kubusPrefab;
                break;
            case "Bola":
                objekAktif = bolaPrefab;
                break;
            default:
                cek.text = "Bentuk tidak dikenali!";
                return;
        }

        // Set posisi dan reset rotasi saat bentuk diubah
        SetObjekAktif(objekAktif, true);
        cek.text = "Bentuk berubah menjadi: " + bentuk;
    }

    // Fungsi untuk mengaktifkan objek dan set posisinya
    private void SetObjekAktif(GameObject objek, bool resetRotasi)
    {
        if (objek != null)
        {
            objek.transform.position = posisiTerakhir;

            // Reset rotasi jika diminta
            if (resetRotasi)
                objek.transform.rotation = Quaternion.Euler(0, 0, 0);

            objek.SetActive(true);
        }
    }

    // Fungsi untuk memutar objek
    public void RotasiObjek(float derajat)
    {
        if (objekAktif != null)
        {
            objekAktif.transform.Rotate(Vector3.up, derajat);
            derajatTerakhir += derajat;
            if (derajatTerakhir == 270f)
            {
                derajatTerakhir = 0;
            }
            cek.text = $"Objek diputar sebesar {derajatTerakhir} derajat";
        }
    }
}
