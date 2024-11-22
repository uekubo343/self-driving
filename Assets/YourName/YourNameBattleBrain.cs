using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleBrain/YourNameBattleBrain")]
public class YourNameBattleBrain : NNBattleBrain
{
    [SerializeField] public new bool[] selectedSensors = new bool[46];

    protected override List<double> ProcessObservation(List<double> observation)
    {
        return RearrangeObservation(observation, CreateSelectedSensorsList(selectedSensors));
    }
}