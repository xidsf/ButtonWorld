using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ButtonManager : Singleton<ButtonManager>
{
    public List<ButtonController>[] buttons;
    public List<ButtonEventSubscriber> eventSubscribers;

    public void SetStageEvent()
    {
        SetButtons();
        SetDoors();
    }

    public void ResetStageEvent()
    {
        foreach (var door in eventSubscribers)
        {
            door.UnSubscribeButton();
        }
        eventSubscribers.Clear();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Clear();
        }
    }

    private void SetButtons()
    {
        buttons = new List<ButtonController>[3];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = new List<ButtonController>();
        }

        ButtonController[] allButtonControllers = FindObjectsByType<ButtonController>(FindObjectsSortMode.None);
        foreach (var button in allButtonControllers)
        {
            buttons[(int)button.buttonType].Add(button);
        }
    }

    private void SetDoors()
    {
        var findDoors = FindObjectsByType<ButtonEventSubscriber>(FindObjectsSortMode.None);

        eventSubscribers = new List<ButtonEventSubscriber>(findDoors);

        foreach (var door in eventSubscribers)
        {
            door.SubscribeButton();
        }
    }
}