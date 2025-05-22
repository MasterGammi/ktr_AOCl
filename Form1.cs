using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.CvEnum;

namespace ktr
{
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage; //глобальная переменная
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);

                imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image<Gray,byte> grayImage = sourceImage.Convert<Gray,byte>(); 
            double cannyThresold = 120.0;
            double cannyThresoldLinking = 80.0;
            Image<Gray,byte> cannyEdges = grayImage.Canny(cannyThresold, cannyThresoldLinking);

            var cannyEdgesBgr = cannyEdges.Convert<Bgr, byte>();
            var resultImage = sourceImage.Sub(cannyEdgesBgr);

            for (int channel = 0; channel < resultImage.NumberOfChannels; channel++)
                for (int x = 0; x < resultImage.Width; x++)
                    for (int y = 0; y < resultImage.Height; y++)
                    {
                        
                        byte color = resultImage.Data[y, x, channel];
                        if (color <= 50)
                            color = 0;
                        else if (color <= 100)
                            color = 25;
                        else if (color <= 150)
                            color = 180;
                        else if (color <= 200)
                            color = 210;
                        else
                            color = 255;
                        resultImage.Data[y, x, channel] = color; 
                    }


            imageBox2.Image = resultImage.Resize(640,480, Inter.Linear);

            
        }
    }
}
