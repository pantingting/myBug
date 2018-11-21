var params = (function () {
    return {
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        },

        setUrlParam: function (row) {
            var query = location.search.substring(1);
            var href = '';
            for (var key in row) {
                var p = new RegExp("(^|" + key + ")=[^&]*");
                if(p.test(query)){
                    query = query.replace(p, "$1=" + row[key]);
                }else{
                    if(query == '' && href == ''){
                        href = key + '=' + row[key];
                    }else{
                        href = '&' + key + '=' + row[key];
                    }
                }
            }
            location.search = '?' + query + href;
        }
    }
})();
