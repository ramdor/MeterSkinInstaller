//MW0LGE 26/03/23
using MeterSkinInstaller.Properties;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using System.Globalization;
using System.IO;

namespace MeterSkinInstaller
{
    public partial class frmMain : Form
    {
        private bool _bDone = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(_bDone)
            {
                tmrGo.Enabled = false;
                this.Close();
                return;
            }

            tmrGo.Enabled = false;

            Cursor c = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;            

            ResourceSet rsrcSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);

            if (rsrcSet != null)
            {
                string sMeterPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OpenHPSDR\\Meters";

                bool bOk = true;
                if (!Directory.Exists(sMeterPath))
                {
                    try
                    {
                        Directory.CreateDirectory(sMeterPath);
                    }
                    catch 
                    {
                        bOk = false;
                    }
                }

                if (bOk)
                {
                    foreach (DictionaryEntry entry in rsrcSet)
                    {
                        if (entry.Value.GetType() == typeof(Bitmap))
                        {
                            Bitmap bmp = entry.Value as Bitmap;
                            if (bmp != null)
                            {
                                //- in filenames become _ in resources, change them back
                                string sFilename = sMeterPath + "\\" + entry.Key.ToString().Replace('_', '-') + ".png";

                                try
                                {
                                    if (File.Exists(sFilename)) File.Delete(sFilename);
                                    bmp.Save(sFilename, System.Drawing.Imaging.ImageFormat.Png);
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            Cursor = c;
            _bDone = true;
            tmrGo.Enabled = true;
        }
    }
}
