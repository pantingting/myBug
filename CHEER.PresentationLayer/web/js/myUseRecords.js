var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
$(function () {
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    var data = { openId: userInfo.openId, mainOpenId: localStorage.getItem("mainOpenId") }

    $.ajax({
        url: apiUrl + "/userOnOff",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                var html = "<div style='text-align: center;padding: 15px;'>暂无使用记录</div>";
                if (res.data.useRecord.length > 0) {
                    html = '';
                    res.data.useRecord.forEach((item) => {
                        html += '<a class="weui-media-box weui-media-box_appmsg">';
                        html += '<div class="weui-media-box__hd">';
                        html += '<img src="' + item.headImage + '" alt="">';
                        html += '</div>';
                        html += '<div class="weui-media-box__bd">';
                        html += '<p class="weui-media-box__title">' + item.nickName + '</p>';
                        html += '</div>';
                        html += '<div class="weui-media-box__bd bd-record">';
                        html += '<h4 class="weui-media-box__title">' + item.description + '</h4>';
                        html += '<p class="weui-media-box__desc">' + (item.datetime == null ? '' : item.datetime) + '</p>';
                        html += '</div>';
                        html += '</a>';
                    });
                }
                $("#onoff").html(html);
            }
            else {
                dialog.alert("提示", res.message);
            }
        }
    });
});