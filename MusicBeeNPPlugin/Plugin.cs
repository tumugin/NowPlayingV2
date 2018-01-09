using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "NowPlayingV2 Plugin";
            about.Description = "plugin for NowPlayingTunesV2.";
            about.Author = "Kazuki Oishi";
            about.TargetApplication = "";   // current only applies to artwork, lyrics or instant messenger name that appears in the provider drop down selector or target Instant Messenger
            about.Type = PluginType.General;
            about.VersionMajor = 1;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 1;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                //Does not have any config for now.

                //Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                //Label prompt = new Label();
                //prompt.AutoSize = true;
                //prompt.Location = new Point(0, 0);
                //prompt.Text = "prompt:";
                //TextBox textBox = new TextBox();
                //textBox.Bounds = new Rectangle(60, 0, 100, textBox.Height);
                //configPanel.Controls.AddRange(new Control[] { prompt, textBox });
            }
            return false;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        public void SaveSettings()
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
        }

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.TrackChanged:
                    var sendmap = new Dictionary<string, string>();
                    sendmap.Add("title", mbApiInterface.NowPlaying_GetFileTag(MetaDataType.TrackTitle));
                    sendmap.Add("albumartist", mbApiInterface.NowPlaying_GetFileTag(MetaDataType.AlbumArtist));
                    sendmap.Add("artist", mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist));
                    sendmap.Add("trackcount", mbApiInterface.NowPlaying_GetFileProperty(FilePropertyType.PlayCount));
                    sendmap.Add("album", mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Album));
                    sendmap.Add("albumart", mbApiInterface.NowPlaying_GetArtwork());
                    sendmap.Add("composer", mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Composer));
                    var json = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue }.Serialize(sendmap.ToDictionary(item => item.Key.ToString(), item => item.Value.ToString()));
                    Task.Run(() =>
                    {
                        try
                        {
                            var bary = Encoding.UTF8.GetBytes(json);
                            var pipe = new NamedPipeClientStream("NowPlayingTunesV2PIPE");
                            pipe.Connect(1000); //set timeout 1000msec.
                            pipe.Write(bary, 0, bary.Count());
                            pipe.Close();
                        }
                        catch { }
                    });
                    break;
            }
        }

        // return an array of lyric or artwork provider names this plugin supports
        // the providers will be iterated through one by one and passed to the RetrieveLyrics/ RetrieveArtwork function in order set by the user in the MusicBee Tags(2) preferences screen until a match is found
        public string[] GetProviders()
        {
            return null;
        }

        // return lyrics for the requested artist/title from the requested provider
        // only required if PluginType = LyricsRetrieval
        // return null if no lyrics are found
        public string RetrieveLyrics(string sourceFileUrl, string artist, string trackTitle, string album, bool synchronisedPreferred, string provider)
        {
            return null;
        }

        // return Base64 string representation of the artwork binary data from the requested provider
        // only required if PluginType = ArtworkRetrieval
        // return null if no artwork is found
        public string RetrieveArtwork(string sourceFileUrl, string albumArtist, string album, string provider)
        {
            //Return Convert.ToBase64String(artworkBinaryData)
            return null;
        }
    }
}
