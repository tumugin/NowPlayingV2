package com.myskng.virtualchemlight.view

import androidx.viewpager.widget.PagerAdapter
import android.view.View
import android.view.ViewGroup

class ViewPagerAdapter : PagerAdapter() {
    override fun destroyItem(container: ViewGroup, position: Int, `object`: Any) {
        container.removeView(`object` as View)
    }

    var PresenterList: MutableList<ViewPagerPresenter> = mutableListOf()

    override fun instantiateItem(container: ViewGroup, position: Int): Any {
        val view = PresenterList.get(position).view
        //workaround 何故か最後のアイテムだけ最初からどこかに所属している状態になるので外してやる
        (view.parent as ViewGroup?)?.removeView(view)
        container.addView(view)
        return view
    }

    override fun isViewFromObject(view: View, `object`: Any): Boolean {
        return view === `object`
    }

    override fun getCount(): Int {
        return PresenterList.count()
    }
}