using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class CamZooms : MonoBehaviour
{

    public CinemachineFreeLook freeLookCam;

    

    public float zoomedOutFOV = 70f;
    public float zoomspeed = 5f;

    private float normalFOV;
    private bool zoomingOut = false;


    
    void Start()
    {
        if (freeLookCam == null)
        {
            freeLookCam = GetComponent<CinemachineFreeLook>();
        }

        if (freeLookCam != null)
        {
            normalFOV = freeLookCam.m_Lens.FieldOfView;

        }
    }
    void Update()
    {
        if (freeLookCam == null) return;

        float targetFOV = zoomingOut ? zoomedOutFOV : normalFOV;

        var lens = freeLookCam.m_Lens;
        lens.FieldOfView = Mathf.Lerp(lens.FieldOfView, targetFOV, Time.deltaTime * zoomspeed);
        freeLookCam.m_Lens = lens;

        if(zoomingOut == true)
        {
            normalFOV = zoomedOutFOV;

            Debug.Log("Zoomed Out");
        }
    }
        

    public void ToggleZoomOut()
    {
        zoomingOut = true;
    }

    public void ToggleZoomIn()
    {
        zoomingOut = false;
    }
}

