using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderStripper : IPreprocessShaders
{
    // Define el orden de ejecución (queremos que corra pronto)
    public int callbackOrder => 0;

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        // Solo nos interesa optimizar el pipeline universal (URP)
        // Si usas shaders custom de fuego, añádelos aquí si quieres filtrarlos también
        if (!shader.name.StartsWith("Universal Render Pipeline"))
            return;

        // Lista para marcar qué variantes borrar
        var variantsToRemove = new List<ShaderCompilerData>();

        for (int i = 0; i < data.Count; ++i)
        {
            var shaderData = data[i];

            // ---------------------------------------------------------
            // 1. ELIMINAR DEBUG Y PANTALLA
            // ---------------------------------------------------------
            // Nunca usaremos modos de depuración en la build final
            if (shaderData.shaderKeywordSet.IsEnabled(new ShaderKeyword("DEBUG_DISPLAY")))
            {
                variantsToRemove.Add(shaderData);
                continue;
            }

            // Screen Space Shadows son muy caras para Quest, las borramos
            if (shaderData.shaderKeywordSet.IsEnabled(new ShaderKeyword("_SCREEN_SPACE_SHADOWS")))
            {
                variantsToRemove.Add(shaderData);
                continue;
            }

            // ---------------------------------------------------------
            // 2. ELIMINAR SOMBRAS DE LUCES SECUNDARIAS (POINT LIGHTS)
            // ---------------------------------------------------------
            // Si en tu URP Asset desactivaste "Additional Light Shadows", 
            // esto asegura que el shader no las incluya ni por error.
            if (shaderData.shaderKeywordSet.IsEnabled(new ShaderKeyword("_ADDITIONAL_LIGHT_SHADOWS")))
            {
                variantsToRemove.Add(shaderData);
                continue;
            }

            // ---------------------------------------------------------
            // 3. ELIMINAR VARIANTES DE SOMBRAS SUAVES
            // ---------------------------------------------------------
            // Si decidiste usar solo Hard Shadows, borramos las suaves.
            if (shaderData.shaderKeywordSet.IsEnabled(new ShaderKeyword("_SHADOWS_SOFT")))
            {
                variantsToRemove.Add(shaderData);
                continue;
            }

            // ---------------------------------------------------------
            // 4. ELIMINAR HDR (Opcional)
            // ---------------------------------------------------------
            // Si tu proyecto no usa HDR, esto ahorra mucho.
            // Descomenta si estás seguro de que no usas HDR.
            /*
            if (shaderData.shaderKeywordSet.IsEnabled(new ShaderKeyword("_HDR")))
            {
                variantsToRemove.Add(shaderData);
                continue;
            }
            */
        }

        // Borrado final
        if (variantsToRemove.Count > 0)
        {
            // Debug.Log($"[ShaderStripper] Eliminadas {variantsToRemove.Count} variantes del shader: {shader.name}");
            foreach (var variant in variantsToRemove)
            {
                data.Remove(variant);
            }
        }
    }
}