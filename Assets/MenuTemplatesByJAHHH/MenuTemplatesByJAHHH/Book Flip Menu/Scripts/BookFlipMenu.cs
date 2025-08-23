using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MenuTemplatesByJAHHH
{

    public class BookFlipMenu : MonoBehaviour
    {
        [SerializeField] private GameObject[] pages; // Array of canvases that represent the pages
        [SerializeField] private Button forwardButton; 
        [SerializeField] private Button backButton;
        [SerializeField] private float flipDuration = 1.0f; 

        private bool isFlipping = false;
        private int currentPageIndex = 0; 

        [SerializeField] private Button controlsButton;
        [SerializeField] private Button controlsBackButton;
        [SerializeField] private GameObject controlsCanvas;
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private AudioManager audioManager;

        void Start()
        {
            controlsButton.onClick.AddListener(OpenControlsCanvas);
            controlsBackButton.onClick.AddListener(CloseControlsCanvas);
            startButton.onClick.AddListener(StartGame);
            quitButton.onClick.AddListener(QuitGame);


            controlsCanvas.SetActive(false);

            if (forwardButton == null || backButton == null)
            {
                Debug.LogError("Buttons not assigned in the Inspector!");
                return;
            }

            forwardButton.onClick.AddListener(GoForwardPage);
            backButton.onClick.AddListener(GoBackPage);

            
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(i == 0); 
                SetPagePivot(pages[i], new Vector2(0, 0.5f)); 
            }

            backButton.interactable = false; 
        }

        private void GoForwardPage()
        {
            if (isFlipping || currentPageIndex >= pages.Length - 1)
                return;

            StartCoroutine(FlipPage(true));
        }

        private void GoBackPage()
        {
            if (isFlipping || currentPageIndex <= 0)
                return;

            StartCoroutine(FlipPage(false));
        }

        private IEnumerator FlipPage(bool forward)
        {
            isFlipping = true;

            GameObject currentPage = pages[currentPageIndex];
            int targetPageIndex = forward ? currentPageIndex + 1 : currentPageIndex - 1;
            GameObject targetPage = pages[targetPageIndex];

           
            targetPage.SetActive(true);

            
            float elapsedTime = 0f;
            float startAngle = forward ? 0f : 180f;
            float endAngle = forward ? 180f : 0f;

            while (elapsedTime < flipDuration)
            {
                elapsedTime += Time.deltaTime;
                float angle = Mathf.Lerp(startAngle, endAngle, elapsedTime / flipDuration);
                currentPage.transform.localRotation = Quaternion.Euler(0, angle, 0);
                yield return null;
            }

            currentPage.transform.localRotation = Quaternion.Euler(0, endAngle, 0);

            if (forward)
            {
                
                if (currentPageIndex < pages.Length - 1)
                {
                    currentPage.transform.SetSiblingIndex(0);
                }
            }

            currentPageIndex = targetPageIndex;

           
            targetPage.transform.localRotation = Quaternion.Euler(0, 0, 0);

            
            for (int i = 0; i <= currentPageIndex; i++)
            {
                pages[i].SetActive(true);
            }

           
            UpdatePageOrder();

            UpdateButtons();
            isFlipping = false;
        }

        private void UpdatePageOrder()
        {
            for (int i = 0; i < pages.Length - 1; i++) 
            {
                if (i == currentPageIndex)
                {
                    pages[i].transform.SetSiblingIndex(pages.Length - 2); 
                }
                else
                {
                    pages[i].transform.SetSiblingIndex(i);
                }
            }
        }

        private void UpdateButtons()
        {
            backButton.interactable = currentPageIndex > 0;
            forwardButton.interactable = currentPageIndex < pages.Length - 1;
        }

        private void SetPagePivot(GameObject page, Vector2 pivot)
        {
            RectTransform rectTransform = page.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.pivot = pivot;
            }
        }



        private void OpenControlsCanvas()
        {
            controlsCanvas.SetActive(true);
        }

        private void CloseControlsCanvas()
        {

            controlsCanvas.SetActive(false);
        }

        private void PlayClickSound()
        {
            audioManager.PlaySound(AudioTracks.buttonClickSound);

        }

        private void PlaySwitchSound()
        {
            audioManager.PlaySound(AudioTracks.screenSwitchSound);
        }
         
        private void StartGame()
        {
            //SceneManager.LoadScene("Level");      CHANGE THIS TO THE SCENE YOU WANT TO LOAD
        }

        private void QuitGame()
        {
#if UNITY_EDITOR
         
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // If in a build, quit the application
        Application.Quit();
#endif
        }
    }

}
