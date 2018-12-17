namespace IcmAlgoStreamserver
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.refid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.emirtipi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menkul = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.alsat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sonfiyat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hesfiy = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.yuzde = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.zaman = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statu = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_yuzde1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_yuzde2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_bslzaman = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_bitzaman = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_parcasayisi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gib_ensonaktifolanparca = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.aktsek = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1169, 68);
            this.panel4.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Gelen Veri";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(423, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(230, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = " Algorithmic Trade Orders  ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1169, 293);
            this.panel1.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1157, 483);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1149, 457);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "    Bekleyen Algo Emirleri     ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1143, 451);
            this.panel2.TabIndex = 2;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.listView1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1143, 451);
            this.panel8.TabIndex = 5;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.refid,
            this.emirtipi,
            this.menkul,
            this.alsat,
            this.lot,
            this.sonfiyat,
            this.hesfiy,
            this.yuzde,
            this.zaman,
            this.statu,
            this.gib_yuzde1,
            this.gib_yuzde2,
            this.gib_bslzaman,
            this.gib_bitzaman,
            this.gib_parcasayisi,
            this.gib_ensonaktifolanparca,
            this.aktsek});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1143, 451);
            this.listView1.TabIndex = 7;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // refid
            // 
            this.refid.Text = "Refid";
            this.refid.Width = 50;
            // 
            // emirtipi
            // 
            this.emirtipi.Text = "EmrTip";
            this.emirtipi.Width = 53;
            // 
            // menkul
            // 
            this.menkul.Text = "Menkul";
            this.menkul.Width = 59;
            // 
            // alsat
            // 
            this.alsat.Text = "A/S";
            this.alsat.Width = 31;
            // 
            // lot
            // 
            this.lot.Text = "Lot";
            this.lot.Width = 61;
            // 
            // sonfiyat
            // 
            this.sonfiyat.Text = "Z.Aktv. Son Fiyat";
            this.sonfiyat.Width = 73;
            // 
            // hesfiy
            // 
            this.hesfiy.Text = "Z.Akt. Hes.Fiyat";
            this.hesfiy.Width = 77;
            // 
            // yuzde
            // 
            this.yuzde.Text = "Z.Aktv.Yuzde";
            this.yuzde.Width = 79;
            // 
            // zaman
            // 
            this.zaman.Text = "Z.Aktv. Zaman";
            this.zaman.Width = 91;
            // 
            // statu
            // 
            this.statu.Text = "Statu";
            this.statu.Width = 71;
            // 
            // gib_yuzde1
            // 
            this.gib_yuzde1.Text = "Gib_Sns1_Yuzde";
            // 
            // gib_yuzde2
            // 
            this.gib_yuzde2.Text = "Gib_Sns2_Yuzde";
            // 
            // gib_bslzaman
            // 
            this.gib_bslzaman.Text = "Gib_Baş.Zaman";
            // 
            // gib_bitzaman
            // 
            this.gib_bitzaman.Text = "Gib_Bit.Zaman";
            // 
            // gib_parcasayisi
            // 
            this.gib_parcasayisi.Text = "Gib_Parça Sayısı";
            // 
            // gib_ensonaktifolanparca
            // 
            this.gib_ensonaktifolanparca.Text = "Gib_En Son Aktv. Parça";
            // 
            // aktsek
            // 
            this.aktsek.Text = "Aktiflesme Şekli";
            this.aktsek.Width = 96;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 361);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Icm Algo Stream Server 1.0";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader refid;
        private System.Windows.Forms.ColumnHeader emirtipi;
        private System.Windows.Forms.ColumnHeader menkul;
        private System.Windows.Forms.ColumnHeader alsat;
        private System.Windows.Forms.ColumnHeader lot;
        private System.Windows.Forms.ColumnHeader sonfiyat;
        private System.Windows.Forms.ColumnHeader hesfiy;
        private System.Windows.Forms.ColumnHeader yuzde;
        private System.Windows.Forms.ColumnHeader zaman;
        private System.Windows.Forms.ColumnHeader statu;
        private System.Windows.Forms.ColumnHeader gib_yuzde1;
        private System.Windows.Forms.ColumnHeader gib_yuzde2;
        private System.Windows.Forms.ColumnHeader gib_bslzaman;
        private System.Windows.Forms.ColumnHeader gib_bitzaman;
        private System.Windows.Forms.ColumnHeader gib_parcasayisi;
        private System.Windows.Forms.ColumnHeader gib_ensonaktifolanparca;
        private System.Windows.Forms.ColumnHeader aktsek;

    }
}

