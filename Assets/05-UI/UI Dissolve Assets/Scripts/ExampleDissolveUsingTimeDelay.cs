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
    public class ExampleDissolveUsingTimeDelay : MonoBehaviour
    {

        [SerializeField]
        private DissolveUI ui_to_dissolve; // the ui we want to dissolve 

        [SerializeField]
        [Range(0, 10)]
        private float delay = 0; // delay in seconds before animation starts


        private float delay_counter = 0;

        private void Start()
        {
            ui_to_dissolve.start_dissolving = false;
        }

        // Update is called once per frame
        void Update()
        {

            // when delay counter counts up to "delay" seconds, its time to call the updateImage function for animation
            if (delay_counter > delay)
            {
                ui_to_dissolve.start_dissolving = true;
            }
            else
            {
                delay_counter += Time.deltaTime;
            }
        }
    }

}
