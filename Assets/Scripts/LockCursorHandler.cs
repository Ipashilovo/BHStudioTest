using System;
using UnityEngine;

public class LockCursorHandler : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}