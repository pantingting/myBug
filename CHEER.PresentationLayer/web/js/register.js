/*获取验证码*/
var isPhone = 1;
function getCode(e) {
    checkPhone();
    if (isPhone) {
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
                     resetCode(); //倒计时
                     //console.log(res.data);
                 } else {
                     dialog.alert('提示', res.message);
                 }
             }
         })
    } else {
        $('#mobileNo').focus();
    }
}

//验证手机号码
function checkPhone() {
    var phone = $('#mobileNo').val();
    var pattern = /^1[3456789]\d{9}$/;
    isPhone = 1;
    if (phone.trim() == '') {
        dialog.alert('提示', '请输入手机号码！');
        isPhone = 0;
        return;
    }
    if (!pattern.test(phone)) {
        dialog.alert('提示', '手机格式不正确！');
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

// 注册
function register() {
    var phoneRegex = /^1[3456789]\d{9}$/;
    var passwordRegex = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$/;
    var mobileNo = $('#mobileNo').val();
    var password = $('#pssword').val();
    var code = $('#verificationCode').val();
    var againPwd = $('#againPwd').val();
//console.log(code);
    if (mobileNo == "" || password.trim() == "" || code.trim() == "" || againPwd.trim() == "") {
        dialog.alert('提示', '请将信息补充完整！');
        return;
    }

    if (phoneRegex.test(mobileNo) == false) {
        dialog.alert('提示', '手机号码格式不正确！');
        return;
    }

    if (password != againPwd) {
        dialog.alert('提示', '两次输入的密码不一致！');
        return; 
    }
    var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
    var openId = userInfo.openId;
    var headImage = userInfo.headImage;
    var nickName = userInfo.nickName;
    var address = userInfo.address;
    //console.log(userInfo);
    var data = {
        code: code,
        loginName: mobileNo,
        nickName: nickName,
        password: password,
        address: address,
        headImage: headImage,
        openId: openId,
    };
    //if (passwordRegex.test(password) == false || passwordRegex.test(againPwd) == false) {
    //    dialog.alert('提示', '密码必须是6-16位字母数字组合！');
    //    return;
    //}
   
    loading.show();
    $.ajax({
        url: apiUrl + "/userRegist",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            loading.hide();
            if (res.success) {
                dialog.alert('提示', '注册成功！', function () {
                    location.href = "login.html?mobileNo=" + $('#mobileNo').val();
                });
            } else {
                dialog.alert('提示', res.message);
            }
        }
    })
}