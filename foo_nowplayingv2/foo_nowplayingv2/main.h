#pragma once
#include "stdafx.h"
#include <QtCore/QString>

class MinatoYukina : public initquit {
public:
    void on_init() override;
    void on_quit() override;
};

static initquit_factory_t<MinatoYukina> minatoyukina_factory;