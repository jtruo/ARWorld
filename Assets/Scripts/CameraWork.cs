// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraWork.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the Camera work to follow the player
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;

/// <summary>
/// Camera work for following players/targets  in the scene.
/// </summary>
public class CameraWork : MonoBehaviour
{


    #region Private Fields

    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("The height of the camera above the target")]
    [SerializeField]
    private float height = 3.0f;


    [Tooltip("Allow the camera to be offsetted vertically from the target")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed. Typically true on a non-networked environment")]
    [SerializeField]
    private bool followOnStart = false;

    [Tooltip("Smoothing for the camera when following the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    // Cached transform of the target
    Transform cameraTransform;

    // A flag maintained internally to reconnect if the target is lost or camera is switched
    bool isFollowing;

    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;

    #endregion

    #region MonoBehaviour Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }

    void LateUpdate()
    {
        // The transform target may not destroy on level load,
        // so this covers the corner cases when the Main Camera different,
        // everytime a new scene is loaded, and reconnect when that happens.
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        // Follow when it's explicity declared
        if (isFollowing)
        {
            Follow();
        }
        
    }


    #endregion


    #region Public Methods

    /// <summary>
    /// Use this when you don't know at the time of editing what to follow,
    /// typically instances managed by the photon network
    /// </summary>
    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;

        Cut();

    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Follow the target smoothly
    /// </summary>
    void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position,
            this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed*Time.deltaTime);

        cameraTransform.LookAt(this.transform.position + centerOffset);
    }

    // Goes right to the player view with the camera
    void Cut()
    {

        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        cameraTransform.LookAt(this.transform.position + centerOffset);
    }

    #endregion


}
