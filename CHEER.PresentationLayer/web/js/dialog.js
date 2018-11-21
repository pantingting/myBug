/**
 * Created by Administrator on 2017/3/31.
 */

var dialog = (function () {
    return {
        confirm: function (title, content, sure, cancel) {
            var html = '<div class="dialog_confirm" id="dialog"><div class="mask"></div> <div class="dialog animated fadeInDown"><div class="dialog_title">' + title + '<span class="close">x</span></div> <div class="dialog_bd">' + content + '</div><div class="dialog_ft"> <a href="javascript:;" class="btn_dialog cancel">取消</a> <a href="javascript:;" class="btn_dialog sure">确认</a></div></div></div>';
            $('body').append(html);

            $('#dialog').on('click', '.sure', function () {
                $('#dialog').remove();
                sure && sure();
            }).on('click', '.cancel', function () {
                $('#dialog').remove();
                cancel && cancel()
            }).on('click', '.close', function () {
                $('#dialog').remove();
            });
        },
        tip:function (content){
            var html = '<div class="dialog_tip" style="z-index:10000"><div class="dialog"><div class="dialog_bd">'+content+'</div></div></div>';
            $('body').append(html);
            setTimeout(function(){
                $('.dialog_tip').remove();
            },2000)
        },
        alert: function (title, content, sure) {
            var html = '<div class="dialog_confirm" id="dialog"><div class="mask"></div> <div class="dialog animated fadeInDown"><div class="dialog_title">' + title + '<span class="close">x</span></div> <div class="dialog_bd">' + content + '</div><div class="dialog_ft"><a href="javascript:;" class="btn_dialog sure">确 认</a></div></div></div>';
            $('body').append(html);

            $('#dialog').on('click', '.sure', function () {
                $('#dialog').remove();
                sure && sure();
            });
        }
    }
})();
