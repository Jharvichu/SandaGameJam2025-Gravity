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
using UnityEngine.UI;



namespace UIDissolve
{
    public class CanvasCamera : MonoBehaviour
    {
        public Camera camera_;
        public Canvas canvas;

        [SerializeField]
        private RawImage raw_image;

        public void assignTexture(Texture texture)
        {
            raw_image.texture = texture;
        }
    }
}