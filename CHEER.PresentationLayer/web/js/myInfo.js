$(function () {
    var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));

    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }
    var headImage = $(".headImage");
    var nickName = $(".nickName");
    var loginName = $(".loginName");
    var address = $(".address");
    var data = { openId: userInfo.openId }
    $.ajax({
        url: apiUrl + "/userInfo",
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                headImage.attr("src", (res.data[0].headImage == '' || res.data[0].headImage == null) ? '../img/021.png' : res.data[0].headImage);
                nickName.val(res.data[0].nickName);
                loginName.val(res.data[0].loginName);
                address.val(res.data[0].address);
            }
            else {
                dialog.alert(res.message);
            }
        }
    });
});

function singOut() {
    dialog.confirm("提示", "确认退出登录", () => {
        localStorage.removeItem("CH_UserInfo");
        localStorage.removeItem("mainOpenId");
        localStorage.removeItem("loginName");
        location.href = "login.html";
    });
}