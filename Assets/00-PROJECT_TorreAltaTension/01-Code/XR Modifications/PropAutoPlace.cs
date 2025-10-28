using Autohand;
using UnityEngine;

public class PropAutoPlace : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Referencia a la cámara principal.")]
    public Camera mainCamera;

    [Tooltip("Referencia al PlaceSocket donde se colocará el objeto.")]
    public PlacePoint placeSocket;

    [Header("Umbrales")]
    [Tooltip("Altura mínima respecto a la cámara para autoasignar el objeto al socket.")]
    public float minHeight = -1f;

    [Tooltip("Tiempo entre verificaciones de distancia y altura (en segundos).")]
    public float checkInterval = 0.5f;

    private Grabbable grabbable;
    private float lastCheckTime;

    private void Start()
    {
        // Obtener el componente Grabbable del objeto
        grabbable = GetComponent<Grabbable>();

        // Si no se asigna una cámara, usar la cámara principal
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Verificar que el PlaceSocket esté asignado
        if (placeSocket == null)
            Debug.LogError("PlaceSocket no está asignado en el inspector.", this);
    }
    private void OnEnable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick += FastTick;
        }
    }
    private void OnDisable()
    {
        if (TickManager.Instance != null)
        {
            TickManager.Instance.OnVeryFastTick -= FastTick;
        }
        CancelInvoke();
    }
    //private void Update()
    //{
    //    // Verificar la distancia y altura en intervalos regulares
    //    if (Time.time - lastCheckTime >= checkInterval)
    //    {
    //    }
    //}
    private void FastTick()
    {
            CheckDistanceAndHeight();
            lastCheckTime = Time.time;
    }

    private void CheckDistanceAndHeight()
    {
        // Si el objeto está siendo agarrado, no hacer nada
        if (grabbable != null && grabbable.HeldCount() > 0)
            return;

        // Calcular la distancia y altura respecto a la cámara
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 objectPosition = transform.position;

        float heightDifference = objectPosition.y - cameraPosition.y;
        //colocarlo en el socket
        if (heightDifference < minHeight)
        {            
            AutoAssignToSocket();
        }
    }

    private void AutoAssignToSocket()
    {
        if (placeSocket != null)
        {
            placeSocket.Place(grabbable);
            grabbable.isGrabbable = true;
            Debug.Log("Objeto autoasignado al socket.", this);
        }
    }
}