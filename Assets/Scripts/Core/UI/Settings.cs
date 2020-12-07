using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class Settings : MonoBehaviour
{
    private Toggle vibr;

    private void Start()
    {
        vibr = transform.GetChild(0).GetComponent<Toggle>();
        vibr.isOn = GameStateController.instance.HasVibrate;
        vibr.onValueChanged.AddListener(delegate { ChangeVibr(vibr.isOn); });
    }

    private void ChangeVibr(bool value)
    {
        GameStateController.instance.HasVibrate = value;
    }
}
