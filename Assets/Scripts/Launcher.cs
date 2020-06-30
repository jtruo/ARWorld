using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.Augment.ARWorld
{

    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Private Serializble Fields

        [Tooltip("Max number of players per room. When the room is full, a new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion


        #region Private Fields

        /// <summary>
        /// Client's game version number
        /// </summary>
        string gameVersion = "1";


        #endregion


        #region MonoBehaviour Callbacks

        /// <summary>
        /// MonoBehavior method called on the GameObject by Unity during initialization
        /// </summary>
        void Awake()
        {
            // #Critical
            // Allows the use of PhotonNetwork.LoadLevel() on the master client and all clients
            // in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;

        }


        #endregion


        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            PhotonNetwork.JoinRandomRoom();
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        }


        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed(). No random room available");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() was called. Client is now in a room");
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Starts the connection processs when the user clicks the play button
        /// If already connected, the client joins a random room
        /// If not, connect the application instance to the Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            if (PhotonNetwork.IsConnected)
            {
                // #Critical, joins random room, if it fails, OnJoinRandomFailed() will return, and we'll create a room
                PhotonNetwork.JoinRandomRoom();

            }
            else
            {
                // #Critical, connect to photon
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion

    }

}

