/////////////////// INITIALIZATIONS /////////////////////////////////////////////
//Step 1: initialize communication with the platform

var platform = new H.service.Platform({
        'app_id': 'ORWs1MBbnXAyzlgdPGpw',
    'app_code': 'ftEQwIdOxSdxiRv6pd1Rvw',
    useHTTPS: true
});
var pixelRatio = window.devicePixelRatio || 1;
var defaultLayers = platform.createDefaultLayers({
    tileSize: pixelRatio === 1 ? 256 : 512,
    ppi: pixelRatio === 1 ? undefined : 320
});

//Step 2: initialize a map
var map = new H.Map(document.getElementById('mapContainer'),
    defaultLayers.normal.map, {
        center: { lat: 33.8263, lng: 36.3431 },
        zoom: 8,
        pixelRatio: pixelRatio
    });
const ui = H.ui.UI.createDefault(map, defaultLayers);

// Enable the event system on the map instance:
const mapEvents = new H.mapevents.MapEvents(map);

// Instantiate the default behavior, providing the mapEvents object (move around, etc.)
const behavior = new H.mapevents.Behavior(mapEvents);

////////////////////////////////// COORD SHOWER //////////////////////////////////////
map.addEventListener('pointerdown', e => {
    coord = map.screenToGeo(e.currentPointer.viewportX, e.currentPointer.viewportY);

    const radios = document.querySelectorAll("#clickType");
    let choice = "";

    for (let i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {
            choice = radios[i].value;
            break;
        }
    }

    switch (choice) {
        case "none": return;
        case "source": setStart(coord); break;
        case "destination": setEnd(coord); break;
    }
});
/*
var platform = new H.service.Platform({
    'app_id': 'ORWs1MBbnXAyzlgdPGpw',
    'app_code': 'ftEQwIdOxSdxiRv6pd1Rvw',
    useHTTPS: true
});
// Retrieve the target element for the map:
var targetElement = document.getElementById('mapContainer');

// Get the default map types from the platform object:
var defaultLayers = platform.createDefaultLayers();

// Instantiate the map:
var map = new H.Map(
    document.getElementById('mapContainer'),
    defaultLayers.normal.map,
    {
        zoom: 10,
        center: { lat: 52.51, lng: 13.4 }
    });
*/
setInterval(() => {
    fixNameTagPositions();
}, 5);
let displayedVehicles = [];
let displayedCenters = [];
let startPicker = [];
let endPicker = [];
let clientsList = [];
var startCity = "";
var endCity = "";

async function addVehicleToMap(vehicleID, latitude, longitude, vehicleInfo, currentDriver, deliveries) {
    console.log("-------------------- IN ADD VEHICLE --------------------");
    // Create a marker icon from an image URL:
    const icon = new H.map.Icon('/images/truck.png');

    // Create a marker using the previously instantiated icon:
    const marker = new H.map.Marker({ lat: latitude, lng: longitude }, { icon: icon });

    // Add the marker to the map:
    map.addObject(marker);

    // Name Tag
    createNameTag('tag' + vehicleID, currentDriver.name);

    // Add to array of displayed vehicles
    displayedVehicles[vehicleID] = {
        "vehicleID": vehicleID,
        "longitude": longitude,
        "latitude": latitude,
        "info": vehicleInfo,
        "currentDriver": currentDriver,
        "marker": marker,
        "deliveries": deliveries,
        "routeObj": null
    };

    // Add route to map if not null
    let flag;
    if (deliveries != null && arrayLength(deliveries) > 0) {
        let currentRoute = { "waypoint0": { "latitude": displayedVehicles[vehicleID].latitude, "longitude": displayedVehicles[vehicleID].longitude } };
        let waypointCount = 1;

        for (delivery in deliveries) {
            currentRoute["waypoint" + waypointCount] = { "latitude": deliveries[delivery].startLatitude, "longitude": deliveries[delivery].startLongitude };
            currentRoute["waypoint" + (waypointCount + 1)] = { "latitude": deliveries[delivery].endLatitude, "longitude": deliveries[delivery].endLongitude };
            waypointCount += 2;
        }

        flag = await addRouteToMap(vehicleID, currentRoute);
        let flag2 = await getDeliveryTimeLeft(vehicleID);

        return new Promise(resolve => {
            console.log("-------------------- OUT ADD VEHICLE --------------------");
            resolve(flag || true);
        });

    }

    return new Promise(resolve => {
        console.log("-------------------- OUT ADD VEHICLE --------------------");
        resolve(flag || true);
    });
}

function updateVehicleInMap(vehicleID, latitude, longitude) {
    // Remove old marker for this vehicle
    const oldMarker = displayedVehicles[vehicleID].marker;
    map.removeObject(oldMarker);

    // Add new market for this vehicle
    // Create a marker icon from an image URL:
    const icon = new H.map.Icon('/images/truck.png');

    // Create a marker using the previously instantiated icon:
    const marker = new H.map.Marker({ lat: latitude, lng: longitude }, { icon: icon });

    // Add the marker to the map:
    map.addObject(marker);

    // Update vehicle data in displayed vehicles array
    displayedVehicles[vehicleID].longitude = longitude;
    displayedVehicles[vehicleID].latitude = latitude;
    displayedVehicles[vehicleID].marker = marker;
}

function removeVehicleFromMap(vehicleID) // Does NOT remove its routes though
{
    console.log("--------------------- IN REMOVE VEHICLE ---------------");

    map.removeObject(displayedVehicles[vehicleID].marker);
    displayedVehicles[vehicleID].latitude = null;
    displayedVehicles[vehicleID].longitude = null;

    return new Promise(resolve => {
        console.log("--------------------- OUT REMOVE VEHICLE ---------------");
        resolve(true);
    });
}

