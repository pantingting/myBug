var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
$(function () {
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    getShareRecode()
});

function getShareRecode() {
    var data = { openId: userInfo.openId };
    loading.show();
    $.ajax({
        url: apiUrl + "/getSharedUser",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            loading.hide();
            var html = "<div style='text-align: center;padding: 15px;'>暂无分享记录</div>";
            if (res.success) {
                if (res.data.shareRecord.length > 0) {
                    html = "";
                    res.data.shareRecord.forEach((item) => {
                        html += '<a class="weui-media-box weui-media-box_appmsg">';
                        html += '<div class="weui-media-box__hd">';
                        html += '<img src="' + item.headImage + '" alt="">';
                        html += '</div>';
                        html += '<div class="weui-media-box__bd">';
                        html += '<p class="weui-media-box__title">' + item.nickName + '</p>';
                        html += '</div>';
                        html += '<div class="weui-media-box__ft bd-record">';
                        html += '<p class="bd-remove" onclick="userAuthorization(' + item.id + ',' + item.shareUserId + ', 0, this)">解除分享</p>';
                        if (item.isAuthorization == 0) {
                            html += '<p class="bd-remove m-t-5" onclick="userAuthorization(' + item.id + ',' + item.shareUserId + ', 1, this)">授权登录</p>';
                        }
                        html += '</div>';
                        html += '</a>';
                    });
                }
                $("#shareUser").html(html);
            }
            else {
                dialog.alert("提示", res.message);
            }
        }
    });
}

// 解除分享、授权登录
function userAuthorization(id, shareUserId, type, self) {
    dialog.confirm("提示", (type == 0 ? "解除分享该用户将不能再使用该设备，是否确认要解除分享?" : "是否授权登录?"), () => {
        loading.show();
        $.ajax({
            url: apiUrl + "/delSharedUser",
            type: "POST",
            data: JSON.stringify({
                id: id,
                shareUserId: shareUserId,
                type: type
            }),
            contentType: "application/json",
            success: function (res) {
                loading.hide();
                if (res.success) {
                    type == 0 ? $(self).parent().parent().remove() : self.remove();
                }
                else {
                    dialog.alert("提示", res.message);
                }
            }
        })
    });
}


//function share(shareUid) {
//    $("#delShare").attr("share_uid", shareUid)

//    $("#zhezhao").show();
//    $("#shanchu").show();
//}




//function delShareUser() {
//    var sid = $("#delShare").attr("share_uid");
//    var str = "[dom_uid='" + sid + "']";



//    var loginName = localStorage.getItem("loginName");
//    if (loginName == null) {
//        dialog.alert("提示", "用户可能登录过期，请重新登录");
//        location.href = "login.html";
//    }
//    var data = { loginName: loginName, shareId: sid};


    //$.ajax({
    //    url: apiUrl + "/delSharedUser",
    //    type: "POST",
    //    data: JSON.stringify(data),
    //    contentType: "application/json",
    //    success: function (res) {
    //        if (res.success == true) {

    //            $(str).remove();
    //            $("#zhezhao").hide();
    //            $("#shanchu").hide();

    //        }
    //        else {


    //            dialog.alert("提示", res.message);
    //        }
    //    }
    //})


  
   






//}







   //$("div.bd-record>p.bd-remove").click(function(e){
   //     e.preventDefault();
   //     $(this).parent().parent().siblings(".del-div,.bg-zhezhao").css("display","block");
   // })
  //  console.log($("div.bd-record>p.bd-remove"));


    //$(".bg-zhezhao").click(function(){
    //    $(this).css("display","none")
    //        .siblings(".del-div").css("display","none");
    //})

