using System;
using Foundation;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScriptingBridge;
using NowPlayingCore.NowPlaying;
using System.IO;

namespace NowPlaying4Mac.MacEventListener
{
    public class NotificationCenterListener
    {
        private NSNotificationCenter center;
        private List<NSObject> observerList = new List<NSObject>();

        public NotificationCenterListener()
        {
            center = NSDistributedNotificationCenter.DefaultCenter as NSNotificationCenter;
        }

        public void StartListener()
        {
            observerList.Add(center.AddObserver(new NSString("com.swinsian.Swinsian-Track-Playing"), async obj =>
            {
                var dict = obj.UserInfo;
                await Task.Run(() =>
                {
                    var npdict = new Dictionary<string, string>();
                    npdict.Add(nameof(SongInfo.Title), dict["title"] as NSString);
                    //Can't get this property
                    npdict.Add(nameof(SongInfo.AlbumArtist), "");
                    npdict.Add(nameof(SongInfo.Album), dict["album"] as NSString);
                    //Can't get this property
                    npdict.Add(nameof(SongInfo.TrackCount), "0");
                    npdict.Add(nameof(SongInfo.Artist), dict["artist"] as NSString);
                    npdict.Add(nameof(SongInfo.Composer), dict["composer"] as NSString);
                    npdict.Add(nameof(SongInfo.Year), dict["year"] as NSString);
                    npdict.Add(nameof(SongInfo.Group), dict["grouping"] as NSString);
                    var apath = dict["thumbnailPath"] as NSString;
                    npdict.Add(nameof(SongInfo.AlbumArtPath), Path.GetDirectoryName(apath) + "/cover_512.png");
                    var songinfo = new SongInfo(npdict);
                });
            }));

            observerList.Add(center.AddObserver(new NSString("com.apple.iTunes.playerInfo"), async obj =>
            {
                var dict = obj.UserInfo;
                await Task.Run(() =>
                {
                    var npdict = new Dictionary<string, string>();
                    npdict.Add(nameof(SongInfo.Title), dict["Name"] as NSString);
                    npdict.Add(nameof(SongInfo.AlbumArtist), dict["Album Artist"] as NSString);
                    npdict.Add(nameof(SongInfo.Album), dict["Album"] as NSString);
                    npdict.Add(nameof(SongInfo.TrackCount), dict["Play Count"] as NSString);
                    npdict.Add(nameof(SongInfo.Artist), dict["Artist"] as NSString);
                    //Can't get those property
                    npdict.Add(nameof(SongInfo.Composer), "");
                    npdict.Add(nameof(SongInfo.Year), "");
                    npdict.Add(nameof(SongInfo.Group), "");
                    //get album art
                    var itunes = SBApplication.FromBundleIdentifier("com.apple.iTunes");
                    var ctrack = itunes.ValueForKey(new NSString("currentTrack"));
                    var front_artwork = ctrack.ValueForKey(new NSString("artworks")) as SBElementArray;
                    if (front_artwork.Count > 0)
                    {
                        var aitem = front_artwork.Get()[0];
                        var raw_image = aitem.ValueForKey(new NSString("rawData")) as NSData;
                        npdict.Add(nameof(SongInfo.AlbumArtBase64), raw_image.GetBase64EncodedString(NSDataBase64EncodingOptions.None));
                    }
                    var songinfo = new SongInfo(npdict);
                });
            }));
        }

        public void StopListener(){
            observerList.ForEach(observer => {
                center.RemoveObserver(observer);
            });
            observerList.Clear();
        }
    }
}
