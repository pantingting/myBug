
C.ready(function () {
    // IDS：default.aspx.cs 中向页面输出的控件客户端ID集合
    var btnExpandAll = C(IDS.btnExpandAll);
    var btnCollapseAll = C(IDS.btnCollapseAll);
    var mainMenu = C(IDS.mainMenu);
    var mainTabStrip = C(IDS.mainTabStrip);
    var windowSourceCode = C(IDS.windowSourceCode);
    var leftPanel = C(IDS.leftPanel);
    var menuSettings = C(IDS.menuSettings);


    // 点击展开菜单
    btnExpandAll.on('click', function () {
        if (IDS.menuType == 'menu') {
            // 左侧为树控件
            mainMenu.expandAll();
        } else {
            // 左侧为树控件+手风琴控件
            var activePane = mainMenu.getActivePane();
            if (activePane) {
                activePane.items[0].expandAll();
            }
        }
    });

    // 点击折叠菜单
    btnCollapseAll.on('click', function () {
        if (IDS.menuType == 'menu') {
            // 左侧为树控件
            mainMenu.collapseAll();
        } else {
            // 左侧为树控件+手风琴控件
            var activePane = mainMenu.getActivePane();
            if (activePane) {
                activePane.items[0].collapseAll();
            }
        }
    });


    function createToolbar(tabConfig) {

        // 由工具栏上按钮获得当前标签页中的iframe节点
        function getCurrentIFrameNode(btn) {
            return $('#' + btn.id).parents('.c-tab').find('iframe');
        }

        var sourcecodeButton = new C.Button({
            text: '源代码',
            type: 'button',
            icon: './res/icon/page_white_code.png',
            listeners: {
                click: function () {
                    var iframeNode = getCurrentIFrameNode(this);
                    var iframeWnd = iframeNode[0].contentWindow

                    var files = [iframeNode.attr('src')];
                    var sourcefilesNode = $(iframeWnd.document).find('head meta[name=sourcefiles]');
                    if (sourcefilesNode.length) {
                        $.merge(files, sourcefilesNode.attr('content').split(';'));
                    }
                    windowSourceCode.c_show('./common/source.aspx?files=' + encodeURIComponent(files.join(';')));
                }
            }
        });

        var openNewWindowButton = new C.Button({
            text: '新标签页中打开',
            type: 'button',
            icon: './res/icon/tab_go.png',
            listeners: {
                click: function () {
                    var iframeNode = getCurrentIFrameNode(this);
                    window.open(iframeNode.attr('src'), '_blank');
                }
            }
        });

        var refreshButton = new C.Button({
            text: '刷新',
            type: 'button',
            icon: './res/icon/reload.png',
            listeners: {
                click: function () {
                    var iframeNode = getCurrentIFrameNode(this);
                    iframeNode[0].contentWindow.location.reload();
                }
            }
        });

        var toolbar = new C.Toolbar({
            items: ['->', sourcecodeButton, '-', refreshButton, '-', openNewWindowButton]
        });

        tabConfig['tbar'] = toolbar;
    }



    // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
    // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
    // mainTabStrip： 选项卡实例
    // createToolbar： 创建选项卡前的回调函数（接受tabConfig参数）
    // updateLocationHash: 切换Tab时，是否更新地址栏Hash值
    // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
    // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame
    C.util.initTreeTabStrip(mainMenu, mainTabStrip, createToolbar, true, false, false);



    // 添加示例标签页
    window.addExampleTab = function (id, url, text, icon, refreshWhenExist) {
        // 动态添加一个标签页
        // mainTabStrip： 选项卡实例
        // id： 选项卡ID
        // url: 选项卡IFrame地址 
        // text： 选项卡标题
        // icon： 选项卡图标
        // addTabCallback： 创建选项卡前的回调函数（接受tabConfig参数）
        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        C.util.addMainTab(mainTabStrip, id, url, text, icon, null, refreshWhenExist);
    };

    // 移除选中标签页
    window.removeActiveTab = function () {
        var activeTab = mainTabStrip.getActiveTab();
        mainTabStrip.removeTab(activeTab.id);
    };



    // 添加工具图标，并在点击时显示上下文菜单
    leftPanel.addTool({
        type: 'gear',
        //tooltip: '系统设置',
        handler: function (event) {
            menuSettings.showBy(this);
        }
    });




    // 打开主题窗口
    var windowThemeRoller = C(IDS.windowThemeRoller);
    $('#header .themeroller').click(function () {
        windowThemeRoller.show();
    });

    $('#header .logo').hover(function () {
        $(this).addClass('ui-state-hover');
    }, function () {
        $(this).removeClass('ui-state-hover');
    });


    // 客户端处理菜单样式和语言
    var menuBtn = C(IDS.MenuStyle);
    if (menuBtn && menuBtn.menu) {
        $.each(menuBtn.menu.items, function (index, item) {
            item.on('checkchange', function () {
                var menuStyle = 'accordion';
                if (item.isChecked() && item.id.indexOf('MenuStyleTree') >= 0) {
                    menuStyle = 'tree';
                }
                C.util.cookie('MenuStyle_Pro', menuStyle, {
                    path: '/',
                    expires: 100
                });
                top.window.location.reload();
            });
        });
    }

    menuBtn = C(IDS.MenuLang);
    if (menuBtn && menuBtn.menu) {
        $.each(menuBtn.menu.items, function (index, item) {
            item.on('checkchange', function () {
                var lang = 'en';
                if (item.isChecked()) {
                    if (item.id.indexOf('MenuLangZHCN') >= 0) {
                        lang = 'zh_CN';
                    } else if (item.id.indexOf('MenuLangZHTW') >= 0) {
                        lang = 'zh_TW';
                    }
                }
                C.util.cookie('Language_Pro', lang, {
                    path: '/',
                    expires: 100
                });
                top.window.location.reload();
            });
        });
    }

});
