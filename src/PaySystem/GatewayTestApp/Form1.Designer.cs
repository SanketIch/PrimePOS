namespace GatewayTestApp
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
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.tcTest = new System.Windows.Forms.TabControl();
            this.tbProcessCC = new System.Windows.Forms.TabPage();
            this.txtEncCCSwipeData = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCCSwipedata = new System.Windows.Forms.TextBox();
            this.txtCCSaleAmount = new System.Windows.Forms.TextBox();
            this.txtCCExpYear = new System.Windows.Forms.TextBox();
            this.txtCCExpMonth = new System.Windows.Forms.TextBox();
            this.txtCCCardNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcessCC = new System.Windows.Forms.Button();
            this.tbVoidCC = new System.Windows.Forms.TabPage();
            this.rbCredit = new System.Windows.Forms.RadioButton();
            this.rbVoid = new System.Windows.Forms.RadioButton();
            this.btnVoidCC = new System.Windows.Forms.Button();
            this.txtVoidCCHistoryID = new System.Windows.Forms.TextBox();
            this.txtVoidCCOrderID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCCVoidAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbProcessDb = new System.Windows.Forms.TabPage();
            this.txtDBSwipe = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnProcessDBCard = new System.Windows.Forms.Button();
            this.txtDBEncryptedSwipeData = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDBEncryptedPinData = new System.Windows.Forms.TextBox();
            this.txtDBSaleAmt = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbVoidDb = new System.Windows.Forms.TabPage();
            this.rbDBCredit = new System.Windows.Forms.RadioButton();
            this.rbDBVoid = new System.Windows.Forms.RadioButton();
            this.btnReverseDebit = new System.Windows.Forms.Button();
            this.txtDBReverseHistoryID = new System.Windows.Forms.TextBox();
            this.txtDBReverseOrderID = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDBReverseAmt = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtAcctID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tcTest.SuspendLayout();
            this.tbProcessCC.SuspendLayout();
            this.tbVoidCC.SuspendLayout();
            this.tbProcessDb.SuspendLayout();
            this.tbVoidDb.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbResult
            // 
            this.rtbResult.Location = new System.Drawing.Point(591, 34);
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.Size = new System.Drawing.Size(332, 394);
            this.rtbResult.TabIndex = 13;
            this.rtbResult.Text = "";
            // 
            // tcTest
            // 
            this.tcTest.CausesValidation = false;
            this.tcTest.Controls.Add(this.tbProcessCC);
            this.tcTest.Controls.Add(this.tbVoidCC);
            this.tcTest.Controls.Add(this.tbProcessDb);
            this.tcTest.Controls.Add(this.tbVoidDb);
            this.tcTest.Location = new System.Drawing.Point(12, 12);
            this.tcTest.Name = "tcTest";
            this.tcTest.SelectedIndex = 0;
            this.tcTest.Size = new System.Drawing.Size(541, 416);
            this.tcTest.TabIndex = 14;
            this.tcTest.SelectedIndexChanged += new System.EventHandler(this.tcTest_SelectedIndexChanged);
            // 
            // tbProcessCC
            // 
            this.tbProcessCC.Controls.Add(this.txtEncCCSwipeData);
            this.tbProcessCC.Controls.Add(this.label6);
            this.tbProcessCC.Controls.Add(this.label5);
            this.tbProcessCC.Controls.Add(this.txtCCSwipedata);
            this.tbProcessCC.Controls.Add(this.txtCCSaleAmount);
            this.tbProcessCC.Controls.Add(this.txtCCExpYear);
            this.tbProcessCC.Controls.Add(this.txtCCExpMonth);
            this.tbProcessCC.Controls.Add(this.txtCCCardNo);
            this.tbProcessCC.Controls.Add(this.label4);
            this.tbProcessCC.Controls.Add(this.label3);
            this.tbProcessCC.Controls.Add(this.label2);
            this.tbProcessCC.Controls.Add(this.label1);
            this.tbProcessCC.Controls.Add(this.btnProcessCC);
            this.tbProcessCC.Location = new System.Drawing.Point(4, 22);
            this.tbProcessCC.Name = "tbProcessCC";
            this.tbProcessCC.Padding = new System.Windows.Forms.Padding(3);
            this.tbProcessCC.Size = new System.Drawing.Size(533, 390);
            this.tbProcessCC.TabIndex = 0;
            this.tbProcessCC.Text = "Process CC";
            this.tbProcessCC.UseVisualStyleBackColor = true;
            // 
            // txtEncCCSwipeData
            // 
            this.txtEncCCSwipeData.Location = new System.Drawing.Point(149, 244);
            this.txtEncCCSwipeData.Multiline = true;
            this.txtEncCCSwipeData.Name = "txtEncCCSwipeData";
            this.txtEncCCSwipeData.Size = new System.Drawing.Size(356, 67);
            this.txtEncCCSwipeData.TabIndex = 36;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Encrypted Swipe Data";
            // 
            // txtCCSwipedata
            // 
            this.txtCCSwipedata.Location = new System.Drawing.Point(149, 151);
            this.txtCCSwipedata.Multiline = true;
            this.txtCCSwipedata.Name = "txtCCSwipedata";
            this.txtCCSwipedata.Size = new System.Drawing.Size(356, 43);
            this.txtCCSwipedata.TabIndex = 33;
            // 
            // txtCCSaleAmount
            // 
            this.txtCCSaleAmount.Location = new System.Drawing.Point(149, 110);
            this.txtCCSaleAmount.Name = "txtCCSaleAmount";
            this.txtCCSaleAmount.Size = new System.Drawing.Size(100, 20);
            this.txtCCSaleAmount.TabIndex = 32;
            // 
            // txtCCExpYear
            // 
            this.txtCCExpYear.Location = new System.Drawing.Point(149, 77);
            this.txtCCExpYear.MaxLength = 4;
            this.txtCCExpYear.Name = "txtCCExpYear";
            this.txtCCExpYear.Size = new System.Drawing.Size(100, 20);
            this.txtCCExpYear.TabIndex = 31;
            // 
            // txtCCExpMonth
            // 
            this.txtCCExpMonth.Location = new System.Drawing.Point(149, 49);
            this.txtCCExpMonth.MaxLength = 2;
            this.txtCCExpMonth.Name = "txtCCExpMonth";
            this.txtCCExpMonth.Size = new System.Drawing.Size(59, 20);
            this.txtCCExpMonth.TabIndex = 30;
            // 
            // txtCCCardNo
            // 
            this.txtCCCardNo.Location = new System.Drawing.Point(149, 21);
            this.txtCCCardNo.MaxLength = 16;
            this.txtCCCardNo.Name = "txtCCCardNo";
            this.txtCCCardNo.Size = new System.Drawing.Size(210, 20);
            this.txtCCCardNo.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Amt $";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Exp year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Exp Month";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Card #";
            // 
            // btnProcessCC
            // 
            this.btnProcessCC.Location = new System.Drawing.Point(216, 361);
            this.btnProcessCC.Name = "btnProcessCC";
            this.btnProcessCC.Size = new System.Drawing.Size(75, 23);
            this.btnProcessCC.TabIndex = 24;
            this.btnProcessCC.Text = "Process";
            this.btnProcessCC.UseVisualStyleBackColor = true;
            this.btnProcessCC.Click += new System.EventHandler(this.btnProcessCC_Click);
            // 
            // tbVoidCC
            // 
            this.tbVoidCC.Controls.Add(this.rbCredit);
            this.tbVoidCC.Controls.Add(this.rbVoid);
            this.tbVoidCC.Controls.Add(this.btnVoidCC);
            this.tbVoidCC.Controls.Add(this.txtVoidCCHistoryID);
            this.tbVoidCC.Controls.Add(this.txtVoidCCOrderID);
            this.tbVoidCC.Controls.Add(this.label9);
            this.tbVoidCC.Controls.Add(this.label8);
            this.tbVoidCC.Controls.Add(this.txtCCVoidAmount);
            this.tbVoidCC.Controls.Add(this.label7);
            this.tbVoidCC.Location = new System.Drawing.Point(4, 22);
            this.tbVoidCC.Name = "tbVoidCC";
            this.tbVoidCC.Padding = new System.Windows.Forms.Padding(3);
            this.tbVoidCC.Size = new System.Drawing.Size(533, 390);
            this.tbVoidCC.TabIndex = 1;
            this.tbVoidCC.Text = "Void CC";
            this.tbVoidCC.UseVisualStyleBackColor = true;
            // 
            // rbCredit
            // 
            this.rbCredit.AutoSize = true;
            this.rbCredit.Location = new System.Drawing.Point(119, 232);
            this.rbCredit.Name = "rbCredit";
            this.rbCredit.Size = new System.Drawing.Size(52, 17);
            this.rbCredit.TabIndex = 8;
            this.rbCredit.TabStop = true;
            this.rbCredit.Text = "Credit";
            this.rbCredit.UseVisualStyleBackColor = true;
            // 
            // rbVoid
            // 
            this.rbVoid.AutoSize = true;
            this.rbVoid.Location = new System.Drawing.Point(22, 232);
            this.rbVoid.Name = "rbVoid";
            this.rbVoid.Size = new System.Drawing.Size(46, 17);
            this.rbVoid.TabIndex = 7;
            this.rbVoid.TabStop = true;
            this.rbVoid.Text = "Void";
            this.rbVoid.UseVisualStyleBackColor = true;
            // 
            // btnVoidCC
            // 
            this.btnVoidCC.Location = new System.Drawing.Point(200, 329);
            this.btnVoidCC.Name = "btnVoidCC";
            this.btnVoidCC.Size = new System.Drawing.Size(133, 23);
            this.btnVoidCC.TabIndex = 6;
            this.btnVoidCC.Text = "Reverse Transaction";
            this.btnVoidCC.UseVisualStyleBackColor = true;
            this.btnVoidCC.Click += new System.EventHandler(this.btnVoidCC_Click);
            // 
            // txtVoidCCHistoryID
            // 
            this.txtVoidCCHistoryID.Location = new System.Drawing.Point(119, 147);
            this.txtVoidCCHistoryID.Name = "txtVoidCCHistoryID";
            this.txtVoidCCHistoryID.Size = new System.Drawing.Size(115, 20);
            this.txtVoidCCHistoryID.TabIndex = 5;
            // 
            // txtVoidCCOrderID
            // 
            this.txtVoidCCOrderID.Location = new System.Drawing.Point(119, 98);
            this.txtVoidCCOrderID.Name = "txtVoidCCOrderID";
            this.txtVoidCCOrderID.Size = new System.Drawing.Size(115, 20);
            this.txtVoidCCOrderID.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "History ID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Order ID";
            // 
            // txtCCVoidAmount
            // 
            this.txtCCVoidAmount.Location = new System.Drawing.Point(119, 37);
            this.txtCCVoidAmount.Name = "txtCCVoidAmount";
            this.txtCCVoidAmount.Size = new System.Drawing.Size(115, 20);
            this.txtCCVoidAmount.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Amount";
            // 
            // tbProcessDb
            // 
            this.tbProcessDb.Controls.Add(this.txtDBSwipe);
            this.tbProcessDb.Controls.Add(this.label17);
            this.tbProcessDb.Controls.Add(this.btnProcessDBCard);
            this.tbProcessDb.Controls.Add(this.txtDBEncryptedSwipeData);
            this.tbProcessDb.Controls.Add(this.label11);
            this.tbProcessDb.Controls.Add(this.label12);
            this.tbProcessDb.Controls.Add(this.txtDBEncryptedPinData);
            this.tbProcessDb.Controls.Add(this.txtDBSaleAmt);
            this.tbProcessDb.Controls.Add(this.label13);
            this.tbProcessDb.Location = new System.Drawing.Point(4, 22);
            this.tbProcessDb.Name = "tbProcessDb";
            this.tbProcessDb.Size = new System.Drawing.Size(533, 390);
            this.tbProcessDb.TabIndex = 2;
            this.tbProcessDb.Text = "Process Debit";
            this.tbProcessDb.UseVisualStyleBackColor = true;
            // 
            // txtDBSwipe
            // 
            this.txtDBSwipe.Location = new System.Drawing.Point(138, 197);
            this.txtDBSwipe.Multiline = true;
            this.txtDBSwipe.Name = "txtDBSwipe";
            this.txtDBSwipe.Size = new System.Drawing.Size(356, 67);
            this.txtDBSwipe.TabIndex = 45;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 197);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 13);
            this.label17.TabIndex = 44;
            this.label17.Text = "Swipe Data";
            // 
            // btnProcessDBCard
            // 
            this.btnProcessDBCard.Location = new System.Drawing.Point(221, 354);
            this.btnProcessDBCard.Name = "btnProcessDBCard";
            this.btnProcessDBCard.Size = new System.Drawing.Size(75, 23);
            this.btnProcessDBCard.TabIndex = 43;
            this.btnProcessDBCard.Text = "Process";
            this.btnProcessDBCard.UseVisualStyleBackColor = true;
            this.btnProcessDBCard.Click += new System.EventHandler(this.btnProcessDBCard_Click);
            // 
            // txtDBEncryptedSwipeData
            // 
            this.txtDBEncryptedSwipeData.Location = new System.Drawing.Point(138, 114);
            this.txtDBEncryptedSwipeData.Multiline = true;
            this.txtDBEncryptedSwipeData.Name = "txtDBEncryptedSwipeData";
            this.txtDBEncryptedSwipeData.Size = new System.Drawing.Size(356, 67);
            this.txtDBEncryptedSwipeData.TabIndex = 42;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 13);
            this.label11.TabIndex = 41;
            this.label11.Text = "Encrypted Swipe Data";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "Encrypted Pin Data";
            // 
            // txtDBEncryptedPinData
            // 
            this.txtDBEncryptedPinData.Location = new System.Drawing.Point(138, 52);
            this.txtDBEncryptedPinData.Multiline = true;
            this.txtDBEncryptedPinData.Name = "txtDBEncryptedPinData";
            this.txtDBEncryptedPinData.Size = new System.Drawing.Size(356, 43);
            this.txtDBEncryptedPinData.TabIndex = 39;
            // 
            // txtDBSaleAmt
            // 
            this.txtDBSaleAmt.Location = new System.Drawing.Point(138, 22);
            this.txtDBSaleAmt.Name = "txtDBSaleAmt";
            this.txtDBSaleAmt.Size = new System.Drawing.Size(100, 20);
            this.txtDBSaleAmt.TabIndex = 38;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 37;
            this.label13.Text = "Amt $";
            // 
            // tbVoidDb
            // 
            this.tbVoidDb.Controls.Add(this.rbDBCredit);
            this.tbVoidDb.Controls.Add(this.rbDBVoid);
            this.tbVoidDb.Controls.Add(this.btnReverseDebit);
            this.tbVoidDb.Controls.Add(this.txtDBReverseHistoryID);
            this.tbVoidDb.Controls.Add(this.txtDBReverseOrderID);
            this.tbVoidDb.Controls.Add(this.label14);
            this.tbVoidDb.Controls.Add(this.label15);
            this.tbVoidDb.Controls.Add(this.txtDBReverseAmt);
            this.tbVoidDb.Controls.Add(this.label16);
            this.tbVoidDb.Location = new System.Drawing.Point(4, 22);
            this.tbVoidDb.Name = "tbVoidDb";
            this.tbVoidDb.Size = new System.Drawing.Size(533, 390);
            this.tbVoidDb.TabIndex = 3;
            this.tbVoidDb.Text = "Void Debit";
            this.tbVoidDb.UseVisualStyleBackColor = true;
            // 
            // rbDBCredit
            // 
            this.rbDBCredit.AutoSize = true;
            this.rbDBCredit.Location = new System.Drawing.Point(127, 230);
            this.rbDBCredit.Name = "rbDBCredit";
            this.rbDBCredit.Size = new System.Drawing.Size(52, 17);
            this.rbDBCredit.TabIndex = 17;
            this.rbDBCredit.TabStop = true;
            this.rbDBCredit.Text = "Credit";
            this.rbDBCredit.UseVisualStyleBackColor = true;
            // 
            // rbDBVoid
            // 
            this.rbDBVoid.AutoSize = true;
            this.rbDBVoid.Location = new System.Drawing.Point(30, 230);
            this.rbDBVoid.Name = "rbDBVoid";
            this.rbDBVoid.Size = new System.Drawing.Size(46, 17);
            this.rbDBVoid.TabIndex = 16;
            this.rbDBVoid.TabStop = true;
            this.rbDBVoid.Text = "Void";
            this.rbDBVoid.UseVisualStyleBackColor = true;
            // 
            // btnReverseDebit
            // 
            this.btnReverseDebit.Location = new System.Drawing.Point(208, 327);
            this.btnReverseDebit.Name = "btnReverseDebit";
            this.btnReverseDebit.Size = new System.Drawing.Size(133, 23);
            this.btnReverseDebit.TabIndex = 15;
            this.btnReverseDebit.Text = "Reverse Transaction";
            this.btnReverseDebit.UseVisualStyleBackColor = true;
            this.btnReverseDebit.Click += new System.EventHandler(this.btnReverseDebit_Click);
            // 
            // txtDBReverseHistoryID
            // 
            this.txtDBReverseHistoryID.Location = new System.Drawing.Point(127, 145);
            this.txtDBReverseHistoryID.Name = "txtDBReverseHistoryID";
            this.txtDBReverseHistoryID.Size = new System.Drawing.Size(115, 20);
            this.txtDBReverseHistoryID.TabIndex = 14;
            // 
            // txtDBReverseOrderID
            // 
            this.txtDBReverseOrderID.Location = new System.Drawing.Point(127, 96);
            this.txtDBReverseOrderID.Name = "txtDBReverseOrderID";
            this.txtDBReverseOrderID.Size = new System.Drawing.Size(115, 20);
            this.txtDBReverseOrderID.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 148);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "History ID";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 99);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 11;
            this.label15.Text = "Order ID";
            // 
            // txtDBReverseAmt
            // 
            this.txtDBReverseAmt.Location = new System.Drawing.Point(127, 35);
            this.txtDBReverseAmt.Name = "txtDBReverseAmt";
            this.txtDBReverseAmt.Size = new System.Drawing.Size(115, 20);
            this.txtDBReverseAmt.TabIndex = 10;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 38);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(43, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Amount";
            // 
            // txtAcctID
            // 
            this.txtAcctID.Location = new System.Drawing.Point(752, 8);
            this.txtAcctID.Name = "txtAcctID";
            this.txtAcctID.Size = new System.Drawing.Size(171, 20);
            this.txtAcctID.TabIndex = 15;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(588, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "AccountID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Swipe Data";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 440);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtAcctID);
            this.Controls.Add(this.tcTest);
            this.Controls.Add(this.rtbResult);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tcTest.ResumeLayout(false);
            this.tbProcessCC.ResumeLayout(false);
            this.tbProcessCC.PerformLayout();
            this.tbVoidCC.ResumeLayout(false);
            this.tbVoidCC.PerformLayout();
            this.tbProcessDb.ResumeLayout(false);
            this.tbProcessDb.PerformLayout();
            this.tbVoidDb.ResumeLayout(false);
            this.tbVoidDb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbResult;
        private System.Windows.Forms.TabControl tcTest;
        private System.Windows.Forms.TabPage tbProcessCC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCCSwipedata;
        private System.Windows.Forms.TextBox txtCCSaleAmount;
        private System.Windows.Forms.TextBox txtCCExpYear;
        private System.Windows.Forms.TextBox txtCCExpMonth;
        private System.Windows.Forms.TextBox txtCCCardNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcessCC;
        private System.Windows.Forms.TabPage tbVoidCC;
        private System.Windows.Forms.TabPage tbProcessDb;
        private System.Windows.Forms.TabPage tbVoidDb;
        private System.Windows.Forms.TextBox txtEncCCSwipeData;
        private System.Windows.Forms.Button btnVoidCC;
        private System.Windows.Forms.TextBox txtVoidCCHistoryID;
        private System.Windows.Forms.TextBox txtVoidCCOrderID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCCVoidAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rbCredit;
        private System.Windows.Forms.RadioButton rbVoid;
        private System.Windows.Forms.TextBox txtAcctID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnProcessDBCard;
        private System.Windows.Forms.TextBox txtDBEncryptedSwipeData;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDBEncryptedPinData;
        private System.Windows.Forms.TextBox txtDBSaleAmt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton rbDBCredit;
        private System.Windows.Forms.RadioButton rbDBVoid;
        private System.Windows.Forms.Button btnReverseDebit;
        private System.Windows.Forms.TextBox txtDBReverseHistoryID;
        private System.Windows.Forms.TextBox txtDBReverseOrderID;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDBReverseAmt;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtDBSwipe;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label5;
    }
}

