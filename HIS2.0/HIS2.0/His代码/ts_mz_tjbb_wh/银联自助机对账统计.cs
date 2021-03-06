using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrasenFrame.Classes;
using TrasenClasses.DatabaseAccess;
using TrasenClasses.GeneralControls;
using TrasenClasses.GeneralClasses;
using ts_mz_class;
using System.IO;
using System.Net;

namespace ts_mz_tjbb
{
    public partial class FrmCUPBankChecking : Form
    {
        #region 变量定义
        private Form _mdiParent;
        private MenuTag _menuTag;
        private string _chineseName;
        private string bDateTime;
        private string eDateTime;
        //单元常量
        private string fHospitalName;//医院名称
        private string fOperName;//操作员名
        private string fApplicationDir; //程序名
        RelationalDatabase db = new MsSqlServer();
        DataSet ds;
        private int selectedMark;//选择标志，选择的报表标志（Bank 2 or His 1）
        private int searchWay = 0;//普通查询 0；带比对的查询 1
        private bool isFiltered = false;//是否过滤了false 未过滤
        //颜色定义
        Color clrNormal = new Color();//正常
        Color clrExtra = new Color();//多出、不匹配
        Color clrError = new Color();//金额不匹配
        Color clrManualConf = new Color();//人工确认
        #endregion

        #region 属性定义
        private int formMode;

        public int FormMode//窗体模式，0--银联POS，1--银联自助机
        {
            get { return formMode; }
            set { formMode = value; }
        }
        #endregion

        #region 常量定义
        //窗体类型
        const int FORM_MODE_POS = 0;
        const int FORM_MODE_SS = 1;
        //所选报表
        const string REPORT_NAME_NULL = "(请单击选择报表)";
        const string REPORT_NAME_HIS = "His报表";
        const string REPORT_NAME_BANK = "银行报表";
        //交易状态
        const string DEAL_STATE_OK = "交易成功";
        const string DEAL_STATE_POSOK = "银联交易成功";
        const string DEAL_STATE_FAILED = "交易失败";
        const string DEAL_STATE_ALL = "全部";
        //HIS报表字段
        const string HIS_DEAL_TIME = "交易支付时间";
        const string HIS_POS_DEAL_TIME = "POS交易时间";
        const string HIS_BANK_NO = "卡号";
        const string HIS_POS_ZDH = "POS返回终端号";
        const string HIS_DEAL_MONEY = "交易金额";
        const string HIS_POS_JYLX = "POS_CR_JYLX";
        const string HIS_POS_PCH = "POS_FH_PCH";
        const string HIS_POS_PZH = "POS_FH_PZH";
        //银行报表字段
        const string POS_BANK_NO = "银行卡号";
        const string POS_TERM_NO = "POS返回终端号";
        const string POS_DEAL_MONEY = "POS返回金额";
        const string POS_DEAL_TIME = "银行交易时间";
        const string POS_JYLX = "BANK_JYLX";
        const string POS_XTCKH = "BANK_XTCKH";
        #endregion

        public FrmCUPBankChecking()
        {
            InitializeComponent();
        }
        //HIS用构造函数
        public FrmCUPBankChecking(MenuTag menuTag, string chineseName, Form mdiParent)
        {
            InitializeComponent();
            _menuTag = menuTag;
            _chineseName = chineseName;
            _mdiParent = mdiParent;
            this.Text = _chineseName;
            db = InstanceForm.BDatabase;
            fHospitalName = TrasenFrame.Classes.Constant.HospitalName;
            fOperName = InstanceForm.BCurrentUser.Name;
            fApplicationDir = Constant.ApplicationDirectory;

            this.Text = _chineseName;
            lblTitle.Text = _chineseName;
            switch (_chineseName)
            {
                case "交通银行自助机对账统计":
                    this.FormMode = FORM_MODE_SS;
                    break;
                case "银联POS对账统计":
                    this.FormMode = FORM_MODE_POS;
                    break;
                default:
                    this.FormMode = FORM_MODE_POS;
                    break;
            }
        }
        //调试用构造函数
        public FrmCUPBankChecking(RelationalDatabase adb)
        {
            InitializeComponent();
            db = adb;
            fHospitalName = "";//直接传入db（调试模式），直接赋空。
            fOperName = "";
            fApplicationDir = @"D:\TS-HIS";
        }

