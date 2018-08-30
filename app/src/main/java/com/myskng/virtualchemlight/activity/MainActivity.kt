package com.myskng.virtualchemlight.activity

import android.content.Intent
import androidx.databinding.DataBindingUtil
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.view.ViewPagerAdapter
import com.myskng.virtualchemlight.view.ViewPagerPresenter
import com.myskng.virtualchemlight.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {
    private var viewPagerAdapter: ViewPagerAdapter = ViewPagerAdapter()
    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        // Data binding
        binding = DataBindingUtil.setContentView(this, R.layout.activity_main)
        binding.handlers = this
        // Prepare ViewPager
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_1)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_2)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_3)))
        binding.tutorialViewpager.adapter = viewPagerAdapter
    }

    fun onAcceptClick(view: View) {
        startActivity(Intent(this, UOActivity::class.java))
    }

    fun onFabClick(view: View) {
        startActivity(Intent(this, SettingsActivity::class.java))
    }
}
