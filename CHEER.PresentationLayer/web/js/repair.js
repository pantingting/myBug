var userInfo = JSON.parse(localStorage.getItem("CH_UserInfo"));
var $tar = null;

$(function () {
    if (userInfo == null || localStorage.getItem("loginName") == null || localStorage.getItem("mainOpenId") == null) {
        location.href = "login.html";
        return;
    }

    $(".nav").on("click", "div", function (e) {
        $tar = $(e.target);
        $tar.addClass("in").css("border", "1px solid #ffae7f")
            .siblings().removeClass("in").css("border", "1px solid transparent");
    });

    $(".filepath").on("change", function () {
        var files = this.files[0];
        if (files == undefined) return;
        var size = Math.ceil(files.size / 1024) / 1024
        if (size > 20) return dialog.alert("提示", "文件大小不能超过20M");
        
        var srcs = getObjectURL(this.files[0]);   //获取路径
        var name = $(this)[0].files[0].name;
        
        //var video = null;

        //var initialize = function () {
        //    video = $(this).nextAll()[2];
        //    video.addEventListener('loadeddata', captureImage);
        //}

        //var captureImage = function () {
        //    var canvas = $(this).nextAll()[3];
        //    var ctx = canvas.getContext('2d');
        //    canvas.width = 100;
        //    canvas.height = 100;
        //    ctx.drawImage(video, 0, 0, 100, 100);
        //    var images = canvas.toDataURL('imgae/png');
        //    $(this).nextAll()[2].poster = images;
        //}

        //initialize();
        $(this).nextAll(".img1").hide();   //this指的是input
        $(this).nextAll(".img2").show();
        $(this).nextAll('.close').show();
        $(this).nextAll(".img2").attr("src", srcs);

        if ($(this)[0].accept == "video/*")
            $(this).parent().nextAll().length == 3 ? $('.upload-video li input').eq(1).removeClass('none') : ($(this).parent().nextAll().length == 2) ? $('.upload-video li input').eq(2).removeClass('none') : '';
        else
            $(this).parent().nextAll().length == 3 ? $('.list-pic li input').eq(1).removeClass('none') : ($(this).parent().nextAll().length == 2) ? $('.list-pic li input').eq(2).removeClass('none') : '';
        $(".close").on("click", function () {
            $(this).hide();     //this指的是span
            $(this).nextAll(".img2").attr("src", '');
            $(this).nextAll(".img2").hide();
            $(this).nextAll(".img1").show();
        });
    });

    // 查询用户账号是否存在异常
    $.ajax({
        url: apiUrl + "/checkUserAccount",
        data: JSON.stringify({
            openId: userInfo.openId,
            mainOpenId: localStorage.getItem("mainOpenId")
        }),
        type: "POST",
        contentType: "application/json",
        success: function (res) {
            if (res.success == false) {
                if (res.errCode == "login") {
                    dialog.alert("提示", res.message, function () {
                        if (userInfo.openId == localStorage.getItem("mainOpenId"))
                            location.href = "bindDevice.html";
                        else
                            location.href = "login.html";
                    });
                } else {
                    dialog.alert("提示", res.message);
                }
            }
        },
        error: function () {

        }
    });
});


function getObjectURL(file) {
    var url = null;
    if (window.createObjectURL != undefined) {
        url = window.createObjectURL(file)
    } else if (window.URL != undefined) {
        url = window.URL.createObjectURL(file)
    } else if (window.webkitURL != undefined) {
        url = window.webkitURL.createObjectURL(file)
    }
    return url
}

var isSubmit = true;

function submitRepairInfo() {
    if (isSubmit == false) return;
    
    if ($tar == null) return; dialog.alert("提示", "请选择故障类型");
    var filelist = $(".filepath");
    
    var formData = new FormData();
    filelist.each(function (index) {
        if ($(this)[0].files[0] != null) {
            formData.append(index, $(this)[0].files[0]);
        }
    });
    loading.show("正在提交中,请耐心等待!");
    isSubmit = false;
    $.ajax({
        url: apiUrl + "/uploadRepair",
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        mimeType: "multipart/form-data",
        dataType: "json",
        success: function (res) {
            if (res.success) {
                var data = {
                    openId: userInfo.openId,
                    repairContent: $tar.text(),
                    imgUrl1: res.image[0],
                    imgUrl2: res.image[1],
                    imgUrl3: res.image[2],
                    videoUrl1: res.video[0],
                    videoUrl2: res.video[1],
                    videoUrl3: res.video[2],
                    mainOpenId: localStorage.getItem("mainOpenId")
                };
                $.ajax({
                    url: apiUrl + "/deviceRepair",
                    data: JSON.stringify(data),
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (res) {
                        loading.hide();
                        if (res.success) {
                            location.href = "repairSuccess.html";
                        }
                        else {
                            dialog.alert("提示", res.message);
                        }
                    },
                    error: function () {
                        loading.hide();
                    }
                });
            }
            else {
                isSubmit = true;
                loading.hide();
                dialog.alert("提示", res.message);
            }
        },
        error: function () {
            isSubmit = true;
            loading.hide();
            console.log("error");
        }
    });
};


