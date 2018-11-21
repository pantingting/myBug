$(function () {
    var loginName = localStorage.getItem("loginName");
    if (loginName == null) {
        dialog.alert("提示", "用户可能登录过期，请重新登录");
        location.href="login.html";
    }
    var data = { loginName: loginName};
    $.ajax({
        url: apiUrl + "/giz/getStatus",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            if (res.success == true) {
                $("#leftFired").html("左:" + res.data.FIRE_L_DOING);
                $("#rightFired").html("右:" + res.data.FIRE_R_DOING);
                $("#fan").html("风机:" + res.data.SPEED_T);
                $("#leftFiredNum").html(res.data.FIRE_L_DOING+"次");
                $("#rightFiredNum").html(res.data.FIRE_R_DOING + "次");
                $("#leftShotUsed").html(res.data.GYZ_L + "小时");
                $("#rightShotUsed").html(res.data.GYZ_R + "小时");
                $("#fanUsed").html(res.data.SPEED_T + "小时");
            }
            else {
                dialog.alert("提示", res.message);
            }
        }
    })
    
});