function addRouteToMap(vehicleID, route) {

  
    console.log("-------------------- IN ADD ROUTE --------------------");
    let routingParameters = {
        'mode': 'fastest;car',
        'representation': 'display'
    };

    for (waypoint in route) {
        routingParameters[waypoint] = `geo!${route[waypoint].latitude},${route[waypoint].longitude}`;
    }

    const router = platform.getRoutingService();
    router.calculateRoute(routingParameters,
        function (result) {
            let route,
                routeShape,
                startPoint,
                endPoint,
                linestring;
          
            if (result.response.route) {

                // Pick the first route from the response:
                route = result.response.route[0];
                // Pick the route's shape:
                routeShape = route.shape;

                // Create a linestring to use as a point source for the route line
                linestring = new H.geo.LineString();

                // Push all the points in the shape into the linestring:
                routeShape.forEach(function (point) {
                    var parts = point.split(',');
                    linestring.pushLatLngAlt(parts[0], parts[1]);
                });
                
                // Retrieve the mapped positions of the requested waypoints:
                startPoint = route.waypoint[0].mappedPosition;
                endPoint = route.waypoint[route.waypoint.length - 1].mappedPosition;

                // Create a polyline to display the route
                routeLine = new H.map.Polyline(linestring, {
                    style: { lineWidth: 10 },
                    arrows: { fillColor: 'white', frequency: 2, width: 0.8, length: 0.7 }
                });

                // Create a marker for the start point:
                const route_start_icon = new H.map.Icon('/images/route_start.png');
                var startMarker = new H.map.Marker({
                    lat: startPoint.latitude,
                    lng: startPoint.longitude
                }, { icon: route_start_icon });

                // Create a marker for the end point:
                const route_end_icon = new H.map.Icon('/images/route_end.png');
                var endMarker = new H.map.Marker({
                    lat: endPoint.latitude,
                    lng: endPoint.longitude
                }, { icon: route_end_icon });

                // Create stop icons if exist
                let stopMarkers = [];
                const source_icon = new H.map.Icon("/images/source.png");
                const dropoff_icon = new H.map.Icon("/images/dropoff2.png");
                for (let i = 1; i < route.waypoint.length - 1; i++) {
                    stopMarkers.push(new H.map.Marker({
                        lat: route.waypoint[i].mappedPosition.latitude,
                        lng: route.waypoint[i].mappedPosition.longitude
                    }, { icon: source_icon }));

                    i++;
                    if (i >= route.waypoint.length - 1) break;

                    stopMarkers.push(new H.map.Marker({
                        lat: route.waypoint[i].mappedPosition.latitude,
                        lng: route.waypoint[i].mappedPosition.longitude
                    }, { icon: dropoff_icon }));
                }

                // Add route object to displayed vehicles array
                if (vehicleID != null) {
                    displayedVehicles[vehicleID].routeObj = { "routeLine": routeLine, "startMarker": startMarker, "endMarker": endMarker, "stops": stopMarkers };
                }

                // Add the route polyline and the two markers to the map:
                //alert("drawing route");
                map.addObjects([routeLine, startMarker, endMarker]);
                if (stopMarkers.length > 0)
                    map.addObjects(stopMarkers);

                // Set the map's viewport to make the whole route visible:
                //map.setViewBounds(routeLine.getBounds());

                return new Promise(resolve => {
                    console.log("-------------------- OUT ADD ROUTE --------------------");
                    resolve(true);
                });
            }
        },
        function (error) {
            alert(error.message);
        });


}

function removeRouteFromMap(vehicleID) {
    console.log("--------------------- IN REMOVE ROUTE --------------------");

    if (/*displayedVehicles[vehicleID].deliveries && arrayLength(displayedVehicles[vehicleID].deliveries) > 0 && */displayedVehicles[vehicleID].routeObj) {
        map.removeObject(displayedVehicles[vehicleID].routeObj.routeLine);
        map.removeObject(displayedVehicles[vehicleID].routeObj.startMarker);
        map.removeObject(displayedVehicles[vehicleID].routeObj.endMarker);
        map.removeObjects(displayedVehicles[vehicleID].routeObj.stops);

        //displayedVehicles[vehicleID].deliveries = [];
        displayedVehicles[vehicleID].routeObj = null;

        return new Promise(resolve => {
            console.log("--------------------- OUT REMOVE ROUTE --------------------");
            resolve(true);
        });
    } else {
        return new Promise(resolve => {
            console.log("--------------------- OUT REMOVE ROUTE --------------------");
            resolve(true);
        });
    }
}

async function updateVehicleRoute(vehicleID, deliveries) {
    console.log("--------------------- IN UPDATE ROUTE --------------------");
    console.log(deliveries);
    //if(displayedVehicles[vehicleID].route == null) return;

    const vehicleLatitude = displayedVehicles[vehicleID].latitude;
    const vehicleLongitude = displayedVehicles[vehicleID].longitude;
    const vehicleDriver = displayedVehicles[vehicleID].currentDriver;
    const vehicleInfo = displayedVehicles[vehicleID].info;

    let flag1 = await removeRouteFromMap(vehicleID);
    let flag2 = await removeVehicleFromMap(vehicleID);
    let flag3 = await addVehicleToMap(vehicleID, vehicleLatitude, vehicleLongitude, vehicleInfo, vehicleDriver, deliveries);
    let flag4 = await getDeliveryTimeLeft(vehicleID);

    return new Promise(resolve => {
        console.log("--------------------- OUT UPDATE ROUTE --------------------");
        resolve(flag1 && flag2 && flag3);
    });
}

