﻿using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.Augment.ARWorld
{

    /// <summary>
    /// Takes care of connecting the user to Photon
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {

        #region Private Serializble Fields

        [Tooltip("Max number of players per room. When the room is full, a new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("UI Panel that lets the user enter name, connect, and play")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("UI Label that informs the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;


        #endregion


        #region Private Fields

        /// <summary>
        /// Client's game version number
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Used to adjust behavior based off of call backs from Photon.
        /// Typically used for OnConnectedToMaster().
        /// </summary>
        bool isConnecting;


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

        void Start()
        {

            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion


        #region MonoBehaviourPunCallbacks Callbacks


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }

           
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
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

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("Loading 'Room for 1'");

                PhotonNetwork.LoadLevel("Room for 1");
            }

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

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                // Joins random room, if it fails, OnJoinRandomFailed() will return, and we'll create a room
                PhotonNetwork.JoinRandomRoom();

            }
            else
            {
                // Connect to photon
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }

           
        }

        #endregion


    }

}

