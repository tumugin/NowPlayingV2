package com.myskng.virtualchemlight.UO

import android.content.Context
import android.hardware.Sensor
import android.hardware.SensorEvent
import android.hardware.SensorEventListener
import android.hardware.SensorManager
import android.util.Log
import kotlin.math.absoluteValue

class UOSensor(val context: Context) : SensorEventListener {
    var onUOIgnition: (() -> Unit)? = null
    var sensorManager: SensorManager = context.getSystemService(Context.SENSOR_SERVICE) as SensorManager

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
        val avg = arrayOf(accX.absoluteValue, accY.absoluteValue, accZ.absoluteValue).average()
        if (accX.absoluteValue >= 45) {
            onUOIgnition?.invoke()
        }
    }
}