<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressBox.ascx.cs" Inherits="CHEER.PresentationLayer.Controls.ProgressBox" %>
<script>
    var timeoutId;
    var url = '<%=this.Request.Url.ToString()%>';
    if (url.indexOf('?') != -1) {
        url += '&GETPROGRESSVALUE=YES';
    }
    else {
        url += '?GETPROGRESSVALUE=YES';
    }
    function goRequestValueMax() {
        Ext.Ajax.request({
            url: url,
            method: 'POST',
            success: function (response) {
                var obj = Ext.decode(response.responseText);
                var val = parseFloat(obj.value), max = parseFloat(obj.max), alertScript = obj.alert;
                if (val == 0) {
                    Ext.MessageBox.progress("请等待", "正在处理数据...");
                    timeoutId = setTimeout(goRequestValueMax, 50);
                }
                else {
                    if (val < max) {
                        Ext.MessageBox.updateProgress(val / max, '进度' + (val / max * 100).toFixed(2) + '%,' + val + '/' + max);
                        timeoutId = setTimeout(goRequestValueMax, 50);
                    }
                    else {
                        Ext.MessageBox.updateProgress(0 / max, '进度' + (0 / max * 100).toFixed(2) + '%,' + 0 + '/' + max);
                        Ext.MessageBox.hide();
                        clearTimeout(timeoutId);
                        if (alertScript !== '') {
                            top.F.alert(alertScript);
                        }
                        eval("<%=postBackFunction%>");
                    }
                }
            },
            failure: function (response) {

            }
        });
    }
</script>