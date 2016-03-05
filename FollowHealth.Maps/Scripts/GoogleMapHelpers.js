var currentlyOpenedInfoWindow = null;
var map_canvas;
var map;
function init_map(map_canvas_id, lat, lng, zoom, markers, infoWindowContents, isSetInterval, isReload, arrDistInd,defaultPosition) {
    var myLatLng = new google.maps.LatLng(lat, lng);
    var options = {
        zoom: zoom,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map_canvas = document.getElementById(map_canvas_id);

    map = new google.maps.Map(map_canvas, options);

       //Markers
    if (markers && markers.length > 0) {
        var bounds = new google.maps.LatLngBounds();
        var marker;
       
        //Create default marker
        if (defaultPosition != undefined && defaultPosition!="") {
            marker = createMarker(bounds, defaultPosition, map, 'blue-dot.png');
        }

        for (var i = 0; i < markers.length; i++) {
            var isFilter = false;

            //locLat = markers[i].position.lat().toFixed(7);
            //locLong = markers[i].position.lng().toFixed(7);
            //isFilter = locLat == lat && locLong == lng
            
            if (getMinDistIndex(arrDistInd, i) != 0 || isReload == false) {
                marker=createMarker(bounds, markers[i].position, map, 'red-dot.png')
                ////if (isFilter == true || isReload == false) {
                //var marker = new google.maps.Marker({
                //    position: markers[i].position,
                //    map: map,
                //    icon: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png',//markers[i].icon,
                //    optimized: false,
                //    size: new google.maps.Size(71, 71),
                //    origin: new google.maps.Point(0, 0),
                //    anchor: new google.maps.Point(17, 34),
                //    scaledSize: new google.maps.Size(35, 35)
                //});

                //marker.setMap(map);
                //bounds.extend(marker.getPosition());
                if (infoWindowContents && infoWindowContents.length > i)
                    createInfoWindow(map, marker, infoWindowContents[i]);

            }
        }

        if (isSetInterval == true) {
            isSetInterval = false;
            setInterval(updateGoogleMap, 60000);
        }
    }
}

function createInfoWindow(map, marker, infoWindowProperties) {
    var info = new google.maps.InfoWindow(infoWindowProperties);
    google.maps.event.addListener(marker, 'click', function () {
        if (currentlyOpenedInfoWindow != null)
            currentlyOpenedInfoWindow.close();
        info.open(map, marker);
        currentlyOpenedInfoWindow = info;

    });
}

//if re-load is required we need to update this method
function reload_map(lat, lng, zoom, markers, infoWindowContents) {
    if (markers && markers.length > 0) {
        var bounds = new google.maps.LatLngBounds();

        for (var i = 0; i < markers.length; i++) {
            var marker = new google.maps.Marker({
                position: markers[i].position,
                map: map,
                icon: markers[i].icon,
                optimized: false
            });

            marker.setMap(map);

            bounds.extend(marker.getPosition());

            if (infoWindowContents && infoWindowContents.length > i)
                createInfoWindow(map, marker, infoWindowContents[i]);
        }
    }
}

function getMinDistIndex(arrDistInd, ind) {
    var temp = 0;
    if (arrDistInd == 0)
        return temp;
    for (var i = arrDistInd.length - 1; i > (arrDistInd.length - 5) ; i--) {
        if (arrDistInd[i] == ind) {
            return ind;
        }
    }
    return temp;
}

function createMarker(bounds, position, map, icon, infoWindowContents)
{
    var marker = new google.maps.Marker({
        position: position,
        map: map,
        icon: 'http://maps.google.com/mapfiles/ms/icons/'+icon,//red-dot.png',//markers[i].icon,
        optimized: false,
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(35, 35)
    });

    marker.setMap(map);
    bounds.extend(marker.getPosition());
    //if (infoWindowContents && infoWindowContents.length > i)
    //    createInfoWindow(map, marker, infoWindowContents[i]);
    return marker;
}