function addOptRoute() {
    let startLatitude = document.querySelector("#startLatitudeOptRouteInput").value;
    let startLongitude = document.querySelector("#startLongitudeOptRouteInput").value;
    let endLatitude = document.querySelector("#endLatitudeOptRouteInput").value;
    let endLongitude = document.querySelector("#endLongitudeOptRouteInput").value;

    const route = {
        "waypoint0": { "latitude": startLatitude, "longitude": startLongitude },
        "waypoint1": { "latitude": endLatitude, "longitude": endLongitude }
    }

    addRouteToMap(null, route);
}

const HttpClient = function () {
    this.get = function (aUrl, aCallback) {
        var anHttpRequest = new XMLHttpRequest();
        anHttpRequest.onreadystatechange = function () {
            if (anHttpRequest.readyState == 4 && anHttpRequest.status == 200)
                aCallback(anHttpRequest.responseText);
        }

        anHttpRequest.open("GET", aUrl, true);
        anHttpRequest.send(null);
    }
}


function getDeliveryTimeLeft(vehicleID) {
    return new Promise(resolve => {
        console.log("--------------------- IN GET DELIVERY TIME --------------------");
        if (displayedVehicles[vehicleID].deliveries == null || arrayLength(displayedVehicles[vehicleID].deliveries) == 0) resolve(0);
        else {

            const deliveries = displayedVehicles[vehicleID].deliveries;
            const currentLatitude = displayedVehicles[vehicleID].latitude;
            const currentLongitude = displayedVehicles[vehicleID].longitude;

            let requestURL = `https://route.api.here.com/routing/7.2/calculateroute.json?app_id=ORWs1MBbnXAyzlgdPGpw&app_code=ftEQwIdOxSdxiRv6pd1Rvw&waypoint0=geo!${currentLatitude},${currentLongitude}&`;

            let waypointCounter = 1;
            for (delivery in deliveries) {
                requestURL += `waypoint${waypointCounter}=geo!${deliveries[delivery].startLatitude},${deliveries[delivery].startLongitude}&`;
                requestURL += `waypoint${waypointCounter + 1}=geo!${deliveries[delivery].endLatitude},${deliveries[delivery].endLongitude}&`;
                waypointCounter += 2;
            }

            requestURL += `mode=fastest;car;traffic:enabled`;

            const client = new HttpClient();
            client.get(requestURL, function (response) {
                const responseJSON = JSON.parse(response);
                const trafficTime = responseJSON.response.route[0].summary.trafficTime;

                console.log("--------------------- OUT GET DELIVERY TIME --------------------");
                resolve(trafficTime);
            });
        }
    });
}

function getRouteTime(route) {
    return new Promise(resolve => {
        console.log("--------------------- IN GET ROUTE TIME --------------------");
        console.log(route);
        let requestURL = `https://route.api.here.com/routing/7.2/calculateroute.json?app_id=ORWs1MBbnXAyzlgdPGpw&app_code=ftEQwIdOxSdxiRv6pd1Rvw&`;

        for (waypoint in route) {
            requestURL += `${waypoint}=geo!${route[waypoint].latitude},${route[waypoint].longitude}&`;
        }


        requestURL += `mode=fastest;car;traffic:enabled`;

        const client = new HttpClient();
        client.get(requestURL, function (response) {
            const responseJSON = JSON.parse(response);
            const trafficTime = responseJSON.response.route[0].summary.trafficTime;

            console.log("--------------------- OUT GET ROUTE TIME --------------------");
            resolve(trafficTime);
        });
    });
}

function addCenterToMap(centerID, centerName, latitude, longitude,image) {
    // Create a marker icon from an image URL for the warehouse
    const icon = new H.map.Icon('/images/'+image);
    const marker = new H.map.Marker({ lat: latitude, lng: longitude }, { icon: icon });
    map.addObject(marker);

    // Name Tag
    createNameTag('ctag' + centerID, centerName);

    // Add to array of displayed vehicles
    displayedCenters[centerID] = {
        "centerID": centerID,
        "centerName": centerName,
        "longitude": longitude,
        "latitude": latitude,
        "image": image,
        "marker": marker,
    };
    fixNameTagPositions();

}

