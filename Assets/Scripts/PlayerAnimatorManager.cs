using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.Augment.ARWorld

{
    public class PlayerAnimatorManager : MonoBehaviourPunCallbacks
    {



        #region Private Fields

        private Animator animator;

       
        [SerializeField]
        private float directionDampTime = 1f;

        #endregion


        #region MonoBehaviour Callbacks



        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();

            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing the Animator Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {

            // Check if the instance is from the correct client/player
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            

            if (!animator)
            {
                return;
            }

            // Deals with jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Jump if player is running
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // Jumps on right mouse click or alt key
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }

            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v < 0)
            {
                v = 0;
            }

            // Squared inputs so it's a positive absolute value
            // Also uses "easing" for more fluid movement
            animator.SetFloat("Speed", h * h + v * v);

            //Smooths out rotation of the player
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);


        


        }


        #endregion
    }


}

