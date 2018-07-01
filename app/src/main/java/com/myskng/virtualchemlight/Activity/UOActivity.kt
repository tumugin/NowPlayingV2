package com.myskng.virtualchemlight.Activity

import android.content.Context
import android.media.MediaPlayer
import android.os.Build
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.os.VibrationEffect
import android.os.Vibrator
import android.util.Log
import android.view.ViewPropertyAnimator
import android.widget.ImageView
import butterknife.BindView
import butterknife.ButterKnife
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.UO.UOSensor

class UOActivity : AppCompatActivity() {
    @BindView(R.id.UOimageViewMAX)
    lateinit var uoImageViewMAX: ImageView
    @BindView(R.id.UOimageViewNormal)
    lateinit var uoImageViewNormal: ImageView
    @BindView(R.id.UOimageViewOFF)
    lateinit var uoImageViewOFF: ImageView

    lateinit var uoSensor: UOSensor
    lateinit var uoSound: MediaPlayer
    lateinit var vibrator: Vibrator

    val uoLock: Boolean = false
    val uoAnimaterList: MutableList<ViewPropertyAnimator> = mutableListOf()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_uo)
        //ButterKnife
        ButterKnife.bind(this)
        //Prepare resource
        vibrator = getSystemService(Context.VIBRATOR_SERVICE) as Vibrator
        uoSound = MediaPlayer.create(this, R.raw.uosound)
        uoSensor = UOSensor(this)
        uoSensor.startSensor()
        uoSensor.onUOIgnition = { onUOIgnition() }
        uoImageViewMAX.alpha = 0f
        uoImageViewNormal.alpha = 0f
    }

    private fun onUOIgnition() {
        //stop animation
        uoAnimaterList.forEach { it ->
            it.cancel()
        }
        uoAnimaterList.clear()
        //reset alpha
        uoImageViewMAX.alpha = 0f
        uoImageViewNormal.alpha = 0f
        //start UO
        uoSound.start()
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            vibrator.vibrate(VibrationEffect.createOneShot(500, 255))
        }
        val anm1 = uoImageViewMAX.animate().alpha(1.0f)
        uoAnimaterList.add(anm1)
        anm1.duration = 500
        anm1.withEndAction {
            uoImageViewNormal.alpha = 1.0f
            val anm2 = uoImageViewMAX.animate().alpha(0.0f)
            uoAnimaterList.add(anm2)
            anm2.duration = 60 * 1000
            anm2.withEndAction {
                val anm3 = uoImageViewNormal.animate().alpha(0.0f)
                uoAnimaterList.add(anm3)
                anm3.duration = 60 * 2 * 1000
            }
        }
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