function RemoveCenterFromMap(centerID) {
    map.removeObject(displayedCenters[centerID].marker);
    ui.removeBubble(displayedCenters[centerID].tag);
    displayedCenters[centerID].latitude = null;
    displayedCenters[centerID].longitude = null;
}
function setStart(coord) {
    // Check if an icon already exists to remove it
    try {
        map.removeObject(startPicker.marker);
    } catch (e) { }

    // Create a start marker icon from an image URL for the start position
    const icon = new H.map.Icon('/images/start.png');
    const marker = new H.map.Marker({ lat: coord.lat, lng: coord.lng }, { icon: icon });
    map.addObject(marker);

    // Save to startPicker object
    startPicker.latitude = coord.lat;
    startPicker.longitude = coord.lng;
    startPicker.marker = marker;

    // Set values to input boxes in the html page (for testing only probably)
    document.querySelector("#startLatitudeOptRouteInput").value = coord.lat;
    document.querySelector("#startLongitudeOptRouteInput").value = coord.lng;

    let url1 = `https://nominatim.openstreetmap.org/reverse?format=json&lon=${coord.lng}&lat=${coord.lat}`;
    var jsonPromise1 = $.getJSON(url1);

    jsonPromise1.done(function (data) {

        if (data.address.hasOwnProperty('city'))
            startCity = data.address.city;
        else
            startCity = data.address.state.split(" ")[0];
        alert(startCity);
    });
    jsonPromise1.fail(function (reason) {
        alert("Error");
    });


}
function setEnd(coord) {
    // Check if an icon already exists to remove it
    try {
        map.removeObject(endPicker.marker);
    } catch (e) { }

    // Create an end marker icon from an image URL for the end position
    const icon = new H.map.Icon('/images/end.png');
    const marker = new H.map.Marker({ lat: coord.lat, lng: coord.lng }, { icon: icon });
    map.addObject(marker);

    // Save to endPicker object
    endPicker.latitude = coord.lat;
    endPicker.longitude = coord.lng;
    endPicker.marker = marker;
    
    // Set values to input boxes in the html page (for testing only probably)
    document.querySelector("#endLatitudeOptRouteInput").value = coord.lat;
    document.querySelector("#endLongitudeOptRouteInput").value = coord.lng;

    let url1 = `https://nominatim.openstreetmap.org/reverse?format=json&lon=${coord.lng}&lat=${coord.lat}`;
    var jsonPromise1 = $.getJSON(url1);

    jsonPromise1.done(function (data) {

        if (data.address.hasOwnProperty('city'))
            endCity=data.address.city;
        else
            endCity = data.address.state.split(" ")[0];
        alert(endCity);
    });
    jsonPromise1.fail(function (reason) {
        alert("Failed1");
    });

}
function getDriverDestination(driver) {
    let destinationWaypoint = null;

    for (waypoint in displayedVehicles[driver].route) {
        destinationWaypoint = displayedVehicles[driver].route[waypoint];
    }

    return destinationWaypoint;
}
async function findOptimalDriverForNewDelivery(startLatitude, startLongitude) {
    console.log("--------------------- IN FIND OPT --------------------");
    let driversResponseTime = [];
  
    for (driver in displayedVehicles) {
        driversResponseTime[driver] = {};

        driversResponseTime[driver].vehicle = displayedVehicles[driver];
        driversResponseTime[driver].driverID = driver;

        let driverCurrentDestination = getDriverDestination(driver);
        if (driverCurrentDestination == null) {
            driverCurrentDestination = {};
            driverCurrentDestination.latitude = displayedVehicles[driver].latitude;
            driverCurrentDestination.longitude = displayedVehicles[driver].longitude;
        }

        const finishTillStartRoute = {
            "waypoint0": { "latitude": driverCurrentDestination.latitude, "longitude": driverCurrentDestination.longitude },
            "waypoint1": { "latitude": startLatitude, "longitude": startLongitude }
        }
      

        const deliveryTimeLeft = await getDeliveryTimeLeft(driver);
      
        const finishTillStartTime = await getRouteTime(finishTillStartRoute);
      
        driversResponseTime[driver].responseTime = deliveryTimeLeft + finishTillStartTime;
    }
    
    console.log("---------------------- GOT OUT OF LOOP ---------------------------")
    let shortestTime = Infinity;
    let optimalDriver = null;
    for (driver in driversResponseTime) {
        const responseTime = driversResponseTime[driver].responseTime;

        if (responseTime < shortestTime) {
            shortestTime = responseTime;
            optimalDriver = driversResponseTime[driver];
        }
    }

    console.log("OPTIMAL DRIVER:");
    console.log(optimalDriver);
    return new Promise(resolve => {
        resolve(optimalDriver);
    });
}
async function addWaypointToVehicle(vehicleID, latitude, longitude) {
    console.log("--------------------- IN ADD WAYPOINT --------------------");

    let waypointCount = 0;
    let newRoute = {};

    if (displayedVehicles[vehicleID].route) {
        for (waypoint in displayedVehicles[vehicleID].route) {
            waypointCount++;
        }

        newRoute = displayedVehicles[vehicleID].route;
    }

    newRoute["waypoint" + waypointCount] = { "latitude": latitude, "longitude": longitude };
    let flag = await updateVehicleRoute(vehicleID, newRoute);

    return new Promise(resolve => {
        console.log("--------------------- OUT ADD WAYPOINT --------------------");
        resolve(flag);
    });
}
async function addDeliveryToVehicle(vehicleID, delivery) {
    console.log("--------------------- IN ADD DELIVERY TO VEH --------------------");

    let currentDeliveries = displayedVehicles[vehicleID].deliveries;
    if (currentDeliveries == null) currentDeliveries = [];
    currentDeliveries[delivery.deliveryID] = delivery;

    let flag = await updateVehicleRoute(vehicleID, currentDeliveries);

    return new Promise(resolve => {
        console.log("--------------------- OUT DELIVERY TO VEH --------------------");
        resolve(flag);
    });
}

async function addDelivery() {
    
        console.log("--------------------- IN ADD DELIVERY --------------------");
        const startLatitude = document.querySelector("#startLatitudeOptRouteInput").value;
        const startLongitude = document.querySelector("#startLongitudeOptRouteInput").value;
        const endLatitude = document.querySelector("#endLatitudeOptRouteInput").value;
        const endLongitude = document.querySelector("#endLongitudeOptRouteInput").value;
        var deliveryQuantity = "" + document.querySelector("#deliveryQuantity").value;
        const vehicleChoice = document.querySelector("#deliveryVehicleSelect").value;
        const clientID = document.querySelector("#clientInput").value;
  
        if (!isFinite(startLatitude) || !isFinite(startLongitude) || !isFinite(endLatitude) || !isFinite(endLongitude)) {
          
            return;
        }
    
        let optDriver = null;
        let vehicleID = null;
    if (vehicleChoice == 'auto') {
        
        optDriver = await findOptimalDriverForNewDelivery(startLatitude, startLongitude);
        
            vehicleID = optDriver.driverID;
        } else {
            optDriver = displayedVehicles[vehicleChoice];
            vehicleID = optDriver.vehicleID;
    }
    

    if (notificationDeliveryId > 0) {
        
      updateDeliveryInDb(vehicleID);
       
     
    }
    else {
        const ajaxPostRequestJSON = {
            vehicleID: `${vehicleID}`,
            driverID: `${displayedVehicles[vehicleID].currentDriver.driverID}`,
            clientID: `${clientID}`,
            startLatitude: `${startLatitude}`,
            startLongitude: `${startLongitude}`,
            endLatitude: `${endLatitude}`,
            endLongitude: `${endLongitude}`,
            quantity: `${deliveryQuantity}`,
            date: `${Date.now()}`,
            sourceCity: `${startCity}`,
            destinationCity: `${endCity}`

        };

        addDeliveryToDB(ajaxPostRequestJSON, vehicleID, startLatitude, startLongitude, endLatitude, endLongitude, deliveryQuantity);
    }
        const route = {
            "waypoint0": { "latitude": startLatitude, "longitude": startLongitude },
            "waypoint1": { "latitude": endLatitude, "longitude": endLongitude }
        }
        addRouteToMap(null, route);
    
}

