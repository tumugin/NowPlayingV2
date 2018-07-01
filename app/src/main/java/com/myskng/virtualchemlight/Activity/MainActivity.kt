package com.myskng.virtualchemlight.Activity

import android.content.Intent
import android.databinding.DataBindingUtil
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v4.view.ViewPager
import android.view.View
import butterknife.BindView
import butterknife.ButterKnife
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.View.ViewPagerAdapter
import com.myskng.virtualchemlight.View.ViewPagerPresenter
import com.myskng.virtualchemlight.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity(), MainActivityEventHandlers {
    @BindView(R.id.tutorial_viewpager)
    lateinit var tutorialViewpager: ViewPager
    private var viewPagerAdapter: ViewPagerAdapter = ViewPagerAdapter()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        //Data binding
        val binding = DataBindingUtil.setContentView<ActivityMainBinding>(this, R.layout.activity_main)
        binding.handlers = this
        //ButterKnife
        ButterKnife.bind(this)
        //Prepare ViewPager
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_1)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_2)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_3)))
        tutorialViewpager.adapter = viewPagerAdapter
    }

    override fun onAcceptClick(view: View) {
        startActivity(Intent(this, UOActivity::class.java))
    }
}
