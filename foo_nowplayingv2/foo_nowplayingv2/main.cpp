#include "stdafx.h"
#include "main.h"
DECLARE_COMPONENT_VERSION("foo_nowplayingv2", "1.0", "plugin for NowPlayingTunesV2");

void MinatoYukina::on_init()
{
    yukina_ptr.reset(new play_callback_minato_yukina());
    console::print("initialized foo_nowplayingv2 on MinatoYukina::on_init().");
}

void MinatoYukina::on_quit()
{
}

void play_callback_minato_yukina::on_playback_new_track(metadb_handle_ptr p_track)
{
    //get playing file info
    file_info_impl fileinfo;
    p_track->get_info(fileinfo);
    QJsonObject jsonobj{
        {"title",fileinfo.meta_exists("TITLE") ? QString(fileinfo.meta_get("TITLE",0)) : "" },
        {"album",fileinfo.meta_exists("ALBUM") ? QString(fileinfo.meta_get("ALBUM",0)) : "" },
        {"artist",fileinfo.meta_exists("ARTIST") ? QString(fileinfo.meta_get("ARTIST",0)) : "" },
        {"albumartist",fileinfo.meta_exists("ALBUM ARTIST") ? QString(fileinfo.meta_get("ALBUM ARTIST",0)) : "" }
    };
    QJsonDocument jsondoc(jsonobj);
    QByteArray sendary = jsondoc.toJson();
    //start send thread
    //copy sendary in lambda
    std::thread([sendary] {
        QLocalSocket ls;
        ls.connectToServer("NowPlayingTunesV2PIPE", QIODevice::WriteOnly);
        if (!ls.waitForConnected(1000)) {
            console::print("could not connect to socket.");
            return;
        }
        ls.write(sendary);
        ls.waitForBytesWritten(5000);
        ls.disconnectFromServer();
    }).detach();
}