async function addDeliveryToDB(ajaxPostRequestJSON, vehicleID, startLatitude, startLongitude, endLatitude, endLongitude, deliveryQuantity) {
   // console.log(ajaxPostRequestJSON);
    
    $.ajax({
        type: "POST",
        url: "/Map/AddDeliveryBySupervisor",
        data: ajaxPostRequestJSON,
        success: async function (result) {
    
            alert("Delivery Added \n with Id : " + result.result);

            console.log("AJAX ADD DELIVERY RESULT:");
            console.log(result);
            let client;
            clientsList.forEach(c => {
                if (c.ID == ajaxPostRequestJSON.clientID) {
                    client = c;
                    return;
                }
            })

            const delivery = {
                "deliveryID": result.result,
                "startLatitude": startLatitude,
                "startLongitude": startLongitude,
                "endLatitude": endLatitude,
                "endLongitude": endLongitude,
                "sourceCity": ajaxPostRequestJSON.sourceCity,
                "destinationCity": ajaxPostRequestJSON.destinationCity,
                "info": {
                    "customerName": client.Name,
                    "customerEmail": client.Email,
                    "deliveryQuantity": deliveryQuantity,
                    "orderTime": date
                }
            }

            let flag = await addDeliveryToVehicle(vehicleID, delivery);

            if (flag) {
                showVehicleDetails(vehicleID);
                unsetSelectors();
            }
        }
    });


   


}

async function cancelDelivery(vehicleID, deliveryID) {
    console.log("--------------------- IN CANCEL DELIVERY --------------------");
    deliveries = displayedVehicles[vehicleID].deliveries;
    let newDeliveries = [];
    deliveries.forEach(d => {
        if (d.deliveryID != deliveryID) {
            newDeliveries.push(d);
        }
    });
    delete deliveries[deliveryID];
    if (arrayLength(deliveries) == 0) {
        deliveries = [];
    }

    //TODO
    ///////////// AJAX remove delivery from DB /////////////
    const ajaxPostRequestJSON = {
        vehicleID: `${vehicleID}`,
        deliveryID: `${deliveryID}`
    }

    $.ajax({
        type: "POST",
        url: "/Map/CancelDelivery",
        data: ajaxPostRequestJSON,

        success: async function (result) {
            let flag = await updateVehicleRoute(vehicleID, newDeliveries);
            if (flag) showVehicleDetails(vehicleID);
            console.log("--------------------- OUT CANCEL DELIVERY --------------------");
        }
    });
    ///////////////////////////////////////////////////////

}

function finishDelivery(vehicleID, deliveryID) {
    console.log("---- IN FINISH DELIVERY ----");
    deliveries = displayedVehicles[vehicleID].deliveries;
    let newDeliveries = [];
    deliveries.forEach(d => {
        if (d.deliveryID != deliveryID) {
            newDeliveries.push(d);
        }
    });
    delete deliveries[deliveryID];
    if (arrayLength(deliveries) == 0) {
        deliveries = null;
    }

    updateVehicleRoute(vehicleID, newDeliveries);
}

function finishDeliveryClicked() {
    const deliveryID = parseInt(document.querySelector("#finishDeliveryIDInput").value);
    const vehicleID = parseInt(document.querySelector("#finishVehicleIDInput").value);

    finishDelivery(vehicleID, deliveryID);
}

function logVehicles() {
    console.log(displayedVehicles);
}

function arrayLength(array) {
    length = 0;
    for (item in array) {
        if (item != null && typeof item != "undefined") length++;
    }

    return length;
}

function focusMap(latitude, longitude) {
    map.setCenter({ lat: latitude, lng: longitude }, true);
}

function unsetSelectors() {
    try {
        map.removeObject(startPicker.marker);
        startPicker = [];
    } catch (e) { }

    try {
        map.removeObject(endPicker.marker);
        endPicker = [];
    } catch (e) { }

    document.querySelector("#startLatitudeOptRouteInput").value = "";
    document.querySelector("#startLongitudeOptRouteInput").value = "";
    document.querySelector("#endLatitudeOptRouteInput").value = "";
    document.querySelector("#endLongitudeOptRouteInput").value = "";
}

