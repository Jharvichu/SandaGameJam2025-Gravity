
using UnityEngine;
using UnityEngine.UI;

namespace a1creator
{
    public class UIElementClamp_Demo : MonoBehaviour
    {
        [SerializeField] private Button _keepInside_Button;
        [SerializeField] private Button _randomize_Button;
        [SerializeField] private RectTransform _keepInside_Rect;

        private void Awake()
        {
            _keepInside_Button.onClick.RemoveAllListeners();
            _keepInside_Button.onClick.AddListener(() => {
                _keepInside_Rect.SetPositionInsideScreen(_keepInside_Rect.position);
            });
            _randomize_Button.onClick.RemoveAllListeners();
            _randomize_Button.onClick.AddListener(() => {
                _keepInside_Rect.position = new Vector2(Screen.width, Screen.height) * 0.5f + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Screen.height;
            });
        }
    }
}