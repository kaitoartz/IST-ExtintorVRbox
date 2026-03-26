using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ConvertShaderWindow : EditorWindow
{
    private Shader shaderDestino;
    private int materialesActualizados = 0;
    private int materialesOmitidos = 0;

    [MenuItem("Window/Utilidades/Actualizador de Shaders V2")]
    public static void MostrarVentana()
    {
        GetWindow<ConvertShaderWindow>("Actualizador de Shaders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Actualizar Shaders de Materiales Seleccionados", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        shaderDestino = (Shader)EditorGUILayout.ObjectField("Shader de Destino", shaderDestino, typeof(Shader), false);

        EditorGUILayout.Space();

        bool deshabilitado = shaderDestino == null;
        if (deshabilitado)
        {
            EditorGUILayout.HelpBox("Por favor, selecciona un 'Shader de Destino' para continuar.", MessageType.Info);
        }

        GUI.enabled = !deshabilitado;

        if (GUILayout.Button("Actualizar Materiales Seleccionados", GUILayout.Height(30)))
        {
            if (shaderDestino == null) return; // Doble chequeo por si acaso

            // Reseteamos los contadores
            materialesActualizados = 0;
            materialesOmitidos = 0;

            ActualizarShadersSeleccionados();
        }

        GUI.enabled = true; // Reactivamos la UI

        if (materialesActualizados > 0 || materialesOmitidos > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox($"Proceso completado:\n" +
                                    $"- Materiales actualizados: {materialesActualizados}\n" +
                                    $"- Materiales omitidos: {materialesOmitidos} (ya usaban el shader correcto)",
                                    MessageType.Info);
        }
    }

    /// <summary>
    /// Contenedor simple para guardar las propiedades de un material
    /// </summary>
    private class PropiedadesMaterialGuardadas
    {
        public Dictionary<string, Color> colores = new Dictionary<string, Color>();
        public Dictionary<string, Vector4> vectores = new Dictionary<string, Vector4>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, Texture> texturas = new Dictionary<string, Texture>();
        // Guardamos Tiling (scale) y Offset como un Vector4 (Scale.x, Scale.y, Offset.x, Offset.y)
        public Dictionary<string, Vector4> tilingOffsets = new Dictionary<string, Vector4>();
    }

    private void ActualizarShadersSeleccionados()
    {
        Object[] seleccionados = Selection.GetFiltered(typeof(Material), SelectionMode.Assets);

        if (seleccionados.Length == 0)
        {
            Debug.LogWarning("No hay ningún material seleccionado en la ventana de Proyecto.");
            return;
        }

        // NUEVO: Registra un solo "Undo" para todos los materiales que vamos a cambiar
        Undo.RecordObjects(seleccionados, "Actualizar Shaders de Materiales");

        foreach (Object obj in seleccionados)
        {
            Material mat = obj as Material;
            if (mat == null) continue;

            Shader shaderAntiguo = mat.shader;

            // Omitir si ya tiene el shader correcto
            if (shaderAntiguo == shaderDestino)
            {
                materialesOmitidos++;
                continue;
            }

            // --- 1. Guardar Propiedades Viejas ---
            PropiedadesMaterialGuardadas propsGuardadas = new PropiedadesMaterialGuardadas();
            int propertyCount = ShaderUtil.GetPropertyCount(shaderAntiguo);

            for (int i = 0; i < propertyCount; i++)
            {
                string nombreProp = ShaderUtil.GetPropertyName(shaderAntiguo, i);
                ShaderUtil.ShaderPropertyType tipoProp = ShaderUtil.GetPropertyType(shaderAntiguo, i);

                switch (tipoProp)
                {
                    case ShaderUtil.ShaderPropertyType.Color:
                        propsGuardadas.colores.Add(nombreProp, mat.GetColor(nombreProp));
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        propsGuardadas.vectores.Add(nombreProp, mat.GetVector(nombreProp));
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range: // Los "Range" son floats
                        propsGuardadas.floats.Add(nombreProp, mat.GetFloat(nombreProp));
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        Texture tex = mat.GetTexture(nombreProp);
                        if (tex != null)
                        {
                            propsGuardadas.texturas.Add(nombreProp, tex);

                            // NUEVO: Guardar Tiling y Offset
                            Vector2 scale = mat.GetTextureScale(nombreProp);
                            Vector2 offset = mat.GetTextureOffset(nombreProp);
                            propsGuardadas.tilingOffsets.Add(nombreProp, new Vector4(scale.x, scale.y, offset.x, offset.y));
                        }
                        break;
                }
            }

            // --- 2. Aplicar Nuevo Shader ---
            mat.shader = shaderDestino;

            // --- 3. Restaurar Propiedades (si existen en el nuevo shader) ---
            foreach (var pair in propsGuardadas.colores)
                if (mat.HasProperty(pair.Key)) mat.SetColor(pair.Key, pair.Value);

            foreach (var pair in propsGuardadas.vectores)
                if (mat.HasProperty(pair.Key)) mat.SetVector(pair.Key, pair.Value);

            foreach (var pair in propsGuardadas.floats)
                if (mat.HasProperty(pair.Key)) mat.SetFloat(pair.Key, pair.Value);

            foreach (var pair in propsGuardadas.texturas)
            {
                if (mat.HasProperty(pair.Key))
                {
                    mat.SetTexture(pair.Key, pair.Value);
                    // Restaurar Tiling y Offset
                    if (propsGuardadas.tilingOffsets.ContainsKey(pair.Key))
                    {
                        Vector4 tilingOffset = propsGuardadas.tilingOffsets[pair.Key];
                        mat.SetTextureScale(pair.Key, new Vector2(tilingOffset.x, tilingOffset.y));
                        mat.SetTextureOffset(pair.Key, new Vector2(tilingOffset.z, tilingOffset.w));
                    }
                }
            }

            EditorUtility.SetDirty(mat); // Marca el material como modificado
            materialesActualizados++;
        }

        // Guardamos los cambios en disco
        AssetDatabase.SaveAssets();

        Debug.Log($"¡Actualización completada! {materialesActualizados} materiales actualizados. {materialesOmitidos} omitidos.");

        // Forzamos a la ventana a redibujarse para mostrar el reporte
        this.Repaint();
    }
}