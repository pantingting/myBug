$(function () {
    var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    if (localStorage.getItem("mainOpenId") == userInfo.openId) {
        var html = '<li class="nav-header">';
        html += '<a onclick="stateGoShare()">';
        html += '<img src="../img/024.png" alt="">';
        html += '<span>用户分享</span>';
        html += '</a>';
        html += '</li>';

        $('.other ul').prepend(html);
    }

    var data = { openId: userInfo.openId, mainOpenId: localStorage.getItem("mainOpenId") };
    $.ajax({
        url: apiUrl + "/userMyDevice",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                $(".portrait img").attr("src", (res.data.userInfo[0].headImage == '' || res.data.userInfo[0].headImage == null) ? '../img/021.png' : res.data.userInfo[0].headImage);
                $(".portrait span").text(res.data.userInfo[0].nickName);

                if (res.data.deviceInfo.length < 1) {
                    $('.device-info').text('暂无绑定设备信息');
                    if (localStorage.getItem("mainOpenId") != userInfo.openId)
                        location.href = "login.html?timestamp=" + (new Date()).getTime();
                    else
                        location.href = "bindDevice.html?timestamp=" + (new Date()).getTime();
                } else {
                    $(".deviceName ").text(res.data.deviceInfo[0].name);
                    $(".deviceMac span").text(res.data.deviceInfo[0].mac);
                }

                if (localStorage.getItem("mainOpenId") == userInfo.openId) $(".deviceMac").append('<a onclick="stateGoDevice()">分享</a><a onclick="stateGoDevice()">删除</a>');
            }
            else {
                if (res.errCode == "login") {
                    dialog.alert("提示", res.message, function () {
                        location.href = "login.html?timestamp=" + (new Date()).getTime();
                    });
                } else {
                    dialog.alert("提示", res.message);
                }
            }
        }
    });
});

function stateGoShare() {
    location.href = 'myShare.html?timestamp=' + (new Date()).getTime();
}

function stateGoDevice() {
    location.href = 'myDevice.html?timestamp=' + (new Date()).getTime();
}