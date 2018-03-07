[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![GitHub release](https://img.shields.io/github/release/tumugin/NowPlayingV2.svg)
![Github All Releases](https://img.shields.io/github/downloads/tumugin/NowPlayingV2/total.svg)
[![Build status](https://ci.appveyor.com/api/projects/status/1336dspwjeqeuenj/branch/master?svg=true)](https://ci.appveyor.com/project/tumugin/nowplayingv2)

# NowPlayingV2(なうぷれTunesV2)
iTunes,MusicBee,foobar2000で#NowPlayingするWindows向けアプリ(Twitter・Mastodon対応)

# ビルド方法
## NowPlayingV2.sln
なうぷれTunesV2の本体部分と各プレイヤー向けのプラグインが入っています。

iTunes向けプラグイン(iTunesNPPlugin)のみiTunesのCOMコンポーネントがビルドする環境に登録されていないと正常にビルドできません。(必要なければビルドから外してください)

## foo_nowplayingv2/foo_nowplayingv2.sln
ビルドに静的ビルドされたQt5Coreが必要になります。無ければビルドしてプロジェクトの設定を編集してライブラリまでのパスを正しく設定してください。
