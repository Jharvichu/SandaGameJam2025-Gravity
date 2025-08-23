/*
 * 
 * Developed by Olusola Olaoye, 2024
 * 
 * To only be used by those who purchased from the Unity asset store
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIDissolve
{
    public class ExampleDissolveUsingAction : MonoBehaviour
    {

        [SerializeField]
        private DissolveUI ui_to_dissolve; // the ui we want to dissolve 


        // Start is called before the first frame update
        void Start()
        {
            ui_to_dissolve.start_dissolving = false; // ui should not start dissolving on start
        }

        // Update is called once per frame
        void Update()
        {
            // you can change this bool condition to whatever you like.
            // Maybe a game over event or some other key pressed
            if (Input.GetKeyDown(KeyCode.K)) 
            {
                ui_to_dissolve.start_dissolving = true;
            }
        }
    }
}