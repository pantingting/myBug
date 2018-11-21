<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="LRSelect.ascx.cs" Inherits="CHEER.PresentationLayer.Controls.LRSelect"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<link href="<%=BaseUrl%>Css/EHR.css" type="text/css" rel="stylesheet">
<table height="100%" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td valign="top" width="200">
            <table height="100%" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr height="1">
                    <td>
                        <asp:Label ID="lblLeft" CssClass="Label" runat="server">待选员工</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="middle">
                        <asp:ListBox ID="LstLeft" Rows="9" SelectionMode="Multiple" runat="server" Width="200"
                            Height="100%"></asp:ListBox>
                    </td>
                </tr>
            </table>
        </td>
        <td width="30">
        </td>
        <td valign="middle" width="41">
            <table cellspacing="0" cellpadding="0" align="center" border="0">
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdSelone" CssClass="Button" runat="server" Width="41px"></asp:Button>
                    </td>
                </tr>
                <tr height="15">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdSelAll" CssClass="Button" runat="server" Width="41px"></asp:Button>
                    </td>
                </tr>
                <tr height="15">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdDelOne" CssClass="Button" runat="server" Width="41px"></asp:Button>
                    </td>
                </tr>
                <tr height="15">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdDelAll" CssClass="Button" runat="server" Width="41px"></asp:Button>
                    </td>
                </tr>
            </table>
        </td>
        <td width="30">
        </td>
        <td valign="top" width="200">
            <table height="100%" cellspacing="0" cellpadding="0" width="100%" border="0">
                <tr height="1">
                    <td>
                        <asp:Label ID="lblRight" CssClass="Label" runat="server">已选员工</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="middle">
                        <asp:ListBox ID="LstRight" Rows="9" SelectionMode="Multiple" runat="server" Width="200"
                            Height="100%"></asp:ListBox>
                    </td>
                </tr>
            </table>
        </td>
        <td width="20">
        </td>
        <td width="21" valign="middle">
            <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdUp" CssClass="Button" runat="server" Width="22px" Height="22px"
                            Visible="False"></asp:Button>
                    </td>
                </tr>
                <tr height="15">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="center">
                        <asp:Button ID="cmdDown" CssClass="Button" runat="server" Width="22px" Height="22px"
                            Visible="False"></asp:Button>
                    </td>
                </tr>
            </table>
        </td>
        <td>
            <asp:TextBox ID="txtLstLeft" runat="server" Width="0px"></asp:TextBox><asp:TextBox
                ID="txtLstRight" runat="server" Width="0px"></asp:TextBox><asp:TextBox ID="txtAllowRepeat"
                    runat="server" Width="0px"></asp:TextBox><asp:TextBox ID="txtLeftListWidth" runat="server"
                        Width="0px">49</asp:TextBox><asp:TextBox ID="txtMaxClientCount" runat="server" Width="0px">1000</asp:TextBox><asp:TextBox
                            ID="txtNeedServerHelp" runat="server" Width="0px">0</asp:TextBox><asp:TextBox ID="txtClientChanged"
                                runat="server" Width="0px"></asp:TextBox>
        </td>
    </tr>
</table>

<script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>

