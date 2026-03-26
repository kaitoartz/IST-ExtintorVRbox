using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PropCanvas : MonoBehaviour
{
    public UnityEvent onCompleteChecklist;
    public Toggle[] toggles;
    public int count;
    public int valueCount = 0;
    private void Start()
    {
        count = toggles.Length;
        valueCount = 0;
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(AddCount);
        }
    }
    public void AddCount(bool value)
    {
        if (value)
        {
            valueCount++;
            if (valueCount == toggles.Length)
            {
                Debug.Log("Checklist Completa");
                onCompleteChecklist.Invoke();
            }
            else return;
        }
    }
}
