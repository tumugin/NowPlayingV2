#pragma once
#include "stdafx.h"

class play_callback_minato_yukina : public play_callback {
public:
    play_callback_minato_yukina(unsigned p_flags = ~0) {
        static_api_ptr_t<play_callback_manager>()->register_callback(this, p_flags, false);
    }
    ~play_callback_minato_yukina() {
        static_api_ptr_t<play_callback_manager>()->unregister_callback(this);
    }
    void play_callback_reregister(unsigned flags, bool refresh = false) {
        static_api_ptr_t<play_callback_manager> api;
        api->unregister_callback(this);
        api->register_callback(this, flags, refresh);
    }
    void on_playback_starting(play_control::t_track_command p_command, bool p_paused) {}
    void on_playback_new_track(metadb_handle_ptr p_track);
    void on_playback_stop(play_control::t_stop_reason p_reason) {}
    void on_playback_seek(double p_time) {}
    void on_playback_pause(bool p_state) {}
    void on_playback_edited(metadb_handle_ptr p_track) {}
    void on_playback_dynamic_info(const file_info & p_info) {}
    void on_playback_dynamic_info_track(const file_info & p_info) {}
    void on_playback_time(double p_time) {}
    void on_volume_change(float p_new_val) {}
};

class MinatoYukina : public initquit {
public:
    void on_init() override;
    void on_quit() override;
private:
    std::unique_ptr<play_callback_minato_yukina> yukina_ptr;
};

static initquit_factory_t<MinatoYukina> minatoyukina_factory;