using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TrasenFrame.Classes;
using TrasenClasses.GeneralControls;
using TrasenClasses.GeneralClasses;
using YpClass;
namespace ts_yp_xtwh
{
	/// <summary>
	/// Frmkcsxx1 的摘要说明。
	/// </summary>
	public class Frmkcsxx1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ProgressBar pb;
		private System.Windows.Forms.Button butok;
		private System.Windows.Forms.Button butquit;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private myDataGrid.myDataGrid myDataGrid1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.TextBox txtsx;
		private System.Windows.Forms.DateTimePicker dtp1;
		private System.Windows.Forms.DateTimePicker dtp2;
		private System.Windows.Forms.Button butcx;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn6;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn7;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn9;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn8;
		private MenuTag _menuTag;
		private string _chineseName;
		private Form _mdiParent;
        private int _DeptID = 0;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn10;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn11;
		private System.Windows.Forms.TextBox txtxx;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbyplx;
        private DataGridTextBoxColumn dataGridTextBoxColumn12;
        private DataGridTextBoxColumn dataGridTextBoxColumn13;
        private DataGridBoolColumn dataGridBoolColumn1;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Frmkcsxx1(MenuTag menuTag,string chineseName,Form mdiParent,int deptid)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			_menuTag=menuTag;
			_chineseName=chineseName;
			_mdiParent=mdiParent;
			this.Text=_chineseName;
            this.Text = this.Text + " [" + InstanceForm._menuTag.Jgbm + "]";
            _DeptID = deptid;
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.butok = new System.Windows.Forms.Button();
            this.butquit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dtp1 = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.dtp2 = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbyplx = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtxx = new System.Windows.Forms.TextBox();
            this.txtsx = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.butcx = new System.Windows.Forms.Button();
            this.myDataGrid1 = new myDataGrid.myDataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn6 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn7 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn12 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn13 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn9 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn8 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn10 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridTextBoxColumn11 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dataGridBoolColumn1 = new System.Windows.Forms.DataGridBoolColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myDataGrid1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pb);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 532);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(824, 41);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(96, 10);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(1067, 21);
            this.pb.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "更新进度";
            // 
            // butok
            // 
            this.butok.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butok.ForeColor = System.Drawing.Color.Navy;
            this.butok.Location = new System.Drawing.Point(885, 31);
            this.butok.Name = "butok";
            this.butok.Size = new System.Drawing.Size(96, 41);
            this.butok.TabIndex = 7;
            this.butok.Text = "更新(&O)";
            this.butok.Click += new System.EventHandler(this.butok_Click);
            // 
            // butquit
            // 
            this.butquit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butquit.ForeColor = System.Drawing.Color.Navy;
            this.butquit.Location = new System.Drawing.Point(992, 31);
            this.butquit.Name = "butquit";
            this.butquit.Size = new System.Drawing.Size(96, 41);
            this.butquit.TabIndex = 8;
            this.butquit.Text = "退出 (&Q)";
            this.butquit.Click += new System.EventHandler(this.butquit_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(171, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 22);
            this.label6.TabIndex = 9;
            this.label6.Text = "日期从";
            // 
            // dtp1
            // 
            this.dtp1.Location = new System.Drawing.Point(224, 41);
            this.dtp1.Name = "dtp1";
            this.dtp1.Size = new System.Drawing.Size(149, 25);
            this.dtp1.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(373, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "到";
            // 
            // dtp2
            // 
            this.dtp2.Location = new System.Drawing.Point(395, 41);
            this.dtp2.Name = "dtp2";
            this.dtp2.Size = new System.Drawing.Size(149, 25);
            this.dtp2.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbyplx);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtxx);
            this.groupBox1.Controls.Add(this.txtsx);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.butcx);
            this.groupBox1.Controls.Add(this.dtp2);
            this.groupBox1.Controls.Add(this.dtp1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.butok);
            this.groupBox1.Controls.Add(this.butquit);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.ForeColor = System.Drawing.Color.Navy;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(824, 103);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "平均日销售量";
            // 
            // cmbyplx
            // 
            this.cmbyplx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbyplx.Location = new System.Drawing.Point(53, 42);
            this.cmbyplx.Name = "cmbyplx";
            this.cmbyplx.Size = new System.Drawing.Size(107, 23);
            this.cmbyplx.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 22);
            this.label1.TabIndex = 18;
            this.label1.Text = "类型";
            // 
            // txtxx
            // 
            this.txtxx.Enabled = false;
            this.txtxx.Location = new System.Drawing.Point(800, 62);
            this.txtxx.Name = "txtxx";
            this.txtxx.Size = new System.Drawing.Size(64, 25);
            this.txtxx.TabIndex = 17;
            this.txtxx.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // txtsx
            // 
            this.txtsx.Enabled = false;
            this.txtsx.Location = new System.Drawing.Point(800, 22);
            this.txtsx.Name = "txtsx";
            this.txtsx.Size = new System.Drawing.Size(64, 25);
            this.txtsx.TabIndex = 16;
            this.txtsx.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(672, 62);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(139, 31);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "更新下限倍数";
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(672, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(139, 30);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "更新上限陪数";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // butcx
            // 
            this.butcx.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.butcx.ForeColor = System.Drawing.Color.Navy;
            this.butcx.Location = new System.Drawing.Point(565, 33);
            this.butcx.Name = "butcx";
            this.butcx.Size = new System.Drawing.Size(86, 42);
            this.butcx.TabIndex = 13;
            this.butcx.Text = "查询(&C)";
            this.butcx.Click += new System.EventHandler(this.butcx_Click);
            // 
            // myDataGrid1
            // 
            this.myDataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.myDataGrid1.CaptionVisible = false;
            this.myDataGrid1.DataMember = "";
            this.myDataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myDataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.myDataGrid1.Location = new System.Drawing.Point(3, 21);
            this.myDataGrid1.Name = "myDataGrid1";
            this.myDataGrid1.Size = new System.Drawing.Size(818, 405);
            this.myDataGrid1.TabIndex = 20;
            this.myDataGrid1.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.myDataGrid1.Click += new System.EventHandler(this.myDataGrid1_Click);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.AllowSorting = false;
            this.dataGridTableStyle1.DataGrid = this.myDataGrid1;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn2,
            this.dataGridTextBoxColumn3,
            this.dataGridTextBoxColumn4,
            this.dataGridTextBoxColumn6,
            this.dataGridTextBoxColumn7,
            this.dataGridTextBoxColumn12,
            this.dataGridTextBoxColumn13,
            this.dataGridTextBoxColumn9,
            this.dataGridTextBoxColumn5,
            this.dataGridTextBoxColumn8,
            this.dataGridTextBoxColumn10,
            this.dataGridTextBoxColumn11,
            this.dataGridBoolColumn1});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // dataGridTextBoxColumn1
            // 
            this.dataGridTextBoxColumn1.Format = "";
            this.dataGridTextBoxColumn1.FormatInfo = null;
            this.dataGridTextBoxColumn1.HeaderText = "序号";
            this.dataGridTextBoxColumn1.ReadOnly = true;
            this.dataGridTextBoxColumn1.Width = 40;
            // 
            // dataGridTextBoxColumn2
            // 
            this.dataGridTextBoxColumn2.Format = "";
            this.dataGridTextBoxColumn2.FormatInfo = null;
            this.dataGridTextBoxColumn2.HeaderText = "品名";
            this.dataGridTextBoxColumn2.ReadOnly = true;
            this.dataGridTextBoxColumn2.Width = 150;
            // 
            // dataGridTextBoxColumn3
            // 
            this.dataGridTextBoxColumn3.Format = "";
            this.dataGridTextBoxColumn3.FormatInfo = null;
            this.dataGridTextBoxColumn3.HeaderText = "规格";
            this.dataGridTextBoxColumn3.ReadOnly = true;
            this.dataGridTextBoxColumn3.Width = 75;
            // 
            // dataGridTextBoxColumn4
            // 
            this.dataGridTextBoxColumn4.Format = "";
            this.dataGridTextBoxColumn4.FormatInfo = null;
            this.dataGridTextBoxColumn4.HeaderText = "厂家";
            this.dataGridTextBoxColumn4.ReadOnly = true;
            this.dataGridTextBoxColumn4.Width = 75;
            // 
            // dataGridTextBoxColumn6
            // 
            this.dataGridTextBoxColumn6.Format = "";
            this.dataGridTextBoxColumn6.FormatInfo = null;
            this.dataGridTextBoxColumn6.HeaderText = "批发价";
            this.dataGridTextBoxColumn6.ReadOnly = true;
            this.dataGridTextBoxColumn6.Width = 0;
            // 
            // dataGridTextBoxColumn7
            // 
            this.dataGridTextBoxColumn7.Format = "";
            this.dataGridTextBoxColumn7.FormatInfo = null;
            this.dataGridTextBoxColumn7.HeaderText = "零售价";
            this.dataGridTextBoxColumn7.ReadOnly = true;
            this.dataGridTextBoxColumn7.Width = 80;
            // 
            // dataGridTextBoxColumn12
            // 
            this.dataGridTextBoxColumn12.Format = "";
            this.dataGridTextBoxColumn12.FormatInfo = null;
            this.dataGridTextBoxColumn12.HeaderText = "总销售量";
            this.dataGridTextBoxColumn12.ReadOnly = true;
            this.dataGridTextBoxColumn12.Width = 60;
            // 
            // dataGridTextBoxColumn13
            // 
            this.dataGridTextBoxColumn13.Format = "";
            this.dataGridTextBoxColumn13.FormatInfo = null;
            this.dataGridTextBoxColumn13.HeaderText = "天数";
            this.dataGridTextBoxColumn13.ReadOnly = true;
            this.dataGridTextBoxColumn13.Width = 0;
            // 
            // dataGridTextBoxColumn9
            // 
            this.dataGridTextBoxColumn9.Format = "";
            this.dataGridTextBoxColumn9.FormatInfo = null;
            this.dataGridTextBoxColumn9.HeaderText = "平均日销售量";
            this.dataGridTextBoxColumn9.Width = 80;
            // 
            // dataGridTextBoxColumn5
            // 
            this.dataGridTextBoxColumn5.Format = "";
            this.dataGridTextBoxColumn5.FormatInfo = null;
            this.dataGridTextBoxColumn5.HeaderText = "单位";
            this.dataGridTextBoxColumn5.ReadOnly = true;
            this.dataGridTextBoxColumn5.Width = 40;
            // 
            // dataGridTextBoxColumn8
            // 
            this.dataGridTextBoxColumn8.Format = "";
            this.dataGridTextBoxColumn8.FormatInfo = null;
            this.dataGridTextBoxColumn8.HeaderText = "cjid";
            this.dataGridTextBoxColumn8.ReadOnly = true;
            this.dataGridTextBoxColumn8.Width = 0;
            // 
            // dataGridTextBoxColumn10
            // 
            this.dataGridTextBoxColumn10.Format = "";
            this.dataGridTextBoxColumn10.FormatInfo = null;
            this.dataGridTextBoxColumn10.HeaderText = "ydwbl";
            this.dataGridTextBoxColumn10.ReadOnly = true;
            this.dataGridTextBoxColumn10.Width = 0;
            // 
            // dataGridTextBoxColumn11
            // 
            this.dataGridTextBoxColumn11.Format = "";
            this.dataGridTextBoxColumn11.FormatInfo = null;
            this.dataGridTextBoxColumn11.HeaderText = "nypdw";
            this.dataGridTextBoxColumn11.ReadOnly = true;
            this.dataGridTextBoxColumn11.Width = 0;
            // 
            // dataGridBoolColumn1
            // 
            this.dataGridBoolColumn1.AllowNull = false;
            this.dataGridBoolColumn1.FalseValue = ((short)(0));
            this.dataGridBoolColumn1.HeaderText = "更新";
            this.dataGridBoolColumn1.NullValue = ((short)(0));
            this.dataGridBoolColumn1.TrueValue = ((short)(1));
            this.dataGridBoolColumn1.Width = 40;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.myDataGrid1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.ForeColor = System.Drawing.Color.Navy;
            this.groupBox2.Location = new System.Drawing.Point(0, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(824, 429);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            // 
            // Frmkcsxx1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(8, 18);
            this.ClientSize = new System.Drawing.Size(824, 573);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Frmkcsxx1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "更新库存上下限";
            this.Load += new System.EventHandler(this.Frmkcsxx1_Load);
            this.Closed += new System.EventHandler(this.Frmkcsxx1_Closed);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.myDataGrid1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void Frmkcsxx1_Load(object sender, System.EventArgs e)
		{

			try
			{
				this.dtp1.Value=DateManager.ServerDateTimeByDBType(InstanceForm.BDatabase);
				this.dtp2.Value=DateManager.ServerDateTimeByDBType(InstanceForm.BDatabase);

				//初始化
				DataTable myTb=new DataTable();
				myTb.TableName="Tb";
			
				for(int i=0;i<=this.myDataGrid1.TableStyles[0].GridColumnStyles.Count-1;i++) 
				{	
					if(this.myDataGrid1.TableStyles[0].GridColumnStyles[i].GetType().ToString()=="System.Windows.Forms.DataGridBoolColumn")
						myTb.Columns.Add(this.myDataGrid1.TableStyles[0].GridColumnStyles[i].HeaderText,Type.GetType("System.Int16"));	
					else
						myTb.Columns.Add(this.myDataGrid1.TableStyles[0].GridColumnStyles[i].HeaderText,Type.GetType("System.String"));	
									   
					this.myDataGrid1.TableStyles[0].GridColumnStyles[i].MappingName=this.myDataGrid1.TableStyles[0].GridColumnStyles[i].HeaderText;
					this.myDataGrid1.TableStyles[0].GridColumnStyles[i].NullText="";
				}

				this.myDataGrid1.DataSource=myTb;
				this.myDataGrid1.TableStyles[0].MappingName ="Tb";

				Yp.AddCmbYplx(true,_DeptID,cmbyplx, InstanceForm.BDatabase);

			}
			catch(System.Exception err)
			{
				MessageBox.Show("发生错误"+err.Message);
			}

		}

		private void butcx_Click(object sender, System.EventArgs e)
		{
			try
			{
				string ssql="select kslx from yp_yjks where deptid="+_DeptID+"";
				DataTable tab=InstanceForm.BDatabase.GetDataTable(ssql);
				if (tab.Rows.Count==0) return;
				string procname="SP_Yk_TJ_PJRXSLTJ";
				if (tab.Rows[0]["kslx"].ToString().Trim()=="药房") procname="SP_YF_TJ_PJRXSLTJ";

				ParameterEx[] parameters=new ParameterEx[4];
				parameters[0].Value=this.dtp1.Value.ToShortDateString();
				parameters[1].Value=this.dtp2.Value.ToShortDateString();
				parameters[2].Value=Convert.ToInt32(cmbyplx.SelectedValue);
				parameters[3].Value=_DeptID;
                parameters[0].Text = "@dtp1";
                parameters[1].Text = "@dtp2";
                parameters[2].Text = "@yplx";
                parameters[3].Text = "@deptid";

				DataTable tb=InstanceForm.BDatabase.GetDataTable(procname,parameters,30);
				FunBase.AddRowtNo(tb);
				tb.TableName="Tb";
				this.myDataGrid1.DataSource=tb;
			}
			catch(System.Exception err)
			{
				MessageBox.Show(err.ToString());
			}
		}

		private void checkBox2_CheckedChanged(object sender, System.EventArgs e)
		{
			this.txtsx.Enabled=this.checkBox1.Checked==true?true:false;
			this.txtxx.Enabled=this.checkBox2.Checked==true?true:false;
		}

		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (Convertor.IsNumeric(txtsx.Text)==false)txtsx.Text="";
			if (Convertor.IsNumeric(txtxx.Text)==false)txtxx.Text="";
		}

		private void butok_Click(object sender, System.EventArgs e)
		{
			string ssql="";

			DataTable tb=(DataTable)this.myDataGrid1.DataSource;
			if (tb.Rows.Count==0) {MessageBox.Show("当前没有记录");return;}
			if (this.checkBox1.Checked==false && this.checkBox2.Checked==false) {MessageBox.Show("请选择更新的对象");return;}
			if (txtsx.Enabled==true && txtsx.Text==""){MessageBox.Show("请输入正确的上限倍数");return;}
			if (txtxx.Enabled==true && txtxx.Text==""){MessageBox.Show("请输入正确的下限倍数");return;}
			if (Convert.ToDecimal(Convertor.IsNull(txtsx.Text,"0"))<Convert.ToDecimal(Convertor.IsNull(txtxx.Text,"0")) && this.checkBox1.Checked==true && checkBox2.Checked==true){MessageBox.Show("上限倍数必须大于下限倍数");return;}
			if (txtsx.Enabled==true && Convert.ToDecimal(Convertor.IsNull(txtsx.Text,"0"))<0){MessageBox.Show("请输入正确的上限倍数");return;}
            if (txtxx.Enabled == true &&  Convert.ToDecimal(Convertor.IsNull(txtxx.Text,"0")) < 0) { MessageBox.Show("请输入正确的下限倍数"); return; }

			this.panel1.Visible=true;

			try
			{
				this.butok.Enabled=false;
				InstanceForm.BDatabase.BeginTransaction();

                pb.Value = 0;
				pb.Maximum=tb.Rows.Count;
				pb.Minimum=0;
				for(int i=0;i<=tb.Rows.Count-1;i++)
				{
                    if (Convert.ToInt16(tb.Rows[i]["更新"]) == 1)
                    {
                            decimal ypsl = Convert.ToDecimal(Convertor.IsNull(tb.Rows[i]["平均日销售量"], "0"));
                            decimal kcsx = ypsl * Convert.ToDecimal(Convertor.IsNull(txtsx.Text,"0"));
                            decimal kcxx = ypsl * Convert.ToDecimal(Convertor.IsNull(txtxx.Text,"0"));
                            int ydwbl = Convert.ToInt32(tb.Rows[i]["ydwbl"]);
                            int nypdw = Convert.ToInt32(tb.Rows[i]["nypdw"]);
                            int cjid = Convert.ToInt32(tb.Rows[i]["cjid"]);
                            kcsx = Convert.ToDecimal(kcsx.ToString("0.000"));
                            kcxx = Convert.ToDecimal(kcxx.ToString("0.000"));

                            ssql = "select id from yp_kcsxx where deptid=" + _DeptID + " and cjid=" + cjid + "";
                            DataTable tx = InstanceForm.BDatabase.GetDataTable(ssql);
                            if (tx.Rows.Count != 0)
                            {
                                if (this.checkBox1.Checked == true && this.checkBox2.Checked == true)
                                    ssql = "update yp_kcsxx set kcsx=" + kcsx + ",kcxx=" + kcxx + ",nypdw=" + nypdw + ",ydwbl=" + ydwbl + " where deptid=" + _DeptID + " and cjid=" + cjid + " ";
                                if (this.checkBox1.Checked == true && this.checkBox2.Checked == false)
                                    ssql = "update yp_kcsxx set kcsx=" + kcsx + ",nypdw=" + nypdw + ",ydwbl=" + ydwbl + " where deptid=" + _DeptID + " and cjid=" + cjid + " ";
                                if (this.checkBox1.Checked == false && this.checkBox2.Checked == true)
                                    ssql = "update yp_kcsxx set kcxx=" + kcxx + ",nypdw=" + nypdw + ",ydwbl=" + ydwbl + " where deptid=" + _DeptID + " and cjid=" + cjid + " ";
                            }
                            else
                            {
                                if (this.checkBox1.Checked == true && this.checkBox2.Checked == true)
                                    ssql = "insert into yp_kcsxx(kcsx,kcxx,nypdw,ydwbl,cjid,deptid)values(" + kcsx + "," + kcxx + "," + nypdw + "," + ydwbl + "," + cjid + "," + _DeptID + ")";
                                if (this.checkBox1.Checked == true && this.checkBox2.Checked == false)
                                    ssql = "insert into yp_kcsxx(kcsx,nypdw,ydwbl,cjid,deptid)values(" + kcsx + "," + nypdw + "," + ydwbl + "," + cjid + "," + _DeptID + ")";
                                if (this.checkBox1.Checked == false && this.checkBox2.Checked == true)
                                    ssql = "insert into yp_kcsxx(kcxx,nypdw,ydwbl,cjid,deptid)values(" + kcxx + "," + nypdw + "," + ydwbl + "," + cjid + "," + _DeptID + ")";
                            }

                            InstanceForm.BDatabase.DoCommand(ssql);
                    }
					pb.Value=pb.Value+1;
				}

				InstanceForm.BDatabase.CommitTransaction();

				MessageBox.Show("更新成功");
				this.butok.Enabled=true;
				this.panel1.Visible=false;
			}
			catch(System.Exception err)
			{
				this.butok.Enabled=true;
				this.panel1.Visible=false;
				InstanceForm.BDatabase.RollbackTransaction();

				MessageBox.Show("发生错误"+err.Message);
			}



		}

		private void butquit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Frmkcsxx1_Closed(object sender, System.EventArgs e)
		{
		}

        private void myDataGrid1_Click(object sender, EventArgs e)
        {
            DataTable tb = (DataTable)this.myDataGrid1.DataSource;
            if (myDataGrid1.TableStyles[0].GridColumnStyles[this.myDataGrid1.CurrentCell.ColumnNumber].HeaderText=="更新")
            {
                tb.Rows[myDataGrid1.CurrentCell.RowNumber]["更新"] = Convert.ToInt16(tb.Rows[myDataGrid1.CurrentCell.RowNumber]["更新"]) == 1 ? 0 : 1;
            }
        }
	}
}
