var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
if (userInfo == null) location.href = "login.html";

function DeviceBind() {
    if ($('#deviceCode').val().trim() == '') {
        dialog.alert('提示', '请输入设备码！');
        return;
    }
    var deviceMac = $('#deviceCode').val();
    
    var data = { openId: userInfo.openId, deviceMac: deviceMac };
    loading.show();
    $.ajax({
        url: apiUrl + "/bindDevice",
        type: "POST",
        data: JSON.stringify(data),
        contentType:"application/json",
        success: function (res) {
            loading.hide();
            if (res.success) {
                //dialog.alert("提示", "绑定成功");
                //location.href = "index.html";
                // getAccessToken();
                //if ((res.data.response.resp ? res.data.response.resp[0].errcode == 100002 : false) || (res.data.response.resp ? res.data.response.resp[0].errcode == 0 : false) || (res.data.response.isAuthorization ? res.data.response.isAuthorization == true : false)) {
                //    if (res.data.accessToken == '') {
                //        loading.hide();
                //        return dialog.alert("提示", "获取微信token失败!");
                //    }

                //    //getQRCode(res.data.accessToken);
                //}

                $('.qrcode-box').fadeIn();
            }
            else {
                //loading.hide();
                dialog.alert("提示",res.message);
            }
        },
        error: function () {
            loading.hide();
            dialog.alert("提示", "绑定失败");
        }
    });
}

//function getQRCode(accessToken) {
//    $.ajax({
//        url: apiUrl + "/getQrcode",
//        type: "POST",
//        data: JSON.stringify({
//            token: accessToken
//        }),
//        contentType: "application/json",
//        success: function (res) {
//            if (res.success) {
//                if (res.data.qrCodeInfo.base_resp.errcode == 0) {
//                    updateDeviceInfo(res.data.qrCodeInfo.qrticket, res.data.qrCodeInfo.deviceid);
//                }
//            } else {
//                loading.hide();
//                dialog.alert("提示", "获取设备二维码失败");
//            }
//        },
//        error: function () {
//            loading.hide();
//            dialog.alert("提示", "获取设备二维码失败");
//        }
//    })
//}

// 获取access_token
//function getAccessToken() {
//    $.ajax({
//        type: 'GET',
//        url: 'https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx1a1acc3ddb1ced36&secret=8f9a92bb1d92c20e1e320680aea1c888',
//        success: function (res) {
//            if (res.access_token) {
//                deviceAuthorize(res.access_token);
//            } else {
//                loading.hide();
//                dialog.alert('提示', '获取微信Toke失败');
//            }
//        },
//        error: function () {
//            loading.hide();
//            dialog.alert('提示', '获取微信Toke失败');
//        }
//    })
//}

// 设备授权
//function deviceAuthorize(token) {
//    let data = {
//        device_num: "1",
//        device_list: [
//            {
//                id: $('#deviceCode').val().trim(),
//                mac: $('#deviceCode').val().trim(),
//                connect_protocol: "4",
//                auth_key: "8c82365499da40648de0eb705e4229dc",
//                close_strategy: "1",
//                conn_strategy: "1",
//                crypt_method: "1",
//                auth_ver: "1",
//                manu_mac_pos: "-1",
//                ser_mac_pos: "-2",
//                ble_simple_protocol: "0"
//            }
//        ],
//        op_type: "0",
//        product_id: "47201"
//    }

//    $.ajax({
//        type: 'POST',
//        dataType: 'jsonp',
//        url: 'https://api.weixin.qq.com/device/authorize_device?access_token=' + token + "&callback=?",
//        data: JSON.stringify(data),
//        contentType: "application/json",
//        success: function (res) {
//            if (res.resp[0].errcode == 100002 || res.resp[0].errcode == 0) {
//                getDeviceInfo(token);
//            } else {
//                loading.hide();
//                dialog.alert('提示', '设备授权失败');
//            }
//        },
//        error: function () {
//            loading.hide();
//            dialog.alert('提示', '设备授权失败');
//        }
//    })
//}

// 获取设备deviceid 和二维码
//function getDeviceInfo(token) {
//    $.ajax({
//        type: 'GET',
//        url: 'https://api.weixin.qq.com/device/getqrcode?access_token=' + token + '&product_id=47201',
//        success: function (res) {
//            if (res.base_resp.errmsg == "ok") {
//                updateDeviceInfo(res.qrticket, res.deviceid);
//            } else {
//                loading.hide();
//                dialog.alert('提示', '获取设备deviceid和二维码失败');
//            }
//        },
//        error: function () {
//            loading.hide();
//            dialog.alert('提示', '获取设备deviceid和二维码失败');
//        }
//    })
//}

// 修改deviceID
//function updateDeviceInfo(qrticket, wxDeviceId) {
//    $.ajax({
//        url: apiUrl + "/updateDeviceInfo",
//        type: "POST",
//        data: JSON.stringify({
//            deviceMac: $('#deviceCode').val().trim(),
//            wxDeviceId: wxDeviceId
//        }),
//        contentType: "application/json",
//        success: function (res) {
//            loading.hide();
//            if (res.success) {
//                //var qrcode = new QRCode(document.getElementById('qrcode'), {
//                //    width: 100,
//                //    height: 100
//                //});
//                //qrcode.makeCode(qrticket);
//                $('.qrcode-box').fadeIn();
//            }
//            else {                
//                dialog.alert(res.message);
//            }
//        },
//        error: function () {
//            dialog.alert("提示", "绑定失败");
//        }
//    });
//}
