# ビルド対象プロジェクト(.NET Framework)
$projects = @(
    "./iTunesNPPlugin/iTunesNPPlugin.csproj", # ビルドにiTunes必須
    "./MusicBeeNPPlugin/MusicBeeNPPlugin.csproj"
)
foreach ($project in $projects) {
    msbuild $project /p:Configuration=Release
}

# foobar2000用のネイティブなプラグインをビルドする(vcpkg必須)
msbuild ./foo_nowplayingv2/foo_nowplayingv2/foo_nowplayingv2.vcxproj /p:Platform=Win32 /p:Configuration=Release

# .NET Core 3.0なやつをビルドする
dotnet publish ./NowPlayingV2/NowPlayingV2.csproj -c Release -o ./NowPlayingV2/bin/release-portable/
dotnet publish ./NowPlayingV2/NowPlayingV2.csproj --self-contained --runtime win-x64 -c Release -o ./NowPlayingV2/bin/release-contained/

# プラグインとかのファイルを展開する
cp ./MusicBeeNPPlugin/bin/Release/mb_MusicBeeNPPlugin.dll ./NowPlayingV2/bin/release-portable/
cp ./MusicBeeNPPlugin/bin/Release/mb_MusicBeeNPPlugin.dll ./NowPlayingV2/bin/release-contained/

cp ./NowPlayingV2/bin/Release/netcoreapp3.0/iTunesNPPlugin* ./NowPlayingV2/bin/release-portable/
cp ./NowPlayingV2/bin/Release/netcoreapp3.0/iTunesNPPlugin* ./NowPlayingV2/bin/release-contained/

cp ./foo_nowplayingv2/foo_nowplayingv2/Release/foo_nowplayingv2.dll ./NowPlayingV2/bin/release-portable/
cp ./foo_nowplayingv2/foo_nowplayingv2/Release/foo_nowplayingv2.dll ./NowPlayingV2/bin/release-contained/

# 不要な物を消す
rm ./NowPlayingV2/bin/release-portable/*.pdb
rm ./NowPlayingV2/bin/release-contained/*.pdb

# リリース用のファイルを作成する
Compress-Archive -Force -Path ./NowPlayingV2/bin/release-portable/* -DestinationPath ./NowPlayingV2/bin/release-portable.zip
Compress-Archive -Force -Path ./NowPlayingV2/bin/release-contained/* -DestinationPath ./NowPlayingV2/bin/release-contained.zip
