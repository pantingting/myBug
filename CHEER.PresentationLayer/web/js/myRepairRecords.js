$(function () {
    var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    var data = { openId: localStorage.getItem("mainOpenId") }
    var image = $(".list li img");
    var title = $(".list li .weui-media-box__title");
    var time = $(".list li .weui-media-box__desc");
    $.ajax({
        url: apiUrl + "/getRepairRecord",
        type: "POST",
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (res) {
            if (res.success) {
                var html = "<li style='text-align: center;list-style: none;'>暂无报修记录</li>";
                if (res.data.repairList.length > 0) {
                    html = '';
                    res.data.repairList.forEach((item) => {
                        html += '<li class="weui-media-box weui-media-box_appmsg">';
                        html += '<div class="weui-media-box__hd">';
                        html += '<' + (item.imgUrl == '' ? (item.videoUrl == '' ? 'img' : 'video') : 'img') + ' class="weui-media-box__thumb" src="' + (item.imgUrl == '' ? (item.videoUrl == '' ? '' : 'http://' + item.videoUrl.split('http://')[1]) : 'http://' + item.imgUrl.split('http://')[1]) + '" alt="">' + (item.imgUrl == '' ? (item.videoUrl == '' ? '' : '</video>') : '');
                        html += '</div>';
                        html += '<div class="weui-media-box__bd">';
                        html += '<div>' + item.nickName +'</div>';
                        html += '<div class="weui-media-box__title">' + item.repairContent + '</div>';
                        if (item.status == 1) {
                            html += '<span>已处理</span>';
                        }
                        html += '<p class="weui-media-box__desc">' + item.repairDTM + '</p>';
                        html +='</div></li>'
                    });
                }

                $('.list').html(html);
            }
            else {
                dialog.alert("提示", res.message);
            }
        }
    });
});