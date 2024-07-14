using UnityEngine;

public class AlignHelmet : MonoBehaviour
{
    [SerializeField] private Camera helmetCamera;
    [SerializeField] private MeshFilter visorFilter;
    [SerializeField] private MeshFilter superiorFilter;
    [SerializeField] private MeshFilter inferiorFilter;
    [SerializeField] private float HELMET_OFFSET;
    private float HELMET_X_ROTATION = -90.0f; // 90 // -180/-90
    private float HELMET_Y_ROTATION = 0.0f; // 90/0 // 90/0
    private float HELMET_Z_ROTATION = 0.0f; // 180/180 // 0/0
    private float SUPERIOR_X_ROTATION = 0.0f;
    private float SUPERIOR_Y_ROTATION = -90.0f;
    private float SUPERIOR_Z_ROTATION = 180.0f;
    private float INFERIOR_X_ROTATION = 0.0f;
    private float INFERIOR_Y_ROTATION = -90.0f;
    private float INFERIOR_Z_ROTATION = 180.0f;
    
    private void Start()
    {
        AlignPositions();
    }

    private void AlignPositions()
    {    
        if (helmetCamera == null || visorFilter == null || superiorFilter == null | inferiorFilter == null)
        {
            Debug.LogError("Please assign the target object and mesh filter in the Inspector.");
            return;
        }
        
        // Align the helmet with the camera
        transform.LookAt(helmetCamera.transform);
        // Readjust the rotation coordinates
        Vector3 visorCenter = helmetCamera.transform.position;
        Vector3 inferiorCenter = helmetCamera.transform.position;
        Vector3 superiorCenter = helmetCamera.transform.position;
        Vector3 cameraForward = helmetCamera.transform.forward;
        // targetCenter += (cameraForward);
        // targetCenter.z += VISOR_OFFSET; 
        visorCenter += (cameraForward*HELMET_OFFSET);
        visorFilter.transform.position = visorCenter;
        visorFilter.transform.rotation = Quaternion.Euler(HELMET_X_ROTATION, HELMET_Y_ROTATION, HELMET_Z_ROTATION);
        
        // Afterwards, want to update the superior and inferior parts of the visor filters
        superiorFilter.transform.rotation = Quaternion.Euler(SUPERIOR_X_ROTATION, SUPERIOR_Y_ROTATION, SUPERIOR_Z_ROTATION);
        
        inferiorFilter.transform.rotation = Quaternion.Euler(INFERIOR_X_ROTATION, INFERIOR_Y_ROTATION, INFERIOR_Z_ROTATION);
    }
}