function showVehicleDetails(vehicleID) {
    console.log("--------------------- IN SHOW VEHICLE DETAILS --------------------");
    console.log("Show vehicle details:");
    console.log(vehicleID);

    const vehicle = displayedVehicles[vehicleID];

    const list = document.querySelector("#driver-details-list");
    const driver = vehicle.currentDriver;
    const deliveries = vehicle.deliveries;

    list.innerHTML = "";
    let inner = "";

    // Driver ID
    inner += '<tr><td>Driver ID</td><td>' + driver.driverID + '</td></tr>';

    // Driver personal info
    inner += '<tr><td colspan=2><h4>Driver Personal Details</h4></td><tr>';
    inner += '<tr><td>Name</td><td>' + driver.name + '</td></tr>';
    inner += '<tr><td>Phone Number</td><td>' + driver.phone + '</td></tr>';
    inner += '<tr><td>Email</td><td>' + driver.email + '</td></tr>';
    inner += '<tr><td>Birth Date</td><td>' + driver.birthdate + '</td></tr>';

    inner += '<tr><td colspan=2><h4>Vehicle Details</h4></td><tr>';

    // Vehicle ID
    inner += '<tr><td>Vehicle ID</td><td>' + vehicle.vehicleID + '</td></tr>';
    inner += '<tr><td>Vehicle Model</td><td>' + vehicle.info.model + '</td></tr>';
    inner += '<tr><td>Manufactured Year</td><td>' + vehicle.info.year + '</td></tr>';
    inner += '<tr><td>Plate Number</td><td>' + vehicle.info.plateNo + '</td></tr>';

    // Deliveries
    inner += '<tr><td colspan=2><h3>Current Deliveries</h3></td><tr>';
    inner += '<tr><td colspan=2>';

    if (deliveries == null || arrayLength(deliveries) == 0) {
        inner += "None";
    } else {
        inner += '<ul class="list-group">';
        for (delivery in deliveries) {
            let del = deliveries[delivery];

            inner += '<li class="list-group-item well">';

            inner += '<table class="table" border=2>';
            inner += '<tr><td>Delivery ID</td><td>' + del.deliveryID + '</td></tr>';
            inner += '<tr><td>From -> To</td><td>' + del.sourceCity + ' -> ' + del.destinationCity + '</td></tr>';
            inner += '<tr><td>Quantity</td><td>' + del.info.quantity + '</td></tr>';
            inner += '<tr><td>Customer Name</td><td>' + del.info.customerName + '</td></tr>';
            inner += '<tr><td>Customer Email</td><td>' + del.info.customerEmail + '</td></tr>';
            inner += '<tr><td>Ordered at</td><td>' + del.info.orderTime + '</td></tr>';
            inner += '<tr><td colspan=2><button onclick="cancelDelivery(' + vehicle.vehicleID + ',' + del.deliveryID + ')" class="btn btn-danger">Cancle Delivery</button></td></tr>';
            inner += '</table>';

            inner += '</li>';
        }
        inner += '</ul>';
    }

    inner += '</td><tr>';

    list.innerHTML = inner;

    ///////// SIDEPANE /////////
    const sidepane = document.querySelector("#driver-details-sidepane");

    let inner2 = "";
    sidepane.innerHTML = "";

    inner2 += '<ul class="list-group">';

    inner2 += '<li class="list-group-item">';
    inner2 += '<h2>Contact & Stats</h2>';
    inner2 += '</li>';

    inner2 += '<li class="list-group-item">';
    inner2 += '<h3>' + driver.name + '</h3>';
    inner2 += '</li>';

    inner2 += '<li class="list-group-item">';
    inner2 += '<img width="100px" src="' + driver.avatar + '" />';
    inner2 += '</li>';

    inner2 += '<li class="list-group-item">';
    inner2 += '<a class="btn btn-success btn-block">Send Email</a>';
    inner2 += '</li>';

    inner2 += '<li class="list-group-item">';
    inner2 += '<a class="btn btn-success btn-block">Check Driver Stats</a>';
    inner2 += '</li>';

    inner2 += '<li class="list-group-item">';
    inner2 += '<a class="btn btn-success btn-block">Check Driver History</a>';
    inner2 += '</li>';
    inner2 += '</ul>';

    sidepane.innerHTML = inner2;
    console.log("--------------------- OUT SHOW VEHICLE DETAILS --------------------");
}

function fillVehicleList(vehicles) {
    const list = document.querySelector("#vehicles-list");
    list.innerHTML = "";

    let inner = "";

    for (vehicle in vehicles) {
        let veh = vehicles[vehicle];

        inner += '<li class="list-group-item" id="vehicle-list-item">';
        inner += '<div class="row">';

        inner += '<div class="col-lg-3">';
        inner += '<img width="70px" src="' + veh.currentDriver.avatar + '" />';
        inner += '</div>';

        inner += '<div class="col-lg-9">';
        inner += 'Driver Name: ' + veh.currentDriver.name + '<br>';
        inner += 'Driver ID: ' + veh.currentDriver.driverID + '<br>';
        inner += 'Vehicle ID: ' + veh.vehicleID + '<br>';
        inner += '<div class="btn-group">';
        inner += '<button onclick="focusMap(' + veh.latitude + ',' + veh.longitude + ')" class="btn btn-primary btn-sm" style="margin-top:8px;margin-right:5px;margin-left:10px;">Show in Map</button>';
        inner += '<button onclick="showVehicleDetails(' + veh.vehicleID + ')" class="btn btn-primary btn-sm" style="margin-top:8px">Details</button>';
        inner += '</div>';
        inner += '</div>';

        inner += '</div>';
        inner += "</li>";
    }

    list.innerHTML = inner;
}


function getVehiclesSM() {
    let result = [];

    for (vehicle in displayedVehicles) {
        let v = displayedVehicles[vehicle];

        result.push({ "driverName": v.currentDriver.name, "vehicleID": v.vehicleID });
    }

    return result;
}

const mapDOM = document.querySelector("#mapContainer");

function createNameTag(id, text) {
    let span = document.createElement("span");
    span.appendChild(document.createTextNode(text));;
    span.id = id;
    span.style.top = 0 + 'px';
    span.style.left = 0 + 'px';
    span.style.zIndex = "10";
    span.style.position = "absolute";
    //span.style.backgroundColor = "#000000";
    span.style.fontSize = "12px";
    span.style.fontWeight = "bold";
    span.style.color = "#000";
    span.style.visibility = "hidden";
    document.querySelector("body").appendChild(span);
    console.log(span);
}

