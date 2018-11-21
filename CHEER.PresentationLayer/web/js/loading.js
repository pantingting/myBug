/**
 * Created by moneyinto on 2017/3/31.
 */
var loading = (function () {
    return {
        show: function (content) {
            var html = '<div class="ui-loading-block"><div class="ui-loading-cnt"> <i class="ui-loading-bright"></i> <p>' + ((content == '' || content == undefined || content == null || content == "undefined") ? '正在加载中...' : content) + '</p></div></div>';
            $('body').append(html);
        },

        hide: function () {
            $('.ui-loading-block').remove();
        }
    }
})();

var apiUrl = 'http://localhost/choositon/eApi';

var headers = {
    Authorization: "Basic " + window.btoa('JieWeifu:JieWeifu201406'),
};

function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

function setCookie(name, value, time) {
    var strsec = getsec(time);
    var exp = new Date();
    exp.setTime(exp.getTime() + strsec * 1);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function getsec(str) {
    var str1 = str.substring(1, str.length) * 1;
    var str2 = str.substring(0, 1);
    if (str2 == "s") {
        return str1 * 1000;
    }
    else if (str2 == "h") {
        return str1 * 60 * 60 * 1000;
    }
    else if (str2 == "d") {
        return str1 * 24 * 60 * 60 * 1000;
    }
}