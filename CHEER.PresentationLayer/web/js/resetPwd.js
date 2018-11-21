var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));

$(function () {
    if (userInfo == null) {
        var code = params.getUrlParam('code');
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
                        userInfo = res.data;
                    }
                    else {
                        console.log(res);
                    }
                }
            });
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
                            window.location.href = res.redirect_uri;
                        }
                        else {
                            console.log(res);
                        }
                    }
                });
            }
        }
    }
});


/*获取验证码*/
var isPhone = 1;
var isClick = false;
function getCode(e) {
    if (isClick) {
        return;
    }
    checkPhone(); //验证手机号码
    if (isPhone) {
        isClick = true;
        loading.show();
        var no = $('#mobileNo').val()
        var data = { phone: no }
        $.ajax({
            url: apiUrl + "/sendSmsCode",
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (res) {
                loading.hide();
                if (res.success) {
                    resetCode();
                } else {
                    isClick = false;
                    dialog.alert('提示', res.message);
                }
            }
        });
    } else {
        $('#mobileNo').focus();
    }
}

//验证手机号码
function checkPhone() {
    var phone = $('#mobileNo').val();
    var pattern = /^1[3456789]\d{9}$/;
    isPhone = 1;
    if (phone == '') {
        dialog.tip('请输入手机号码！');
        isPhone = 0;
        return;
    }
    if (!pattern.test(phone)) {
        dialog.tip('手机格式不正确！');
        isPhone = 0;
        return;
    }
}

//倒计时
function resetCode() {
    $('#get-code-btn').css('background-color', '#8c8c8c');
    $('#get-code-btn').html('59s后重发');
    $('#get-code-btn').removeClass('btn-hover');
    var second = 59;
    var timer = null;
    timer = setInterval(function () {
        second -= 1;
        if (second > 0) {
            $('#get-code-btn').html(second + "s后重发");
        } else {
            $('#get-code-btn').css('color', '#fff');
            $('#get-code-btn').html('获取验证码');
            $('#get-code-btn').css('background-color', '#f6ad3e');
            isClick = false;
            clearInterval(timer);
        }
    }, 1000);
}

//修改密码
function modifyPwd() {
    var phoneRegex = /^1[3456789]\d{9}$/;
    // var passwordRegex = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$/;

    var mobileNo = $('#mobileNo').val();
    var password = $('#pssword').val().trim();
    var code = $('#verificationCode').val();
    var againPwd = $('#againPwd').val().trim();

    if (mobileNo == "" || password == "" || code == "" || againPwd == "") {
        dialog.tip('请将信息补充完整！');
        return;
    }

    if (phoneRegex.test(mobileNo) == false) {
        dialog.tip('手机号码格式不正确！');
        return;
    }

    if (password != againPwd) {
        dialog.tip('两次输入的密码不一致！');
        return;
    }

    loading.show();
    $.ajax({
        url: apiUrl + "/ResetPassword",
        type: "POST",
        data: JSON.stringify({
            loginName: mobileNo,
            password: password,
            code: code,
            openId: userInfo.openId
        }),
        contentType: "application/json",
        success: function (res) {
            loading.hide();
            if (res.success) {
                dialog.alert('提示', '重置密码成功！', function () {
                    location.href = "login.html?mobileNo=" + $('#mobileNo').val();
                });
            } else {
                dialog.alert('提示', res.message);
            }
        }
    });
}