using UnityEngine;

public class TestBotonesColor : MonoBehaviour
{
    // Arrastra aquí el objeto que quieres que agrande
    public Transform objetoParaAgrandar;

    // Cuánto agranda cada vez que aprietas
    public float incrementoEscala = 0.1f;

    void Update()
    {
        // Tu ciclo for salvador
        for (int i = 0; i < 20; i++)
        {
            KeyCode key = KeyCode.JoystickButton0 + i;

            if (Input.GetKeyDown(key))
            {
                Debug.Log("🚨 ¡ENCONTRADO CTM! El botón es: JoystickButton" + i);

                // Aquí hacemos la magia: Agrandar al toque
                AgrandarObjeto();
            }
        }
    }

    void AgrandarObjeto()
    {
        if (objetoParaAgrandar != null)
        {
            objetoParaAgrandar.localScale += Vector3.one * incrementoEscala;
            Debug.Log($"Nueva escala: {objetoParaAgrandar.localScale}");
        }
        else
        {
            Debug.LogWarning("Oye wn, te faltó asignar el Transform en el inspector!");
        }
    }
}