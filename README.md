# self-driving
**解説**: [https://github.com/ibalaboratory/self-driving/wiki](https://github.com/ibalaboratory/self-driving/wiki)

# 各versionの説明
## v0
0~14,40,42
50,70,90,110,130
ファイルは全てデフォルト

100-85-4, 8-1-4-6

20世代まで

## v1
0~19,40,42
50,70,90,110,130

100-85-4, 8-3-4-6

20世代まで


# 今後やること
## NNBattleBrain.csの編集
    これダメでは？？
/// <summary>
    /// `BattleBrain`の`GetAction()`関数をoverrideする.
    /// 入力のうち0, 1, 2, 3, 4, 40, 42番目のみを使う.
    /// </summary>
    public override double[] GetAction(List<double> observation)
    {
        return brain.GetAction(RearrangeObservation(observation, new List<int>{0, 1, 2, 3, 4, 40, 42}));
    }