using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace iTunesNPPlugin
{
    /// <summary>
    /// COMオブジェクトを解放する機能を提供します。
    /// </summary>
    public static class ComRelease
    {
        /// <summary>
        /// 複数のCOMオブジェクトの参照カウントを０までデクリメントし、解放します。
        /// </summary>
        /// <param name="objects">解放するCOMオブジェクトの配列。</param>
        /// <remarks>解放は配列の要素順に行います。</remarks>
        public static void FinalReleaseComObjects(params object[] objects)
        {
            foreach (object o in objects)
            {
                try
                {
                    if (o == null)
                        continue;
                    if (Marshal.IsComObject(o) == false)
                        continue;
                    Marshal.FinalReleaseComObject(o);
                    //System.GC.Collect();
                    //System.GC.WaitForPendingFinalizers();
                    //System.GC.Collect();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
