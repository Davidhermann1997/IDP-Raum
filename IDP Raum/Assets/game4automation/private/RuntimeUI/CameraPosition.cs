﻿using System.Runtime.Serialization;
using game4automation;

using UnityEngine;

using UnityEngine.EventSystems;


// $Revision$ 

namespace game4automation
{
    //! Saves the camera position for game view. 
    //! Each position can get a name and a Hoteky to display the camera position
    //! Multiple of this objects can be attachted to the camera for saving multiple positions
    public class CameraPosition : Game4AutomationBehavior
    {
        public string ViewName; //!< The view name
        public string KeyCode; //!< The Key code (hotkey) do display the vidw
        public bool ActivateOnStart; //!< True if view should be activated on start
        public TouchInteraction DoubleTapGesture; //!< Reference to TouchInteraction position should be displayed on double tap

        [ReadOnly] public Vector3 CameraRot; //!< [ReadOnly] Camera rotation
        [ReadOnly] public Vector3 TargetPos; //!< [ReadOnly] Camera target position    
        [ReadOnly] public float CameraDistance; //!< [ReadOnly] Camera distance

        private bool _display = true;

        private EventSystem _event;

        //! Gets the current position of the game view and saves it to this object
        public void GetCameraPosition()
        {
            SceneMouseNavigation nav = GetComponent<SceneMouseNavigation>();
            CameraRot = nav.currentRotation.eulerAngles;
            CameraDistance = nav.currentDistance;
            TargetPos = nav.target.position;
        }


        private void SetCameraPositionNoDisplay()
        {
            _display = false;
            SetCameraPosition();
            _display = true;
        }


        void OnEnable()
        {
            _event = GetComponent<EventSystem>();
            if (DoubleTapGesture != null)
                DoubleTapGesture.doubleTouchDelegate += TapGestureHandler;
        }

        private void TapGestureHandler(Vector2 pos)
        {
            SetCameraPosition();
        }

        //! Sets the game view position to the saved positoin and displays a message
            public void SetCameraPosition()
            {
                Debug.Log("Switched to View " + ViewName);
                SceneMouseNavigation nav = GetComponent<SceneMouseNavigation>();
                nav.SetNewCameraPosition(TargetPos, CameraDistance, CameraRot);
                if (_display)
                {
                    Game4AutomationController.MessageBox("View changed to " + ViewName, true, 2);
                }
            }

            public void Start()
            {
                if (ActivateOnStart)
                {
                    Invoke("SetCameraPositionNoDisplay", 0.1f);
                }
            }

            public void Update()
            {
                if (KeyCode != "")
                {
                    if (Input.GetKeyDown(KeyCode))
                    {
                        SetCameraPosition();
                    }
                }
            }
        }
    }