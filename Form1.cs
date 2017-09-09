using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        delegate void SetTextCallback(string text);
        delegate void SetImageCallback(Image img);

        string filePath;
        Random random = new Random();
        private static Bitmap destroyedImage;

        private void button2_Click(object sender, EventArgs e)
        {
            Stream myStream = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "JPEG Image files|*.jpg|PNG Image files|*.png|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        pictureBox1.Image =  Image.FromStream(myStream);
                        filePath = openFileDialog1.FileName;
                        openFileDialog1.Dispose();
                        myStream.Dispose();
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to destroy this?\nThis action is PERMANENT!",
                                     "Destruction Confirmation",
                                     MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    destroyedImage = (Bitmap)pictureBox1.Image;
                    int fuckingWidth = pictureBox1.Image.Width;
                    int fuckingHeight = pictureBox1.Image.Height;
                    foreach (int y in Enumerable.Range(0, fuckingHeight))
                    {
                        foreach (int x in Enumerable.Range(0, fuckingWidth))
                        {

                            Color c = destroyedImage.GetPixel(x, y);
                            c = setRandomColor(c);
                            destroyedImage.SetPixel(x, y, c);
                            this.SetText(x + ", " + y);


                        }
                        pictureBox2.Image = destroyedImage;
                        this.Update();

                    }
                    this.SetText("DONE");
                    var confirmResult2 = MessageBox.Show("This is your LAST CHANCE! Press \"No\" to cancel overwriting the file!",
                         "Final Destruction Confirmation",
                         MessageBoxButtons.YesNo);
                    if (confirmResult2 == DialogResult.Yes)
                    {
                        Image out1 = (Image)destroyedImage;
                        Bitmap output = (Bitmap)out1;
                        if (filePath.EndsWith(".jpg")){
                            output.Save(filePath, ImageFormat.Jpeg);
                        } else
                        {
                            output.Save(filePath, ImageFormat.Png);
                        }
                       
                        
                    }
                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                }
            }
            
            

        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.button1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.button1.Text = text;
            }
        }
        private void SetAltPic(Image img)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.pictureBox2.InvokeRequired)
            {
                SetImageCallback d = new SetImageCallback(SetAltPic);
                this.Invoke(d, new object[] { img });
            }
            else
            {
                this.pictureBox2.Image = img;
            }
        }

        private int getRandom8BitNumber()
        {
            
            int rand = random.Next(255);
            return rand;
        }
        private Color setRandomColor(Color c)
        {
            c = Color.FromArgb(getRandom8BitNumber(), getRandom8BitNumber(), getRandom8BitNumber());
            return c;
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            pictureBox1.Image = new Bitmap(ImageDestroyer.Properties.Resources.image);
        }
    }
}
