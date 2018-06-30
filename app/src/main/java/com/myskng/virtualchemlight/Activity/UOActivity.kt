package com.myskng.virtualchemlight.Activity

import android.media.MediaPlayer
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.UO.UOSensor

class UOActivity : AppCompatActivity() {
    lateinit var uoSensor: UOSensor
    lateinit var uoSound: MediaPlayer

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_uo)
        uoSound = MediaPlayer.create(this, R.raw.uosound)
        uoSensor = UOSensor(this)
        uoSensor.startSensor()
        uoSensor.onUOIgnition = { onUOIgnition() }
    }

    fun onUOIgnition() {
        uoSound.start()
        Log.i("UO", "UO START!!")
    }

    override fun onPause() {
        super.onPause()
        uoSensor.stopSensor()
    }

    override fun onResume() {
        super.onResume()
        uoSensor.startSensor()
    }
}
