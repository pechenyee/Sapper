using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restart : MonoBehaviour
{
    public FieldController fieldController;
    public void OnClick()
    {
        foreach (var fieldCon in fieldController.buttons)
        {
            Destroy(fieldCon.gameObject);
        }
        fieldController.buttons.Clear();
        fieldController.congr.enabled = false;
        fieldController.Build();
    }
}
