using Unity.Cinemachine;
using UnityEditor.Search;
using UnityEngine;

public class CamZooms : MonoBehaviour
{
    
    [SerializeField]
    public CinemachineFreeLook freeLookCam;

    public CamZooms camZooms;

    public float zoomedOutFOV = 70f;
    public float zoomspeed = 5f;

    private float normalFOV;
    private bool zoomingOut = false;


    
    void Start()
    {
        if (freeLookCam != null)
        {
            freeLookCam = GetComponent<CinemachineFreeLook>();
            normalFOV = freeLookCam.m_Lens.FieldOfView;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float targetFOV = zoomingOut ? zoomedOutFOV : normalFOV;

        freeLookCam.m_Lens.FieldOfView = Mathf.Lerp(freeLookCam.m_Lens.FieldOfView, targetFOV, Time.deltaTime * zoomspeed);
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

