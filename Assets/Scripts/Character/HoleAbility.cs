using UnityEngine;
using UnityEngine.InputSystem;

public class HoleAbility : MonoBehaviour
{
    [SerializeField] private Hole black;
    [SerializeField] private Hole white;

    private GameObject currentHole;
    private bool blackHoleReady;
    private bool whiteHoleReady;

    public void PrepareBlackHole(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            blackHoleReady = true;
            //Posible cambio de sprite, animacion
        }
        else
        {
            blackHoleReady = false;
        }
    }

    public void PrepareWhiteHole(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            whiteHoleReady = true;
            //Posible cambio de sprite, animacion
        }
        else
        {
            whiteHoleReady = false;
        }
    }

    public void ActivateHole(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (blackHoleReady) CastBlackHole(mousePos);
            if (whiteHoleReady) CastWhiteHole(mousePos);
        }
    }

    private void CastBlackHole(Vector2 position)
    {
        DestroyCurrentHole();
        currentHole = Instantiate(black.HolePrefab, position, Quaternion.identity);
    }

    private void CastWhiteHole(Vector2 position)
    {
        DestroyCurrentHole();
        currentHole = Instantiate(white.HolePrefab, position, Quaternion.identity);
    }

    private void DestroyCurrentHole()
    {
        if (currentHole != null)
        {
            Destroy(currentHole);
        }
    }

}
