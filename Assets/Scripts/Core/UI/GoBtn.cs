using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Gameplay;

public class GoBtn : MonoBehaviour
{
    private Suitcase suitcase;
    private StartSlots startslots;
    private TakeController controller;
    private Button btn;

    private void Start()
    {
        btn = GetComponentInChildren<Button>();
        suitcase = FindObjectOfType<Suitcase>();
        startslots = FindObjectOfType<StartSlots>();
        controller = FindObjectOfType<TakeController>();
        btn.onClick.AddListener(CloseSuitcase);
    }

    private void Update()
    {           
        if(startslots.items.Count == 0  && controller.gettingItem == null)
        {
            btn.gameObject.SetActive(true);
        }
        else
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void CloseSuitcase()
    {
        controller.enabled = false;
        suitcase.Close();
        btn.onClick.RemoveListener(CloseSuitcase);
    }
}
