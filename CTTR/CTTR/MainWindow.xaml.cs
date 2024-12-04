using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CTTR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            bmp = new Bitmap(100,100);
            savedbmp = new Bitmap(100, 100);
            savedbmp2 = new Bitmap(100, 100);
            btnStop.IsEnabled = false;
            bst = 0;
            lst = 0;
            tbxBSV.IsEnabled = false;
            tbxLSV.IsEnabled = false;

            //create files if they're missing
            if(!File.Exists("saved.bmp"))
            {
                savedbmp.Save("saved.bmp");
            }

            if(!File.Exists("saved2.bmp"))
            {
                savedbmp.Save("saved2.bmp");
            }

            if (!File.Exists("settings.ini"))
            {
                string[] tmpSettings = new string[] { "0", "0", "100", "100", "0", "0", "30" };
                File.WriteAllLines("settings.ini", tmpSettings);
            }
            else //otherwise load settings
            {
                string[] settings = File.ReadAllLines("settings.ini");

                textBox.Text = settings[0];
                textBox1.Text = settings[1];
                textBox2.Text = settings[2];
                textBox3.Text = settings[3];
                tbxBST.Text = settings[4];
                tbxLST.Text = settings[5];
                fps = Convert.ToInt32(settings[6]);
            }
            
        }

        Timer timer;
        Bitmap bmp;
        Bitmap savedbmp;
        Bitmap savedbmp2;
        int bst, lst;
        int fps;

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            bst = Convert.ToInt32(tbxBST.Text);
            lst = Convert.ToInt32(tbxLST.Text);
            tbxBST.IsEnabled = false;
            tbxLST.IsEnabled = false;
            savedbmp = new Bitmap("saved.bmp");
            savedbmp2 = new Bitmap("saved2.bmp");
            timer = new Timer(1000/fps);
            timer.Elapsed += OnTimedEvent;

            timer.Start();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            tbxBST.IsEnabled = true;
            tbxLST.IsEnabled = true;
            savedbmp.Dispose();
            savedbmp2.Dispose();
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                //set default values
                int x = 0;
                int y = 0;
                int w = 100;
                int h = 100;

                //attempt to read values from UI
                Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        x = Convert.ToInt32(textBox.Text);
                    }
                    catch (Exception ex)
                    {
                        x = 0;
                    }

                    try
                    {
                        y = Convert.ToInt32(textBox1.Text);
                    }
                    catch (Exception ex)
                    {
                        y = 0;
                    }
                    try
                    {
                        w = Convert.ToInt32(textBox2.Text);
                    }
                    catch (Exception ex)
                    {
                        w = 100;
                    }
                    try
                    {
                        h = Convert.ToInt32(textBox3.Text);
                    }
                    catch (Exception ex)
                    {
                        h = 100;
                    }
                }
                ));
                Bitmap bitmap = new Bitmap(w, h);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(x, y, 0, 0, bitmap.Size);
                }

                bmp = (Bitmap)bitmap.Clone();

                try
                {
                    Bitmap bm1 = new Bitmap(bmp, new System.Drawing.Size(bmp.Width / 2, bmp.Height / 2));
                    Bitmap bm2 = new Bitmap(savedbmp, new System.Drawing.Size(savedbmp.Width / 2, savedbmp.Height / 2));
                    Bitmap bm3 = new Bitmap(savedbmp2, new System.Drawing.Size(savedbmp2.Width / 2, savedbmp2.Height / 2));

                    bool flag1 = false;
                    bool flag2 = false;

                    float val1;
                    float val2;

                    string response1 = "";
                    string response2 = "";

                    try
                    {
                        val1 = (compare(bm1, bm2) + 1) / 10000000;
                        val2 = (compare(bm1, bm3) + 1) / 10000000;


                        if (val1 < bst)
                        {
                            flag1 = true;
                        }

                        if (val2 < lst)
                        {
                            flag2 = true;
                        }

                        response1 = val1.ToString();
                        response2 = val2.ToString();
                    }
                    catch
                    {
                        //if compare functions fail
                        response1 = "Image Size Error";
                        response2 = "Image Size Error";
                    }


                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (flag1 || flag2)
                        {
                            checkBox.IsChecked = true;
                        }
                        else if (!flag1 && !flag2)
                        {
                            checkBox.IsChecked = false;
                        }

                        if (cbxPreview.IsChecked == true)
                        {
                            //resize image to fit preview
                            Bitmap tmp = new Bitmap(bitmap, new System.Drawing.Size(150, 150));
                            imgPreview.Source = BitmapToImageSource(tmp);
                            tbxBSV.Text = response1;
                            tbxLSV.Text = response2;
                        }
                    }));
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception exp)
            {
            }
        }

        private float compare(Bitmap bmp1, Bitmap bmp2)
        {
            List<Color> im1 = new List<Color>(HistoGram(bmp1));
            List<Color> im2 = new List<Color>(HistoGram(bmp2));

            float val = 0;
            for (int i = 0; i < im1.Count; i++)
            {
                val += Math.Abs((float)(im1.ElementAt(i).ToArgb() - im2.ElementAt(i).ToArgb()));
            }

            return val;
        }

        private List<Color> HistoGram(Bitmap bitmap)
        {
            Bitmap newbitmap = (Bitmap)bitmap.Clone();
            // Store the histogram in a dictionary          
            List<Color> histo = new List<Color>();
            for (int x = 0; x < newbitmap.Width; x++)
            {
                for (int y = 0; y < newbitmap.Height; y++)
                {
                    // Get pixel color 
                    Color c = newbitmap.GetPixel(x, y);
                    // If it exists in our 'histogram' increment the corresponding value, or add new
                    histo.Add(c);
                }
            }
            return histo;
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            string[] settings = new string[6];

            settings[0] = textBox.Text;
            settings[1] = textBox1.Text;
            settings[2] = textBox2.Text;
            settings[3] = textBox3.Text;
            settings[4] = tbxBST.Text;
            settings[5] = tbxLST.Text;

            File.WriteAllLines("settings.ini", settings);
            MessageBox.Show("Saved!");
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] settings = File.ReadAllLines("settings.ini");

                textBox.Text = settings[0];
                textBox1.Text = settings[1];
                textBox2.Text = settings[2];
                textBox3.Text = settings[3];
                tbxBST.Text = settings[4];
                tbxLST.Text = settings[5];
            }
            catch(Exception ex)
            {
                MessageBox.Show("No settings found, be sure to save settings first");
            }
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmp);
                savedbmp.Dispose();
                bmp.Dispose();
                newbmp.Save("saved.bmp");
                savedbmp = newbmp;
                bmp = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }
            
        }

        private void btnImage2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Bitmap newbmp = new Bitmap(bmp);
                savedbmp2.Dispose();
                bmp.Dispose();
                newbmp.Save("saved2.bmp");
                savedbmp2 = newbmp;
                bmp = newbmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, is the image or livesplit open?\n" + ex.Message);
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
