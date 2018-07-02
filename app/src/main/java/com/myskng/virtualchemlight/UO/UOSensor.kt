package com.myskng.virtualchemlight.UO

import android.content.Context
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.util.Log
import java.util.*
import kotlin.math.absoluteValue

class UOSensor(val context: Context) : SensorEventListener {
    var onUOIgnition: (() -> Unit)? = null
    var sensorManager: SensorManager = context.getSystemService(Context.SENSOR_SERVICE) as SensorManager
    private var lastDate: Date = Date()

    fun startSensor() {
        val sensor = sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER)
        sensorManager.registerListener(this, sensor, SensorManager.SENSOR_DELAY_GAME)
    }

    fun stopSensor() {
        sensorManager.unregisterListener(this)
    }

    override fun onAccuracyChanged(sensor: Sensor?, accuracy: Int) {
        //DO NOTHING
    }

    override fun onSensorChanged(event: SensorEvent?) {
        val accX = event!!.values[0]
        val accY = event.values[1]
        val accZ = event.values[2]
        val max = arrayOf(accX.absoluteValue, accY.absoluteValue, accZ.absoluteValue).max()!!
        if (max.absoluteValue >= 40) {
            //前回の点火から2秒以上経過している時のみイベントを発生させる
            if ((Date().time - lastDate.time) > 2000) {
                onUOIgnition?.invoke()
                lastDate = Date()
            }
        }
    }
}