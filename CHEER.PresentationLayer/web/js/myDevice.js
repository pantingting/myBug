var deviceId = "";
var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
var wxDeviceId = "";

var openId = localStorage.getItem("CH_openId");

$(function () {
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    if (userInfo.openId == localStorage.getItem("mainOpenId")) {
        var html = '<div class="btn">';
        html += '<a class="del-btn" >删除</a>';
        html += '<a class="share-btn" >分享</a>';
        html += '</div>';

        $('.list').after(html);
        
        $.ajax({
            url: "http://a.choositon.com/Controllers/user/getSignature",
            type: "POST",
            data: JSON.stringify({
                url: location.href
            }),
            contentType: "application/json",
            success: function (res) {
                if (res.success) {
                    wx.config({
                        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                        appId: res.appId, // 必填，公众号的唯一标识
                        timestamp: res.timestamp, // 必填，生成签名的时间戳
                        nonceStr: res.nonceStr, // 必填，生成签名的随机串
                        signature: res.signature,// 必填，签名
                        jsApiList: ['onMenuShareAppMessage'] // 必填，需要使用的JS接口列表
                    });
                }
                else {
                    console.log(res.message);
                }
            }
        });

        wx.ready(function () {
            // config信息验证后会执行ready方法，所有接口调用都必须在config接口获得结果之后，config是一个客户端的异步操作，所以如果需要在页面加载时就调用相关接口，则须把相关接口放在ready函数中调用来确保正确执行。对于用户触发时才调用的接口，则可以直接调用，不需要放在ready函数中。

            //这里就是分享信息配置
            wx.onMenuShareAppMessage({
                title: '设备分享', // 分享标题  
                desc: '设备分享', // 分享描述  
                link: "http://" + window.location.host + "/web/pages/login.html?deviceId=" + deviceId, // 分享链接  
                imgUrl: '', // 分享图标  
                type: '', // 分享类型,music、video或link，不填默认为link  
                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空  
                success: function () {
                    dialog.tip("设备分享成功");
                },
                cancel: function () {
                    // 用户取消分享后执行的回调函数  
                }
            });
        });
    }
    
    var deviceName = $("input ");
    var deviceMac = $(".list-id");
    var data = { openId: userInfo.openId, mainOpenId: localStorage.getItem("mainOpenId") };
    $.ajax({
        url: apiUrl + "/userMyDevice",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                deviceName.val(res.data.deviceInfo[0].name);
                deviceMac.text(res.data.deviceInfo[0].mac);
                deviceId = res.data.deviceInfo[0].id;
                wxDeviceId = res.data.deviceInfo[0].wxDeviceId;
            }
            else {
                dialog.alert("提示", res.message);
            }
        }
    });

    $(".del-btn").click(function () {
        $(this).parent().siblings(".del-div,.bg-zhezhao").css("display", "block");
    })
    $(".share-btn").click(function () {
        $(this).parent().siblings(".share-img,.bg-zhezhao").css("display", "block");
    })
    $(".bg-zhezhao").click(function () {
        $(this).css("display", "none")
            .siblings(".share-img,.del-div").css("display", "none");
    });
});

function Del() {
    var data = {
        device_id: wxDeviceId.toUpperCase(),
        openid: userInfo.openId
    };

    if (userInfo.openId != localStorage.getItem("mainOpenId")) return dialog.alert("提示", "您没有权限删除该设备!");

    loading.show();

    $.ajax({
        url: apiUrl + "/unbindDevice",
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json",
        success: function (res) {
            loading.hide();
            if (res.success) {
                $('.del-div,.bg-zhezhao').hide();
                dialog.alert("提示", "删除成功", () => {
                    localStorage.removeItem("CH_UserInfo");
                    localStorage.removeItem("mainOpenId");
                    localStorage.removeItem("loginName");
                    location.href = "bindDevice.html";
                });
            }
            else {
                $('.del-div,.bg-zhezhao').hide();
                dialog.alert("提示", "删除失败!");
                console.log(res.message);
            }
        }
    });
}



