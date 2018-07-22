package com.myskng.virtualchemlight.Activity

import android.annotation.TargetApi
import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.content.res.Configuration
import android.media.RingtoneManager
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.preference.*
import android.text.TextUtils
import android.view.MenuItem
import com.myskng.virtualchemlight.R

class SettingsActivity : AppCompatPreferenceActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setupActionBar()
        fragmentManager.beginTransaction().replace(android.R.id.content, SettingFragment()).commit()
    }

    private fun setupActionBar() {
        supportActionBar?.setDisplayHomeAsUpEnabled(true)
    }

    class SettingFragment : PreferenceFragment(), SharedPreferences.OnSharedPreferenceChangeListener {
        val sharedPreferences: SharedPreferences by lazy {
            preferenceManager.sharedPreferences
        }

        override fun onCreate(savedInstanceState: Bundle?) {
            super.onCreate(savedInstanceState)
            addPreferencesFromResource(R.xml.pref_screen)
            sharedPreferences.registerOnSharedPreferenceChangeListener(this)
            sharedPreferences.all.forEach { item ->
                val prefobj = findPreference(item.key)
                if (prefobj is EditTextPreference) prefobj.summary = item.value.toString()
            }
        }

        override fun onSharedPreferenceChanged(sharedPreferences: SharedPreferences?, key: String?) {
            val prefmap = sharedPreferences!!.all
            val changedpref = prefmap[key]
            val prefobj = findPreference(key)
            if (prefobj is EditTextPreference) {
                prefobj.summary = changedpref.toString()
            }
        }

        override fun onDestroy() {
            super.onDestroy()
            sharedPreferences.unregisterOnSharedPreferenceChangeListener(this)
        }
    }
}
