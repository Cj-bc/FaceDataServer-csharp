English: [README.md](README.md)

---

# FaceDataServer-Unity

Unity用のアセット・スクリプトです。
現在はVRMのみに対応しています。

# Unityで使う

現状実装されている、VRMにデータを適用する使い方を紹介します。

## 1. 準備

1. Releaseからunitypackageを取得します。
2. [FaceDataServerComponent.cs](FaceDataServerComponent.cs)をオブジェクトにアタッチします。
   なんのオブジェクトにアタッチしても構いません。
3. [ApplyFaceDataToVRM.cs](ApplyFaceDataToVRM.cs)を__VRMモデルのルートオブジェクトに__アタッチします。
4. VRMモデルに使用する`AnimatorController`をProjectメニューから選択し、メニューアイテムの'FaceDataServer/Inject Required Layer & Paramertes'をクリックして実行します。
   これにより、必要なlayerとAnimation Parametersが自動的に設定されます。

## 2. ランタイム時

[FaceDataServerComponent.cs](FaceDataServerComponent.cs)が自動的にバックエンドからデータを受け取ります。  
[ApplyFaceDataToVRM.cs](ApplyFaceDataToVRM.cs)が自動的にデータをモデルに適用します。
特別することはありません。
