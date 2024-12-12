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


## v6
0~14,40,42, 43,44,45
50,70,90,110,130

// WayPoint通過時に報酬を与える
AddReward(1.0f / (WaypointIndex + 1));

plusrewardをtrueに
currentMaxのとき到達距離の評価を*1

velocity<10なら-0.1
Donewithrewardの中身をaddrewardにする
全部-1に

AddReward(0.01f * TotalDistance); // 前進した距離に応じて微小な報酬を付与

ランダム報酬

100-85-4, 8-1-4-6

## v7
0~14,40,42, 43,44,45
50,70,90,110,130
100-85-4, 8-1-4-6
v6をsetRewardに
最強（96びょう）


## v8

v7のまま

var v = CarRb.velocity.magnitude;
        if (v < 5) {
            AddReward(-0.04f);
        }
        else if (v < 10) {
            AddReward(-0.02f);
        };

        if (UnityEngine.Random.Range(0, 100) < 5) { // ランダムなイベント（5%）
            AddReward(0.01f); // 探索報酬
        }

        // WayPoint通過時に報酬を与える
        AddReward(2.0f / (WaypointIndex + 1));


        if (CurrentStep % 50 == 0) { // 50ステップごとに進行状況に応じた報酬
            AddReward(0.1f * TotalDistance);
        }

早かったが壁にぶつかって死んだ

## v9
逆走2倍、waypoint通過時の報酬を5倍
速度報酬を2倍
他の設定は一緒
くねくねして死んだ

## v10
v7の速度補正だけ段階的に変更
右側79s、左側死ぬ
くねくねする
最初のカーブでアウトを走っている->インコースを走ると＋評価できるようにすれば？

## v11
v11に方向評価を追加
30世代まで
最初の岩で死ぬ

## v12
0~14,40,41,42,43,44,45
v11の方向評価、速度評価を極端＋正の報酬中心にした
100-85-8, 16-4-4-8

## v13
0~14,40,41,42,43,44,45
v11の方向評価、速度評価を極端＋正の報酬中心にした
100-85-8, 8-1-4-6
v7よりちょっとだけ速くなった
坂で減速

## v14
0~14,40,41,42,43,44,45
100-85-4, 8-1-4-6
方向評価消す
速度、距離の評価方式を変更
AddReward(0.001f*v)
AddReward(0.01f * TotalDistance); // 前進した距離に応じて微小な報酬を付与
右側だと72s

## v15
v14と同じ
横は1030507090
めっちゃ早かったが終盤ぶつかって死んだ

## v16
v14と同じ
横は10,30,50,70,90
後ろ10,20,160,170の4つ
element18まで、19個
->失敗ぽかったので
後ろ20,160の2つにした
->ましにはなったが失敗っぽい
後ろ10,170の2つにした
->完全失敗
15,165の2つにした
->だめっぽい

後ろ30,150の2つにした
->めっちゃいい25世代までやってv16として記録
早かったけど最初のカーブで死んだ

## v17
後ろ20,30,150,160にした
->微妙

後ろ30,150の2つにした
16-1-4-6

最後の岩に衝突して死んだ

## v18
v17に
角度報酬（小さめ追加）
12-1-4-6
->微妙

角度報酬消して
12-1-4-6の速度報酬強め（最初の100は速度報酬なし）

## v19
角度報酬消して
6-1-4-6の速度報酬強め
（最初の100とそれ以降で速度報酬変える）
感想はできたがv7に速度で負けた

## v20
v19と同じ
6-1-4-10
急ハンドルに罰則

## v21
v19と同じ
12-1-4-8
急ハンドルに罰則

## v22
v19と同じ
8-1-4-8
急ハンドルの罰則無し、坂道でのアクセルに報酬

右：72秒！最速！！
左：70秒！最速！！

これを中間発表に提出する


## v23
v19と同じ
8-1-4-8

次、次の次まで取得
0~16,40~51

→ゴールはできたが96秒と遅い
→要素として与えるとしてもzはいらないかも、もしくは報酬を別途つけてみる？


## v24
v23と同じ
8-1-4-8
次、次の次まで取得
0~16,40~42,43,44,46,47,49,50（目標のz成分は計算に入れないことにした）

真っ直ぐのときアクセル-ブレーキ/2の報酬を与える
遅い

## v25
v23と同じ
8-1-4-8
次、次の次まで取得
0~16,40~42,43,44,46,47,49,50（目標のz成分は計算に入れないことにした）

真っ直ぐのときアクセル-ブレーキ*2の報酬を与える
→80秒


## v26
v23と同じ
8-1-4-8
次、次の次まで取得
0~16,40~42,43,44,46,47,49,50（目標のz成分は計算に入れないことにした）

上り坂でアクセル-ブレーキ*2の報酬を与える
真っ直ぐのときアクセル-ブレーキ*3の報酬を与える
challenge2で実行
左：79秒でゴール
右；途中で逆走


## v27
v26と同じ
currentstepmax = 10000
8-4-4-8

## v28
今までの努力が水の泡になった
走りきれない
currentstepmax=3000

## v29
waypoint通過時の報酬を10に
12-2-4-8
100-70-10
currentstepmax=3000


## v30
currentstepmaxを動的に変更
→過学習で動かない

## v31
ここからchallenge3
currentstepmaxを動的に変更(+250 , <50000)
16-4-3-8


## v32
challenge4
8-1-4-8
→微妙


## v33
challenge3
単純にする
currentstepmaxを動的に変更(+100 , <5000)
localstepmaxを100に
Reward+TotalDistance/2

0~14,15,16,40,41,42,43,44,46,47
角度は一緒
8-1-3-8

→過去最速に早かったがカーブを曲がりきれず

## v34
challenge2
currentstepmaxを動的に変更(+100 , <5000)
localstepmaxを150に
Reward+TotalDistance/2

0~14,15,16,40,41,42,43,44,46,47
角度
60-70-90-110-120
10-20-40-60-80
20-160

100-85-4
8-1-3-8

→微妙！！

## v35
challenge2
currentstepmaxを動的に変更(+100 , <5000)
localstepmaxを150に
Reward+TotalDistance/2
坂道のアクセルの評価値を5倍に

0~14,15,16,17,18,40,41,42,43,44,46,47
角度
60-70-90-110-120
10-20-40-60-80
0-30-150-180

100-85-4
8-2-3-8



# 今後やること
## NNBattleBrain.csの編集
急ハンドルに罰則をつける
逆になめらかな変化に報酬を与える
→できそう。閾値は要検討
角度の調整→微妙だった
角速度に罰則をつける？
z軸方向の速度を取得し、上り坂でアクセルを踏むことに報酬を与える
できそう。閾値は要検討

斜めに走らせる（何個か後のポイントの方向にたいして報酬を与えてみる）

# 発見
NNの隠れ層をいじるとくねくねする
v4のように低速にペナをつけると速くなる