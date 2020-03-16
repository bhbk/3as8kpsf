using System;
using System.Collections;
using System.IO;
using System.Media;
using System.Resources;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.Audio
{
    public class wav
    {
        private static SoundPlayer jukie;
        public static Boolean Play(String file)
        {
//            Stream sound = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Clip_TheMatrix");
            try
            {
                jukie = new SoundPlayer(file);
                jukie.Play();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
            return true;
        }
        public static Boolean Stop()
        {
            try
            {
                jukie.Stop();
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}
