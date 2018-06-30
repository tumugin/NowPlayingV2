package com.myskng.virtualchemlight.Activity

import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v4.view.ViewPager
import butterknife.BindView
import butterknife.ButterKnife
import com.myskng.virtualchemlight.R
import com.myskng.virtualchemlight.View.ViewPagerAdapter
import com.myskng.virtualchemlight.View.ViewPagerPresenter

class MainActivity : AppCompatActivity() {
    @BindView(R.id.tutorial_viewpager)
    lateinit var tutorialViewpager: ViewPager
    private var viewPagerAdapter: ViewPagerAdapter = ViewPagerAdapter()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        ButterKnife.bind(this)
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_1)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_2)))
        viewPagerAdapter.PresenterList.add(ViewPagerPresenter(findViewById(R.id.tutorial_viewpager_layout_3)))
        tutorialViewpager.adapter = viewPagerAdapter
    }
}
