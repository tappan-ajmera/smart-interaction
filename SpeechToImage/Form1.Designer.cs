namespace SpeechToImage
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Console = new System.Windows.Forms.TextBox();
            this.canvas = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sourceList = new System.Windows.Forms.ListView();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BasicDep = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ProcessedBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SentiBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.KeywordBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AnswerBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.SystemColors.Window;
            this.Console.Location = new System.Drawing.Point(533, 26);
            this.Console.Multiline = true;
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.ShortcutsEnabled = false;
            this.Console.Size = new System.Drawing.Size(201, 72);
            this.Console.TabIndex = 0;
            this.Console.TabStop = false;
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.Dock = System.Windows.Forms.DockStyle.Left;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(527, 479);
            this.canvas.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Green;
            this.button1.Cursor = System.Windows.Forms.Cursors.Default;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(533, 228);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::SpeechToImage.Properties.Resources._1319701530_Mic;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.Location = new System.Drawing.Point(576, 359);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(131, 109);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // sourceList
            // 
            this.sourceList.BackColor = System.Drawing.SystemColors.Control;
            this.sourceList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sourceList.Location = new System.Drawing.Point(612, 257);
            this.sourceList.MultiSelect = false;
            this.sourceList.Name = "sourceList";
            this.sourceList.Size = new System.Drawing.Size(60, 29);
            this.sourceList.TabIndex = 5;
            this.sourceList.UseCompatibleStateImageBehavior = false;
            this.sourceList.View = System.Windows.Forms.View.Details;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.DarkRed;
            this.button2.Cursor = System.Windows.Forms.Cursors.Default;
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(657, 228);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "stop";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(618, 344);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Recording...";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(564, 327);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "processing...please wait";
            this.label2.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(533, 257);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(199, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Voice Command On";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(536, 292);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(198, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Voice Command Off";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(533, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Recorded Audio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(735, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Basic Dependencies";
            // 
            // BasicDep
            // 
            this.BasicDep.Location = new System.Drawing.Point(738, 26);
            this.BasicDep.Multiline = true;
            this.BasicDep.Name = "BasicDep";
            this.BasicDep.Size = new System.Drawing.Size(245, 196);
            this.BasicDep.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(533, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Processed Output";
            // 
            // ProcessedBox
            // 
            this.ProcessedBox.Location = new System.Drawing.Point(533, 129);
            this.ProcessedBox.Multiline = true;
            this.ProcessedBox.Name = "ProcessedBox";
            this.ProcessedBox.Size = new System.Drawing.Size(202, 93);
            this.ProcessedBox.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(739, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Sentiment :";
            // 
            // SentiBox
            // 
            this.SentiBox.Location = new System.Drawing.Point(805, 231);
            this.SentiBox.Name = "SentiBox";
            this.SentiBox.Size = new System.Drawing.Size(78, 20);
            this.SentiBox.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(739, 262);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Keyword :";
            // 
            // KeywordBox
            // 
            this.KeywordBox.Location = new System.Drawing.Point(805, 255);
            this.KeywordBox.Name = "KeywordBox";
            this.KeywordBox.Size = new System.Drawing.Size(78, 20);
            this.KeywordBox.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(742, 292);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Answer :";
            // 
            // AnswerBox
            // 
            this.AnswerBox.Location = new System.Drawing.Point(805, 285);
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.Size = new System.Drawing.Size(78, 20);
            this.AnswerBox.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 479);
            this.Controls.Add(this.AnswerBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.KeywordBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SentiBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ProcessedBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BasicDep);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.sourceList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.Console);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "SpeechToImage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Console;
        private System.Windows.Forms.Panel canvas;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView sourceList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox BasicDep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ProcessedBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox SentiBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox KeywordBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox AnswerBox;
    }
}

