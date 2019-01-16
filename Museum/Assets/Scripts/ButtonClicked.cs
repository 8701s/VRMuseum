using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour {

    public Button buttonplus;
    public Button buttonminus;

    public BotRuntimeSet BotsRuntimeSet;

    public float currentValue = 5.0f;
    public float minValue = 0.0f;
    public float maxValue = 15.0f;
    public float stepSize = 1.0f;

    void Start () {
        buttonplus.onClick.AddListener(TaskOnPlusClick);
        buttonminus.onClick.AddListener(TaskOnMinusClick);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void TaskOnPlusClick()
    {
        currentValue += stepSize;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
        UpdateValuesOnBots();
    }

    void TaskOnMinusClick()
    {
        currentValue -= stepSize;
        if (currentValue < minValue)
        {
            currentValue = minValue;
        }
        UpdateValuesOnBots();
    }

    public void UpdateValuesOnBots()
    {
        foreach (Bot botItem in BotsRuntimeSet.Items)
        {
            if (botItem.autoUpdateFading)
            {
                botItem.distanceToFadeMax = currentValue;
                botItem.distanceToFadeMin = currentValue * 0.75f;
                botItem.distanceToDisappear = currentValue * 0.5f;
            }
        }
    }

}
