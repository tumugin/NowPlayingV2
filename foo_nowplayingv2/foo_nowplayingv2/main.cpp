#include "stdafx.h"
#include "main.h"
DECLARE_COMPONENT_VERSION("foo_nowplayingv2", "1.0", "plugin for NowPlayingTunesV2");

void MinatoYukina::on_init()
{
    QString qst = u8"–©—FŠó“ß";
    console::print("initialized foo_nowplayingv2 on MinatoYukina::on_init().");
}

void MinatoYukina::on_quit()
{
}
