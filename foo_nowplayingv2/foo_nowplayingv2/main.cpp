#include "stdafx.h"
#include "main.h"
#include <QtCore/QString>
#include <QtCore/QJsonObject>
#include <QtCore/QJsonDocument>
#include <QtNetwork/QLocalSocket>

DECLARE_COMPONENT_VERSION("foo_nowplayingv2", "1.4", "plugin for NowPlayingTunesV2");

void MinatoYukina::on_init()
{
    yukina_ptr.reset(new play_callback_minato_yukina());
    console::print(
        std::string("initialized foo_nowplayingv2 build(" + std::string(__DATE__) + " " + std::string(__TIME__) + ")").
        c_str());
}

void MinatoYukina::on_quit()
{
}

void play_callback_minato_yukina::on_playback_new_track(metadb_handle_ptr p_track)
{
    //get playing file info
    file_info_impl fileinfo;
    p_track->get_info(fileinfo);
    abort_callback_impl abortcallback;
    pfc::list_single_ref_t<metadb_handle_ptr> tracklist(p_track);
    pfc::list_single_ref_t<GUID> guidlist(album_art_ids::cover_front);
    auto iptr = static_api_ptr_t<album_art_manager_v3>()->open_v3(tracklist, guidlist, NULL, abortcallback);
    QByteArray qcoverary;
    try
    {
        auto dataptr = iptr->query(album_art_ids::cover_front, abortcallback);
        auto memptr = dataptr->get_ptr();
        std::stringstream ss;
        ss << memptr;
        console::print((std::string("cover address:") + ss.str()).c_str());
        console::print((std::string("cover size:") + std::to_string(dataptr->get_size())).c_str());
        std::unique_ptr<char[]> coverdataptr(new char[dataptr->get_size()]);
        memcpy(coverdataptr.get(), dataptr->get_ptr(), dataptr->get_size());
        qcoverary = QByteArray::QByteArray(coverdataptr.get(), dataptr->get_size());
    }
    catch (exception_album_art_not_found)
    {
        console::print("albumart not found");
    }
    QJsonObject jsonobj{
        {"title", fileinfo.meta_exists("TITLE") ? QString(fileinfo.meta_get("TITLE", 0)) : ""},
        {"album", fileinfo.meta_exists("ALBUM") ? QString(fileinfo.meta_get("ALBUM", 0)) : ""},
        {"artist", fileinfo.meta_exists("ARTIST") ? QString(fileinfo.meta_get("ARTIST", 0)) : ""},
        {"albumartist", fileinfo.meta_exists("ALBUM ARTIST") ? QString(fileinfo.meta_get("ALBUM ARTIST", 0)) : ""},
        {"composer", fileinfo.meta_exists("COMPOSER") ? QString(fileinfo.meta_get("COMPOSER", 0)) : ""},
        {"year", fileinfo.meta_exists("DATE") ? QString(fileinfo.meta_get("DATE", 0)) : ""},
        {"albumart", QString(qcoverary.toBase64())}
    };
    QJsonDocument jsondoc(jsonobj);
    QByteArray sendary = jsondoc.toJson();
    //start send thread
    //copy sendary in lambda
    std::thread([sendary]
    {
        QLocalSocket ls;
        ls.connectToServer("NowPlayingTunesV2PIPE", QIODevice::WriteOnly);
        if (!ls.waitForConnected(1000))
        {
            console::print("could not connect to socket.");
            return;
        }
        ls.write(sendary);
        ls.waitForBytesWritten(5000);
        ls.disconnectFromServer();
    }).detach();
}
