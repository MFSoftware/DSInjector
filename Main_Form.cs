using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DSInjector
{
    public class Main_Form : Form
    {
        public string ExePath = "";
        private IContainer components = (IContainer) null;
        private Server serv;
        private Button select_btn;
        private Button inject_btn;
        private RichTextBox log_tb;
        private LinkLabel linkLabel1;
        private Label label1;

        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int EnumProcessModules(IntPtr hProcess, [Out] IntPtr lphModule, uint cb, out uint lpcbNeeded);

        [DllImport("psapi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        public Main_Form()
        {
            this.InitializeComponent();
            this.serv = new Server();
        }

        public void Log(string txt)
        {
            this.log_tb.AppendText(txt + "\n");
        }

        private void CheckModule(Process process, string module)
        {
            bool flag1 = false;
            do
            {
                IntPtr[] numArray = new IntPtr[1024];
                GCHandle gcHandle = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
                IntPtr lphModule = gcHandle.AddrOfPinnedObject();
                uint cb = (uint) (Marshal.SizeOf(typeof (IntPtr)) * numArray.Length);
                uint lpcbNeeded = 0;
                if (Main_Form.EnumProcessModules(process.Handle, lphModule, cb, out lpcbNeeded) == 1)
                {
                    int num = (int) ((long) lpcbNeeded / (long) Marshal.SizeOf(typeof (IntPtr)));
                    for (int index = 0; index < num; ++index)
                    {
                        StringBuilder lpBaseName = new StringBuilder(1024);
                        int moduleFileNameEx = (int) Main_Form.GetModuleFileNameEx(process.Handle, numArray[index], lpBaseName, (uint) lpBaseName.Capacity);
                        if (Path.GetFileName(lpBaseName.ToString()) == module)
                        {
                            IntPtr hProcess = Inject.OpenProcess(2035711U, 1, process.Id);
                            this.Log(hProcess.ToString());
                            this.Log(Directory.GetCurrentDirectory() + "/Inject.dll");
                            Inject.InjectDLL(hProcess, Directory.GetCurrentDirectory() + "/Inject.dll");
                            this.Log(Path.GetFileName(lpBaseName.ToString()) + " is loaded");
                            flag1 = true;
                        }
                    }
                    this.Log("Number of Modules: " + (object) num);
                }
                gcHandle.Free();
            }
            while (!flag1);
        }

        private void inject_btn_Click(object sender, EventArgs e)
        {
            if (this.ExePath != "")
            {
                new Highlight_Form().Show();
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = Path.GetDirectoryName(this.ExePath),
                    FileName = this.ExePath
                };
                process.Start();
                this.Log("Process ID: " + (object) process.Id);
                this.Log("\nInjecting DLL...");
                this.CheckModule(process, "php5ts.dll");
                this.Log("Injecting End");
            }
            else
                this.Log("\nSelect File!");
        }

        private void log_tb_TextChanged(object sender, EventArgs e)
        {
            this.log_tb.SelectionStart = this.log_tb.Text.Length;
            this.log_tb.ScrollToCaret();
        }

        private void select_btn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exe File|*.exe";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            this.ExePath = openFileDialog.FileName;
            this.Log("Selected file: " + this.ExePath);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://vk.com/mfsoftware");
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.serv.didntClosing = false;
            this.serv.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.select_btn = new Button();
            this.inject_btn = new Button();
            this.log_tb = new RichTextBox();
            this.linkLabel1 = new LinkLabel();
            this.label1 = new Label();
            this.SuspendLayout();
            this.select_btn.Location = new Point(12, 12);
            this.select_btn.Name = "select_btn";
            this.select_btn.Size = new Size(122, 46);
            this.select_btn.TabIndex = 0;
            this.select_btn.Text = "Select File";
            this.select_btn.UseVisualStyleBackColor = true;
            this.select_btn.Click += new EventHandler(this.select_btn_Click);
            this.inject_btn.Location = new Point(12, 82);
            this.inject_btn.Name = "inject_btn";
            this.inject_btn.Size = new Size(122, 46);
            this.inject_btn.TabIndex = 1;
            this.inject_btn.Text = "Inject";
            this.inject_btn.UseVisualStyleBackColor = true;
            this.inject_btn.Click += new EventHandler(this.inject_btn_Click);
            this.log_tb.AccessibleRole = AccessibleRole.None;
            this.log_tb.BackColor = SystemColors.Control;
            this.log_tb.BorderStyle = BorderStyle.FixedSingle;
            this.log_tb.ImeMode = ImeMode.Katakana;
            this.log_tb.Location = new Point(165, 12);
            this.log_tb.Name = "log_tb";
            this.log_tb.ReadOnly = true;
            this.log_tb.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.log_tb.Size = new Size(489, 125);
            this.log_tb.TabIndex = 2;
            this.log_tb.Text = "";
            this.log_tb.BackColor = Color.White;
            this.log_tb.TextChanged += new EventHandler(this.log_tb_TextChanged);
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new Point(560, 151);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(78, 13);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "vk.com/mfsoftware";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 151);
            this.label1.Name = "label1";
            this.label1.Size = new Size(503, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "The source is created by exLune group, MFSoftware modification";
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(666, 176);
            this.Controls.Add((Control) this.label1);
            this.Controls.Add((Control) this.linkLabel1);
            this.Controls.Add((Control) this.log_tb);
            this.Controls.Add((Control) this.inject_btn);
            this.Controls.Add((Control) this.select_btn);
            this.Name = nameof (Main_Form);
            this.Text = "DSInjector";
            this.BackColor = Color.White;
            this.FormClosing += new FormClosingEventHandler(this.Main_Form_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}