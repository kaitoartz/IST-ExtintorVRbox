using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Controls;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public class ExtinguisherNative
{
    static ExtinguisherNative()
    {
        RegisterLayout();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void RegisterLayout()
    {
        // Registramos nuestro layout personalizado
        InputSystem.RegisterLayout<ExtinguisherDevice>(
            name: "ExtinguisherPro",
            matches: new InputDeviceMatcher()
                .WithInterface("HID")
                .WithCapability("vendorId", 58626) // 0xE502
                .WithCapability("productId", 48043) // 0xBBAB
            );

        Debug.Log("Driver Nativo C# Registrado");
    }
}

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct ExtinguisherState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('H', 'I', 'D');

    // Boton Principal
    [InputControl(name = "trigger", layout = "Button", format = "BIT", offset = 0, bit = 0)]
    [FieldOffset(0)]
    public byte buttonsByte1;

    // Definimos un segundo boton por si acaso
    [InputControl(name = "button2", layout = "Button", format = "BIT", offset = 0, bit = 1)]
    [FieldOffset(0)] // Mismo byte, diferente bit definido arriba
    public byte buttonsByte1_overlap;
}

// DEFINIMOS EL DISPOSITIVO
[InputControlLayout(stateType = typeof(ExtinguisherState))]
public class ExtinguisherDevice : InputDevice
{
    public ButtonControl trigger { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        trigger = GetChildControl<ButtonControl>("trigger");
    }
}