using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaulosMenuController
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Should the game pause when opening the menu ?")]
        [SerializeField]
        private bool pauseOnOpen = true;
        [Space(10)]
        [SerializeField]
        private GameObject mainCanvasObj;
        [SerializeField]
        private GameObject mainMenuPanelObj, optionsPanelObj, graphicsPanelObj, audioPanelObj;
        [SerializeField]
        private GameObject closeGameImageObj;

        private float previousTimescale;
        private bool menuOpen;
        
        private void Start()
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            mainCanvasObj.SetActive(true);
            graphicsPanelObj.SetActive(false);
            audioPanelObj.SetActive(false);
            optionsPanelObj.SetActive(false);
            mainMenuPanelObj.SetActive(true);

            closeGameImageObj.SetActive(false);
        }

        public void ButtonPlayGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ButtonQuitGame()
        {
            Application.Quit();
            Debug.Log("Quit");
        }
    }
}
