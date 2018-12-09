using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DSInjector
{
    public class Highlight_Form : Form
    {
        public List<string[]> files = new List<string[]>();
        private IContainer components = (IContainer) null;
        private FastColoredTextBox scintilla;
        private ListBox code_lb;

        public Highlight_Form()
        {
            this.InitializeComponent();
            Server.OnReceived += new Server.Callback(this.Recived);
        }

        public void Recived(string content)
        {
            if (!(content != ""))
                return;
            this.code_lb.Items.Add((object) Path.GetFileName(content));
            this.files.Add(new string[2]
            {
            content,
            Path.GetFileName(content)
            });
        }

        private void code_lb_DoubleClick(object sender, EventArgs e)
        {
            if (code_lb.SelectedIndex > -1)
            {
                this.scintilla.Text = File.ReadAllText(this.files[this.code_lb.SelectedIndex][0]);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.scintilla = new FastColoredTextBox();
            this.code_lb = new ListBox();
            this.SuspendLayout();
            this.scintilla.AcceptsTab = false;
            this.scintilla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.scintilla.Location = new Point(210, 0);
            this.scintilla.Margin = new Padding(0);
            this.scintilla.Size = new Size(500, 489);
            this.scintilla.TabIndex = 0;
            this.scintilla.IndentBackColor = Color.White;
            this.scintilla.LineNumberColor = Color.Black;
            this.code_lb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.code_lb.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
            this.code_lb.FormattingEnabled = true;
            this.code_lb.ImeMode = ImeMode.NoControl;
            this.code_lb.IntegralHeight = false;
            this.code_lb.ItemHeight = 15;
            this.code_lb.Location = new Point(0, 0);
            this.code_lb.Name = "code_lb";
            this.code_lb.Size = new Size(211, 489);
            this.code_lb.TabIndex = 1;
            this.code_lb.DoubleClick += new EventHandler(this.code_lb_DoubleClick);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(710, 490);
            this.Controls.Add((Control) this.code_lb);
            this.Controls.Add((Control) this.scintilla);
            this.Name = nameof (Highlight_Form);
            this.BackColor = Color.White;
            this.Text = "DSInjector";
            this.ResumeLayout(false);
        }
    }
}