function fixNameTagPositions() {
    for (vehicle in displayedVehicles) {
        let veh = displayedVehicles[vehicle];
        let pos = map.geoToScreen({ lat: veh.latitude, lng: veh.longitude });

        updateNameTagPosition('tag' + veh.vehicleID, pos.x, pos.y);
    }

    for (center in displayedCenters) {
        let cen = displayedCenters[center];
        let pos = map.geoToScreen({ lat: cen.latitude, lng: cen.longitude });

        updateNameTagPosition('ctag' + cen.centerID, pos.x, pos.y);
    }
}

function updateNameTagPosition(id, x, y) {
    let span = document.querySelector(`#${id}`);

    if (isInsideMap(x, y)) {
        span.style.visibility = "visible";
        span.style.top = (y +60) + 'px';
        span.style.left = (x-40) + 'px';
    }
    else {
        span.style.visibility = "hidden";
        span.style.top = 0;
        span.style.left = 0;
    }


}

function isInsideMap(x, y) {
    let mapX1 = mapDOM.clientLeft;
    let mapX2 = mapX1 + mapDOM.clientWidth;

    let mapY1 = mapDOM.clientTop + 60;
    let mapY2 = mapY1 + mapDOM.clientHeight;

    if (x > mapX1 && x < mapX2 && y > mapY1 && y < mapY2) return true;
    else return false;
}

    function addVehicles(vehicles) {

    }

async function refetchAndRefresh() {
    
    //TODO
    ///////////// AJAX get vehicles from DB /////////////
    const ajaxPostRequestJSON = {
        isActive: true
    }

    $.ajax({
        type: "POST",
        url: "/Map/GetVehicles",
        data: ajaxPostRequestJSON,
        error: () => { console.log("Failed to get update") },
        success: async function (result) {
           // console.log(JSON.parse(result.result));

            ////// Steps 2 -> 5 HERE /////
            const vList = JSON.parse(result.result);
            for (veh in vList) { // Step 2
                // Step 3
                let v = vList[veh];
                if (displayedVehicles[v.Id]) { // This vehicle is currently displayed, update it
                    let flag = await updateVehicleInMap(v.Id, v.Latitude, v.Longtitude);
                    if (flag) await updateVehicleDeliveries(v.Id, v.Deliveries);
                }
                else { // This vehicle is not currently displayed, add it
                    let vDelList = [];

                    if (!(v.Deliveries == null) && arrayLength(v.Deliveries) > 0) {
                        for (del in v.Deliveries) {
                            let d = v.Deliveries[del];

                            vDelList.push({
                                "deliveryID": `${d.deliveryID}`,
                                "startLatitude": `${d.SourceLatitude}`,
                                "startLongitude": `${d.SourceLongtitude}`,
                                "endLatitude": `${d.DestinationLatitude}`,
                                "endLongitude": `${d.DestinationLongtitude}`,
                                "sourceCity": `${d.SourceCity}`,
                                "destinationCity": `${d.DestinationCity}`,
                                "info": {
                                    "customerName": `${d.info.customerName}`,
                                    "customerEmail": `${d.info.customerEmail}`,
                                    "quantity": `${d.info.quantity}`,
                                    "orderTime": `${d.info.orderTime}`
                                }
                            });

                        }

                    }

                    let flag = await addVehicleToMap(v.Id, v.Latitude, v.Longtitude,
                        {
                            "model": `${v.Make}`,
                            "year": `${v.Model}`,
                            "plateNo": `${v.LicensePlate}`
                        },
                        {
                            "driverID": `${v.CurrentDriverId}`,
                            "name": `${v.CurrentDriverName}`,
                            "avatar": "/images/avatar1.jpg",
                            "phone": `${v.CurrentDriverPhonenumber}`,
                            "email": "In DB but not attributed to Driver table",
                            "birthdate": `${v.CurrentDriverBirthdate}`
                        },
                        vDelList
                    );
                    if (flag) await fillVehicleList(displayedVehicles);
                }
            }


            for (veh in displayedVehicles) { // Step 4
                // Step 5
                let v = displayedVehicles[veh];

                let active = false;
                for (veh2 in vList) {
                    let v2 = vList[veh2];

                    if (v.vehicleID == v2.Id) { active = true; break; }
                }
                if (!active) {
                    removeRouteFromMap(v.vehicleID);
                    const flag = await removeVehicleFromMap(v.vehicleID);
                    if (flag) fillVehicleList(displayedVehicles);
                }
            }

            /////////////////////////////
        }
    });
    ///////////////////////////////////////////////////////
    
}

function updateVehicleDeliveries(vehicleID, deliveries) {
    // Remove any finished deliveries
    console.log("---- IN UPDATE VEHICLE DELIVERIES ----");
    for (del in displayedVehicles[vehicleID].deliveries) {
        let d = displayedVehicles[vehicleID].deliveries[del];
        console.log("D:");
        console.log(d);

        let active = false;
        for (del2 in deliveries) {
            let d2 = deliveries[del2];

            console.log("js == c# ?");
            console.log(d.deliveryID + " == " + d2.deliveryID + " ? ");
            if (d.deliveryID == d2.deliveryID) { active = true; break; }
        }
        if (!active) {
            finishDelivery(vehicleID, d.deliveryID);
        }
    }

    // Add any new deliveries
    for (del in deliveries) {
        let d = deliveries[del];

        let shown = false;
        for (del2 in displayedVehicles[vehicleID].deliveries) {
            let d2 = displayedVehicles[vehicleID].deliveries[del2];

            if (d2.deliveryID == d.deliveryID) { shown = true; break; }
        }
        if (!shown) {
            let newDelivery = {
                "deliveryID": `${d.deliveryID}`,
                "startLatitude": `${d.SourceLatitude}`,
                "startLongitude": `${d.SourceLongtitude}`,
                "endLatitude": `${d.DestinationLatitude}`,
                "endLongitude": `${d.DestinationLongtitude}`,
                "sourceCity": `${d.SourceCity}`,
                "destinationCity": `${d.DestinationCity}`,
                "info": {
                    "customerName": `${d.info.customerName}`,
                    "customerEmail": `${d.info.customerEmail}`,
                    "quantity": `${d.info.quantity}`,
                    "orderTime": `${d.info.orderTime}`
                }
            };
            addDeliveryToVehicle(vehicleID, newDelivery);
        }
    }
}


    const shift = 0.0002;



