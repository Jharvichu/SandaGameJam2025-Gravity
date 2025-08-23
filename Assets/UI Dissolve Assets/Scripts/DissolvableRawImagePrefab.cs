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
    public class DissolvableRawImagePrefab : MonoBehaviour
    {

        public enum DissolveType
        {
            Noise, // random dissolve
            LeftToRight, // dissolve from left to right
            RightToLeft, // dissolve from right to left
            TopToBottom, // dissolve from top to bottom
            BottomToTop, // dissolve from bottom to top
            InOut // star dissolving from the center
        }

        public DissolveType dissolve_type; // the dissolve type

        private RawImage raw_image; // the raw image

        private Texture2D texture_2D; // fill this in with color data

        private List<Vector2Int> dissolve_pixels = new List<Vector2Int>();

        private int dissolve_rate = 10;

        private int section = 10; // this is used by the other dissolve types aside from the noise dissolve. it determines
                                  // the side of the dissolve area for the mean time. For example, if we are dissolving from left to right
                                  // and we have a section size of 10, then all the pixels in the texture with X values from 0 to 9 will be
                                  // dissolved before the ones that have X values from 10 - 19, and so on it continues. 

       
        private int section_counter = 0; // to know the current section of the image we are dissolving
                                        // does not apply to noise dissolve

        private Color dissolve_color; // the color this image will dissolve into



        public void initializeImage(int width, int height, DissolveType dissolve_type, int dissolve_rate, int section, Color start_color, Color dissolve_color, int level_of_detail)
        {

            this.dissolve_type = dissolve_type;
            this.dissolve_rate = dissolve_rate;
            this.section = section;
            this.dissolve_color = dissolve_color;



            if (texture_2D == null)
            {
                // create texture
                texture_2D = new Texture2D(width/level_of_detail, height/level_of_detail);
            }


            texture_2D.wrapMode = TextureWrapMode.Clamp;


            // initially fill in texture with start color
            for (int i = 0; i < texture_2D.width; i++)
            {
                for (int j = 0; j < texture_2D.height; j++)
                {
                    texture_2D.SetPixel(i, j, start_color);
                }
            }


            initializeRawImage(width, height);

            initializeSectionCounter(width, height); // initialize section counter based on the dissolve type
        }


        public void initializeImage(CanvasCamera canvas_camera, int width, int height, DissolveType dissolve_type, int dissolve_rate, int section, Color dissolve_color, int level_of_detail)
        {

            this.dissolve_type = dissolve_type;
            this.dissolve_rate = dissolve_rate;
            this.section = section;
            this.dissolve_color = dissolve_color;


            canvas_camera.camera_.targetTexture = RenderTexture.GetTemporary(Screen.width / level_of_detail, Screen.height / level_of_detail);


            if (texture_2D == null)
            {
                // create texture 2d
                texture_2D = new Texture2D(canvas_camera.camera_.targetTexture.width, canvas_camera.camera_.targetTexture.height, TextureFormat.ARGB32, false);
            }

            // render the canvas camera
            canvas_camera.camera_.Render();

            RenderTexture.active = canvas_camera.camera_.targetTexture;

            // read pixels from the canvas camera
            texture_2D.ReadPixels(new Rect(0, 0, canvas_camera.camera_.targetTexture.width, canvas_camera.camera_.targetTexture.height), 0, 0);

            texture_2D.Apply();

            RenderTexture.active = null;


            initializeRawImage(width, height);

            initializeSectionCounter(width, height);
        }


        private void initializeRawImage(int width, int height)
        {
            // get component for raw image
            raw_image = GetComponent<RawImage>();

            // set width height and position for the raw image
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        private void initializeSectionCounter(int width, int height)
        {
            // assign section counter based on the dissolve type
            switch (dissolve_type)
            {

                case DissolveType.RightToLeft:
                    section_counter = width; // going from right to left, section counter should start from the width value
                    break;


                case DissolveType.TopToBottom:

                    section_counter = height; // going from top to bottom, section counter should start from the height value

                    break;


                default:
                    section_counter = 0; // every other dissolve type, section counter should start from 0
                    break;
            }
        }




        // update image based on the dissolve type
        public void updateImage()
        {
            switch (dissolve_type)
            {
                case DissolveType.Noise:

                    noiseDissolve();
                    break;


                case DissolveType.LeftToRight:

                    dissolveLeftRight();
                    break;


                case DissolveType.RightToLeft:

                    dissolveRightLeft();
                    break;


                case DissolveType.TopToBottom:

                    dissolveUpDown();
                    break;


                case DissolveType.BottomToTop:

                    dissolveDownUp();
                    break;


                case DissolveType.InOut:

                    dissolveInOut();
                    break;

            }

            texture_2D.Apply();
            raw_image.texture = texture_2D;

        }



        void dissolve(System.Action action_if_pixel_count_is_empty)
        {
            // if there are 1 or more elements in the pixels list
            if (dissolve_pixels.Count > 0)
            {
                // the higher the dissolve rate, the more pixels will vanish at once
                // the higher this value is, the 
                for (int i = 0; i < Mathf.Min( dissolve_rate, dissolve_pixels.Count); i++)
                {
                    // pick a random pixel in the list
                    int random_pixel_position = UnityEngine.Random.Range(0, dissolve_pixels.Count - 1);

                    Color c = new Color((float)UnityEngine.Random.Range(0, 20) / 10, 
                                        (float)UnityEngine.Random.Range(0, 20) / 10, 
                                        (float)UnityEngine.Random.Range(0, 20) / 10,
                                        (float)UnityEngine.Random.Range(0, 20) / 10);


                    Color d = Color.Lerp(Color.red, Color.green, (float)i / Mathf.Min(dissolve_rate, dissolve_pixels.Count));
                                                                                                                                                                                        
                    texture_2D.SetPixel(dissolve_pixels[random_pixel_position].x, dissolve_pixels[random_pixel_position].y, dissolve_color);

                    // remove the pixel from the list
                    dissolve_pixels.RemoveAt(random_pixel_position);
                }
            }
            else
            {
                // if pixel count is empty (zero) invoke this action
                // This is where different dissolve types get to cause different dissolve animations
                // each dissolve type has a way in which it adds pixels to the pixel list.
                action_if_pixel_count_is_empty.Invoke();
            }

           
        }

        void noiseDissolve()
        {

            dissolve(() => {

                // with noise dissolve, we are adding all pixels to the list immediately
                for (int i = 0; i < texture_2D.width; i++)
                {
                    for (int j = 0; j < texture_2D.height; j++)
                    {
                        dissolve_pixels.Add(new Vector2Int(i, j));
                    }
                }
            });
        }

        void dissolveLeftRight()
        {
            // with this dissolve type, we are adding pixels starting from the left. The number of pixels we add at once 
            // will be based on the section value
            dissolve(() => {

                for (int i = section_counter; i < section + section_counter; i++)
                {
                    for (int j = 0; j < texture_2D.height; j++)
                    {
                        dissolve_pixels.Add(new Vector2Int(i, j));
                    }
                }
                section_counter += section;
            });
        }


        void dissolveRightLeft()
        {
            // with this dissolve type, we are adding pixels starting from the right. The number of pixels we add at once 
            // will be based on the section value
            dissolve(() => {

                for (int i = section_counter; i >= section_counter - section; i -= 1)
                {
                    for (int j = 0; j < texture_2D.height; j++)
                    {
                        dissolve_pixels.Add(new Vector2Int(i, j));
                    }
                }
                section_counter -= section;
            });
        }


        void dissolveDownUp()
        {

            // with this dissolve type, we are adding pixels starting from the bottom. The number of pixels we add at once 
            // will be based on the section value
            dissolve(() => {

                for (int i = 0; i <= texture_2D.width; i++)
                {
                    for (int j = section_counter; j < section + section_counter; j++)
                    {
                        dissolve_pixels.Add(new Vector2Int(i, j));
                    }
                }
                section_counter += section;
            });
        }




        void dissolveUpDown()
        {

            // with this dissolve type, we are adding pixels starting from the top. The number of pixels we add at once 
            // will be based on the section value
            dissolve(() => {
                for (int i = 0; i <= texture_2D.width; i++)
                {
                    for (int j = section_counter; j >= section_counter - section; j -= 1)
                    {
                        dissolve_pixels.Add(new Vector2Int(i, j));
                    }
                }
                section_counter -= section;
            });
        }


        void dissolveInOut()
        {
            // with this dissolve type, we are adding pixels accordingly based on their proximity to the center. The number of pixels we add at once 
            // will be based on the section value
            dissolve(() => {
                for (int i = 0; i <= texture_2D.width; i++)
                {
                    for (int j = 0; j <= texture_2D.height; j++)
                    {
                        if (Vector2.Distance(new Vector2(i, j), new Vector2(texture_2D.width / 2, texture_2D.height / 2)) < section_counter)
                        {
                            dissolve_pixels.Add(new Vector2Int(i, j));
                        }
                    }
                }
                section_counter += section;
            });
        }

    }

}
