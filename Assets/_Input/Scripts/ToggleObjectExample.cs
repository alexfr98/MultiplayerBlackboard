using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleObjectExample : MonoBehaviour
{
    public InputActionReference toggleReference = null;
    private bool isActive = false;
    void Update()
    {
        if (isActive == true){
            Debug.Log("PencilActivated");
        }
    }

    private void Awake()
    {
        toggleReference.action.started += Toggle;
    }


    private void OnDestroy()
    {
        toggleReference.action.started -= Toggle;
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
    }
}
