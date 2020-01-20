using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction.Helper
{
    public class VolumeSizeHelper
    {
        private static VolumeSizeHelper _soundHelper;
        public static VolumeSizeHelper Instance => _soundHelper ?? (_soundHelper = new VolumeSizeHelper());

        public CoreAudioDevice defaultPlaybackDevice { get; set; }= new CoreAudioController().DefaultPlaybackDevice;

        ~VolumeSizeHelper() 
        {
            defaultPlaybackDevice?.Dispose();
        }
    }
}
