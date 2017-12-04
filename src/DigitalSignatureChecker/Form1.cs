using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalSignatureChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AllowDrop = true;
            DragEnter += new DragEventHandler(Form1_DragEnter);
            DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;

        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 0)
                return;


            using (var fs = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var input = new byte[fs.Length];
                fs.Read(input, 0, input.Length);
                var output = CalculateHash(input);
                var hexOutput = BitConverter.ToString(output).Replace("-", "").ToLower();
                label2.Visible = true;
                textBox1.Visible = true;
                textBox1.Text = hexOutput;
            }

        }


        public byte[] CalculateHash(byte[] data)
        {
            var cloner = new byte[data.Length];
            Array.Copy(data, cloner, data.Length);
            var hasher = new KeccakDigest(256);
            hasher.BlockUpdate(cloner, 0, cloner.Length);
            var output = new byte[hasher.GetDigestSize()];
            hasher.DoFinal(output, 0);
            return output;
        }

     
    }
}
