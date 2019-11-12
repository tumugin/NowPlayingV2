using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using NowPlayingCore.Matsuri;
using NowPlayingCore.NowPlaying;
using NowPlayingV2.Matsuri;

namespace NowPlayingV2.UI.View
{
    public class Model4SongView : SongInfo
    {
        public Model4SongView(SongInfo sbase)
        {
            //Get the list of properties available in base class
            var properties = sbase.GetType().GetProperties();
            properties.ToList().ForEach(property =>
            {
                //Check whether that property is present in derived class
                var isPresent = this.GetType().GetProperty(property.Name);
                if (isPresent != null)
                {
                    //If present get the value and map it
                    var value = sbase.GetType().GetProperty(property.Name)!.GetValue(sbase, null);
                    this.GetType().GetProperty(property.Name)!.SetValue(this, value, null);
                }
            });
        }

        public BitmapSource? BsImage
        {
            get
            {
                try
                {
                    if (!IsAlbumArtAvaliable())
                    {
                        return null;
                    }

                    return GdiUtils.ToImageSource(GetAlbumArt()!);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}