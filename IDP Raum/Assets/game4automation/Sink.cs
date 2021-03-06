﻿// Game4Automation (R) Framework for Automation Concept Design, Virtual Commissioning and 3D-HMI
// (c) 2019 in2Sight GmbH - Usage of this source code only allowed based on License conditions see https://game4automation.com/lizenz  

using System.Collections.Generic;
using game4automation;
using UnityEngine;

namespace game4automation
{
    [SelectionBase]
    [RequireComponent(typeof(BoxCollider))]
    public class Sink : Game4AutomationBehavior
    {
        // Public - UI Variables 
        [Header("Settings")] public bool DeleteMus = true;
        public string DeleteOnlyTag;

        [Header("Sink IO's")] public PLCOutputBool Delete;
        private bool _lastdeletemus = false;
    
        [Header("Status")] 
        public List<GameObject> CollidingObjects;

        private bool _isDeleteNotNull;

        // Use this when Script is inserted or Reset is pressed
        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }    
    
        // Use this for initialization
        private void Start()
        {
            _isDeleteNotNull = Delete != null;
        }

        public void DeleteMUs()
        {
            var tmpcolliding = CollidingObjects;
            foreach (var obj in tmpcolliding.ToArray())
            {
                var mu = GetTopOfMu(obj);
                if (mu != null)
                {
                    if (DeleteOnlyTag=="" || (mu.gameObject.tag == DeleteOnlyTag))
                        Destroy(mu.gameObject);
                }

                CollidingObjects.Remove(obj);
            }
        }
    
        // ON Collission Enter
        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            CollidingObjects.Add(obj);
            if (DeleteMus==true)
            {
                // Act as Sink
                DeleteMUs();
            }
        }
    
        // ON Collission Exit
        private void OnTriggerExit(Collider other)
        {
            GameObject obj = other.gameObject;
            CollidingObjects.Remove(obj);
        }

        private void Update()
        {
            if (_isDeleteNotNull)
            {
                DeleteMus = Delete.Value;
            }
        
            if (DeleteMus && !_lastdeletemus)
            {
                DeleteMUs();
            }
            _lastdeletemus = DeleteMus;

        }
    }
}