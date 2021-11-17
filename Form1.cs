using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archiver
{
    public partial class Form1 : Form
    {
        static BitArray bitArray;
        static StaticHaffmanAlgorithm staticHaffman;
        static LZWAlgorithm LZW;

        public Form1()
        {
            InitializeComponent();
        }

        const string file = "EncodeBinary.txt";

        private void button1_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("Encode.txt");
            var message = textBox1.Text;
            staticHaffman = new StaticHaffmanAlgorithm();
            var array = staticHaffman.EncodeMessage(message);

            var stringBuilder = new StringBuilder();
            foreach (bool bit in array)
            {
                stringBuilder.Append((bit ? 1 : 0) + "");
            }
            textBox2.Text = stringBuilder.ToString();
            bitArray = array;


            using (BinaryWriter writer = new BinaryWriter(File.Open(file, FileMode.Create)))
            {
                foreach (bool bit in array)
                {
                    writer.Write(bit);
                }
            }


            foreach (bool bit in array)
            {
                sw.Write((bit ? 1 : 0) + "");
            }
            MessageBox.Show(bitArray.Length.ToString());
            sw.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = staticHaffman.Decode(bitArray);
            StreamWriter sw = new StreamWriter("Decode.txt");
            sw.Write(textBox3.Text);
            sw.Close();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("Encode.txt");
            var message = textBox4.Text;
            LZW = new LZWAlgorithm();
            var compressed = LZW.Compress(message);
            textBox5.Text = string.Join("", compressed.Select(x => x.ToString()));
            sw.Write(textBox5.Text);
            sw.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox6.Text = LZW.Decompress(LZW.Compress(textBox4.Text));
            StreamWriter sw = new StreamWriter("Decode.txt");
            sw.Write(textBox6.Text);
            sw.Close();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("Message.txt");
            var message = sr.ReadLine();
            textBox1.Text = message;
            textBox4.Text = message;
        }
    }
}
