package com.myskng.virtualchemlight.activity

import android.content.Context
import androidx.databinding.DataBindingUtil
import android.media.AudioManager
import android.media.MediaPlayer
import android.os.Build
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.os.VibrationEffect
import android.os.Vibrator
import com.google.android.material.snackbar.Snackbar
import android.util.Log
import android.view.*
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.uo.UOSensor
import com.myskng.virtualchemlight.databinding.ActivityUoBinding
import kotlinx.coroutines.experimental.Job
import kotlinx.coroutines.experimental.android.UI
import kotlinx.coroutines.experimental.launch
import kotlin.coroutines.experimental.suspendCoroutine
import kotlin.math.absoluteValue

class UOActivity : AppCompatActivity() {
    private lateinit var binding: ActivityUoBinding

    private val parentView by lazy { findViewById<View>(android.R.id.content) }

    private lateinit var uoSensor: UOSensor
    private lateinit var uoSound: MediaPlayer
    private lateinit var vibrator: Vibrator

    private var uoLock: Boolean = false
    private val uoAnimatorList: MutableList<ViewPropertyAnimator> = mutableListOf()
    private val rootJob = Job()

    private val onGestureListener = object : GestureDetector.SimpleOnGestureListener() {
        override fun onFling(e1: MotionEvent?, e2: MotionEvent?, velocityX: Float, velocityY: Float): Boolean {
            // null check
            if (e1 == null || e2 == null) return super.onFling(e1, e2, velocityX, velocityY)
            // log
            Log.i("UO", "velocityX:${velocityX.absoluteValue}")
            Log.i("UO", "velocityY:${velocityY.absoluteValue}")
            Log.i("UO", "distanceX:${if (e2.x > e1.x) e2.x - e1.x else e1.x - e2.x}")
            Log.i("UO", "distanceY:${if (e2.y > e1.y) e2.y - e1.y else e1.y - e2.y}")
            // detect left right swipe
            val distX = if (e2.x > e1.x) e2.x - e1.x else e1.x - e2.x
            val distY = if (e2.y > e1.y) e2.y - e1.y else e1.y - e2.y
            val detected = when (true) {
                distY > 300 -> false
                velocityX > 10000 -> true
                distX > 500 -> true
                distX > 300 && velocityX > 3000 -> true
                else -> false
            }
            if (detected) onUODispose()
            return super.onFling(e1, e2, velocityX, velocityY)
        }
    }
    private lateinit var gestureDetector: GestureDetector

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_uo)
        // Binding
        binding = DataBindingUtil.setContentView(this, R.layout.activity_uo)
        // Gesture
        gestureDetector = GestureDetector(this, onGestureListener)
        // Prepare resource
        vibrator = getSystemService(Context.VIBRATOR_SERVICE) as Vibrator
        uoSound = MediaPlayer.create(this, R.raw.uosound)
        uoSensor = UOSensor(this)
        uoSensor.startSensor()
        uoSensor.onUOIgnition = this::onUOIgnition
        binding.UOimageViewMAX.alpha = 0f
        binding.UOimageViewNormal.alpha = 0f
        // UI
        window.decorView.systemUiVisibility = View.SYSTEM_UI_FLAG_HIDE_NAVIGATION
        // set screen brightness
        val lp = window.attributes
        lp.screenBrightness = WindowManager.LayoutParams.BRIGHTNESS_OVERRIDE_FULL
        window.attributes = lp
        window.addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON)
        // get volume and show notification
        val audioManager = getSystemService(Context.AUDIO_SERVICE) as AudioManager
        val musicVolume = audioManager.getStreamVolume(AudioManager.STREAM_MUSIC)
        if (musicVolume == 0) {
            with(Snackbar.make(parentView, getText(R.string.message_volume_min), Snackbar.LENGTH_LONG)) {
                this.setAction(getText(R.string.text_setvolumemax)) {
                    val maxvol = audioManager.getStreamMaxVolume(AudioManager.STREAM_MUSIC)
                    audioManager.setStreamVolume(AudioManager.STREAM_MUSIC, maxvol, AudioManager.FLAG_SHOW_UI)
                }
                this.show()
            }
        }
    }

    private fun onUOIgnition() {
        launch(UI, parent = rootJob) {
            // check if locked
            if (uoLock) return@launch
            uoLock = true
            // stop animation
            uoAnimatorList.forEach { it ->
                it.cancel()
            }
            uoAnimatorList.clear()
            // reset alpha
            binding.UOimageViewMAX.alpha = 0f
            binding.UOimageViewNormal.alpha = 0f
            // start UO
            uoSound.start()
            // vibrate
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                vibrator.vibrate(VibrationEffect.createOneShot(500, 255))
            }
            Log.i("UO", "UO START!!")
            // uo effect
            suspendCoroutine<Unit> {
                with(binding.UOimageViewMAX.animate().alpha(1.0f)) {
                    uoAnimatorList.add(this)
                    this.duration = 500
                    this.withEndAction { it.resume(Unit) }
                }
            }
            suspendCoroutine<Unit> {
                binding.UOimageViewNormal.alpha = 1.0f
                with(binding.UOimageViewMAX.animate().alpha(0.0f)) {
                    uoAnimatorList.add(this)
                    this.duration = 60 * 1000
                    this.withEndAction { it.resume(Unit) }
                }
            }
            suspendCoroutine<Unit> {
                with(binding.UOimageViewNormal.animate().alpha(0.0f)) {
                    uoAnimatorList.add(this)
                    this.duration = 60 * 2 * 1000
                    this.withEndAction { it.resume(Unit) }
                }
            }
            uoLock = false
            Log.i("UO", "UO END!!")
        }
    }

    private fun onUODispose() {
        // stop animation
        uoAnimatorList.forEach { it ->
            it.cancel()
        }
        uoAnimatorList.clear()
        // reset alpha with animation
        with(binding.UOimageViewMAX.animate().alpha(0.0f)) {
            this.duration = 500
            this.withEndAction { uoLock = false }
            uoAnimatorList.add(this)
        }
        with(binding.UOimageViewNormal.animate().alpha(0.0f)) {
            this.duration = 500
            this.withEndAction { uoLock = false }
            uoAnimatorList.add(this)
        }
    }

    override fun dispatchTouchEvent(event: MotionEvent?): Boolean {
        gestureDetector.onTouchEvent(event)
        return super.dispatchTouchEvent(event)
    }

    override fun onPause() {
        super.onPause()
        uoSensor.stopSensor()
    }

    override fun onResume() {
        super.onResume()
        uoSensor.startSensor()
    }

    override fun onDestroy() {
        super.onDestroy()
        rootJob.cancel()
    }
}
