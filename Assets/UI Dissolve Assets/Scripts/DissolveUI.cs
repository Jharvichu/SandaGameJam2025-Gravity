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
    public class DissolveUI : MonoBehaviour
    {

        public enum ImageInitialization
        {
            Plain, // can work for texts and images. images must be single color
            CanvasCamera, // for images, but can dissolve images with multiple colors
        }

        [SerializeField]
        [Tooltip("plain can work for texts and images. images must be monochromatic, canvas camera is for only images, but can dissolve images with multiple colors")]
        private ImageInitialization image_initialization;


        private RectTransform rect_transform; // the rext transofrm of this UI object

        [SerializeField]
        private DissolvableRawImagePrefab mask_image_prefab; // the mask image prefab which will help with masking

        private DissolvableRawImagePrefab mask_image; // the mask image which will be created from the prefab


        [SerializeField]
        private CanvasCamera canvas_camera_prefab;

        [SerializeField]
        private DissolvableRawImagePrefab.DissolveType dissolve_type; // the type of dissolve animation 


        [SerializeField]
        [Tooltip("how fast object will dissolve")]
        [Range(1, 200)]
        private int dissolve_rate = 10; // how fast object will dissolve


        [SerializeField]
        [Tooltip("This is used by the other dissolve types aside from the noise dissolve. it determines the size of the dissolve area.")]
        [Range(1, 40)]
        private int section = 10; // this is used by the other dissolve types aside from the noise dissolve. it determines
                                  // the size of the dissolve area for the mean time. For example, if we are dissolving from left to right
                                  // and we have a section size of 10, then all the pixels in the texture with X values from 0 to 9 will be
                                  // dissolved before the ones that have X values from 10 - 19, and so on it continues. 


        [SerializeField]
        [Tooltip("this is basically the level of detail here, lower lod means higher details and pixels, slower dessolve rate and more computation (lower performance.)")]
        [Range(1,7)]
        private int level_of_detail = 2; // the level of detail controls the dissolve image quality, lower level of detail value means higher details and pixels, slower dissolve rate and more computation (lower performance.)
                        // higher level of detail value means lower details and pixels, higher dissolve rate and less computation (more performance)


        [SerializeField]
        private Color start_color = Color.white; // the start color of the mask for plain image initialization

        [SerializeField]
        private Color dissolve_color; // the end color of the mask. If you want image or text to disappear, then set this color to transparent




        // this UI should start dissolving by default unless ateted otherwise
        // you can set this to false if you want to delay dissolve by timer or some kind of event. 
        // see example scripts.
        public bool start_dissolving
        {
            get;
            set;
        } = true;



        // Start is called before the first frame update
        void Start()
        {
            rect_transform = GetComponent<RectTransform>();

            mask_image = Instantiate(mask_image_prefab);
            mask_image.transform.SetParent(transform, false);

            gameObject.AddComponent<Mask>().showMaskGraphic = false;



            // if this game object does not have an image or a raw image as a component (for example if it is a text)
            // then make sure the image initialisation method used is the plain method
            if(!GetComponent<Image>() && !GetComponent<RawImage>())
            {
                image_initialization = ImageInitialization.Plain;
            }

            switch (image_initialization)
            {
                case ImageInitialization.Plain:

                    // initialize mask image
                    mask_image.initializeImage((int)rect_transform.rect.width, (int)rect_transform.rect.height, dissolve_type, dissolve_rate, section, start_color, dissolve_color, level_of_detail);

                    break;


                case ImageInitialization.CanvasCamera:
                    // create canvas camera
                    CanvasCamera canvas_camera = Instantiate(canvas_camera_prefab);

                    // get the texture in the imaage
                    if(GetComponent<Image>())
                    {
                        canvas_camera.assignTexture(GetComponent<Image>().mainTexture);
                    }

                    // if it's a raw image instead, then get the texture
                    else if(GetComponent<RawImage>())
                    {
                        canvas_camera.assignTexture(GetComponent<RawImage>().mainTexture);
                    }
                    // initialise the mask image
                    mask_image.initializeImage(canvas_camera, (int)rect_transform.rect.width, (int)rect_transform.rect.height, dissolve_type, dissolve_rate, section, dissolve_color, level_of_detail);

                    break;

            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if(start_dissolving)
            {
                mask_image.updateImage();
            }
        }
    }

}