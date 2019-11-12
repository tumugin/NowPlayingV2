#include "stdafx.h"
#include "main.h"
#include <QtCore/QString>
#include <QtCore/QJsonObject>
#include <QtCore/QJsonDocument>
#include <QtNetwork/QLocalSocket>

DECLARE_COMPONENT_VERSION("foo_nowplayingv2", "1.5", "plugin for NowPlayingTunesV2");

void MinatoYukina::on_init()
{
	yukina_ptr.reset(new play_callback_minato_yukina());
#ifdef _DEBUG
	console::print(
		std::string("initialized foo_nowplayingv2 debug build(" + std::string(__DATE__) + " " + std::string(__TIME__) + ")").
		c_str());
#else
	console::print(
		std::string("initialized foo_nowplayingv2 release build(" + std::string(__DATE__) + " " + std::string(__TIME__) + ")").
		c_str());
#endif
}

void MinatoYukina::on_quit()
{
}

void play_callback_minato_yukina::on_playback_new_track(metadb_handle_ptr p_track)
{
	//get playing file info
	std::unique_ptr<file_info_impl> fileInfo(new file_info_impl());
	p_track->get_info(*fileInfo);
	abort_callback_impl abortCallback;
	pfc::list_single_ref_t<metadb_handle_ptr> trackList(p_track);
	pfc::list_single_ref_t<GUID> guidList(album_art_ids::cover_front);
	auto iPtr = static_api_ptr_t<album_art_manager_v3>()->open_v3(trackList, guidList, NULL, abortCallback);
	QByteArray qCoverAry;
	try
	{
		auto dataPtr = iPtr->query(album_art_ids::cover_front, abortCallback);
		auto memPtr = dataPtr->get_ptr();
#ifdef _DEBUG
		std::stringstream ss;
		ss << memPtr;
		console::print((std::string("[foo_nowplayingv2] cover address:") + ss.str()).c_str());
		console::print((std::string("[foo_nowplayingv2] cover size:") + std::to_string(dataPtr->get_size())).c_str());
#endif
		std::unique_ptr<char[]> coverDataPtr(new char[dataPtr->get_size()]);
		memcpy(coverDataPtr.get(), dataPtr->get_ptr(), dataPtr->get_size());
		qCoverAry = QByteArray::QByteArray(coverDataPtr.get(), dataPtr->get_size());
	}
	catch (exception_album_art_not_found)
	{
#ifdef _DEBUG
		console::print("[foo_nowplayingv2] albumart not found.");
#endif
	}
	QJsonObject jsonObj{
		{"title", fileInfo->meta_exists("TITLE") ? QString(fileInfo->meta_get("TITLE", 0)) : ""},
		{"album", fileInfo->meta_exists("ALBUM") ? QString(fileInfo->meta_get("ALBUM", 0)) : ""},
		{"artist", fileInfo->meta_exists("ARTIST") ? QString(fileInfo->meta_get("ARTIST", 0)) : ""},
		{"albumartist", fileInfo->meta_exists("ALBUM ARTIST") ? QString(fileInfo->meta_get("ALBUM ARTIST", 0)) : ""},
		{"composer", fileInfo->meta_exists("COMPOSER") ? QString(fileInfo->meta_get("COMPOSER", 0)) : ""},
		{"year", fileInfo->meta_exists("DATE") ? QString(fileInfo->meta_get("DATE", 0)) : ""},
		{"albumart", QString(qCoverAry.toBase64())}
	};
	QJsonDocument jsonDoc(jsonObj);
	QByteArray sendAry = jsonDoc.toJson();
	//start send thread
	//copy sendary in lambda
	std::thread([sendAry]
	{
		QLocalSocket localSocket;
		localSocket.connectToServer("NowPlayingTunesV2PIPE", QIODevice::WriteOnly);
		if (!localSocket.waitForConnected(1000))
		{
			console::print("[foo_nowplayingv2] could not connect to socket.");
			return;
		}
		localSocket.write(sendAry);
		localSocket.waitForBytesWritten(5000);
		localSocket.disconnectFromServer();
	}).detach();
}
