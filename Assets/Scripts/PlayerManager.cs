using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;



namespace Com.Augment.ARWorld
{

    /// <summary>
    /// Player manager.
    /// Handles fire input and beams.
    /// </summary>
    public class PlayerManager : MonoBehaviourPunCallbacks
    {



        #region Private Fields

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;

        bool IsFiring;

        #endregion

        #region Public Fields

        [Tooltip("The current Health of the player")]
        public float Health = 1f;

        #endregion

        #region MonoBehaviour Callbacks

        void Awake()
        {
            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            } else
            {
                beams.SetActive(false);
            }
        }

        void Start()
        {
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            } else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        // Update is called once per frame
        void Update()
        {

            if (photonView.IsMine)
            {
                ProcessInputs();

                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }
          
            // activate beams
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
           

        }

        /// <summary>
        /// Called when collider enters the trigger
        /// Affects the health of the player if the collider is a beam
        /// Jumping and firing may cause the player to hurt itself with its beam
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {

            // Don't do anything if it's not the local player
            if (!photonView.IsMine)
            {
                return;
            }

            // In the future to use tags to find the beam not by name
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;

        }

        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (!other.name.Contains("Beam"))
            {
                return;
            }

            // Needs to be affected at time so the player doesn't lose health rapidly
            Health -= 0.1f * Time.deltaTime;
        }

        #endregion

        #region Custom

        /// <summary>
        /// Processes inputs. Keeps track of when user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {

            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }

        }

        #endregion

    }

}
