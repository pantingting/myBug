namespace CHEER.PresentationLayer.Controls
{
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Text.RegularExpressions;
    using CHEER.PresentationLayer.CommonUse;
    using CHEER.Common;

    /// <summary>
    ///		LR 的摘要说明。
    /// </summary>
    public class LRSelect : CHEER.PresentationLayer.BaseControls
    {
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.ListBox LstLeft;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdSelone;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdSelAll;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdDelOne;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdDelAll;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtLstLeft;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtLstRight;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblLeft;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Label lblRight;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.ListBox LstRight;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdUp;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.Button cmdDown;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtLeftListWidth;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtAllowRepeat;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtMaxClientCount;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtNeedServerHelp;
        /// <summary>
        /// 
        /// </summary>
        protected System.Web.UI.WebControls.TextBox txtClientChanged;
        /// <summary>
        /// 客户端可以处理数据的最大数量
        /// </summary>
        public int MaxClientCount
        {
            get
            {
                int iMaxClientCount = int.Parse(txtMaxClientCount.Text);
                return iMaxClientCount;

            }
            set
            {
                txtMaxClientCount.Text = value.ToString();
            }
        }

        /// <summary>
        /// 左边list的宽度
        /// </summary>
        public int LeftListWidth
        {
            get
            {
                int iWidth = int.Parse(txtLeftListWidth.Text);
                return iWidth;

            }
            set
            {
                if (value <= 0)
                {
                    txtLeftListWidth.Text = "1";
                }
                else
                {
                    txtLeftListWidth.Text = value.ToString();
                }

            }
        }
        /// <summary>
        /// 是否显示上下异动的按钮
        /// </summary>
        public bool ShowUpDownButton
        {
            get
            {
                return this.cmdUp.Visible;
            }
            set
            {
                this.cmdUp.Visible = value;
                this.cmdDown.Visible = value;
            }
        }
        /// <summary>
        /// 是否允许重复ClientID
        /// </summary>
        private string AllowRepeat_ClientID
        {
            get
            {
                return txtAllowRepeat.ClientID;
            }
        }
        /// <summary>
        /// 是否显示上下异动的按钮
        /// </summary>
        public bool ShowLabel
        {
            get
            {
                return this.lblLeft.Visible;
            }
            set
            {
                this.lblLeft.Visible = value;
                this.lblRight.Visible = value;
            }
        }
        /// <summary>
        /// 设置左边的标签
        /// </summary>
        /// <param name="Text"></param>
        public void SetLeftLabel(string Text)
        {
            this.lblLeft.Text = Text;
        }
        /// <summary>
        /// 设置右边的标签
        /// </summary>
        /// <param name="Text"></param>
        public void SetRightLabel(string Text)
        {
            this.lblRight.Text = Text;
        }
        /// <summary>
        /// 左边list中是否允许有重复的项
        /// </summary>
        public bool AllowRepeat
        {
            get
            {
                if (txtAllowRepeat.Text.Trim().ToUpper() == "TRUE")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                txtAllowRepeat.Text = value.ToString().ToUpper();
            }
        }
        /// <summary>
        /// Base Url
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return GetBaseURL();
            }
        }
        /// <summary>
        /// 左边List 的ClientID
        /// </summary>
        public string LeftListBoxClientID
        {
            get
            {
                return LstLeft.ClientID;
            }
        }
        /// <summary>
        /// 右边List 的ClientID
        /// </summary>
        public string RightListBoxClientID
        {
            get
            {
                return LstRight.ClientID;
            }
        }
        /// <summary>
        /// 左边List的Textbox 的ClientID
        /// </summary>
        public string txtLstLeftClientID
        {
            get
            {
                return txtLstLeft.ClientID;
            }
        }

        /// <summary>
        /// 右边List的Textbox 的ClientID
        /// </summary>
        public string txtLstRightClientID
        {
            get
            {
                return txtLstRight.ClientID;
            }
        }
        /// <summary>
        /// 左边的List Box
        /// </summary>
        public ListBox LeftListBox
        {
            get
            {
                return LstLeft;
            }
        }
        /// <summary>
        /// 右边的List Box
        /// </summary>
        public ListBox RightListBox
        {
            get
            {
                return LstRight;
            }
        }
        /// <summary>
        /// 行数
        /// </summary>
        public int Rows
        {
            get
            {
                return this.LstLeft.Rows;
            }
            set
            {
                this.LstLeft.Rows = value;
                this.LstRight.Rows = value;
            }
        }
        /// <summary>
        /// 取得站点前缀
        /// </summary>
        /// <returns></returns>
        public string GetBaseURL()
        {
            if (this.Request.IsSecureConnection)
            {
                return @"https://" + this.Request.Url.Authority + this.Request.ApplicationPath + "/";
            }
            else
            {
                return @"http://" + this.Request.Url.Authority + this.Request.ApplicationPath + "/";
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            TextToList();
            if (!Page.IsPostBack)
            {
                iniPage();
            }
        }
        private void iniPage()
        {
            cmdSelone.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_right_out.gif')");
            cmdSelone.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_right_out.gif\")';");
            cmdSelone.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_right_over.gif\")';");
            cmdSelAll.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_rightall_out.gif')");
            cmdSelAll.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_rightall_out.gif\")';");
            cmdSelAll.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_rightall_over.gif\")';");
            cmdDelOne.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_left_out.gif')");
            cmdDelOne.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_left_out.gif\")';");
            cmdDelOne.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_left_over.gif\")';");
            cmdDelAll.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_leftall_out.gif')");
            cmdDelAll.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_leftall_out.gif\")';");
            cmdDelAll.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_leftall_over.gif\")';");
            cmdUp.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_top_out.gif')");
            cmdUp.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_top_out.gif\")';");
            cmdUp.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_top_over.gif\")';");
            cmdDown.Style.Add("background-image", "url('" + GetBaseURL() + "Image/button_down_out.gif')");
            cmdDown.Attributes.Add("onmouseout", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_down_out.gif\")';");
            cmdDown.Attributes.Add("onmouseover", "this.style.backgroundImage='url(\"" + GetBaseURL() + "Image/button_down_over.gif\")';");
            //			CommonMethod.LoadImageForButton(cmdSelone,CommonMethod.ButtonType.RIGHTMOVE);
            //			CommonMethod.LoadImageForButton(cmdSelAll,CommonMethod.ButtonType.RIGHTMOVEALL);
            //			CommonMethod.LoadImageForButton(cmdDelOne,CommonMethod.ButtonType.LEFTMOVE);
            //			CommonMethod.LoadImageForButton(cmdDelAll,CommonMethod.ButtonType.LEFTMOVEALL);
            string strS = LeftListBoxClientID + "," + RightListBoxClientID + "," + this.txtLstLeftClientID + "," + txtLstRightClientID + "," + this.txtAllowRepeat.ClientID + "," + txtNeedServerHelp.ClientID + "," + txtClientChanged.ClientID;
            this.cmdSelone.Attributes.Add("onclick", " return SelectItem(" + strS + ");");
            this.cmdSelAll.Attributes.Add("onclick", " return SelectAllItem(" + strS + ");");
            this.cmdDelOne.Attributes.Add("onclick", " return DeleteItem(" + strS + ");");
            this.cmdDelAll.Attributes.Add("onclick", " return DeleteAllItem(" + strS + ");");
            string strS2 = RightListBoxClientID + "," + txtLstRightClientID + "," + txtNeedServerHelp.ClientID + "," + txtClientChanged.ClientID;
            this.cmdUp.Attributes.Add("onclick", " return MoveUp(" + strS2 + ");");
            this.cmdDown.Attributes.Add("onclick", " return MoveDown(" + strS2 + ");");
            this.LeftListBox.Attributes.Add("ondblclick", " return SelectItem(" + strS + ");");
            this.LeftListBox.Attributes.Add("onkeydown", "if(event.keyCode==39){ return SelectItem(" + strS + ");}");
            this.RightListBox.Attributes.Add("ondblclick", " return DeleteItem(" + strS + ");");
            this.RightListBox.Attributes.Add("onkeydown", "if(event.keyCode==37){ return DeleteItem(" + strS + ");}else if(event.keyCode==38){return MoveUp(" + strS2 + ");}else if(event.keyCode==40){return MoveDown(" + strS2 + ");}");
        }

        /// <summary>
        /// 00,01,11,10 四种情况，第一位代表上一次情况，第二位代表当前情况
        /// </summary>
        /// <returns></returns>
        private string isNeedServerHelp()
        {
            string strLast = txtNeedServerHelp.Text.Trim().DBReplace();
            //			if(LstLeft.Items.Count>MaxClientCount || LstRight.Items.Count>MaxClientCount)
            //			{
            //				txtNeedServerHelp.Text="1";
            //			}
            //			else
            //			{
            //				txtNeedServerHelp.Text="0";
            //			}
            return strLast + txtNeedServerHelp.Text;
        }
        /// <summary>
        /// 判断是否需要服务器帮忙
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        private bool AdjustIsNeedHelp(string Code)
        {
            if (Code == "00" || Code == "01")
            {
                return false;
            }
            if (Code == "10" || Code == "11")
            {
                return true;
            }
            return false;
        }
        private void TextToList()
        {
            if (AdjustIsNeedHelp(isNeedServerHelp()))
            {
                return;
            }
            if (this.txtClientChanged.Text.Trim() != "1")
            {
                return;
            }
            this.txtClientChanged.Text = "";
            string strPatten = @"(\<OPTION.*?\>)(.*?)(\</OPTION\>)";
            string strPattenValue = @"(value=)([""|']?)(.*?)([""|'|\s|\>])";
            string strPattenSelect = @"selected";
            MatchCollection mc = Regex.Matches(txtLstLeft.Text, strPatten, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Match _value;
            LstLeft.Items.Clear();
            foreach (Match _current in mc)
            {
                _value = Regex.Match(_current.Groups[1].Value, strPattenValue, RegexOptions.IgnoreCase);
                ListItem listitem = new ListItem(_current.Groups[2].Value, _value.Groups[3].Value);
                if (Regex.IsMatch(_current.Groups[1].Value, strPattenSelect, RegexOptions.IgnoreCase))
                {
                    listitem.Selected = true;
                }
                LstLeft.Items.Add(listitem);
            }
            mc = Regex.Matches(txtLstRight.Text, strPatten, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            LstRight.Items.Clear();
            foreach (Match _current in mc)
            {
                _value = Regex.Match(_current.Groups[1].Value, strPattenValue, RegexOptions.IgnoreCase);
                ListItem listitem = new ListItem(_current.Groups[2].Value, _value.Groups[3].Value);
                if (Regex.IsMatch(_current.Groups[1].Value, strPattenSelect, RegexOptions.IgnoreCase))
                {
                    listitem.Selected = true;
                }
                LstRight.Items.Add(listitem);
            }

        }
        /// <summary>
        /// 同步客户端TXT文本，每次加载和清除项后必须调用此方法
        /// </summary>
        public void SynchronizationText()
        {
            //return ;
            if (LstLeft.Items.Count > MaxClientCount || LstRight.Items.Count > MaxClientCount)
            {
                txtNeedServerHelp.Text = "1";
                return;
            }
            else
            {
                txtNeedServerHelp.Text = "0";
            }
            //this.txtClientChanged.Text="1";
            //ListToText();

        }

        /// <summary>
        /// list 数据同步到 Text
        /// </summary>
        private void ListToText()
        {
            return;

        }
        #region Web 窗体设计器生成的代码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
            //
            InitializeComponent();
            this.cmdSelone.Click += new System.EventHandler(this.cmdSelone_Click);
            this.cmdSelAll.Click += new System.EventHandler(this.cmdSelAll_Click);
            this.cmdDelOne.Click += new System.EventHandler(this.cmdDelOne_Click);
            this.cmdDelAll.Click += new System.EventHandler(this.cmdDelAll_Click);
            this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            base.OnInit(e);
            this.lblLeft.Text = base.getString("ZGAIA03505");
            lblRight.Text = base.getString("ZGAIA03530");
        }
        /// <summary>
        ///		设计器支持所需的方法 - 不要使用代码编辑器
        ///		修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
        private void cmdSelone_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            fLength = LstLeft.Items.Count;
            for (i = 0; i < fLength; i++)
            {
                if (LstLeft.Items[i].Selected)
                {
                    LstLeft.Items[i].Selected = false;
                    if (AllowRepeat)
                    {
                        ListItem Item = new ListItem();
                        Item.Value = LstLeft.Items[i].Value;
                        Item.Text = LstLeft.Items[i].Text;
                        LstRight.Items.Add(Item);
                    }
                    else
                    {
                        if (!LstRight.Items.Contains(LstLeft.Items[i]))
                        {
                            LstRight.Items.Add(LstLeft.Items[i]);
                        }
                        LstLeft.Items.Remove(LstLeft.Items[i]);
                        i--;
                        fLength--;
                    }
                }
            }
            ListToText();
        }
        private void cmdSelAll_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            fLength = LstLeft.Items.Count;
            for (i = 0; i < fLength; i++)
            {
                if (AllowRepeat)
                {
                    ListItem Item = new ListItem();
                    Item.Value = LstLeft.Items[i].Value;
                    Item.Text = LstLeft.Items[i].Text;
                    LstRight.Items.Add(Item);
                }
                else
                {
                    if (!LstRight.Items.Contains(LstLeft.Items[i]))
                    {
                        LstRight.Items.Add(LstLeft.Items[i]);
                    }
                    LstLeft.Items.Remove(LstLeft.Items[i]);
                    i--;
                    fLength--;
                }
            }
            ListToText();
        }
        private void cmdDelOne_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            fLength = LstRight.Items.Count;
            for (i = 0; i < fLength; i++)
            {
                if (LstRight.Items[i].Selected)
                {
                    if (!LstLeft.Items.Contains(LstRight.Items[i]))
                    {
                        LstLeft.Items.Add(LstRight.Items[i]);
                    }
                    LstRight.Items.Remove(LstRight.Items[i]);
                    i--;
                    fLength--;

                }
            }
            ListToText();
        }
        private void cmdDelAll_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            fLength = LstRight.Items.Count;
            for (i = 0; i < fLength; i++)
            {
                if (!LstLeft.Items.Contains(LstRight.Items[i]))
                {
                    LstLeft.Items.Add(LstRight.Items[i]);
                }
                LstRight.Items.Remove(LstRight.Items[i]);
                i--;
                fLength--;
            }
            ListToText();
        }
        private void cmdUp_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            string ItemCode;
            string ItemText;
            fLength = LstRight.Items.Count;
            for (i = 1; i < fLength; i++)
            {
                if (LstRight.Items[i].Selected)
                {
                    LstRight.Items[i].Selected = false;
                    ItemCode = LstRight.Items[i - 1].Value;
                    ItemText = LstRight.Items[i - 1].Text;

                    LstRight.Items[i - 1].Value = LstRight.Items[i].Value;
                    LstRight.Items[i - 1].Text = LstRight.Items[i].Text;

                    LstRight.Items[i].Value = ItemCode;
                    LstRight.Items[i].Text = ItemText;

                    LstRight.Items[i - 1].Selected = true;

                }
            }
            ListToText();

        }
        private void cmdDown_Click(object sender, System.EventArgs e)
        {
            int i, fLength;
            string ItemCode;
            string ItemText;
            fLength = LstRight.Items.Count;
            for (i = fLength - 2; i > -1; i--)
            {
                if (LstRight.Items[i].Selected)
                {
                    LstRight.Items[i].Selected = false;
                    ItemCode = LstRight.Items[i + 1].Value;
                    ItemText = LstRight.Items[i + 1].Text;

                    LstRight.Items[i + 1].Value = LstRight.Items[i].Value;
                    LstRight.Items[i + 1].Text = LstRight.Items[i].Text;

                    LstRight.Items[i].Value = ItemCode;
                    LstRight.Items[i].Text = ItemText;

                    LstRight.Items[i + 1].Selected = true;

                }
            }
            ListToText();
        }
    }
}


