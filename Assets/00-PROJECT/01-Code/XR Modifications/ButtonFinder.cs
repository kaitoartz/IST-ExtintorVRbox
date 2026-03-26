using UnityEngine;

public class ButtonFinder : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            KeyCode key = KeyCode.JoystickButton0 + i;
            if (Input.GetKeyDown(key))
            {
                Debug.LogError("¡ENCONTRADO! El botón es: JoystickButton" + i);
            }
        }
    }
}