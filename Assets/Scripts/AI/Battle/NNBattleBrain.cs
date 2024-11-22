using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleBrain/NNBattleBrain")]
public class NNBattleBrain : BattleBrain
{
    [SerializeField] private TextAsset brainData = null;

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

    protected virtual List<double> ProcessObservation(List<double> observation)
    {
        return RearrangeObservation(observation, new List<int>{0, 1, 2, 3, 4, 40, 42});
    }

    public List<int> CreateSelectedInputsList(bool[] selectedInputs) {
        List<int> selectedInputsList = new List<int>();
        for (int i = 0; i < selectedInputs.Length; i++)
        {
            if (selectedInputs[i]) selectedInputsList.Add(i);
        }
        return selectedInputsList;
    }
}
