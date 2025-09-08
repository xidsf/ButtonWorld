using UnityEngine;

public class DebuggingCode : MonoBehaviour
{
     ButtonEventSubscriber myButton;


    private void Pressing()
    {
        Debug.Log("Button Pressed");
    }

    private void UnPressing()
    {
        Debug.Log("Button UnPressed");
    }

}
