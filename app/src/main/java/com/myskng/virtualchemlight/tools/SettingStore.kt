package com.myskng.virtualchemlight.tools

import android.content.Context
import android.content.SharedPreferences
import android.preference.PreferenceManager

class SettingStore(val context: Context) {
    private val sharedPreferences: SharedPreferences = PreferenceManager.getDefaultSharedPreferences(context)

    var uoForce: Float
        get() {
            return sharedPreferences.getString("pref_key_uoforce", "40").toFloat()
        }
        set(value) {
            sharedPreferences.edit().putString("pref_key_uoforce", value.toString()).apply()
        }
}