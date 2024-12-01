namespace ChipTagValidatorUI
{
    partial class formWindow
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formWindow));
            openFileDialog1 = new OpenFileDialog();
            parseButton = new Button();
            embossFileTextBox = new TextBox();
            specificationTextBox = new TextBox();
            chipFormTextBox = new TextBox();
            specificationButton = new Button();
            embossFileButton = new Button();
            chipFormButton = new Button();
            chipDataTextBox = new TextBox();
            embossFileLabel = new Label();
            specificationLabel = new Label();
            chipFormLabel = new Label();
            delimiterLabel = new Label();
            logLabel = new Label();
            openFileDialog2 = new OpenFileDialog();
            logTextBox = new RichTextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // parseButton
            // 
            parseButton.Location = new Point(27, 161);
            parseButton.Name = "parseButton";
            parseButton.Size = new Size(122, 33);
            parseButton.TabIndex = 1;
            parseButton.Text = "Parse";
            parseButton.UseVisualStyleBackColor = true;
            // 
            // embossFileTextBox
            // 
            embossFileTextBox.Location = new Point(27, 44);
            embossFileTextBox.Name = "embossFileTextBox";
            embossFileTextBox.Size = new Size(266, 23);
            embossFileTextBox.TabIndex = 2;
            // 
            // specificationTextBox
            // 
            specificationTextBox.Location = new Point(439, 45);
            specificationTextBox.Name = "specificationTextBox";
            specificationTextBox.Size = new Size(266, 23);
            specificationTextBox.TabIndex = 3;
            // 
            // chipFormTextBox
            // 
            chipFormTextBox.Location = new Point(27, 107);
            chipFormTextBox.Name = "chipFormTextBox";
            chipFormTextBox.Size = new Size(266, 23);
            chipFormTextBox.TabIndex = 4;
            // 
            // specificationButton
            // 
            specificationButton.Location = new Point(713, 44);
            specificationButton.Name = "specificationButton";
            specificationButton.Size = new Size(75, 24);
            specificationButton.TabIndex = 5;
            specificationButton.Text = "Search";
            specificationButton.UseVisualStyleBackColor = true;
            specificationButton.Click += specificationButton_Click;
            // 
            // embossFileButton
            // 
            embossFileButton.Location = new Point(313, 44);
            embossFileButton.Name = "embossFileButton";
            embossFileButton.Size = new Size(75, 24);
            embossFileButton.TabIndex = 6;
            embossFileButton.Text = "Search";
            embossFileButton.UseVisualStyleBackColor = true;
            embossFileButton.Click += embossFileButton_Click;
            // 
            // chipFormButton
            // 
            chipFormButton.Location = new Point(313, 107);
            chipFormButton.Name = "chipFormButton";
            chipFormButton.Size = new Size(75, 23);
            chipFormButton.TabIndex = 7;
            chipFormButton.Text = "Search";
            chipFormButton.UseVisualStyleBackColor = true;
            chipFormButton.Click += chipFormButton_Click;
            // 
            // chipDataTextBox
            // 
            chipDataTextBox.Location = new Point(439, 108);
            chipDataTextBox.Name = "chipDataTextBox";
            chipDataTextBox.Size = new Size(100, 23);
            chipDataTextBox.TabIndex = 9;
            // 
            // embossFileLabel
            // 
            embossFileLabel.AutoSize = true;
            embossFileLabel.Location = new Point(27, 26);
            embossFileLabel.Name = "embossFileLabel";
            embossFileLabel.Size = new Size(69, 15);
            embossFileLabel.TabIndex = 11;
            embossFileLabel.Text = "Emboss File";
            embossFileLabel.Click += label1_Click;
            // 
            // specificationLabel
            // 
            specificationLabel.AutoSize = true;
            specificationLabel.Location = new Point(439, 25);
            specificationLabel.Name = "specificationLabel";
            specificationLabel.Size = new Size(119, 15);
            specificationLabel.TabIndex = 12;
            specificationLabel.Text = "Emboss Specification";
            // 
            // chipFormLabel
            // 
            chipFormLabel.AutoSize = true;
            chipFormLabel.Location = new Point(27, 89);
            chipFormLabel.Name = "chipFormLabel";
            chipFormLabel.Size = new Size(63, 15);
            chipFormLabel.TabIndex = 13;
            chipFormLabel.Text = "Chip Form";
            // 
            // delimiterLabel
            // 
            delimiterLabel.AutoSize = true;
            delimiterLabel.Location = new Point(439, 89);
            delimiterLabel.Name = "delimiterLabel";
            delimiterLabel.Size = new Size(110, 15);
            delimiterLabel.TabIndex = 14;
            delimiterLabel.Text = "Chip Data Delimiter";
            // 
            // logLabel
            // 
            logLabel.AutoSize = true;
            logLabel.Location = new Point(27, 259);
            logLabel.Name = "logLabel";
            logLabel.Size = new Size(27, 15);
            logLabel.TabIndex = 15;
            logLabel.Text = "Log";
            // 
            // openFileDialog2
            // 
            openFileDialog2.FileName = "openFileDialog2";
            openFileDialog2.FileOk += openFileDialog2_FileOk;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(27, 286);
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(761, 250);
            logTextBox.TabIndex = 16;
            logTextBox.Text = "";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // formWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(834, 556);
            Controls.Add(logTextBox);
            Controls.Add(logLabel);
            Controls.Add(delimiterLabel);
            Controls.Add(chipFormLabel);
            Controls.Add(specificationLabel);
            Controls.Add(embossFileLabel);
            Controls.Add(chipDataTextBox);
            Controls.Add(chipFormButton);
            Controls.Add(embossFileButton);
            Controls.Add(specificationButton);
            Controls.Add(chipFormTextBox);
            Controls.Add(specificationTextBox);
            Controls.Add(embossFileTextBox);
            Controls.Add(parseButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "formWindow";
            Text = "Chip File Parser";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private OpenFileDialog openFileDialog1;
        private Button parseButton;
        private TextBox embossFileTextBox;
        private TextBox specificationTextBox;
        private TextBox chipFormTextBox;
        private Button specificationButton;
        private Button embossFileButton;
        private Button chipFormButton;
        private Button button6;
        private TextBox chipDataTextBox;
        private Label embossFileLabel;
        private Label specificationLabel;
        private Label chipFormLabel;
        private Label delimiterLabel;
        private Label logLabel;
        private TextBox textBox5;
        private OpenFileDialog openFileDialog2;
        private RichTextBox logTextBox;
        private ContextMenuStrip contextMenuStrip1;
    }
}