        private void FrmCUPBankChecking_Load(object sender, EventArgs e)
        {
            //设置分割线位置
            panel1.Width = this.Width / 2;
            //处理记录类型
            cbbJLZT.Items.Add(DEAL_STATE_OK);
            cbbJLZT.Items.Add(DEAL_STATE_POSOK);
            cbbJLZT.Items.Add(DEAL_STATE_FAILED);
            cbbJLZT.Items.Add(DEAL_STATE_ALL);
            cbbJLZT.SelectedIndex = 0;
            //处理开始、结束日期时间
            bDateTime = dtpBegin.Value.ToString("yyyy-MM-dd 00:00:00");
            eDateTime = dtpEnd.Value.ToString("yyyy-MM-dd 23:59:59");
            //Mod by Hxy 设置查询日期范围为上月初到月末
            DateTime dtBegin = DateTime.Parse(bDateTime);//当日零点
            DateTime dtEnd = DateTime.Parse(eDateTime);//当日23:59:59
            dtpBegin.Value = dtBegin.AddMonths(-1).AddDays(1 - dtBegin.Day);//获取月初
            dtpEnd.Value = dtEnd.AddDays(1 - dtEnd.Day - 1);//月末
            //设置当前操作报表名称
            selectedMark = 0;
            lblReportName.Text = REPORT_NAME_NULL;
            //配置颜色常量
            clrNormal = Color.DarkBlue;
            clrExtra = Color.Red;
            clrError = Color.DarkOrange;
            clrManualConf = Color.Blue;
            //设置颜色说明Panel
            pnlClrInfo.Location = new Point(btnCompare.Location.X - pnlClrInfo.Width, 0);
            lblClr1.ForeColor = clrNormal;
            lblClr2.ForeColor = clrError;
            lblClr3.ForeColor = clrExtra;
            lblClr4.ForeColor = clrManualConf;
            pnlClrInfo.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoSearch();
        }
        //查询操作
        private void DoSearch()
        {
            searchWay = 0;
            //查询前记录选中行
            int _currentRow = 0;
            //int _currentColumn = 0;
            switch (selectedMark)
            {
                case 1:
                    if (dgvHis.CurrentCell != null)
                    {
                        _currentRow = this.dgvHis.CurrentCell.RowIndex;
                        //_currentColumn = this.dgvHis.CurrentCell.ColumnIndex ;
                    }
                    break;
                case 2:
                    if (dgvBank.CurrentCell != null)
                    {
                        _currentRow = this.dgvBank.CurrentCell.RowIndex;
                        //_currentColumn = this.dgvBank.CurrentCell.ColumnIndex;
                    }
                    break;
            }
            try
            {
                ParameterEx[] parameters = new ParameterEx[4];
                parameters[0].Text = "@StartTime";
                parameters[0].Value = dtpBegin.Value.ToString("yyyy-MM-dd HH:mm:ss");

                parameters[1].Text = "@EndTime";
                parameters[1].Value = dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

                parameters[2].Text = "@TerminalNo";
                parameters[2].Value = txtTerminalNo.Text.Trim();

                parameters[3].Text = "@JLZT";
                parameters[3].Value = cbbJLZT.SelectedIndex;

                ds = new DataSet();
                if (this.FormMode == FORM_MODE_POS)//Mod by Hxy 20150108
                {
                    db.AdapterFillDataSet("SP_BANK_CUP_TJ", parameters, ds, "tjmx", 30);
                }
                else
                {
                    db.AdapterFillDataSet("SP_BANK_CUP_TJ_ZJJ", parameters, ds, "tjmx", 30);
                }
                Fun.AddRowtNo(ds.Tables[0]);
                Fun.AddRowtNo(ds.Tables[1]);
                this.dgvHis.DataSource = ds.Tables[0];
                this.dgvBank.DataSource = ds.Tables[1];

                // ds.Tables[0].WriteXml("test.xml");

                this.DGVShowSet();

                lblyyzs.Text = ds.Tables[0].Rows.Count.ToString();
                lblyyje.Text = ds.Tables[0].Compute("SUM(" + HIS_DEAL_MONEY + ")", "").ToString();

                lblyhzs.Text = ds.Tables[1].Rows.Count.ToString();
                if (this.FormMode == FORM_MODE_POS)
                {
                    lblyhje.Text = ds.Tables[1].Compute("SUM(" + POS_DEAL_MONEY + ")", "").ToString();
                }
                else
                {
                    lblyhje.Text = ds.Tables[1].Compute("SUM(" + "交易金额" + ")", "").ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //设置选中行
            switch (selectedMark)
            {
                case 1:
                    if (dgvHis.Rows.Count > 0)
                    {
                        if (dgvHis.Rows.Count > _currentRow)
                        {
                            dgvHis.Rows[_currentRow].Selected = true;
                            dgvHis.FirstDisplayedScrollingRowIndex = _currentRow;
                        }
                        else
                        {
                            dgvHis.Rows[dgvHis.Rows.Count - 1].Selected = true;
                            dgvHis.FirstDisplayedScrollingRowIndex = dgvHis.Rows.Count - 1;
                        }

                    }
                    //                     if (dgvHis.Columns.Count > _currentColumn)
                    //                     {
                    //                         dgvHis.Columns[_currentColumn].Selected = true;
                    //                     }
                    //                     else
                    //                     {
                    //                         dgvHis.Columns[dgvHis.Columns.Count - 1].Selected = true;
                    //                     }
                    break;
                case 2:
                    if (dgvBank.Rows.Count > 0)
                    {
                        if (dgvBank.Rows.Count > _currentRow)
                        {
                            dgvBank.Rows[_currentRow].Selected = true;
                            dgvBank.FirstDisplayedScrollingRowIndex = _currentRow;
                        }
                        else
                        {
                            dgvBank.Rows[dgvBank.Rows.Count - 1].Selected = true;
                            dgvBank.FirstDisplayedScrollingRowIndex = dgvBank.Rows.Count - 1;
                        }
                    }
                    //                     if (dgvBank.Columns.Count > _currentColumn)
                    //                     {
                    //                         dgvBank.Columns[_currentColumn].Selected = true;
                    //                     }
                    //                     else
                    //                     {
                    //                         dgvBank.Columns[dgvBank.Columns.Count - 1].Selected = true;
                    //                     }
                    break;
            }
            //设置颜色
            //this.DGVColorSet();
        }
        #region 作废
        //DGV颜色设置
        //         private void DGVColorSet()
        //         {
        //             if ((dgvHis.Rows.Count <= 0) || (dgvBank.Rows.Count <= 0))
        //             {
        //                 return;
        //             }
        //             int matchState = 0;
        //             DataGridViewRow drHis = null;
        //             DataGridViewRow drBank = null;
        //             //循环处理dgvHis
        //             for (int i = 0; i < dgvHis.Rows.Count; i++)
        //             {
        //                 drHis = dgvHis.Rows[i];
        //                 matchState = Convert.ToInt32(drHis.Cells[2].Value.ToString());
        //                 //0未匹配，1匹配，2金额不匹配，3未匹配，4未匹配但人工确认
        //                 switch (matchState)
        //                 {
        //                     case MATCH_STATE_NORMAL:
        //                         drHis.DefaultCellStyle.ForeColor = clrNormal;
        //                         break;
        //                     case MATCH_STATE_ERROR:
        //                         drHis.DefaultCellStyle.ForeColor = clrError;
        //                         break;
        //                     case MATCH_STATE_EXTRA:
        //                         drHis.DefaultCellStyle.ForeColor = clrExtra;
        //                         break;
        //                     case MATCH_STATE_MANUALCONF:
        //                         drHis.DefaultCellStyle.ForeColor = clrManualConf;
        //                         break;
        //                 }
        //             }
        //             //循环处理dgvBank
        //             for (int i = 0; i < dgvBank.Rows.Count; i++)
        //             {
        //                 drBank = dgvBank.Rows[i];
        //                 matchState = Convert.ToInt32(drBank.Cells[2].Value.ToString());
        //                 //0未匹配，1匹配，2金额不匹配，3未匹配，4未匹配但人工确认
        //                 switch (matchState)
        //                 {
        //                     case MATCH_STATE_NORMAL:
        //                         drBank.DefaultCellStyle.ForeColor = clrNormal;
        //                         break;
        //                     case MATCH_STATE_ERROR:
        //                         drBank.DefaultCellStyle.ForeColor = clrError;
        //                         break;
        //                     case MATCH_STATE_EXTRA:
        //                         drBank.DefaultCellStyle.ForeColor = clrExtra;
        //                         break;
        //                     case MATCH_STATE_MANUALCONF:
        //                         drBank.DefaultCellStyle.ForeColor = clrManualConf;
        //                         break;
        //                 }
        //             }
        //         }
        #endregion

        private void tsmt_excel1_Click(object sender, EventArgs e)
        {
            string swhere = "";

            swhere = "对账日期从:" + dtpBegin.Value.ToString() + " 到:" + dtpEnd.Value.ToString();
            ts_jc_log.ExcelOper.ExportData_ForDataTable(this.dgvHis, fHospitalName + lblTitle.Text + "(医院端) " + swhere);
        }

        private void tsmt_excel2_Click(object sender, EventArgs e)
        {
            string swhere = "";

            swhere = "对账日期从:" + dtpBegin.Value.ToString() + " 到:" + dtpEnd.Value.ToString();
            ts_jc_log.ExcelOper.ExportData_ForDataTable(this.dgvBank, fHospitalName + lblTitle.Text + "(银行端) " + swhere);
        }

        private void btnAddRemarks_Click(object sender, EventArgs e)
        {
            bool isModOk = true;
            if (!this.CheckValid())
            {
                return;
            }
            switch (selectedMark)
            {
                case 1:
                    if (!ModHisRemarks())
                    {
                        isModOk = false;
                    }
                    break;
                case 2:
                    if (!ModBankRemarks())
                    {
                        isModOk = false;
                    }
                    break;
            }
            if (!isModOk)
            {
                MessageBox.Show("修改失败！", "提示");
            }
            else
            {
                if (chkModMemoThenRefresh.Checked)
                {
                    MessageBox.Show("修改成功！", "提示");
                }
                else
                {
                    MessageBox.Show("修改成功，若要查看修改结果请重新查询！", "提示");
                }
            }
        }
        //处理输入的单引号
        private string InputStrFormated(string aInputStr)
        {
            return aInputStr.Replace("'", "''");
        }
        //修改备注
        private bool ModHisRemarks()
        {
            string strSQL = "";
            if (this.FormMode == FORM_MODE_POS)
            {
                strSQL = string.Format(@"update POS_PAY_RECORD set BZ = '{0}' where PAY_ID = '{1}'",
                   InputStrFormated(textRemarks.Text.Trim()), dgvHis.SelectedRows[0].Cells[0].Value.ToString());
            }
            else
            {
                strSQL = string.Format(@"update ZZ_SAVE_LSH_BCM set Memo = '{0}' where PAY_ID = '{1}'",
                    InputStrFormated(textRemarks.Text.Trim()), dgvHis.SelectedRows[0].Cells[0].Value.ToString());
            }
            int SQLresult = 0;
            SQLresult = db.DoCommand(strSQL);
            if (SQLresult < 0)
            {
                return false;
            }
            if (chkModMemoThenRefresh.Checked)
            {
                switch (searchWay)
                {
                    case 0:
                        this.DoSearch();
                        break;
                    case 1:
                        this.CompareData();
                        if (isFiltered)
                        {
                            this.GetException();
                        }
                        break;
                }
            }
            return true;
        }
        private bool ModBankRemarks()
        {
            string strSQL = "";
            if (this.FormMode == FORM_MODE_POS)
            {
                strSQL = string.Format("update BANK_SAVE_HNYL_RECORD set BZ = '{0}' where ID = '{1}'",
                    InputStrFormated(textRemarks.Text.Trim()), dgvBank.SelectedRows[0].Cells[0].Value.ToString());
            }
            else
            {
                //strSQL = string.Format("update BANK_SAVE_HNYL_RECORD set BZ = '{0}' where ID = '{1}'",
                //    InputStrFormated(textRemarks.Text.Trim()), dgvBank.SelectedRows[0].Cells[0].Value.ToString());
            }
            int SQLresult = 0;
            SQLresult = db.DoCommand(strSQL);
            if (SQLresult < 0)
            {
                return false;
            }
            if (chkModMemoThenRefresh.Checked)
            {
                switch (searchWay)
                {
                    case 0:
                        this.DoSearch();
                        break;
                    case 1:
                        this.CompareData();
                        if (isFiltered)
                        {
                            this.GetException();
                        }
                        break;
                }
            }
            return true;
        }
        //检查合法性
        private bool CheckValid()
        {
            if ((textRemarks.Text).Length > 50)
            {
                MessageBox.Show("备注的长度需要小于50个字符！", "提示");
                return false;
            }
            if (selectedMark == 0)
            {
                MessageBox.Show("请先单击选择一个报表再做修改！", "提示");
                return false;
            }
            else if (selectedMark == 1)
            {
                if (dgvHis.Rows.Count == 0)
                {
                    MessageBox.Show("所选报表没有数据！", "提示");
                    return false;
                }
            }
            else if (selectedMark == 2)
            {
                if (dgvBank.Rows.Count == 0)
                {
                    MessageBox.Show("所选报表没有数据！", "提示");
                    return false;
                }
            }
            return true;
        }

        private void dgvBank_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedMark = 2;
                lblReportName.Text = REPORT_NAME_BANK;
                if (dgvBank.SelectedRows.Count > 0)
                {
                    textRemarks.Text = dgvBank.SelectedRows[0].Cells["备注"].Value.ToString();//设置为单选
                }
                else
                {
                    textRemarks.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void dgvHis_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                selectedMark = 1;
                if (pnlDetail.Visible)//单击报表时，如果Detail没有关闭，则关闭
                {
                    pnlDetail.Visible = false;
                }
                lblReportName.Text = REPORT_NAME_HIS;
                if (dgvHis.SelectedRows.Count > 0)
                {
                    textRemarks.Text = dgvHis.SelectedRows[0].Cells["备注"].Value.ToString();//设置为单选
                }
                else
                {
                    textRemarks.Text = "";
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void FrmCUPBankChecking_Resize(object sender, EventArgs e)
        {
            //设置分割线位置
            panel1.Width = this.Width / 2;
            //设置Detail窗体的位置
            pnlDetail.Location = new System.Drawing.Point(panel4.Location.X, panel4.Location.Y);
            pnlDetail.Size = new System.Drawing.Size(panel4.Size.Width, panel4.Size.Height);
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDateTime(dtpEnd.Value) - Convert.ToDateTime(dtpBegin.Value)).Days > 31)
            {
                if (MessageBox.Show("当选择过长的日期段进行数据比对时，需要较长的时间，确认继续？",
                    "提示", MessageBoxButtons.OKCancel) != DialogResult.OK) return;
            }
            if (this.FormMode == FORM_MODE_POS)
            {
                CompareData();
            }
            else
            {
                CompareDataZZ();
            }
        }

        //按照时间进行数据比对，如果交易发生在同一秒（且银行卡号相同——基本不可能），则数据匹配会出现不正确的情况，需要人工核对。——因为His的记录表中没有Pos流水号。
        //(strHisBankNo.Remove(0, strHisBankNo.Length - 4) == strPosBankNo.Remove(0, strPosBankNo.Length - 4)
        //Mod by Hxy 20140923 根据与银联方沟通，可以根据“交易日期”+“批次号”+“凭证号”来进行对账。
        private void CompareData()
        {
            ////先查询一次，并使之按时间排序，能够节省后续比对时间。
            //ClearComparedData();
            DoSearch();
            searchWay = 1;
            SetEnable(false);
            //             strHisIDSet = "0";
            //             strPosIDSet = "0";
            try
            {
                if ((dgvHis.Rows.Count <= 0) || (dgvBank.Rows.Count <= 0))
                {
                    MessageBox.Show("请先完成查询并确认表格内有数据再进行比对！", "提示");
                    return;
                }
                DataGridViewRow drHis = null;
                DataGridViewRow drBank = null;
                string strHisID = "";
                string strHisPosDealTime = "";//交易日期
                string strHisDealMoney = "";//交易金额
                //string strHisBankNo = "";
                string strHisPosZDH = "";//终端号
                string strHisPosJYLX = "";//交易类型
                string strHisPosPCH = "";//批次号
                string strHisPosPZH = "";//凭证号

                string strPosID = "";
                string strPosDealTime = "";
                string strPosDealMoney = "";
                string strPosZDH = "";//终端号
                string strPosJYLX = "";//交易类型
                string strPosXTCKH = "";//系统参考号= 批次号+凭证号
                //string strPosBankNo = "";
                for (int i = 0; i < dgvHis.Rows.Count; i++)
                {
                    drHis = dgvHis.Rows[i];
                    strHisID = drHis.Cells[0].Value.ToString().Trim();
                    strHisPosDealTime = Convert.ToDateTime(drHis.Cells[HIS_POS_DEAL_TIME].Value.ToString()).ToString("yyyy-MM-dd");//交易日期
                    strHisDealMoney = drHis.Cells[HIS_DEAL_MONEY].Value.ToString().Trim();//交易金额
                    strHisPosZDH = drHis.Cells[HIS_POS_ZDH].Value.ToString().Trim();//终端号
                    strHisPosJYLX = drHis.Cells[HIS_POS_JYLX].Value.ToString().Trim();
                    strHisPosPCH = drHis.Cells[HIS_POS_PCH].Value.ToString().Trim();
                    strHisPosPZH = drHis.Cells[HIS_POS_PZH].Value.ToString().Trim();
                    //strHisBankNo = drHis.Cells[HIS_BANK_NO].Value.ToString().Trim();
                    for (int j = 0; j < dgvBank.Rows.Count; j++)
                    {
                        drBank = dgvBank.Rows[j];
                        if ((drBank.DefaultCellStyle.ForeColor == clrNormal) ||
                            (drBank.DefaultCellStyle.ForeColor == clrError))//如果是已经匹配过的
                        {
                            continue;
                        }
                        strPosID = drBank.Cells[0].Value.ToString().Trim();
                        strPosDealTime = Convert.ToDateTime(drBank.Cells[POS_DEAL_TIME].Value.ToString()).ToString("yyyy-MM-dd");
                        strPosDealMoney = drBank.Cells[POS_DEAL_MONEY].Value.ToString().Trim();
                        strPosZDH = drBank.Cells[POS_TERM_NO].Value.ToString().Trim();
                        strPosJYLX = drBank.Cells[POS_JYLX].Value.ToString().Trim();
                        strPosXTCKH = drBank.Cells[POS_XTCKH].Value.ToString().Trim();
                        //strPosBankNo = drBank.Cells[POS_BANK_NO].Value.ToString().Trim();
                        //根据文档要求，对账方法：日期+终端号+交易类型+批次号、凭证号（系统参考号）+ 交易金额
                        if ((strHisPosDealTime == strPosDealTime) &&
                            (strHisPosZDH == strPosZDH) &&
                            (strHisPosJYLX == strPosJYLX) &&
                            ((strHisPosPCH + strHisPosPZH) == strPosXTCKH) &&
                            (strHisDealMoney == strPosDealMoney))
                        /*                        if (strHisPosDealTime == strPosDealTime)*/
                        {
                            drHis.DefaultCellStyle.ForeColor = clrNormal;
                            drBank.DefaultCellStyle.ForeColor = clrNormal;
                            if (i != dgvHis.Rows.Count - 1)//如果不是His表最后一条，则找到匹配，直接退出。
                            {
                                break;
                            }
                        }
                        if (i == dgvHis.Rows.Count - 1)
                        {
                            //如果His表已经全部遍历完，而Bank表还没找到匹配，且不是已经人工确认的，则全部认为是多出来的。
                            if ((drBank.DefaultCellStyle.ForeColor != clrNormal)
                                && (drBank.DefaultCellStyle.ForeColor != clrError)
                                && (drBank.Cells[1].Value.ToString() != "1"))//代表是人工确认的。
                            {
                                drBank.DefaultCellStyle.ForeColor = clrExtra;
                                // strPosIDSet += "," + strPosID;
                            }
                            else if (drBank.Cells[1].Value.ToString() == "1")
                            {
                                drBank.DefaultCellStyle.ForeColor = clrManualConf;
                            }
                        }
                    }
                    if ((drHis.DefaultCellStyle.ForeColor != clrNormal) &&
                        (drHis.DefaultCellStyle.ForeColor != clrError))//如果His报表没能找到匹配，说明是多余的。
                    {
                        drHis.DefaultCellStyle.ForeColor = clrExtra;
                        //strHisIDSet += "," + strHisID;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                SetEnable(true);
            }
        }

        //自助机的对账。根据交通文件上传文件的内容，只需要根据系统参考号和金额来进行对账。
        private void CompareDataZZ()
        {
            ////先查询一次，并使之按时间排序，能够节省后续比对时间。
            DoSearch();
            searchWay = 1;
            SetEnable(false);
            try
            {
                if ((dgvHis.Rows.Count <= 0) || (dgvBank.Rows.Count <= 0))
                {
                    MessageBox.Show("请先完成查询并确认表格内有数据再进行比对！", "提示");
                    return;
                }
                DataGridViewRow drHis = null;
                DataGridViewRow drBank = null;
                string strHisID = "";
                string strHisDealMoney = "";//交易金额
                string strHisXTCKH = "";//系统参考号

                string strPosID = "";
                string strPosDealMoney = "";
                string strPosXTCKH = "";//系统参考号
                for (int i = 0; i < dgvHis.Rows.Count; i++)
                {
                    drHis = dgvHis.Rows[i];
                    strHisID = drHis.Cells[0].Value.ToString().Trim();
                    strHisDealMoney = drHis.Cells["交易金额"].Value.ToString().Trim();//交易金额
                    strHisXTCKH = drHis.Cells["系统参考号"].Value.ToString().Trim();
                    for (int j = 0; j < dgvBank.Rows.Count; j++)
                    {
                        drBank = dgvBank.Rows[j];
                        if ((drBank.DefaultCellStyle.ForeColor == clrNormal) ||
                            (drBank.DefaultCellStyle.ForeColor == clrError))//如果是已经匹配过的
                        {
                            continue;
                        }
                        strPosID = drBank.Cells[0].Value.ToString().Trim();
                        strPosDealMoney = drBank.Cells["交易金额"].Value.ToString().Trim();
                        strPosXTCKH = drBank.Cells["系统参考号"].Value.ToString().Trim();
                        //系统参考号和金额需要对上
                        if ((strHisXTCKH == strPosXTCKH) &&
                            (strHisDealMoney == strPosDealMoney))
                        {
                            drHis.DefaultCellStyle.ForeColor = clrNormal;
                            drBank.DefaultCellStyle.ForeColor = clrNormal;
                            if (i != dgvHis.Rows.Count - 1)//如果不是His表最后一条，则找到匹配，直接退出。
                            {
                                break;
                            }
                        }
                        if (i == dgvHis.Rows.Count - 1)
                        {
                            if ((drBank.DefaultCellStyle.ForeColor != clrNormal)
                                && (drBank.DefaultCellStyle.ForeColor != clrError))
                            {
                                drBank.DefaultCellStyle.ForeColor = clrExtra;
                            }
                        }
                    }
                    if ((drHis.DefaultCellStyle.ForeColor != clrNormal) &&
                        (drHis.DefaultCellStyle.ForeColor != clrError))//如果His报表没能找到匹配，说明是多余的。
                    {
                        drHis.DefaultCellStyle.ForeColor = clrExtra;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                SetEnable(true);
            }
        }

        //设置界面可用性
        private void SetEnable(bool aState)
        {
            panel1.Enabled = aState;
            panel2.Enabled = aState;
            panel4.Enabled = aState;
        }

        private void textRemarks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnAddRemarks_Click(null, null);
            }
        }
        //设置颜色提示板的显示
        private void btnCompare_MouseEnter(object sender, EventArgs e)
        {
            if (pnlClrInfo.Visible == false)
            {
                pnlClrInfo.Visible = true;
            }
        }
        //鼠标离开时控制颜色提示板
        private void btnCompare_MouseLeave(object sender, EventArgs e)
        {
            if (pnlClrInfo.Visible == true)
            {
                pnlClrInfo.Visible = false;
            }
        }
        //设置DataGridView的显示，如是否隐藏，列宽等。注意隐藏的列与对账有关。
        private void DGVShowSet()
        {
            if ((dgvHis.DataSource == null) || (dgvBank.DataSource == null))
            {
                return;
            }
            if (this.FormMode == FORM_MODE_POS)
            {
                //Mod by Hxy 20140812隐藏列
                this.dgvHis.Columns[0].Visible = false;//ID
                this.dgvHis.Columns[1].Visible = false;//记录状态
                this.dgvHis.Columns[2].Visible = false;//HISLB
                this.dgvHis.Columns[3].Visible = false;//MZJS_ID
                this.dgvHis.Columns[4].Visible = false;//ZYDISCHARGE_ID
                this.dgvHis.Columns[5].Visible = false;//ZYDEPOSITS_ID
                this.dgvHis.Columns[6].Visible = false;//批次号POS_FH_PCH
                this.dgvHis.Columns[7].Visible = false;//凭证号POS_FH_PZH
                this.dgvHis.Columns[8].Visible = false;//交易类型POS_CR_JYLX

                this.dgvBank.Columns[0].Visible = false;
                this.dgvBank.Columns[1].Visible = false;
                this.dgvBank.Columns[2].Visible = false;//BANK_XTCKH
                this.dgvBank.Columns[3].Visible = false;
                //Mod by Hxy 20140812 设置列长度
                this.dgvHis.Columns[HIS_DEAL_TIME].Width = 160;
                this.dgvHis.Columns[HIS_BANK_NO].Width = 160;
                this.dgvHis.Columns[HIS_POS_DEAL_TIME].Width = 160;
                this.dgvHis.Columns["序号"].Width = 50;
                this.dgvBank.Columns[POS_BANK_NO].Width = 160;
                this.dgvBank.Columns[POS_DEAL_TIME].Width = 160;//银行的交易日期精确到秒
                this.dgvBank.Columns["序号"].Width = 50;
                //Add by Hxy 20140826 列格式设置
                this.dgvHis.Columns[HIS_DEAL_TIME].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvHis.Columns[HIS_POS_DEAL_TIME].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvHis.Columns["交易金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dgvBank.Columns[POS_DEAL_TIME].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvBank.Columns["POS返回金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if(this.FormMode == FORM_MODE_SS)
            {
                //Mod by Hxy 20140812隐藏列
                this.dgvHis.Columns[0].Visible = false;//ID
                this.dgvHis.Columns[1].Visible = false;//记录状态
                this.dgvHis.Columns[2].Visible = false;//HISLB

                this.dgvBank.Columns[0].Visible = false;
                //Mod by Hxy 20140812 设置列长度
                this.dgvHis.Columns[HIS_DEAL_TIME].Width = 160;
                this.dgvHis.Columns[HIS_BANK_NO].Width = 160;
                this.dgvHis.Columns[HIS_POS_DEAL_TIME].Width = 160;
                this.dgvHis.Columns["序号"].Width = 50;
                this.dgvBank.Columns["银行交易时间"].Width = 160;
                this.dgvBank.Columns["序号"].Width = 50;
                //Add by Hxy 20140826 列格式设置
                this.dgvHis.Columns[HIS_DEAL_TIME].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvHis.Columns[HIS_POS_DEAL_TIME].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvHis.Columns["交易金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                this.dgvBank.Columns["银行交易时间"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                this.dgvBank.Columns["交易金额"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        //双击跳转到POS屏蔽管理
        private void dgvBank_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.FormMode == FORM_MODE_SS)
            //    return;
            //if (e.RowIndex < 0)
            //    return;
            //string strPosNo = dgvBank.SelectedRows[0].Cells[POS_TERM_NO].Value.ToString();
            //FrmCUPBankCheckingPosMgr frmPosMgr = new FrmCUPBankCheckingPosMgr(this.db, strPosNo, dtpBegin.Value, dtpEnd.Value);
            //string str = searchWay.ToString();
            //frmPosMgr.ShowDialog();
            //if (searchWay == 1)
            //{
            //    this.CompareData();
            //    if (isFiltered)
            //    {
            //        this.GetException();
            //    }
            //}
            //else
            //{
            //    this.DoSearch();
            //}
        }

        private void tsmt_print1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tb = (DataTable)dgvHis.DataSource;
                ts_mz_report.DataSet1 dset = new ts_mz_report.DataSet1();
                DataRow dr;
                for (int i = 0; i <= tb.Rows.Count - 1; i++)
                {
                    dr = dset._银联自助机对账统计_医院端_.NewRow();
                    int x = i + 1;

                    dr["POS返回终端号"] = Convert.ToString(tb.Rows[i]["POS返回终端号"]);
                    dr["交易金额"] = Convert.ToDecimal(Convertor.IsNull(tb.Rows[i]["交易金额"], "0"));
                    dr["卡号"] = Convert.ToString(tb.Rows[i]["卡号"]);
                    dr["POS交易时间"] = Convert.ToString(tb.Rows[i]["POS交易时间"]);
                    dr["操作员名称"] = Convert.ToString(tb.Rows[i]["操作员名称"]);
                    dr["银行名称"] = Convert.ToString(tb.Rows[i]["银行名称"]);
                    dr["备注"] = Convert.ToString(tb.Rows[i]["备注"]);
                    dset._银联自助机对账统计_医院端_.Rows.Add(dr);
                }

                ParameterEx[] parameters = new ParameterEx[4];

                parameters[0].Text = "医院名称";
                parameters[0].Value = fHospitalName;

                parameters[1].Text = "填报日期";
                parameters[1].Value = DateManager.ServerDateTimeByDBType(db).ToShortDateString();
                parameters[2].Text = "备注";
                parameters[2].Value = "统计日期:" + dtpBegin.Value.ToString("yyyy-MM-dd HH:mm:ss") + " 到 " + dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

                parameters[3].Text = "统计人";
                parameters[3].Value = fOperName;

                string strReportDir = fApplicationDir + "\\Report\\MZ_银联对账统计(医院端).rpt";
                if (this.FormMode == FORM_MODE_SS)
                {
                    strReportDir = fApplicationDir + "\\Report\\MZ_银联对账统计(医院端)_ZZ.rpt";
                }
                TrasenFrame.Forms.FrmReportView f = new TrasenFrame.Forms.FrmReportView(dset._银联自助机对账统计_医院端_, strReportDir, parameters);
                if (f.LoadReportSuccess)
                    f.Show();
                else
                    f.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmt_print2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tb = (DataTable)dgvBank.DataSource;
                ts_mz_report.DataSet1 dset = new ts_mz_report.DataSet1();
                DataRow dr;
                for (int i = 0; i <= tb.Rows.Count - 1; i++)
                {
                    dr = dset._银联自助机对账统计_银行端_.NewRow();
                    int x = i + 1;
                    if (this.FormMode == FORM_MODE_POS)
                    {
                        dr["POS返回商户号"] = Convert.ToString(tb.Rows[i]["POS返回商户号"]);
                        dr["POS返回终端号"] = Convert.ToString(tb.Rows[i]["POS返回终端号"]);
                        dr["流水号"] = Convert.ToString(tb.Rows[i]["流水号"]);
                        dr["POS返回金额"] = Convert.ToDecimal(Convertor.IsNull(tb.Rows[i]["POS返回金额"], "0"));
                        dr["银行卡号"] = Convert.ToString(tb.Rows[i]["银行卡号"]);
                        dr["银行交易时间"] = Convert.ToString(tb.Rows[i]["银行交易时间"]);
                        dr["备注"] = Convert.ToString(tb.Rows[i]["备注"]);
                    }
                    else
                    {
                        dr["流水号"] = Convert.ToString(tb.Rows[i]["系统参考号"]);
                        //dr["POS返回终端号"] = Convert.ToString(tb.Rows[i]["POS返回终端号"]);
                        //dr["流水号"] = Convert.ToString(tb.Rows[i]["流水号"]);
                        dr["POS返回金额"] = Convert.ToDecimal(Convertor.IsNull(tb.Rows[i]["交易金额"], "0"));
                        dr["银行卡号"] = Convert.ToString(tb.Rows[i]["诊疗卡号"]);
                        dr["银行交易时间"] = Convert.ToString(tb.Rows[i]["银行交易时间"]);
                        dr["备注"] = Convert.ToString(tb.Rows[i]["备注"]);
                    }
                    for (int j = 1; j <= 5; j++)
                    {
                        dr["备用" + j.ToString()] = "";
                    }
                    dset._银联自助机对账统计_银行端_.Rows.Add(dr);
                }
                string rptName = "";
                if (this.FormMode == FORM_MODE_POS)
                {
                    rptName = "\\Report\\MZ_银联对账统计(银行端).rpt";
                }
                else
                {
                    rptName = "\\Report\\MZ_银联对账统计(银行端)_ZZ.rpt";
                }

                ParameterEx[] parameters = new ParameterEx[4];

                parameters[0].Text = "医院名称";
                parameters[0].Value = fHospitalName;

                parameters[1].Text = "填报日期";
                parameters[1].Value = DateManager.ServerDateTimeByDBType(db).ToShortDateString();

                parameters[2].Text = "备注";
                parameters[2].Value = "统计日期:" + dtpBegin.Value.ToString("yyyy-MM-dd HH:mm:ss") + " 到 " + dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss");

                parameters[3].Text = "统计人";
                parameters[3].Value = fOperName;

                TrasenFrame.Forms.FrmReportView f = new TrasenFrame.Forms.FrmReportView(dset._银联自助机对账统计_银行端_, fApplicationDir +
                    rptName, parameters);
                if (f.LoadReportSuccess)
                    f.Show();
                else
                    f.Dispose();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnJumpAbnormal_Click(object sender, EventArgs e)
        {
            if (searchWay != 1)
            {
                MessageBox.Show("请先进行数据比对后再使用此功能！", "提示");
                return;
            }
            if (selectedMark == 0)
            {
                MessageBox.Show("请先选中一张报表！", "提示");
                return;
            }
            switch (selectedMark)
            {
                case 1:
                    if (dgvHis.Rows.Count > 0)
                    {
                        if (JumpAbnormal(dgvHis) == 0)
                        {
                            MessageBox.Show("从选中行向下没有找到异常数据，如需重新检索请选中报表首行！", "提示");
                        }
                    }
                    else
                    {
                        MessageBox.Show("所选报表没有数据！", "提示");
                    }
                    break;
                case 2:
                    if (dgvBank.Rows.Count > 0)
                    {
                        if (JumpAbnormal(dgvBank) == 0)
                        {
                            MessageBox.Show("从选中行向下没有找到异常数据，如需重新检索请选中报表首行！", "提示");
                        }
                    }
                    else
                    {
                        MessageBox.Show("所选报表没有数据！", "提示");
                    }
                    break;
            }
        }
        //通用版
        private int JumpAbnormal(DataGridView adgv)
        {
            DataGridViewRow dgvRow = null;

            for (int i = Convert.ToInt32(adgv.SelectedRows[0].Cells["序号"].Value.ToString()) + 1; i < adgv.Rows.Count; i++)//从当前选中行下一行开始
            {
                dgvRow = adgv.Rows[i];
                if (dgvRow.DefaultCellStyle.ForeColor == clrExtra || dgvRow.DefaultCellStyle.ForeColor == clrError)//有多出来的，或错误的。则选中
                {
                    //选中，并跳转
                    adgv.Rows[i].Selected = true;
                    adgv.FirstDisplayedScrollingRowIndex = i;
                    return 1;//返回1找到异常点
                }
            }
            return 0;//返回0未找到异常点
        }
        //鼠标移动到右上角的时候显示关闭按钮
        private void dgvHisDetail_MouseMove(object sender, MouseEventArgs e)
        {
            Rectangle rec = new Rectangle(dgvHisDetail.Size.Width - btnCloseDetail.Size.Height, 0, btnCloseDetail.Size.Height, btnCloseDetail.Size.Height);
            if (rec.Contains(e.X, e.Y))
            {
                btnCloseDetail.Visible = true;
            }
            else
            {
                btnCloseDetail.Visible = false;
            }
        }
        //关闭Detail窗口
        private void btnCloseDetail_Click(object sender, EventArgs e)
        {
            btnCloseDetail.Visible = false;
            pnlDetail.Visible = false;
        }
        //双击打开Detail窗口
        private void dgvHis_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.FormMode == FORM_MODE_SS)
                return;
            if (e.RowIndex < 0)
                return;
            string strHISLB = dgvHis.SelectedRows[0].Cells[2].Value.ToString();
            string strMZJS_ID = dgvHis.SelectedRows[0].Cells[3].Value.ToString();
            string strZYDISCHARGE_ID = dgvHis.SelectedRows[0].Cells[4].Value.ToString();
            string strZYDEPOSITS_ID = dgvHis.SelectedRows[0].Cells[5].Value.ToString();
            //设置Detail窗体的位置
            pnlDetail.Location = new System.Drawing.Point(panel4.Location.X, panel4.Location.Y);
            pnlDetail.Size = new System.Drawing.Size(panel4.Size.Width, panel4.Size.Height);
            btnCloseDetail.Visible = false;
            pnlDetail.Visible = true;
            switch (strHISLB)
            {
                case "1"://门诊
                    GetHisDetail(1, strMZJS_ID, strZYDISCHARGE_ID, strZYDEPOSITS_ID);
                    break;
                case "2"://住院
                    if (strZYDISCHARGE_ID != Guid.Empty.ToString())
                    {
                        GetHisDetail(2, strMZJS_ID, strZYDISCHARGE_ID, strZYDEPOSITS_ID);
                    }
                    else
                    {
                        GetHisDetail(3, strMZJS_ID, strZYDISCHARGE_ID, strZYDEPOSITS_ID);
                    }
                    break;
                default:

                    break;
            }
        }
        //获取His报表详细信息
        private void GetHisDetail(int aSearchType, string aMZJS_ID, string aZYDISCHARGE_ID, string aZYDEPOSITS_ID)
        {
            try
            {
                ParameterEx[] parameters = new ParameterEx[4];
                parameters[0].Text = "@MZJS_ID";
                parameters[0].Value = aMZJS_ID;

                parameters[1].Text = "@ZYDISCHARGE_ID";
                parameters[1].Value = aZYDISCHARGE_ID;

                parameters[2].Text = "@ZYDEPOSITS_ID";
                parameters[2].Value = aZYDEPOSITS_ID;

                parameters[3].Text = "@SearchType";
                parameters[3].Value = aSearchType;

                ds = new DataSet();
                db.AdapterFillDataSet("SP_BANK_CUP_HIS_DETAIL", parameters, ds, "tjmx", 30);
                this.dgvHisDetail.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //获取终端号到下输入框
        private void tsmGetTerminalNo1_Click(object sender, EventArgs e)
        {
            txtTerminalNo.Text = dgvHis.SelectedRows[0].Cells[POS_TERM_NO].Value.ToString();
        }
        //获取终端号到下输入框
        private void tsmGetTerminalNo_Click(object sender, EventArgs e)
        {
            txtTerminalNo.Text = dgvBank.SelectedRows[0].Cells[POS_TERM_NO].Value.ToString();
        }
        //过滤出异常的账务信息
        private void btnGetException_Click(object sender, EventArgs e)
        {
            GetException();
        }
        private void GetException()
        {
            if (searchWay != 1)
            {
                return;
            }
            DataGridViewRow drHis = null;
            DataGridViewRow drBank = null;
            DataTable dtHisTemp = new DataTable();
            DataTable dtBankTemp = new DataTable();
            dtHisTemp = ((DataTable)dgvHis.DataSource).Clone();//获取列
            dtBankTemp = ((DataTable)dgvBank.DataSource).Clone();
            for (int i = 0; i < dgvHis.Rows.Count; i++)
            {
                drHis = dgvHis.Rows[i];
                if (drHis.DefaultCellStyle.ForeColor == clrExtra)
                {
                    dtHisTemp.Rows.Add((drHis.DataBoundItem as DataRowView).Row.ItemArray);//DataGridViewRow转化为DataRow
                }
            }
            for (int i = 0; i < dgvBank.Rows.Count; i++)
            {
                drBank = dgvBank.Rows[i];
                if (drBank.DefaultCellStyle.ForeColor == clrExtra)
                {
                    dtBankTemp.Rows.Add((drBank.DataBoundItem as DataRowView).Row.ItemArray);
                }
            }
            dgvHis.DataSource = dtHisTemp;
            dgvBank.DataSource = dtBankTemp;
            isFiltered = true;
        }

        private void chkModMemoThenRefresh_Click(object sender, EventArgs e)
        {
            if (chkModMemoThenRefresh.Checked)
            {
                MessageBox.Show("勾选此项时，若修改备注，会即时更新报表内容，效率较低，建议不勾选此项，在全部修改完成后，重新查询报表！", "提示");
            }
        }
        /*作废
        //获取门诊发票信息
        private void GetMZFPDetail(string aMZJS_ID)
        {
            string strSQL = string.Format(@"SELECT BLH AS 门诊号,
	BRXM AS 患者姓名,
	SFRQ AS 收费日期,
	emp.NAME AS 收费员姓名,
	DNLSH AS 电脑流水号,
	FPH AS 发票号,
	ZJE AS 总金额,
	dept.NAME AS 科室名称,
	emp2.NAME AS 医生名称
	--dept2.NAME AS 住院科室名称
 FROM dbo.MZ_FPB fpb
 LEFT JOIN dbo.JC_EMPLOYEE_PROPERTY emp ON fpb.SFY = emp.EMPLOYEE_ID
 LEFT JOIN dbo.JC_DEPT_PROPERTY dept ON fpb.KSDM = dept.DEPT_ID
 LEFT JOIN JC_EMPLOYEE_PROPERTY emp2 ON fpb.YSDM = emp.EMPLOYEE_ID
 WHERE JSID = '{0}' AND fpb.BSCBZ = 0", aMZJS_ID);
            DataTable dt = db.GetDataTable(strSQL);
            dgvHisDetail.DataSource = dt;
        }
        //获取住院信息
        private int GetZY_DISCHARGEDetail(string aZYDISCHARGE_ID)
        {
            string strSQL = string.Format(@"SELECT pat.INPATIENT_NO AS 住院号,
 pat.NAME AS 患者姓名,
 dept.NAME AS 所在科室,
 pat.DISCHARGE_DATE AS 结算时间,
 emp.NAME AS 操作人,
 NFEE AS 总费用,
 BILLNO AS 发票号,
 OLDBILLNO AS 老发票号
FROM dbo.ZY_DISCHARGE zydis
LEFT JOIN dbo.ZY_INPATIENT pat ON zydis.INPATIENT_ID = pat.INPATIENT_ID
LEFT JOIN dbo.JC_DEPT_PROPERTY dept ON zydis.DEPT_ID = dept.DEPT_ID
LEFT JOIN dbo.JC_EMPLOYEE_PROPERTY emp ON zydis.USERID = emp.EMPLOYEE_ID
WHERE id = '{0}' AND zydis.CANCEL_BIT = 0", aZYDISCHARGE_ID);
            DataTable dt = db.GetDataTable(strSQL);
            dgvHisDetail.DataSource = dt;
            return dt.Rows.Count;
        }
        //获取住院预交金信息
        private void GetZYDEPOSITSDetail(string aZYDEPOSITS_ID)
        {
            string strSQL = string.Format(@"SELECT pat.INPATIENT_NO 住院号,
	NVALUES 金额,
	BILLNO 票据号,
	OLDBILLNO 老票据号,
	pat.BOOK_DATE 登记时间,
	emp.NAME 登记员,
	CASE ARRIVE_BIT WHEN 1 THEN '已到账' ELSE '未知' END 到账状态,
	ARRIVE_DATE 到账时间,
	emp2.NAME 到账操作员
 FROM dbo.ZY_DEPOSITS zydep
LEFT JOIN dbo.ZY_INPATIENT pat ON zydep.INPATIENT_ID = pat.INPATIENT_ID
LEFT JOIN dbo.JC_EMPLOYEE_PROPERTY emp ON zydep.BOOK_USERID = emp.EMPLOYEE_ID
LEFT JOIN dbo.JC_EMPLOYEE_PROPERTY emp2 ON zydep.ARRIVE_USERID = emp2.EMPLOYEE_ID
WHERE id = '{0}' AND zydep.CANCEL_BIT = 0", aZYDEPOSITS_ID);
            DataTable dt = db.GetDataTable(strSQL);
            dgvHisDetail.DataSource = dt;
        }
         */
    }
}