<script type="text/javascript">
    window.onload=function()
    {
        if(!document.all)
        {
            var $E = function() { var c = $E.caller; while (c.caller) c = c.caller; return c.arguments[0] };
		    __defineGetter__("event", $E);
        }
    }
    var MaxClientCount=<%=MaxClientCount%>;
    SynchronizationOnClient("<%=ClientID%>");

	function SynchronizationOnClient(clientID)
	{
		var _lstLeft=document.getElementById(clientID+"_LstLeft");
		var _lstRight=document.getElementById(clientID+"_LstRight");
		var _txtLstRight=document.getElementById(clientID+"_LstRight");
		var _txtLstLeft=document.getElementById(clientID+"_txtLstLeft");
		
		_txtLstLeft.value=_lstLeft.innerHTML;
		_txtLstRight.value=_lstRight.innerHTML;
		
	}

		function AllowRepeat(AllowRepeat)
		{
			return AllowRepeat.value=="TRUE";
		}

		function IsNeedServerHelp(need)
		{
			return need.value=="1";
		}

		
		///list 是否包含选项
		function ContainValue(lst,value)
		{
			if(lst==null || value==null)
			{
				return false;
			}
	
			if(lst[value]==undefined)
			{
				return false;
			}
			else
			{
				return true;
			}
			
		}
		
		function GetIds(List)
		{
			var _arr=new Array();
			
			if(List==null)
			{
				return _arr;
			}
			for(var i=0;i<List.options.length;i++)
			{
				_arr[List.options[i].value]="";
			}
			
			return _arr;
			
			
		}
		
		function SelectItem(lstF,lstT,textWait,textHad,AllowID,needServerHelp,ClientChanged )
		{
			var i,fLength,tLength;
			var ItemCode;
			var ItemText;
			if(lstF==null || lstT==null)
			{
				return false;
			}
			
			fLength=lstF.options.length;
			tLength=lstT.options.length;
			if(IsNeedServerHelp(needServerHelp))
			{
			   return true;
			}
			if(document.all)
			{
			    var strLstF=lstF.outerHTML.match("\<SELECT.*?\>");
			    var strLstT=lstT.outerHTML.match("\<SELECT.*?\>");
			}
			else
			{
			    var strLstF=lstF.outerHTML.match("\<select.*?\>");
			    var strLstT=lstT.outerHTML.match("\<select.*?\>");
			}
			
			var arr=GetIds(lstT);

			var arrT=new Array();
			var arrF=new Array();
			var iSC=0;
			var max=0;
			for(i=0;i<fLength;i++)
			{	
				ItemCode=lstF.options[i].value;
				ItemText=lstF.options[i].text;
				if( AllowRepeat(AllowID))
				{
					if (lstF.options[i].selected)
					{
					    max=i;
						arrT[arrT.length]="<option value="+ItemCode+">"+ItemText+"</option>";
						iSC++;
					}
				}
				else
				{
					if (lstF.options[i].selected)
					{
					    max=i;
						if(!ContainValue(arr,ItemCode))
						{
							arrT[arrT.length]="<option value="+ItemCode+">"+ItemText+"</option>";
						}
						iSC++;
					}
					else
					{
						arrF[arrF.length]="<option value="+ItemCode+">"+ItemText+"</option>";
					}
				}
			}
			//alert(lstT.innerHTML);
			//lstF.options[15].selected=true;
			if(iSC>0)
			{
				ClientChanged.value="1";
				textHad.value=lstT.innerHTML+"\\r\\n"+arrT.join("\\r\\n");
				lstT.outerHTML=strLstT+textHad.value+"</SELECT>";
				if(!AllowRepeat(AllowID))
				{
//					textWait.value=arrF.join("\\r\\n");
//					lstF.outerHTML=strLstF+textWait.value+"</SELECT>";
                    if(lstF.selectedIndex==-1)
                    {
                        return false;
                    }
                    try
                    {
                        for(var i=0;i<lstF.options.length;i++)
                        {
                            if(lstF.options[i].selected)
                            {
	                            lstF.options.remove(i);
	                            i--;
                            }
                        }
        		        
                    }
                    catch(e)
                    {
                    }
	            }
			}
	        return false;
		}
		
		function DeleteItem(lstT,lstF,textHad,textWait,AllowID,needServerHelp,ClientChanged)
		{
			var i,fLength,tLength;
			var ItemCode;
			var ItemText;
			
			if(lstF==null || lstT==null)
			{
				
				return ;
			}
			
			fLength=lstF.options.length;
			tLength=lstT.options.length;
			if(IsNeedServerHelp(needServerHelp))
			{
				
				return true;
			}
		
			if(document.all)
			{
			    var strLstF=lstF.outerHTML.match("\<SELECT.*?\>");
			    var strLstT=lstT.outerHTML.match("\<SELECT.*?\>");
			}
			else
			{
			    var strLstF=lstF.outerHTML.match("\<select.*?\>");
			    var strLstT=lstT.outerHTML.match("\<select.*?\>");
			}
			
			var arr=GetIds(lstT);
			
			var arrT=new Array();
			var arrF=new Array();
			var iSC=0;
			
			for(i=0;i<fLength;i++)
			{	
				ItemCode=lstF.options[i].value;
				ItemText=lstF.options[i].text;
				if (lstF.options[i].selected)
				{
					if(!ContainValue(arr,ItemCode) )
					{
						arrT[arrT.length]="<option value="+ItemCode+">"+ItemText+"</option>";
					}
					iSC++;	
				}
				else
				{
					arrF[arrF.length]="<option value="+ItemCode+">"+ItemText+"</option>";
				}
				
			}
			if(iSC>0)
			{
				
				ClientChanged.value="1";
				textHad.value=lstT.innerHTML+"\\r\\n"+arrT.join("\\r\\n");
				lstT.outerHTML=strLstT+textHad.value+"</SELECT>";
				textWait.value=arrF.join("\\r\\n");
				//lstF.outerHTML=strLstF+textWait.value+"</SELECT>";
				if(lstF.selectedIndex==-1)
                {
	                return false;
                }
                try
                {
	                for(var i=0;i<lstF.options.length;i++)
	                {
		                if(lstF.options[i].selected)
		                {
			                lstF.options.remove(i);
			                i--;
		                }
	                }
                }
                catch(e)
                {
                }
			}
			return false;
		}
		
		function SelectAllItem(lstF,lstT,textWait,textHad,AllowID,needServerHelp,ClientChanged)
		{
		
			var i,fLength,tLength;
			var ItemCode;
			var ItemText;
			
			if(lstF==null || lstT==null)
			{
				return ;
			}
			
//			try
//	        {
//		        while(lstF.options.length>0)
//		        {
//			        var oOption = document.createElement("OPTION");
//			        lstT.options.add(oOption);
//			        oOption.innerText=lstF.options[0].innerText;
//			        oOption.value=lstF.options[0].value;
//			        oOption.selected=lstF.options[0].selected;
//			        lstF.options.remove(0);
//		        }
//	        }
//	        catch(e)
//	        {
//	        }
//	        return false;
			
			fLength=lstF.options.length;
			tLength=lstT.options.length;
			
			if(fLength==0)
			{
				return false;
			}
			
			if(IsNeedServerHelp(needServerHelp))
			{
			   return true;
			}
			if(document.all)
			{
			    var strLstF=lstF.outerHTML.match("\<SELECT.*?\>");
			    var strLstT=lstT.outerHTML.match("\<SELECT.*?\>");
			}
			else
			{
			    var strLstF=lstF.outerHTML.match("\<select.*?\>");
			    var strLstT=lstT.outerHTML.match("\<select.*?\>");
			}
			
			if(AllowRepeat(AllowID))
			{
				ClientChanged.value="1";
				textHad.value=lstT.innerHTML+"\\r\\n"+lstF.innerHTML;
				lstT.outerHTML=strLstT+lstT.innerHTML+"\\r\\n"+lstF.innerHTML+"</SELECT>";
			}
			else
			{
				var arr=GetIds(lstT);
				var arrT=new Array();
				for(i=0;i<fLength;i++)
				{	
					ItemCode=lstF.options[i].value;
					ItemText=lstF.options[i].text;
					if(!ContainValue(arr,ItemCode))
					{
						arrT[arrT.length]="<option value="+ItemCode+">"+ItemText+"</option>";
					}
				}
				ClientChanged.value="1";
				textHad.value=lstT.innerHTML+"\\r\\n"+arrT.join("\\r\\n");
				lstT.outerHTML=strLstT+textHad.value+"</SELECT>";
				textWait.value="";
				lstF.outerHTML=strLstF+""+"</SELECT>";
				
			}
			
			return false;		
		}
		
		function DeleteAllItem(lstT,lstF,textHad,textWait,AllowID,needServerHelp,ClientChanged)
		{
			var i,fLength,tLength;
			var ItemCode;
			var ItemText;
			
			if(lstF==null || lstT==null)
			{
				return false;
			}
			
//			try
//	        {
//		        while(lstF.options.length>0)
//		        {
//			        var oOption = document.createElement("OPTION");
//			        lstT.options.add(oOption);
//			        oOption.innerText=lstF.options[0].innerText;
//			        oOption.value=lstF.options[0].value;
//			        oOption.selected=lstF.options[0].selected;
//			        lstF.options.remove(0);
//		        }
//	        }
//	        catch(e)
//	        {
//	        }
//	        return false;
			
			fLength=lstF.options.length;
			tLength=lstT.options.length;
			
			if(fLength==0)
			{
				return false;
			}
			
			if(IsNeedServerHelp(needServerHelp))
			{
			   return true;
			}
			
			if(document.all)
			{
			    var strLstF=lstF.outerHTML.match("\<SELECT.*?\>");
			    var strLstT=lstT.outerHTML.match("\<SELECT.*?\>");
			}
			else
			{
			    var strLstF=lstF.outerHTML.match("\<select.*?\>");
			    var strLstT=lstT.outerHTML.match("\<select.*?\>");
			}
			
			var arr=GetIds(lstT);
			var arrT=new Array();
			for(i=0;i<fLength;i++)
			{	
				ItemCode=lstF.options[i].value;
				ItemText=lstF.options[i].text;
				if(!ContainValue(arr,ItemCode) )
				{
					arrT[arrT.length]="<option value="+ItemCode+">"+ItemText+"</option>";
				}	
				
			}
			ClientChanged.value="1";
			textHad.value=lstT.innerHTML+"\\r\\n"+arrT.join("\\r\\n");
			lstT.outerHTML=strLstT+textHad.value+"</SELECT>";
			textWait.value="";
			lstF.outerHTML=strLstF+"</SELECT>";

			return false;	
		}
		
		function MoveUp(lstF,text,needServerHelp,ClientChanged)
		{
			var i,fLength;
			var ItemCode;
			var ItemText;

			if(lstF==null )
			{
				return false;
			}

			fLength=lstF.options.length;
			if(IsNeedServerHelp(needServerHelp))
			{
			   return true;
			}
			var iSC=0;
			
			for(i=1;i<fLength;i++)
			{	
				if (lstF.options[i].selected)
				{
					lstF.options[i].selected=false;
					ItemCode=lstF.options[i-1].value;
					ItemText=lstF.options[i-1].text;
					
					lstF.options[i-1].value=lstF.options[i].value;
					lstF.options[i-1].text=lstF.options[i].text;
					
					lstF.options[i].value=ItemCode;
					lstF.options[i].text=ItemText;	
					
					lstF.options[i-1].selected=true;	
					
					iSC++;
					
				}
			}
			
			if(iSC>0)
			{
				ClientChanged.value="1";
				text.value=lstF.innerHTML;
			}
			
			return false;	
		}
		
		function MoveDown(lstF,text,needServerHelp,ClientChanged)
		{
			var i,j,fLength;
			var ItemCode;
			var ItemText;

			if(lstF==null )
			{
				return false;
			}

			fLength=lstF.options.length;
			if(IsNeedServerHelp(needServerHelp))
			{
			   return true;
			}
			var iSC=0;
			
			for(i=fLength-2;i>-1;i--)
			{	
				if (lstF.options[i].selected)
				{
					lstF.options[i].selected=false;
					ItemCode=lstF.options[i+1].value;
					ItemText=lstF.options[i+1].text;
					
					lstF.options[i+1].value=lstF.options[i].value;
					lstF.options[i+1].text=lstF.options[i].text;
					
					lstF.options[i].value=ItemCode;
					lstF.options[i].text=ItemText;	
					
					lstF.options[i+1].selected=true;	
					iSC++;
				}
			}
			if(iSC>0)
			{
				ClientChanged.value="1";
				text.value=lstF.innerHTML.replace();
			}
			return false;	
		}
		

</script>