function setStart1(latitude,longitude) {
   

    // Create a start marker icon from an image URL for the start position
    const icon = new H.map.Icon('/images/start.png');
    const marker = new H.map.Marker({ lat:latitude, lng: longitude }, { icon: icon });
    map.addObject(marker);

    // Save to startPicker object
    startPicker.latitude = latitude;
    startPicker.longitude =longitude;
    startPicker.marker = marker;

    // Set values to input boxes in the html page (for testing only probably)
    document.querySelector("#startLatitudeOptRouteInput").value = latitude;
    document.querySelector("#startLongitudeOptRouteInput").value = longitude;

   


}

function setEnd1(latitude, longitude) {
    // Check if an icon already exists to remove it
   

    // Create a start marker icon from an image URL for the start position
    const icon = new H.map.Icon('/images/end.png');
    const marker = new H.map.Marker({ lat: latitude, lng: longitude }, { icon: icon });
    map.addObject(marker);

    // Save to startPicker object
    startPicker.latitude = latitude;
    startPicker.longitude = longitude;
    startPicker.marker = marker;

    // Set values to input boxes in the html page (for testing only probably)
    document.querySelector("#endLatitudeOptRouteInput").value = latitude;
    document.querySelector("#endLongitudeOptRouteInput").value = longitude;




}

function showCentersSidepane() {
    const centersListDOM = document.querySelector("#centers-list");

    centersListDOM.innerHTML = "";

    let inner = "";

    for (center in displayedCenters) {
        let c = displayedCenters[center];

        inner += '<li class="list-group-item" id="center-list-item">';
        inner += '<div class="row">';

        inner += '<div class="col-lg-3">';
        inner += '<img width="70px" src="/images/' + c.image + '" />';
        inner += '</div>';

        inner += '<div class="col-lg-9">';
        inner += 'Center Name: ' + c.centerName + '<br>';
        inner += 'Center ID: ' + c.centerID + '<br>';
        inner += '<div class="btn-group">';
        inner += '<button onclick="focusMap(' + c.latitude + ',' + c.longitude + ')" class="btn btn-primary btn-sm"  style="margin-top:8px;margin-right:5px;margin-left:10px;">Show in Map</button>';
        inner += '<br><button onclick="setStart({\'lat\':\'' + c.latitude + '\', \'lng\':\'' + c.longitude + '\'})" class="btn btn-primary btn-sm"  style="margin-top:8px;margin-right:5px;margin-left:10px;" >Set as start</button>';
        inner += '<button onclick="setEnd({\'lat\':\'' + c.latitude + '\', \'lng\':\'' + c.longitude + '\'})" class="btn btn-primary btn-sm"  style="margin-top:8px" >Set as end</button>';
        inner += '</div>';
        inner += '</div>';

        inner += '</div>';
        inner += "</li>";
    }

    centersListDOM.innerHTML = inner;
}


function addRouteToMap1() {
    
    var routingParameters = {
        // The routing mode:
        'mode': 'fastest;car',
        // The start point of the route:
        'waypoint0': 'geo!33.832,35.5129893',
        // The end point of the route:
        'waypoint1': 'geo!33.54139,35.37048',
        // To retrieve the shape of the route we choose the route
        // representation mode 'display'
        'representation': 'display'
    };

    // Define a callback function to process the routing response:
    var onResult = function (result) {
        var route,
            routeShape,
            startPoint,
            endPoint,
            linestring;
        if (result.response.route) {
            // Pick the first route from the response:
            route = result.response.route[0];
            // Pick the route's shape:
            routeShape = route.shape;

            // Create a linestring to use as a point source for the route line
            linestring = new H.geo.LineString();

            // Push all the points in the shape into the linestring:
            routeShape.forEach(function (point) {
                var parts = point.split(',');
                linestring.pushLatLngAlt(parts[0], parts[1]);
            });

            // Retrieve the mapped positions of the requested waypoints:
            startPoint = route.waypoint[0].mappedPosition;
            endPoint = route.waypoint[1].mappedPosition;

            // Create a polyline to display the route:
            var routeLine = new H.map.Polyline(linestring, {
                style: { strokeColor: 'blue', lineWidth: 10 },
                arrows: { fillColor: 'white', frequency: 2, width: 0.8, length: 0.7 }
            });


            // Create a marker for the start point:
            var startMarker = new H.map.Marker({
                lat: startPoint.latitude,
                lng: startPoint.longitude
            });

            // Create a marker for the end point:
            var endMarker = new H.map.Marker({
                lat: endPoint.latitude,
                lng: endPoint.longitude
            });

            // Add the route polyline and the two markers to the map:
            map.addObjects([routeLine, startMarker, endMarker]);

            // Set the map's viewport to make the whole route visible:
            map.setViewBounds(routeLine.getBounds());
        }
    };

    // Get an instance of the routing service:
    var router = platform.getRoutingService();

    // Call calculateRoute() with the routing parameters,
    // the callback and an error callback function (called if a
    // communication error occurs):
    router.calculateRoute(routingParameters, onResult,
        function (error) {
            alert(error.message);
        });

}





