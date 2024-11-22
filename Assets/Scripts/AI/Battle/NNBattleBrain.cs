using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleBrain/NNBattleBrain")]
public class NNBattleBrain : BattleBrain
{
    [SerializeField] private TextAsset brainData = null;
    [SerializeField] public bool[] selectedSensors = new bool[46];

    private NNBrain brain;

    public override void Initialize() {
        base.Initialize();
        try
        {
            brain = NNBrain.Load(brainData);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to load NNBrain.", ex);
        }
    }

    public override double[] GetAction(List<double> observation)
    {
        return brain.GetAction(ProcessObservation(observation));
    }

    // YourNameBattleBrain.csにオーバーライドされる.
    protected virtual List<double> ProcessObservation(List<double> observation)
    {
        return RearrangeObservation(observation, CreateSelectedSensorsList(selectedSensors));
    }

    public List<int> CreateSelectedSensorsList(bool[] selectedSensors) {
        List<int> selectedSensorsList = new List<int>();
        for (int i = 0; i < selectedSensors.Length; i++)
        {
            if (selectedSensors[i]) selectedSensorsList.Add(i);
        }
        return selectedSensorsList;
    }
}
