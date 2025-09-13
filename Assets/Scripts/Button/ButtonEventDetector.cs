using System;
using UnityEngine;

public class ButtonEventDetector : MonoBehaviour
{
    public Action<bool> changePressed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        changePressed?.Invoke(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        changePressed?.Invoke(false);
    }

}
