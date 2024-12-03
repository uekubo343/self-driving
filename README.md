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

## v2
0~14,40,42
50,70,90,110,130
currentMaxのとき到達距離の評価を*2
carAgent衝突を-10に

100-85-4, 16-2-4-6

25世代まで

## C1v2
challenge1
0~14,40,41,42
50,70,90,110,130
currentMaxのとき到達距離の評価を*1
carAgent衝突を0に

100-85-4, 24-2-4-8

25世代まで

## C3v3
challenge3
0~14,40,41,42
50,70,90,110,130
plusrewardをfalseに
currentMaxのとき到達距離の評価を*1
LocalMaxのとき評価*-5
carAgent衝突を*-10
逆走を*-3


100-85-4, 8-1-4-8

25世代まで

## v4
0~14,40,42
50,70,90,110,130
sideは30,60,90,120,150
plusrewardをtrueに
currentMaxのとき到達距離の評価を*1
velocity<10なら-0.1
全て-1

100-85-4, 8-1-4-6


## v5
// WayPoint通過時に報酬を与える
AddReward(1.0f / (WaypointIndex + 1));

Donewithrewardの中身をaddrewardにする
全部-100に

var v = CarRb.velocity.magnitude;
AddReward(v/10.0f);


0~14,40,42
50,70,90,110,130
100-85-4, 8-1-4-6


# 今後やること
## NNBattleBrain.csの編集
これダメでは？？
```cs
/// <summary>
/// `BattleBrain`の`GetAction()`関数をoverrideする.
/// 入力のうち0, 1, 2, 3, 4, 40, 42番目のみを使う.
/// </summary>
public override double[] GetAction(List<double> observation)
{
    return brain.GetAction(RearrangeObservation(observation, new List<int>{0, 1, 2, 3, 4, 40, 42}));
}
```


# 発見
NNの隠れ層をいじるとくねくねする
v4のように低速にペナをつけると速くなる