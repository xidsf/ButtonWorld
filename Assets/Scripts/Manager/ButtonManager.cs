using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ButtonManager : Singleton<ButtonManager>
{
    public List<ButtonController>[] buttons;

    protected override void Awake()
    {
        base.Awake();
        SetButtons(gameObject.scene, LoadSceneMode.Additive);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetButtons;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetButtons;
    }

    public void SetButtons(Scene scene, LoadSceneMode mode)
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

}