Felica Cashing System V2
========================

## 概要
非接触 IC カード FeliCa を用いた組織内電子決済システムです。

|スクリーンショット|ポスター|
|------------------|--------|
|[![スクリーンショット](SS-min.png)](SS.png)|[![ポスター](Poster-min.png)](http://cdn.rawgit.com/pine613/FelicaCashingSystemV2/master/Poster.pdf)|

## 開発言語
- C#
- XAML

## 開発環境
- Windows 7 / 8.1
- Visual Studio 2013 Professional
- SONY PaSoRi RC-S380

## 利用しているライブラリ
- .NET Framework 4.5
  - Windows Forms (一部のみ)
  - WPF
- MashApps.Metro ([カスタマイズ版](https://github.com/pine613/MahApps.Metro/tree/felica_master) を使用)
- [MongoDB](http://www.mongodb.org) (v2.6)
- iTextSharp
- Adobe Acrobat 7.0 Browser Control Type Library 1.0
- PC/SC (WinScard.dll)
- NuGet

## ビルド
ソリューションを Visual Studio で開き、構成を **Release** にしてビルドしてください。ビルドには、Adobe Reader と NuGet コマンドラインツール、及び PowerShell がインストールされている必要があります。

## インストール
ビルド結果をインストール先にコピーしてください。
起動には、Adobe Reader と PaSoRi のドライバが必要です。

## 関係するプロジェクト
- [FelicaSharp](https://github.com/pine613/FelicaSharp)<br />
  カードリーダー (PaSoRi) を C# から利用するためのライブラリ
- [FelicaDataV2](https://github.com/pine613/FelicaDataV2)<br />
  Felica Cashing System V2 のデータベースレイヤを担当するモジュール
- [KutDormitoryReport](https://github.com/pine613/KutDormitoryReport)<br />
  高知工科大学ドミトリーの門限超過届けを自動発行する部分のモジュール
- [FelicaCashingSystemV2_Settings](https://github.com/RobotClubKut/FelicaCashingSystemV2_Settings)<br />
  ロボット倶楽部で運用している Felica Cashing System V2 の設定 (非公開)