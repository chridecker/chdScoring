namespace chdScoring.PrintService
{
    partial class PrintForm
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
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new Point(69, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(402, 23);
            this.comboBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new Size(51, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Drucker:";
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "PrintForm";
            this.Text = "PrintForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ComboBox comboBox1;
        private Label label1;
    }
}
