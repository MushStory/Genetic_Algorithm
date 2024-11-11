namespace Genetic_Algorithm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnReset = new Button();
            btnOperate = new Button();
            labelGeneration = new Label();
            labelFitness = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 500);
            panel1.TabIndex = 0;
            panel1.Paint += Panel1_Paint;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(518, 79);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(96, 31);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += BtnReset_Click;
            // 
            // btnOperate
            // 
            btnOperate.Location = new Point(620, 79);
            btnOperate.Name = "btnOperate";
            btnOperate.Size = new Size(96, 31);
            btnOperate.TabIndex = 2;
            btnOperate.Text = "Operate";
            btnOperate.UseVisualStyleBackColor = true;
            btnOperate.Click += BtnOperate_Click;
            // 
            // labelGeneration
            // 
            labelGeneration.AutoSize = true;
            labelGeneration.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            labelGeneration.Location = new Point(518, 23);
            labelGeneration.Name = "labelGeneration";
            labelGeneration.Size = new Size(99, 16);
            labelGeneration.TabIndex = 3;
            labelGeneration.Text = "Generation : 0";
            // 
            // labelFitness
            // 
            labelFitness.AutoSize = true;
            labelFitness.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            labelFitness.Location = new Point(518, 49);
            labelFitness.Name = "labelFitness";
            labelFitness.Size = new Size(73, 16);
            labelFitness.TabIndex = 4;
            labelFitness.Text = "Fitness : 0";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            textBox1.Location = new Point(523, 133);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(401, 379);
            textBox1.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(949, 530);
            Controls.Add(textBox1);
            Controls.Add(labelFitness);
            Controls.Add(labelGeneration);
            Controls.Add(btnOperate);
            Controls.Add(btnReset);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button btnReset;
        private Button btnOperate;
        private Label labelGeneration;
        private Label labelFitness;
        private TextBox textBox1;
    }
}