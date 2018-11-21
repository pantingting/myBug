
function baidu_map(data) {

    var map = new BMap.Map(data.box_sct);
    var myGeo = new BMap.Geocoder();//地址解析与反解析类

    var currentPoint;
    var marker1;
    var marker2;
    map.enableScrollWheelZoom();//启用地图滚轮放大缩小
    if (data) {
        var point = new BMap.Point(data.lng, data.lat);
        marker1 = new BMap.Marker(point);        // 创建标注
        map.addOverlay(marker1);
        var opts = {
            width: 220, // 信息窗口宽度 220-730
            height: 60, // 信息窗口高度 60-650
            title: "" // 信息窗口标题
        };
        var infoWindow = new BMap.InfoWindow("原本位置 " + data.adr + " ,移动红点修改位置!你也可以直接修改上方位置系统自动定位!", opts);  // 创建信息窗口对象
        marker1.openInfoWindow(infoWindow);      // 打开信息窗口

        doit(point);
 
        //window.setTimeout(function () {
        //    loadmap();
        //}, 2000);
    } else {
        var point = new BMap.Point(118.190326, 24.491286);
        doit(point);
        window.setTimeout(function () {
            auto();
        }, 100);
    }
    map.enableDragging();//启用地图拖拽事件，默认启用(可不写)
    map.enableContinuousZoom();//可以联系缩放
    map.addControl(new BMap.NavigationControl());  //向地图中添加缩放控件
    map.addControl(new BMap.ScaleControl());//向地图中添加比例尺控件
    map.addControl(new BMap.OverviewMapControl());//向地图中添加缩略图控件



    function auto() {
        var geolocation = new BMap.Geolocation();
        geolocation.getCurrentPosition(function (r) {
            if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                //var mk = new BMap.Marker(r.point);  
                //map.addOverlay(mk);  
                // point = r.point;  
                //map.panTo(r.point); 

                var point = new BMap.Point(r.point.lng, r.point.lat);
                marker1 = new BMap.Marker(point);        // 创建标注
                map.addOverlay(marker1);
                var opts = {
                    width: 220,     // 信息窗口宽度 220-730
                    height: 60,     // 信息窗口高度 60-650
                    title: ""  // 信息窗口标题
                }

                var infoWindow = new BMap.InfoWindow("定位成功这是你当前的位置!,移动红点标注目标位置，你也可以直接修改上方位置,系统自动定位!", opts);  // 创建信息窗口对象
                marker1.openInfoWindow(infoWindow);      // 打开信息窗口
                doit(point);

            } else {
                //alert('failed' + this.getStatus());
            }
        })
    }
    function doit(point) {

        if (point) {
          
            document.getElementById(data.lat_sct).value = point.lat;
            document.getElementById(data.lng_sct).value = point.lng;
            map.setCenter(point);
            map.centerAndZoom(point, 15);
            map.panTo(point);

            var cp = map.getCenter();
            myGeo.getLocation(point, function (result) {
                if (result) {
                    document.getElementById(data.text_sct +'-inputEl').value = result.address;
                }
            });

            if (marker2==undefined) {
                marker2 = new BMap.Marker(point);        // 创建标注  
                var opts = {
                    width: 220, // 信息窗口宽度 220-730
                    height: 60, // 信息窗口高度 60-650
                    title: "" // 信息窗口标题
                };
            }
           
            var infoWindow = new BMap.InfoWindow("拖拽地图或红点，在地图上用红点标注您的店铺位置。", opts);  // 创建信息窗口对象
            marker2.openInfoWindow(infoWindow);      // 打开信息窗口

            map.addOverlay(marker2);                     // 将标注添加到地图中


            marker2.enableDragging();
            marker2.addEventListener("dragend", function (e) {
                document.getElementById(data.lat_sct).value = e.point.lat;
                document.getElementById(data.lng_sct).value = e.point.lng;
                myGeo.getLocation(new BMap.Point(e.point.lng, e.point.lat), function (result) {
                    if (result) {
                        $('#' + data.text_sct +'-inputEl').value = result.address;
                      
                        marker2.setPoint(new BMap.Point(e.point.lng, e.point.lat));
                        map.panTo(new BMap.Point(e.point.lng, e.point.lat));
                        
                    }
                });
            });

            map.addEventListener("dragend", function showInfo() {
                var cp = map.getCenter();
                myGeo.getLocation(new BMap.Point(cp.lng, cp.lat), function (result) {
                    if (result) {
                        document.getElementById(data.text_sct +'-inputEl').value = result.address;
                        document.getElementById(data.lat_sct).value = cp.lat;
                        document.getElementById(data.lng_sct).value = cp.lng;
                    
                        marker2.setPoint(new BMap.Point(cp.lng, cp.lat));
                        map.panTo(new BMap.Point(cp.lng, cp.lat));
                       
                    }
                });
            });

            map.addEventListener("dragging", function showInfo() {
                var cp = map.getCenter();
               
                marker2.setPoint(new BMap.Point(cp.lng, cp.lat));
                map.panTo(new BMap.Point(cp.lng, cp.lat));
                map.centerAndZoom(marker2.getPoint(), map.getZoom());
            });


        }
    }

    function loadmap() {
        
        var city = document.getElementById('MainPanel_form_ctl02_ctl00_ctl00_address-inputEl').value;
        var myCity = new BMap.LocalCity();
        // 将结果显示在地图上，并调整地图视野  
        myGeo.getPoint(city, function (point) {
            if (point) {
             
            

                marker2.setPoint(new BMap.Point(point.lng, point.lat));
               
                document.getElementById(data.lat_sct).value = point.lat;
                document.getElementById(data.lng_sct).value = point.lng;
                map.panTo(new BMap.Point(marker2.getPoint().lng, marker2.getPoint().lat));
                map.centerAndZoom(marker2.getPoint(), map.getZoom());
            }
        });
    }

    function setarrea(address, city) {
        $('#' + data.text_sct +'-inputEl').value = address;
        
        window.setTimeout(function () {
            loadmap();
        }, 2000);
    }

    function initarreawithpoint(lng, lat) {
        window.setTimeout(function () {
           
            marker2.setPoint(new BMap.Point(lng, lat));
            
            map.panTo(new BMap.Point(lng, lat));
            map.centerAndZoom(marker2.getPoint(), map.getZoom());
        }, 2000);
    }

    $("#" + data.text_sct +'-inputEl').change(function () { loadmap(); });
    $("#" + data.btn_sct).click(function () { loadmap(); });

}
