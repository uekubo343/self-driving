using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleBrain/YourNameBattleBrain")]
public class YourNameBattleBrain : NNBattleBrain
{
    protected override List<double> ProcessObservation(List<double> observation)
    {
        return RearrangeObservation(observation, CreateSelectedInputsList(selectedInputs));
    }
}