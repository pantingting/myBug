//如果login页为用户分享页，会附带deviceId
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
var deviceId = null;
if (location.search) {
    deviceId = GetQueryString("deviceId");
    if (GetQueryString("mobileNo") != null) $('#mobileNo').val(GetQueryString("mobileNo"));
}
var code = params.getUrlParam('code');

var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));

$(function () {
    //获取微信信息
    if (code) {
        $.ajax({
            headers: headers,
            url: apiUrl + '/user/getWXUserInfo',
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify({
                code: code
            }),
            success: function (res) {
                if (res.success) {
                    localStorage.CH_UserInfo = JSON.stringify(res.data);
                    //openid = res.data.openid;
                    userInfo = res.data;
                    if (deviceId != null) {
                        insertChildUser(res.data.openId);
                    }
                }
                else {
                    console.log(res);
                }
            }
        })
    } else {
        if (!localStorage.CH_UserInfo) {
            $.ajax({
                headers: headers,
                url: apiUrl + '/user/getWXUserInfoRedirect',
                type: 'POST',
                contentType: "application/json",
                data: JSON.stringify({
                    url: encodeURIComponent(window.location.href)
                }),
                success: function (res) {
                    if (res.success) {
                        console.log(res.redirect_uri);
                        window.location.href = res.redirect_uri;
                    }
                    else {
                        console.log(res);
                    }
                }
            })
        } else {
            if (deviceId != null) {
                insertChildUser(userInfo.openId);
            }
        }
    }
})

function insertChildUser(openid) {
    $.ajax({
        url: apiUrl + '/insertSubUser',
        type: 'POST',
        contentType: "application/json",
        data: JSON.stringify({
            deviceId: deviceId,
            openId: openid
        }),
        success: function (res) {
            if (res.success == false) {
                dialog.alert("提示", res.message);
            }
        }
    });
}

//绑定分享设备
//用户首先获取微信信息包含openid
//var data = { Id: deviceId, openId: openid };
//if (deviceId != null) {
//    $.ajax({
//        url: apiUrl + "/bindShareDevice",
//        data: JSON.stringify(data),
//        type: "POST",
//        contentType: "application/json",
//        success: function (res) {
//            if (res.success) {
//                dialog.alert("提示", "绑定分享设备成功");
//            }
//            else {
//                dialog.alert("提示", "绑定分享设备失败" + res.message);
//            }
//        }
//    });
//}





//用户登录
function userLogin() {
    if ($('#mobileNo').val().trim() == '') {
        dialog.alert('提示', '请输入手机号！');
        console.log("mobile" + $('#mobileNo').val());
        return;
    }

    if ($('#password').val().trim() == '') {
        dialog.alert('提示', '请输入密码！');
        return;
    }

    var mobileNo = $('#mobileNo').val();
    var password = $('#password').val();
    var data = {
        loginName: mobileNo,
        password: password,
        openId: userInfo.openId
    };

    loading.show();

    $.ajax({
        url: apiUrl + "/userLogin",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            loading.hide();
            if (res.success) {
                localStorage.setItem("loginName", mobileNo);
                localStorage.setItem("mainOpenId", res.openId);
                if (res.Level == 0) {
                    if (res.isDevice == false)
                        location.href = "bindDevice.html?timestamp=" + (new Date()).getTime();
                    else
                        location.href = "index.html?timestamp=" + (new Date()).getTime();
                } else {
                    if (res.isDevice == false)
                        dialog.alert("提示", "主账号未绑定设备");
                    else 
                        location.href = "index.html?timestamp=" + (new Date()).getTime();
                }
            }
            else {
                if (res.errCode == "login") {
                    dialog.alert("提示", res.message, function () {
                        location.href = "login.html";
                    });
                } else {
                    dialog.alert("提示", res.message);
                }
            }
        },
        error: function () {
            loading.hide();
        }
    });
}