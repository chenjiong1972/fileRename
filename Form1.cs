using System;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FileRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowNewFolderButton = false;
            DialogResult r = f.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox1.Text = f.SelectedPath; 
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            DirectoryInfo folder = new DirectoryInfo(textBox1.Text);
            FullFileListBox(folder);
            label1.Text = listBox1.Items.Count.ToString();
            FileStream fStream = new FileStream(textBox1.Text + "\\Reanem.abc", FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new StreamWriter(fStream);
            for (int i = 0; i < listBox1.Items.Count; i++)
            {

                string[] s = listBox1.Items[i].ToString().Split(";".ToCharArray());
                string newName = s[0].Replace(s[1], Guid.NewGuid().ToString());
                StringBuilder sb = new StringBuilder(s[0]);
                sb.Append(";");
                sb.Append(newName);
                File.Move(s[0], newName);
                sw.WriteLine(sb.ToString());
            }
            sw.Close();
            fStream.Close();
        }

        private void FullFileListBox(DirectoryInfo folder)
        {
            FileInfo [] files=folder.GetFiles ();
            FileInfo file;
            
            for (int i = files.GetLowerBound(0); i <= files.GetUpperBound(0);i++ )
            {
                file = files[i];
                if (file.Name.CompareTo (  "Thumbs.db")!=0)
                {
                    listBox1.Items.Add(file.FullName + ";" + file.Name);
                }

                
            }
            DirectoryInfo[] folders = folder.GetDirectories();
            DirectoryInfo f;
            for (int i= folders.GetLowerBound (0);i <=folders.GetUpperBound (0);i ++ )
            {
                f = folders[i]; 
                FullFileListBox(f);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "(*.ABC)|*.ABC";
            DialogResult r = f.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox2.Text = f.FileName; 
            }
            FullListBoxFromFile(textBox2.Text);
        }

        private void FullListBoxFromFile(string f)
        {
            if (!File.Exists(f))
                return;
            listBox1.Items.Clear();
            StreamReader sr = new StreamReader(f);
            while (!sr.EndOfStream)
            {
                listBox1.Items.Add(sr.ReadLine());
            }
            sr.Close();
            label2.Text = listBox1.Items.Count.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string[] s = listBox1.Items[i].ToString().Split(";".ToCharArray());
                if (File.Exists(s[1]))
                    File.Move(s[1], s[0]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.ShowNewFolderButton = false;
            DialogResult r = f.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox3.Text = f.SelectedPath;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            DirectoryInfo folder = new DirectoryInfo(textBox3.Text);
            FileInfo[] files = folder.GetFiles("*.*",SearchOption.AllDirectories);
            string exts = "," + textBox4.Text;
            foreach (FileInfo file in files)
            {
                if (exts.Contains( file.Extension))
                {
                    string dst=Path.Combine(file.DirectoryName, file.Name.Replace(file.Extension, textBox5.Text));
                    listBox1.Items.Add(file.FullName +"->"+dst);
                    File.Move(file.FullName, dst);
                }
            }
        }
         
    }
}
