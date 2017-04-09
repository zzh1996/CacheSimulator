using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CacheSimulator
{
    public partial class Form1 : Form
    {
        private Cache cache = new Cache();
        private List<String> din = new List<string>();
        private int dinp = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PB14011086 郑子涵");
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                din.Clear();
                while (!sr.EndOfStream)
                {
                    din.Add(sr.ReadLine());
                }
                sr.Close();
                dinp = 0;
                listBox1.DataSource = din;
                listBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            comboBox1.SelectedIndex = 5;
            comboBox2.SelectedIndex = 1;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            cache.Reset();
            dinp = 0;
            UpdateCounts();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
        }

        private void RunString(String s)
        {
            string[] l = s.Split();
            if (l.Length >= 2)
            {
                cache.RunOnce(Convert.ToInt32(l[0]), Convert.ToUInt32(l[1], 16));
            }
            UpdateCounts();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RunString(textBox1.Text);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dinp < din.Count)
            {
                RunString(din[dinp]);
                dinp++;
                if (dinp < din.Count)
                    listBox1.SelectedIndex = dinp;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (dinp < din.Count)
            {
                RunString(din[dinp]);
                dinp++;
            }
            listBox1.SelectedIndex = dinp - 1;
        }

        private void UpdateCounts()
        {
            int TotalCount = cache.ReadDCount + cache.ReadICount + cache.WriteCount;
            int TotalMiss = cache.ReadDMiss + cache.ReadIMiss + cache.WriteMiss;
            textBox2.Text = String.Format(
                "访问总次数: {0}\t不命中次数: {1}\t不命中率: {2:P2}" + Environment.NewLine +
                "读指令次数: {3}\t不命中次数: {4}\t不命中率: {5:P2}" + Environment.NewLine +
                "读数据次数: {6}\t不命中次数: {7}\t不命中率: {8:P2}" + Environment.NewLine +
                "写数据次数: {9}\t不命中次数: {10}\t不命中率: {11:P2}",
                TotalCount, TotalMiss, (double)TotalMiss / TotalCount,
                cache.ReadICount, cache.ReadIMiss, (double)cache.ReadIMiss / cache.ReadICount,
                cache.ReadDCount, cache.ReadDMiss, (double)cache.ReadDMiss / cache.ReadDCount,
                cache.WriteCount, cache.WriteMiss, (double)cache.WriteMiss / cache.WriteCount
            );
        }

        private void SetOptions()
        {
            cache.CacheSize = 2048 << comboBox1.SelectedIndex;
            cache.LineSize = 16 << comboBox2.SelectedIndex;
            cache.Associativity = 1 << comboBox3.SelectedIndex;
            cache.ReplaceMethod = comboBox4.SelectedIndex;
            cache.PrefetchMethod = comboBox5.SelectedIndex;
            cache.WriteMethod = comboBox6.SelectedIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOptions();
        }
    }
}
