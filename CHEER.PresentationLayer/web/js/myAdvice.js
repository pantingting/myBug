function Advice() {
    var userInfo = JSON.parse(localStorage.CH_UserInfo);
    if (userInfo == null) {
        location.href = "login.html";
        return;
    }

    var text = $(".advice").val();

    if (text.trim() == '') return dialog.alert('提示', '请输入您的反馈意见!');

    var data = { openId: userInfo.openId, content: text };
    $.ajax({
        url: apiUrl + "/userFeedBack",
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                dialog.alert("提示", "反馈成功", () => {
                    location.href = "my.html";
                });
            }
            else {
                dialog.alert(res.message);
            }
        }
    });
}
//function getNowFormatDate() {
//    var date = new Date();
//    var seperator1 = "-";
//    var seperator2 = ":";
//    var month = date.getMonth() + 1;
//    var strDate = date.getDate();
//    if (month >= 1 && month <= 9) {
//        month = "0" + month;
//    }
//    if (strDate >= 0 && strDate <= 9) {
//        strDate = "0" + strDate;
//    }
//    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
//        + " " + date.getHours() + seperator2 + date.getMinutes()
//        + seperator2 + date.getSeconds();
//    return currentdate;
//}