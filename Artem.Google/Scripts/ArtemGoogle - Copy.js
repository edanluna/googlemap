///<reference name="MicrosoftAjax.js"/>

// ------------------------------------------------------------------------------------------------
// Copyright (C) ArtemBG.
// ------------------------------------------------------------------------------------------------
// GoogleMap4.debug.js
// GoogleMap Control v4.5 javascipt library (debug).
//
// Assembly:    Artem.GooleMap
// Version:     4.5.0.0
// Project:     http://googlemap.codeplex.com
// Demo:        http://googlemap.artembg.com
// Author:      Velio Ivanov - velio@artembg.com
//              http://artembg.com
// License:     Microsoft Permissive License (Ms-PL) v1.1
//              http://www.codeplex.com/googlemap/license
// API:         http://code.google.com/apis/maps/

Type.registerNamespace("Artem.Google");

//#region Map class 

Artem.Google.Map = function Artem_Google_Map(clientState) {
    /// <summary>This class represents the client GoogleMap control object.</summary>
    /// <param name="clientState" type="Artem.Google.MapState"></param>
    this._init(clientState);
};

Artem.Google.Map.prototype = {

    //#region Fields 
    Address: null,
    AddressNotFound: false,
    BaseCountryCode: null,
    ClientID: null,
    ClientMapID: null,
    ClientStateID: null,
    DefaultAddress: null,
    DefaultMapView: null,
    EnableContinuousZoom: null,
    EnableDoubleClickZoom: null,
    EnableDragging: null,
    EnableGoogleBar: null,
    EnableInfoWindow: null,
    EnableMarkerManager: null,
    EnableReverseGeocoding: null,
    EnableScrollWheelZoom: null,
    EnterpriseKey: null,
    Height: null,
    IsGeolocation: false,
    IsLoaded: false,
    IsStatic: null,
    IsStreetView: null,
    Key: null,
    Latitude: null,
    Longitude: null,
    MarkerManagerOptions: null,
    ShowMapTypeControl: null,
    ShowScaleControl: null,
    ShowTraffic: null,
    StreetViewMode: null,
    StreetViewPanoID: null,
    Width: null,
    Zoom: null,
    ZoomPanType: null,
    // events
    ClientEvents: null,
    ServerEvents: null,
    MarkerEvents: null,
    PolygonEvents: null,
    PolylineEvents: null,
    ClentAddressNotFoundIndex: null,
    ServerAddressNotFoundIndex: null,
    ClientGeoLoadedIndex: null,
    ServerGeoLoadedIndex: null,
    ClientLocationLoadedIndex: null,
    ServerLocationLoadedIndex: null,
    // collections
    Actions: [],
    Directions: [],
    Markers: [],
    Polygons: [],
    Polylines: [],
    // origin
    GMap: null,
    GMapPano: null,
    MarkerManager: null,
    // geocoder
    Geocoder: null,
    //#endregion

    //#region Private Methods 

    _init: function Artem_Google_Map$_init(config) {

        // properties
        this.Address = config.Address;
        this.BaseCountryCode = config.BaseCountryCode;
        this.DefaultAddress = config.DefaultAddress;
        this.DefaultMapView = config.DefaultMapView;
        this.ClientID = config.ClientID;
        this.ClientMapID = config.ClientMapID;
        this.ClientStateID = config.ClientStateID;
        this.IsStatic = config.IsStatic;
        this.IsStreetView = config.IsStreetView;
        this.EnterpriseKey = config.EnterpriseKey;
        this.Key = config.Key;
        this.Latitude = config.Latitude;
        this.Longitude = config.Longitude;
        this.ShowMapTypeControl = config.ShowMapTypeControl;
        this.ShowScaleControl = config.ShowScaleControl;
        this.ShowTraffic = config.ShowTraffic;
        this.StreetViewMode = config.StreetViewMode;
        this.StreetViewPanoID = config.StreetViewPanoID;
        this.Zoom = config.Zoom;
        this.ZoomPanType = config.ZoomPanType;
        // 
        this.Width = config.Width;
        this.Height = config.Height;
        // behaviour
        this.EnableContinuousZoom = config.EnableContinuousZoom;
        this.EnableDoubleClickZoom = config.EnableDoubleClickZoom;
        this.EnableDragging = config.EnableDragging;
        this.EnableGoogleBar = config.EnableGoogleBar;
        this.EnableInfoWindow = config.EnableInfoWindow;
        this.EnableMarkerManager = config.EnableMarkerManager;
        this.EnableReverseGeocoding = config.EnableReverseGeocoding;
        this.EnableScrollWheelZoom = config.EnableScrollWheelZoom;
        // events
        if (config.MapEvents) {
            this.ClientEvents = config.MapEvents.ClientEvents;
            this.ServerEvents = config.MapEvents.ServerEvents;
        }
        this.MarkerEvents = config.MarkerEvents;
        this.PolygonEvents = config.PolygonEvents;
        this.PolylineEvents = config.PolylineEvents;
        // geocoder
        this.Geocoder = new GClientGeocoder();
        if (config.BaseCountryCode)
            this.Geocoder.setBaseCountryCode(config.BaseCountryCode);
        // initialize manager
        Artem.Google.Manager.initialize();
        Artem.Google.Manager.addMap(this);
        // initialize
        if (!this.IsStatic && !(this.IsStreetView && this.StreetViewMode == 0)) {
            var options;
            if (this.Width && this.Height)
                options = { size: new GSize(this.Width, this.Height) };
            this.GMap = new GMap2(this.getElement(), options);
            //events
            this.attachEvents(this.ClientEvents, true);
            this.attachEvents(this.ServerEvents, false);
        }
    },

    _renderMarkerManager: function Artem_Google_Map$_renderMarkerManager() {
        if (this.EnableMarkerManager) {
            var marker;
            for (var i = 0; i < this.Markers.length; i++) {
                marker = this.Markers[i];
                this.MarkerManager.addMarker(marker.GMarker, marker.MinZoom, marker.MaxZoom);
            }
            this.MarkerManager.refresh();
        }
    },
    //#endregion

    //#region ClientState

    destroy: function () {
    },

    loadState: function () {
    },

    saveState: function Artem_Google_Map$saveState() {
        var state = "{";
        state += "\"Address\":\"" + this.Address + "\"";
        state += ",\"BaseCountryCode\":\"" + this.BaseCountryCode + "\"";
        state += ",\"DefaultMapView\":" + Artem.Google.MapView.convert(this.getCurrentMapType()); // this.DefaultMapView;
        state += ",\"EnableGoogleBar\":" + this.EnableGoogleBar;
        state += ",\"EnableMarkerManager\":" + this.EnableMarkerManager;
        state += ",\"EnableScrollWheelZoom\":" + this.EnableScrollWheelZoom;
        state += ",\"IsStatic\":" + this.IsStatic;
        var center = this.getCenter();
        if (center) {
            state += ",\"Latitude\":" + center.lat();
            state += ",\"Longitude\":" + center.lng();
        }
        else {
            state += ",\"Latitude\":" + this.Latitude;
            state += ",\"Longitude\":" + this.Longitude;
        }
        state += ",\"ShowMapTypeControl\":" + this.ShowMapTypeControl;
        state += ",\"ShowTraffic\":" + this.ShowTraffic;
        state += ",\"Zoom\":" + this.getZoom(); //this.Zoom;
        state += ",\"ZoomPanType\":" + this.ZoomPanType;
        // bounds
        var bounds = new Artem.Google.Bounds(this.getBounds());
        state += ",\"Bounds\":";
        state += bounds.saveState();
        // markers
        if (this.Markers) {
            state += ",\"Markers\":[";
            for (var i = 0; i < this.Markers.length; i++) {
                if (this.Markers[i].isLoaded())
                    state += this.Markers[i].saveState();
            }
            state += "]";
        }
        // directions
        if (this.Directions) {
            state += ",\"Directions\":[";
            for (var i = 0; i < this.Directions.length; i++) {
                state += this.Directions[i].saveState();
            }
            state += "]";
        }
        // polylines
        if (this.Polylines) {
            state += ",\"Polylines\":[";
            for (var i = 0; i < this.Polylines.length; i++) {
                state += this.Polylines[i].saveState();
            }
            state += "]";
        }
        // polygons
        if (this.Polygons) {
            state += ",\"Polygons\":[";
            for (var i = 0; i < this.Polygons.length; i++) {
                state += this.Polygons[i].saveState();
            }
            state += "]";
        }
        //
        state += "}";
        var bag = document.getElementById(this.ClientStateID);
        bag.value = state;
    },
    //#endregion

    //#region Public Methods 

    addAction: function Artem_Google_Map$addAction(action) {
        if (!this.Actions) this.Actions = new Array();
        this.Actions.push(action);
    },

    addDirection: function Artem_Google_Map$addDirection(config, render) {
        if (!this.Directions) this.Directions = new Array();
        var dir = new Artem.Google.Direction(this, config);
        this.Directions.push(dir);
        if (render) this.renderDirection(dir);
    },

    addMarker: function Artem_Google_Map$addMarker(config, render) {
        var marker = new Artem.Google.Marker(this, this.Markers.length, config);
        this.Markers.push(marker);
        if (render) this.renderMarker(marker);
    },

    addPolygon: function Artem_Google_Map$addPolygon(config, render) {
        if (!this.Polygons) this.Polygons = new Array();
        var polygon = new Artem.Google.Polygon(this, this.Polygons.length, config);
        this.Polygons.push(polygon);
        if (render) this.renderPolygon(polygon);
    },

    addPolyline: function Artem_Google_Map$addPolyline(config, render) {
        if (!this.Polylines) this.Polylines = new Array();
        var polyline = new Artem.Google.Polyline(this, this.Polylines.length, config);
        this.Polylines.push(polyline);
        if (render) this.renderPolyline(polyline);
    },

    attachEvents: function Artem_Google_Map$attachEvents(events, clients) {
        if (events) {
            var key;
            for (var i = 0; i < events.length; i++) {
                key = events[i].Key;
                if (key != 'geoload' && key != 'addressnotfound' && key != 'locationloaded') {
                    if (clients) {
                        GEvent.addListener(this.GMap, key,
                            Function.Delegate.createFromString(this, events[i].Handler));
                    }
                    else {
                        var handler = events[i].Handler;
                        var delegate = Function.Delegate.create(this, this.raiseEvent);
                        GEvent.addListener(this.GMap, key, function (overlay, args) {
                            delegate.call(this, handler, args);
                        });
                    }
                }
                else if (key == 'locationloaded') {
                    if (clients)
                        this.ClientLocationLoadedIndex = i;
                    else
                        this.ServerLocationLoadedIndex = i;
                }
                else if (key == 'addressnotfound') {
                    if (clients)
                        this.ClientAddressNotFoundIndex = i;
                    else
                        this.ServerAddressNotFoundIndex = i;
                }
                else {
                    if (clients)
                        this.ClientGeoLoadedIndex = i;
                    else
                        this.ServerGeoLoadedIndex = i;
                }
            }
        }
    },

    clearActions: function Artem_Google_Map$clearActions(action) {
        if (this.Actions) this.Actions = new Array();
    },

    clearDirections: function Artem_Google_Map$clearDirections(config) {
        if (this.Directions) this.Directions = new Array();
    },

    clearMarkers: function Artem_Google_Map$clearMarkers() {
        if (this.Markers) {
            var len = this.Markers.length;
            for (var i = 0; i < len; i++) {
                this.GMap.removeOverlay(this.Markers[i].GMarker);
            }
            this.Markers = new Array();
        }
    },

    clearPolygons: function Artem_Google_Map$clearPolygons(config) {
        if (this.Polygons) this.Polygons = new Array();
    },

    clearPolylines: function Artem_Google_Map$clearPolylines(config) {
        if (this.Polylines) this.Polylines = new Array();
    },

    getElement: function Artem_Google_Map$getElement() {
        return document.getElementById(this.ClientID);
    },

    initialize: function Artem_Google_Map$initialize() {
        // behaviour
        (this.EnableContinuousZoom) ? this.enableContinuousZoom() : this.disableContinuousZoom();
        (this.EnableDoubleClickZoom) ? this.enableDoubleClickZoom() : this.disableDoubleClickZoom();
        (this.EnableDragging) ? this.enableDragging() : this.disableDragging();
        (this.EnableGoogleBar) ? this.enableGoogleBar() : this.disableGoogleBar();
        (this.EnableInfoWindow) ? this.enableInfoWindow() : this.disableInfoWindow();
        (this.EnableScrollWheelZoom) ? this.enableScrollWheelZoom() : this.disableScrollWheelZoom();
        // controls
        switch (this.ZoomPanType) {
            case 1:
                this.GMap.addControl(new GLargeMapControl());
                break;
            case 2:
                this.GMap.addControl(new GSmallZoomControl());
                break;
            case 3:
                this.GMap.addControl(new GSmallZoomControl3D());
                break;
            case 4:
                this.GMap.addControl(new GLargeMapControl3D());
                break;
            default:
                this.GMap.addControl(new GSmallMapControl());
                break;
        }
        if (this.ShowMapTypeControl) this.GMap.addControl(new GMapTypeControl());
        if (this.ShowScaleControl) this.GMap.addControl(new GScaleControl());
        if (this.ShowTraffic) this.GMap.addOverlay(new GTrafficOverlay());
        // enable marker manager
        if (this.EnableMarkerManager) this.MarkerManager = new MarkerManager(this.GMap, this.MarkerManagerOptions);
        //        //events
        //        this.attachEvents(this.ClientEvents, true);
        //        this.attachEvents(this.ServerEvents, false);
        // map view
        this.setMapView();
        // street view
        if (this.IsStreetView && this.StreetViewMode == 1) {
            var panoID = this.StreetViewPanoID || (this.ClientID + "_Pano");
            this.GMapPano = new GStreetviewPanorama(document.getElementById(panoID));
            this.GMap.addOverlay(new GStreetviewOverlay());
            GEvent.addListener(this.GMap, "click", Function.Delegate.create(this, this.setStreetView));
            //            function(overlay, latlng) {
            //                pano.setLocationAndPOV(latlng);
            //            });
        }
    },

    load: function Artem_Google_Map$load(point) {
        if (point) {
            if (!this.IsStatic && !(this.IsStreetView && this.StreetViewMode == 0)) {
                this.Latitude = point.lat();
                this.Longitude = point.lng();
                this.setCenter(point, this.Zoom);
                if (this.IsGeolocation) {
                    this.IsGeolocation = false;
                    if (this.ClientGeoLoadedIndex != null)
                        Function.Delegate.callFromString(this, this.ClientEvents[this.ClientGeoLoadedIndex].Handler);
                    if (this.ServerGeoLoadedIndex != null) {
                        var handler = this.ServerEvents[this.ServerGeoLoadedIndex].Handler;
                        handler = handler.replace("ARGS", this.Address);
                        Function.Delegate.callFromString(this, handler);
                    }
                }
                if (this.EnableReverseGeocoding && !this.Address) {
                    var delegate = Function.Delegate.create(this, this.setAddress);
                    this.Geocoder.getLocations(point, delegate);
                }
                this.initialize();
                this.render();
                this.checkResize();
            }
            else if (this.IsStreetView) {
                this.loadStreetView(point);
            }
            else {
                this.loadStatic();
            }
            this.IsLoaded = true;
        }
        else {
            if ((this.Latitude != 0) && (this.Longitude != 0))
                this.load(new GLatLng(this.Latitude, this.Longitude));
            else {
                if (!this.IsGeolocation) {
                    this.IsGeolocation = true;
                    this.Geocoder.getLatLng(this.Address, Function.Delegate.create(this, this.load));
                }
                else if (!this.AddressNotFound) {
                    if (this.ClientAddressNotFoundIndex != null)
                        Function.Delegate.callFromString(this, this.ClientEvents[this.ClientAddressNotFoundIndex].Handler);
                    if (this.ServerAddressNotFoundIndex != null) {
                        var handler = this.ServerEvents[this.ServerAddressNotFoundIndex].Handler;
                        handler = handler.replace("ARGS", this.Address);
                        Function.Delegate.callFromString(this, handler);
                    }
                    this.AddressNotFound = true;
                    if (this.DefaultAddress) {
                        this.Address = this.DefaultAddress;
                        this.Geocoder.getLatLng(this.Address, Function.Delegate.create(this, this.load));
                    }
                }
            }
        }
    },

    loadAddress: function Artem_Google_Map$loadAddress(address) {
        this.Address = address;
        this.IsGeolocation = true;
        this.Geocoder.getLatLng(this.Address, Function.Delegate.create(this, this.load));
    },

    loadStatic: function Artem_Google_Map$loadStatic() {
        var el = this.getElement();
        //
        var width = 512;
        if (this.Didth && this.Width < 512) width = this.Width;
        var height = 512;
        if (this.Height && this.Height < 512) height = this.Height;
        //
        var src = "http:\/\/maps.google.com\/staticmap?";
        src += "center=" + this.Latitude + "," + this.Longitude + "&";
        src += "zoom=" + this.Zoom + "&";
        src += "size=" + width + "x" + height + "&";
        if (this.EnterpriseKey)
            src += "enterpriseKey=" + this.EnterpriseKey + "&";
        src += "key=" + this.Key;
        // markers
        if (this.Markers) {
            var i;
            src += "&markers=";
            for (i = 0; i < this.Markers.length; i++)
                src += this.Markers[i].Latitude + "," + this.Markers[i].Longitude + "|";
        }
        // 
        var img = document.createElement("img");
        img.src = src;
        el.appendChild(img);
    },

    loadStreetView: function Artem_Google_Map$loadStreetView(point) {
        this.GMap = new GStreetviewPanorama(this.getElement(), { latlng: point });
        this.GMap.checkResize();
        //        GEvent.addListener(this.GMapPano, "error", function() {
        //            if (errorCode == 603) {
        //                alert("Error: Flash doesn't appear to be supported by your browser");
        //                return;
        //            }
        //        });
    },

    raiseEvent: function Artem_Google_Map$raiseEvent(handler, args) {
        if (this.IsLoaded) {
            this.saveState();
            if (handler) {
                if (args)
                    handler = handler.replace("ARGS", args);
                eval(handler);
            }
        }
    },

    render: function Artem_Google_Map$render() {
        // markers
        if (this.Markers) {
            var loader = new Artem.Google.Geoloader(this.Geocoder,
                                Function.Delegate.create(this, this._renderMarkerManager));
            for (var i = 0; i < this.Markers.length; i++) {
                if (!this.renderMarker(this.Markers[i]))
                    loader.addMarker(this.Markers[i]);
            }
            loader.load();
        }
        // directions
        if (this.Directions) {
            for (var i = 0; i < this.Directions.length; i++) {
                this.renderDirection(this.Directions[i]);
            }
        }
        // polylines
        if (this.Polylines) {
            for (var i = 0; i < this.Polylines.length; i++) {
                this.renderPolyline(this.Polylines[i]);
            }
        }
        // polygons
        if (this.Polygons) {
            for (var i = 0; i < this.Polygons.length; i++) {
                this.renderPolygon(this.Polygons[i]);
            }
        }
        // fire actions
        if (this.Actions) {
            for (var i = 0; i < this.Actions.length; i++) {
                Function.Delegate.callFromString(this, this.Actions[i]);
            }
        }
    },

    renderDirection: function Artem_Google_Map$renderDirection(d) {
        d.loadDefault();
    },

    renderMarker: function Artem_Google_Map$renderMarker(m) {
        if ((m.Latitude != 0) && (m.Longitude != 0)) {
            try {
                m.load(new GLatLng(m.Latitude, m.Longitude));
            }
            catch (ex) { }
            return true;
        }
        else {
            return false;
        }
    },

    renderPolygon: function Artem_Google_Map$renderPolygon(pg) {
        this.addOverlay(pg.GPolygon);
    },

    renderPolyline: function Artem_Google_Map$renderPolyline(pl) {
        this.addOverlay(pl.GPolyline);
    },

    setAddress: function Artem_Google_Map$setAddress(addresses) {
        if (addresses.Status.code == 200) {
            try {
                this.Address = addresses.Placemark[0].address;
                this.saveState();
                if (this.ClientLocationLoadedIndex != null) {
                    var delegate = Function.Delegate.createFromString(this, this.ClientEvents[this.ClientLocationLoadedIndex].Handler);
                    delegate.call(this, this.Address);
                }
                if (this.ServerLocationLoadedIndex != null) {
                    var handler = this.ServerEvents[this.ServerLocationLoadedIndex].Handler;
                    handler = handler.replace("ARGS", this.Address);
                    Function.Delegate.callFromString(this, handler);
                }
            }
            catch (ex) { }
        }
    },

    setMapView: function Artem_Google_Map$setMapView() {
        // set view
        if (this.DefaultMapView) {
            switch (this.DefaultMapView) {
                case Artem.Google.MapView.Normal:
                    this.GMap.setMapType(G_NORMAL_MAP);
                    break;
                case Artem.Google.MapView.Satellite:
                    this.GMap.setMapType(G_SATELLITE_MAP);
                    break;
                case Artem.Google.MapView.Hybrid:
                    this.GMap.setMapType(G_HYBRID_MAP);
                    break;
                case Artem.Google.MapView.Physical:
                    this.GMap.addMapType(G_PHYSICAL_MAP);
                    this.GMap.setMapType(G_PHYSICAL_MAP);
                    break;
                case Artem.Google.MapView.MoonElevation:
                    this.GMap.addMapType(G_MOON_ELEVATION_MAP);
                    this.GMap.setMapType(G_MOON_ELEVATION_MAP);
                    break;
                case Artem.Google.MapView.MoonVisible:
                    this.GMap.addMapType(G_MOON_VISIBLE_MAP);
                    this.GMap.setMapType(G_MOON_VISIBLE_MAP);
                    break;
                case Artem.Google.MapView.MarsElevation:
                    this.GMap.addMapType(G_MARS_ELEVATION_MAP);
                    this.GMap.setMapType(G_MARS_ELEVATION_MAP);
                    break;
                case Artem.Google.MapView.MarsVisible:
                    this.GMap.addMapType(G_MARS_VISIBLE_MAP);
                    this.GMap.setMapType(G_MARS_VISIBLE_MAP);
                    break;
                case Artem.Google.MapView.MarsInfrared:
                    this.GMap.addMapType(G_MARS_INFRARED_MAP);
                    this.GMap.setMapType(G_MARS_INFRARED_MAP);
                    break;
                case Artem.Google.MapView.SkyVisible:
                    this.GMap.addMapType(G_SKY_VISIBLE_MAP);
                    this.GMap.setMapType(G_SKY_VISIBLE_MAP);
                    break;
                case Artem.Google.MapView.Satellite3D:
                    this.GMap.addMapType(G_SATELLITE_3D_MAP);
                    this.GMap.setMapType(G_SATELLITE_3D_MAP);
                    break;
                case Artem.Google.MapView.MapMakerNormal:
                    this.GMap.addMapType(G_MAPMAKER_NORMAL_MAP);
                    this.GMap.setMapType(G_MAPMAKER_NORMAL_MAP);
                    break;
                case Artem.Google.MapView.MapMakerHybrid:
                    this.GMap.addMapType(G_MAPMAKER_HYBRID_MAP);
                    this.GMap.setMapType(G_MAPMAKER_HYBRID_MAP);
                    break;
            }
        }
    },

    setStreetView: function Artem_Google_Map$setStreetView(overlay, latlng) {
        this.GMapPano.setLocationAndPOV(latlng);
    },
    //#endregion

    //#region Google Maps API Wrapped 

    addControl: function Artem_Google_Map_google$addControl(control, position) {
        this.GMap.addControl(control, position);
    },

    addMapType: function Artem_Google_Map_google$addMapType(type) {
        this.GMap.addMapType(type);
    },

    addOverlay: function Artem_Google_Map_google$addOverlay(overlay) {
        this.GMap.addOverlay(overlay);
    },

    checkResize: function Artem_Google_Map_google$checkResize() {
        this.GMap.checkResize();
    },

    clearOverlays: function Artem_Google_Map_google$clearOverlays() {
        this.GMap.clearOverlays();
    },

    closeInfoWindow: function Artem_Google_Map_google$closeInfoWindow() {
        this.GMap.closeInfoWindow();
    },

    continuousZoomEnabled: function Artem_Google_Map_google$continuousZoomEnabled() {
        return this.GMap.continuousZoomEnabled();
    },

    disableContinuousZoom: function Artem_Google_Map_google$disableContinuousZoom() {
        this.GMap.disableContinuousZoom();
    },

    disableDoubleClickZoom: function Artem_Google_Map_google$disableDoubleClickZoom() {
        this.GMap.disableDoubleClickZoom();
    },

    disableDragging: function Artem_Google_Map_google$disableDragging() {
        this.GMap.disableDragging();
    },

    disableGoogleBar: function Artem_Google_Map_google$disableGoogleBar() {
        this.GMap.disableGoogleBar();
    },

    disableInfoWindow: function Artem_Google_Map_google$disableInfoWindow() {
        this.GMap.disableInfoWindow();
    },

    disableScrollWheelZoom: function Artem_Google_Map_google$disableScrollWheelZoom() {
        this.GMap.disableScrollWheelZoom();
    },

    doubleClickZoomEnabled: function Artem_Google_Map_google$doubleClickZoomEnabled() {
        return this.GMap.doubleClickZoomEnabled();
    },

    draggingEnabled: function Artem_Google_Map_google$draggingEnabled() {
        return this.GMap.draggingEnabled();
    },

    enableContinuousZoom: function Artem_Google_Map_google$enableContinuousZoom() {
        this.GMap.enableContinuousZoom();
    },

    enableDoubleClickZoom: function Artem_Google_Map_google$enableDoubleClickZoom() {
        this.GMap.enableDoubleClickZoom();
    },

    enableDragging: function Artem_Google_Map_google$enableDragging() {
        this.GMap.enableDragging();
    },

    enableGoogleBar: function Artem_Google_Map_google$enableGoogleBar() {
        this.GMap.enableGoogleBar();
    },

    enableInfoWindow: function Artem_Google_Map_google$enableInfoWindow() {
        this.GMap.enableInfoWindow();
    },

    enableScrollWheelZoom: function Artem_Google_Map_google$enableScrollWheelZoom() {
        this.GMap.enableScrollWheelZoom();
    },

    fromContainerPixelToLatLng: function Artem_Google_Map_google$fromContainerPixelToLatLng(pixel) {
        return this.GMap.fromContainerPixelToLatLng(pixel);
    },

    fromDivPixelToLatLng: function Artem_Google_Map_google$fromDivPixelToLatLng(pixel) {
        return this.GMap.fromDivPixelToLatLng(pixel);
    },

    fromLatLngToDivPixel: function Artem_Google_Map_google$fromLatLngToDivPixel(latlng) {
        return this.GMap.fromLatLngToDivPixel(latlng);
    },

    getBounds: function Artem_Google_Map_google$getBounds() {
        return this.GMap.getBounds();
    },

    getBoundsZoomLevel: function Artem_Google_Map_google$getBoundsZoomLevel() {
        return this.GMap.getBoundsZoomLevel();
    },

    getCenter: function Artem_Google_Map_google$getCenter() {
        return this.GMap.getCenter();
    },

    getContainer: function Artem_Google_Map_google$getContainer() {
        return this.GMap.getContainer();
    },

    getCurrentMapType: function Artem_Google_Map_google$getCurrentMapType() {
        return this.GMap.getCurrentMapType();
    },

    getDragObject: function Artem_Google_Map_google$getDragObject() {
        return this.GMap.getDragObject();
    },

    getInfoWindow: function Artem_Google_Map_google$getInfoWindow() {
        return this.GMap.getInfoWindow();
    },

    getMapTypes: function Artem_Google_Map_google$getMapTypes() {
        return this.GMap.getMapTypes();
    },

    getPane: function Artem_Google_Map_google$getPane(pane) {
        return this.GMap.getPane();
    },

    getSize: function Artem_Google_Map_google$getSize() {
        return this.GMap.getSize();
    },

    getZoom: function Artem_Google_Map_google$getZoom() {
        return this.GMap.getZoom();
    },

    infoWindowEnabled: function Artem_Google_Map_google$infoWindowEnabled() {
        return this.GMap.infoWindowEnabled();
    },

    isLoaded: function Artem_Google_Map_google$isLoaded() {
        return this.GMap.isLoaded();
    },

    openInfoWindow: function Artem_Google_Map_google$openInfoWindow(point, node, opts) {
        this.GMap.openInfoWindow(point, node, opts);
    },

    openInfoWindowHtml: function Artem_Google_Map_google$openInfoWindowHtml(point, html, opts) {
        this.GMap.openInfoWindowHtml(point, html, opts);
    },

    panBy: function Artem_Google_Map_google$panBy(distance) {
        this.GMap.panBy(distance);
    },

    panDirection: function Artem_Google_Map_google$panDirection(dx, dy) {
        this.GMap.panDirection(dx, dy);
    },

    panTo: function Artem_Google_Map_google$panTo(center) {
        this.GMap.panTo(center);
    },

    removeControl: function Artem_Google_Map_google$removeControl(control) {
        this.GMap.removeControl(control);
    },

    removeMapType: function Artem_Google_Map_google$removeMapType(type) {
        this.GMap.removeMapType();
    },

    removeOverlay: function Artem_Google_Map_google$removeOverlay(overlay) {
        this.GMap.removeOverlay(overlay);
    },

    returnToSavedPosition: function Artem_Google_Map_google$returnToSavedPosition() {
        this.GMap.returnToSavedPosition();
    },

    savePosition: function Artem_Google_Map_google$savePosition() {
        this.GMap.savePosition();
    },

    scrollWheelZoomEnabled: function Artem_Google_Map_google$scrollWheelZoomEnabled() {
        return this.GMap.scrollWheelZoomEnabled();
    },

    setCenter: function Artem_Google_Map_google$setCenter(point, zoom, type) {
        this.GMap.setCenter(point, zoom, type);
    },

    setMapType: function Artem_Google_Map_google$setMapType(type) {
        this.GMap.setMapType(type);
    },

    setZoom: function Artem_Google_Map_google$setZoom(level) {
        this.GMap.setZoom(level);
    },

    zoomIn: function Artem_Google_Map_google$zoomIn() {
        this.GMap.zoomIn();
    },

    zoomOut: function Artem_Google_Map_google$zoomOut() {
        this.GMap.zoomOut();
    },
    //#endregion

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Map"
};
//#endregion

//#region MapState class //////////////////////////////////////////////////////////////////////////

Artem.Google.MapState = function (map) {
    /// <param name="map" type="Artem.Google.Map"></param>
    /// <field name="Address"></field>
    /// <field name="BaseCountryCode"></field>
    /// <field name="DefaultAddress"></field>
    /// <field name="DefaultMapView"></field>
    /// <field name="Directions"></field>
    /// <field name="EnableContinuousZoom"></field>
    /// <field name="EnableDoubleClickZoom"></field>
    /// <field name="EnableDragging"></field>
    /// <field name="EnableGoogleBar"></field>
    /// <field name="EnableInfoWindow"></field>
    /// <field name="EnableMarkerManager"></field>
    /// <field name="EnableReverseGeocoding"></field>
    /// <field name="EnableScrollWheelZoom"></field>
    /// <field name="EnterpriseKey"></field>
    /// <field name="Height"></field>
    /// <field name="IsStatic"></field>
    /// <field name="IsStreetView"></field>
    /// <field name="Key"></field>
    /// <field name="Latitude"></field>
    /// <field name="Longitude"></field>
    /// <field name="MarkerManagerOptions"></field>
    /// <field name="Markers"></field>
    /// <field name="Polygons"></field>
    /// <field name="Polylines"></field>
    /// <field name="ShowMapTypeControl"></field>
    /// <field name="ShowScaleControl"></field>
    /// <field name="ShowTraffic"></field>
    /// <field name="StreetViewMode"></field>
    /// <field name="StreetViewPanoID"></field>
    /// <field name="Width"></field>
    /// <field name="Zoom"></field>
    /// <field name="ZoomPanType"></field>

    this._map = map;
    this.deserialize();
    this._out();
}

Artem.Google.MapState.prototype = {

    //#region Fields 

    Address: null,
    BaseCountryCode: null,
    DefaultAddress: null,
    DefaultMapView: null,
    Directions: [],
    EnableContinuousZoom: false,
    EnableDoubleClickZoom: false,
    EnableDragging: true,
    EnableGoogleBar: false,
    EnableInfoWindow: true,
    EnableMarkerManager: false,
    EnableReverseGeocoding: false,
    EnableScrollWheelZoom: false,
    EnterpriseKey: null,
    Height: null,
    IsStatic: false,
    IsStreetView: false,
    Key: null,
    Latitude: null,
    Longitude: null,
    MarkerManagerOptions: null,
    Markers: null,
    Polygons: null,
    Polylines: null,
    ShowMapTypeControl: true,
    ShowScaleControl: null,
    ShowTraffic: null,
    StreetViewMode: null,
    StreetViewPanoID: null,
    Width: null,
    Zoom: null,
    ZoomPanType: null,

    //#endregion

    //#region Methods

    deserialize: function () {
    },

    serialize: function () {
    },

    //#endregion

    //#region Private Methods 

    _in: function () {
        this._transfer(this._map, this);
    },

    _out: function () {
        this._transfer(this, this._map);
    },

    _transfer: function (source, dest) {
        /// <param name="source" type="Artem.Google.MapState"></param>
        /// <param name="dest" type="Artem.Google.MapState"></param>
        dest.Address = source.Address;
        dest.BaseCountryCode = source.BaseCountryCode;
        dest.DefaultAddress = source.DefaultAddress;
        dest.DefaultMapView = source.DefaultMapView;
        dest.Directions = source.Directions;
        dest.EnableContinuousZoom = source.EnableContinuousZoom;
        dest.EnableDoubleClickZoom = source.EnableDoubleClickZoom;
        dest.EnableDragging = source.EnableDragging;
        dest.EnableGoogleBar = source.EnableGoogleBar;
        dest.EnableInfoWindow = source.EnableInfoWindow;
        dest.EnableMarkerManager = source.EnableMarkerManager;
        dest.EnableReverseGeocoding = source.EnableReverseGeocoding;
        dest.EnableScrollWheelZoom = source.EnableScrollWheelZoom;
        dest.EnterpriseKey = source.EnterpriseKey;
        dest.Height = source.Height;
        dest.IsStatic = source.IsStatic;
        dest.IsStreetView = source.IsStreetView;
        dest.Key = source.Key;
        dest.Latitude = source.Latitude;
        dest.Longitude = source.Longitude;
        dest.MarkerManagerOptions = source.MarkerManagerOptions;
        dest.Markers = source.Markers;
        dest.Polygons = source.Polygons;
        dest.Polylines = source.Polylines;
        dest.ShowMapTypeControl = source.ShowMapTypeControl;
        dest.ShowScaleControl = source.ShowScaleControl;
        dest.ShowTraffic = source.ShowTraffic;
        dest.StreetViewMode = source.StreetViewMode;
        dest.StreetViewPanoID = source.StreetViewPanoID;
        dest.Width = source.Width;
        dest.Zoom = source.Zoom;
        dest.ZoomPanType = source.ZoomPanType;
    }
    //#endregion
};
//#endregion

//#region Marker class ///////////////////////////////////////////////////////////////////////////////////

Artem.Google.Marker = function Artem_Google_Marker(map, index, config) {
    this._init(map, index, config);
};

Artem.Google.Marker.prototype = {

    // Fields -------------------------------------------------------------------------------------

    GMarker: null,
    Map: null,
    Index: null,
    Address: null,
    AutoPan: null,
    Bouncy: null,
    Clickable: null,
    Draggable: null,
    DragCrossMove: null,
    IconAnchor: null,
    IconSize: null,
    IconUrl: null,
    InfoWindowAnchor: null,
    Latitude: null,
    Longitude: null,
    MaxZoom: null,
    MinZoom: null,
    OpenInfoBehaviour: null,
    OpenWindowContent: null,
    ShadowSize: null,
    ShadowUrl: null,
    Text: null,
    Title: null,

    ClientEvents: null,
    ServerEvents: null,

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Marker$_init(map, index, config) {

        this.Map = map;
        this.Index = index;

        this.Address = config.Address;
        this.AutoPan = config.AutoPan;
        this.Bouncy = config.Bouncy;
        this.Clickable = config.Clickable;
        this.Draggable = config.Draggable;
        this.DragCrossMove = config.DragCrossMove;
        this.IconAnchor = config.IconAnchor;
        this.IconSize = config.IconSize;
        this.IconUrl = config.IconUrl;
        this.InfoWindowAnchor = config.InfoWindowAnchor;
        this.Latitude = config.Latitude;
        this.Longitude = config.Longitude;
        this.MaxZoom = config.MaxZoom;
        this.MinZoom = config.MinZoom;
        this.OpenInfoBehaviour = config.OpenInfoBehaviour;
        this.ShadowSize = config.ShadowSize;
        this.ShadowUrl = config.ShadowUrl;
        this.Text = config.Text;
        this.Title = config.Title;

        // events
        if (map.MarkerEvents) {
            this.ClientEvents = map.MarkerEvents.ClientEvents;
            this.ServerEvents = map.MarkerEvents.ServerEvents;
        }
    },

    attachEvents: function Artem_Google_Marker$attachEvents(events, clients) {
        if (events) {
            for (var i = 0; i < events.length; i++) {
                var key = events[i].Key;
                if (clients) {
                    GEvent.addListener(this.GMarker, key,
                        Function.Delegate.createFromString(this, events[i].Handler));
                }
                else {
                    var handler = events[i].Handler;
                    var delegate = Function.Delegate.create(this, this.raiseEvent);
                    GEvent.addListener(this.GMarker, key, function (args) {
                        delegate.call(this, handler, args);
                    });
                }
            }
        }
    },

    initialize: function Artem_Google_Marker$initialize() {
        // open info behaviour
        var eventName;
        switch (this.OpenInfoBehaviour) {
            case Artem.Google.OpenInfoBehaviour.Click:
                eventName = "click";
                break;
            case Artem.Google.OpenInfoBehaviour.DoubleClick:
                eventName = "dblclick";
                break;
            case Artem.Google.OpenInfoBehaviour.MouseDown:
                eventName = "mousedown";
                break;
            case Artem.Google.OpenInfoBehaviour.MouseOut:
                eventName = "mouseout";
                break;
            case Artem.Google.OpenInfoBehaviour.MouseOver:
                eventName = "mouseover";
                break;
            case Artem.Google.OpenInfoBehaviour.MouseUp:
                eventName = "mouseup";
                break;
        }
        if (eventName)
            GEvent.addListener(this.GMarker, eventName, Function.Delegate.create(this, this.openDefaultInfoWindow));
        //events
        this.attachEvents(this.ClientEvents, true);
        this.attachEvents(this.ServerEvents, false);
    },

    isLoaded: function Artem_Google_Marker$isLoaded() {
        return (this.GMarker != null);
        //        if (this.GMarker == null) throw "Cannot use it before marker been loaded!";
    },

    load: function Artem_Google_Marker$load(point) {
        if (point) {
            // persist point
            this.Latitude = point.lat();
            this.Longitude = point.lng();
            // options
            var options = new Object();
            options.autoPan = this.AutoPan;
            options.bouncy = this.Bouncy;
            options.clickable = this.Clickable;
            options.draggable = this.Draggable;
            options.dragCrossMove = this.DragCrossMove;
            options.title = this.Title;
            options.icon = this.createIcon();
            // create
            this.GMarker = new GMarker(point, options);
            this.Map.addOverlay(this.GMarker);
            this.initialize();
        }
    },

    raiseEvent: function Artem_Google_Marker$raiseEvent(handler, args) {
        this.Map.saveState();
        if (handler) {
            handler = handler.replace("INDEX", this.Index);
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
        }
    },

    saveState: function Artem_Google_Marker$saveState() {
        var state = "{";
        //
        state += "\"Address\":\"" + this.Address + "\",";
        state += "\"AutoPan\":" + this.AutoPan + ",";
        state += "\"Bouncy\":" + this.Bouncy + ",";
        state += "\"Clickable\":" + this.Clickable + ",";
        state += "\"Draggable\":" + this.Draggable + ",";
        state += "\"DragCrossMove\":" + this.DragCrossMove + ",";
        if (this.IconAnchor)
            state += "\"IconAnchor\":{X:" + this.IconAnchor.X + ",Y:" + this.IconAnchor.Y + "},";
        if (this.IconSize)
            state += "\"IconSize\":{Width:" + this.IconSize.Width + ",Height:" + this.IconSize.Height + "},";
        state += "\"IconUrl\":\"" + this.IconUrl + "\",";
        if (this.InfoWindowAnchor)
            state += "\"InfoWindowAnchor\":{X:" + this.InfoWindowAnchor.X + ",Y:" + this.InfoWindowAnchor.Y + "},";
        var point = this.getLatLng();
        if (point) {
            state += "\"Latitude\":" + point.lat() + ",";
            state += "\"Longitude\":" + point.lng() + ",";
        }
        else {
            state += "\"Latitude\":" + this.Latitude + ",";
            state += "\"Longitude\":" + this.Longitude + ",";
        }
        state += "\"OpenInfoBehaviour\":" + this.OpenInfoBehaviour + ",";
        if (this.ShadowSize)
            state += "\"ShadowSize\":{Width:" + this.ShadowSize.Width + ",Height:" + this.ShadowSize.Height + "},";
        state += "\"ShadowUrl\":\"" + this.ShadowUrl + "\",";
        /*
        * >> FIX:   removed from post back in order to solve the issues with page ValidateRequest
        *           http://googlemap.codeplex.com/WorkItem/View.aspx?WorkItemId=7470
        */
        //        state += "\"Text\":\"" + this.Text + "\",";
        /*
        * << FIX
        */
        state += "\"Title\":\"" + this.Title + "\",";
        //
        state += "}";
        return state;
    },

    // Google Maps API Wrapped --------------------------------------------------------------------

    closeInfoWindow: function Artem_Google_Marker_google$closeInfoWindow() {
        if (this.isLoaded())
            this.GMarker.closeInfoWindow();
    },

    createIcon: function Artem_Google_Marker_google$createIcon() {
        var icon = null;
        if (this.IconUrl) {
            icon = new GIcon();
            icon.image = this.IconUrl;
            if (this.IconSize)
                icon.iconSize = new GSize(this.IconSize.Width, this.IconSize.Height);
            if (this.IconAnchor)
                icon.iconAnchor = new GPoint(this.IconAnchor.X, this.IconAnchor.Y);
            if (this.InfoWindowAnchor)
                icon.infoWindowAnchor = new GPoint(this.InfoWindowAnchor.X, this.InfoWindowAnchor.Y);
            if (this.ShadowUrl)
                icon.shadow = this.ShadowUrl;
            if (this.ShadowSize)
                icon.shadowSize = new GSize(this.ShadowSize.Width, this.ShadowSize.Height);
        }
        return icon;
    },

    disableDragging: function Artem_Google_Marker_google$disableDragging() {
        if (this.isLoaded())
            this.GMarker.disableDragging();
    },

    draggable: function Artem_Google_Marker_google$draggable() {
        if (this.isLoaded())
            return this.GMarker.draggable();
    },

    draggingEnabled: function Artem_Google_Marker_google$draggingEnabled() {
        if (this.isLoaded())
            return this.GMarker.draggingEnabled();
    },

    enableDragging: function Artem_Google_Marker_google$enableDragging() {
        if (this.isLoaded())
            this.GMarker.enableDragging();
    },

    getIcon: function Artem_Google_Marker_google$getIcon() {
        if (this.isLoaded())
            return this.GMarker.getIcon();
    },

    getLatLng: function Artem_Google_Marker_google$getLatLng() {
        if (this.isLoaded())
            return this.GMarker.getLatLng();
    },

    getPoint: function Artem_Google_Marker_google$getPoint() {
        if (this.isLoaded())
            return this.GMarker.getPoint();
    },

    getTitle: function Artem_Google_Marker_google$getTitle() {
        if (this.isLoaded())
            return this.GMarker.getTitle();
    },

    hide: function Artem_Google_Marker_google$hide() {
        if (this.isLoaded())
            this.GMarker.hide();
    },

    isHidden: function Artem_Google_Marker_google$isHidden() {
        if (this.isLoaded())
            return this.GMarker.isHidden();
    },

    openDefaultInfoWindow: function Artem_Google_Marker_google$openDefaultInfoWindow() {
        if (this.isLoaded()) {
            if (this.OpenWindowContent) {
                var node = document.getElementById(this.OpenWindowContent);
                this.openInfoWindow(node.cloneNode(true));
            }
            else
                this.openInfoWindowHtml(this.Text);
        }
    },

    openInfoWindow: function Artem_Google_Marker_google$openInfoWindow(domnode) {
        if (this.isLoaded())
            this.GMarker.openInfoWindow(domnode);
    },

    openInfoWindowHtml: function Artem_Google_Marker_google$openInfoWindowHtml(content) {
        if (this.isLoaded())
            this.GMarker.openInfoWindowHtml(content);
    },

    setImage: function Artem_Google_Marker_google$setImage(url) {
        if (this.isLoaded())
            this.GMarker.setImage(url);
    },

    setLatLng: function Artem_Google_Marker_google$setLatLng(point) {
        if (this.isLoaded())
            this.GMarker.setLatLng(point);
    },

    setPoint: function Artem_Google_Marker_google$setPoint(point) {
        if (this.isLoaded())
            this.GMarker.setPoint(point);
    },

    show: function Artem_Google_Marker_google$show() {
        if (this.isLoaded())
            this.GMarker.show();
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Marker"
};
//#endregion

//#region Direction class ////////////////////////////////////////////////////////////////////////////////

Artem.Google.Direction = function Artem_Google_Direction(map, config) {
    this._init(map, config);
};

Artem.Google.Direction.prototype = {

    // Fields -------------------------------------------------------------------------------------

    GDirections: null,
    Locale: null,
    Query: null,
    PreserveViewport: null,
    RoutePanelId: null,

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Direction$_init(map, config) {
        this.Locale = config.Locale;
        this.Query = config.Query;
        this.RoutePanelId = config.RoutePanelId;
        this.PreserveViewport = config.PreserveViewport;
        // origin
        var pane = null;
        if (this.RoutePanelId) pane = document.getElementById(this.RoutePanelId);
        this.GDirections = new GDirections(map.GMap, pane);
    },

    loadDefault: function Artem_Google_Direction$loadDefault() {
        var options;
        if (this.Locale && this.PreserveViewport) {
            options = { locale: this.Locale, preserveViewport: this.PreserveViewport };
        } else if (this.Locale) {
            options = { locale: this.Locale };
        } else if (this.PreserveViewport) {
            options = { preserveViewport: this.PreserveViewport };
        }
        this.load(this.Query, options);
    },

    saveState: function Artem_Google_Direction$saveState() {
        var state = "{";
        state += "\"Locale\":\"" + this.Locale + "\",";
        state += "\"Query\":\"" + this.Query + "\",";
        state += "\"RoutePanelId\":\"" + this.RoutePanelId + "\",";
        state += "\"PreserveViewport\":\"" + this.PreserveViewport + "\",";
        // distance
        var distance = this.getDistance();
        state += "\"Distance\":{\"Meters\":" + distance.meters + ",\"Html\":\"" + distance.html + "\"},";
        // duration
        var duration = this.getDuration();
        state += "\"Duration\":{\"Seconds\":" + duration.seconds + ",\"Html\":\"" + duration.html + "\"},";
        // bounds
        var bounds = new Artem.Google.Bounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.saveState();
        //
        state += "}";
        return state;
    },

    // Google Maps API Wrapped --------------------------------------------------------------------

    clear: function Artem_Google_Direction_google$clear() {
        this.GDirections.clear();
    },

    getBounds: function Artem_Google_Direction_google$getBounds() {
        return this.GDirections.getBounds();
    },

    getCopyrightsHtml: function Artem_Google_Direction_google$getCopyrightsHtml() {
        return this.GDirections.getCopyrightsHtml();
    },

    getDistance: function Artem_Google_Direction_google$getDistance() {
        return this.GDirections.getDistance();
    },

    getDuration: function Artem_Google_Direction_google$getDuration() {
        return this.GDirections.getDuration();
    },

    getGeocode: function Artem_Google_Direction_google$getGeocode(i) {
        return this.GDirections.getGeocode(i);
    },

    getMarker: function Artem_Google_Direction_google$getMarker(i) {
        return this.GDirections.getMarker(i);
    },

    getNumGeocodes: function Artem_Google_Direction_google$getNumGeocodes() {
        return this.GDirections.getNumGeocodes();
    },

    getNumRoutes: function Artem_Google_Direction_google$getNumRoutes() {
        return this.GDirections.getNumRoutes();
    },

    getPolyline: function Artem_Google_Direction_google$getPolyline() {
        return this.GDirections.getPolyline();
    },

    getRoute: function Artem_Google_Direction_google$getRoute(i) {
        return this.GDirections.getRoute(i);
    },

    getSummaryHtml: function Artem_Google_Direction_google$getSummaryHtml() {
        return this.GDirections.getSummaryHtml();
    },

    getStatus: function Artem_Google_Direction_google$getStatus() {
        return this.GDirections.getStatus();
    },

    load: function Artem_Google_Direction_google$load(query, options) {
        this.GDirections.load(query, options);
    },

    loadFromWaypoints: function Artem_Google_Direction_google$loadFromWaypoints(waypoints, options) {
        this.GDirections.loadFromWaypoints(waypoints, options);
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Direction"
};
//#endregion

//#region Polygon class //////////////////////////////////////////////////////////////////////////////////

Artem.Google.Polygon = function Artem_Google_Polygon(map, index, config) {
    this._init(map, index, config);
};

Artem.Google.Polygon.prototype = {

    // Fields -------------------------------------------------------------------------------------

    GPolygon: null,
    Map: null,
    Index: null,
    Clickable: null,
    EnableDrawing: null,
    EnableEditing: null,
    FillColor: null,
    FillOpacity: null,
    Points: null,
    StrokeColor: null,
    StrokeOpacity: null,
    StrokeWeight: null,

    ClientEvents: null,
    ServerEvents: null,

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Polygon$_init(map, index, config) {

        this.Map = map;
        this.Index = index;
        this.Clickable = config.Clickable;
        this.EnableDrawing = config.EnableDrawing;
        this.EnableEditing = config.EnableEditing;
        this.FillColor = config.FillColor;
        this.FillOpacity = config.FillOpacity;
        this.Points = config.Points;
        this.StrokeColor = config.StrokeColor;
        this.StrokeOpacity = config.StrokeOpacity;
        this.StrokeWeight = config.StrokeWeight;

        // origin
        var points = new Array();
        var options = new Object();
        if (this.Points) {
            for (var i = 0; i < this.Points.length; i++) {
                points.push(new GLatLng(this.Points[i].Latitude, this.Points[i].Longitude));
            }
        }
        options.clickable = this.Clickable;

        this.GPolygon = new GPolygon(points,
                            this.StrokeColor, this.StrokeWeight, this.StrokeOpacity, this.FillColor, this.FillOpacity, options);
        if (this.EnableDrawing) this.enableDrawing();
        if (this.EnableEditing) this.enableEditing();

        // events
        if (map.PolygonEvents) {
            this.ClientEvents = map.PolygonEvents.ClientEvents;
            this.ServerEvents = map.PolygonEvents.ServerEvents;
        }
        this.attachEvents(this.ClientEvents, true);
        this.attachEvents(this.ServerEvents, false);
    },

    attachEvents: function Artem_Google_Polygon$attachEvents(events, clients) {
        if (events) {
            var key;
            for (var i = 0; i < events.length; i++) {
                key = events[i].Key;
                if (clients) {
                    GEvent.addListener(this.GPolygon, key,
                        Function.Delegate.createFromString(this, events[i].Handler));
                }
                else {
                    var handler = events[i].Handler;
                    var delegate = Function.Delegate.create(this, this.raiseEvent);
                    GEvent.addListener(this.GPolygon, key, function (args) {
                        delegate.call(this, handler, args);
                    });
                }
            }
        }
    },

    raiseEvent: function Artem_Google_Polygon$raiseEvent(handler, args) {
        this.Map.saveState();
        handler = handler.replace("INDEX", this.Index);
        handler = handler.replace("ARGS", args);
        eval(handler);
    },

    saveState: function Artem_Google_Polygon$saveState() {
        var state = "{";
        //
        state += "\"Clickable\":" + this.Clickable + ",";
        state += "\"FillColor\":\"" + this.FillColor + "\",";
        state += "\"FillOpacity\":" + this.FillOpacity + ",";
        state += "\"StrokeColor\":\"" + this.StrokeColor + "\",";
        state += "\"StrokeOpacity\":" + this.StrokeOpacity + ",";
        state += "\"StrokeWeight\":" + this.StrokeWeight + ",";
        // points
        if (this.Points) {
            state += "\"Points\":[";
            var point;
            for (var i = 0; i < this.Points.length; i++) {
                point = this.Points[i];
                state += "{\"Latitude\":" + point.Latitude + ",\"Longitude\":" + point.Longitude + "},";
            }
            state = state.substr(0, state.length - 1);
            state += "],";
        }
        // bounds
        var bounds = new Artem.Google.Bounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.saveState();
        //
        state += "}";
        return state;
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Polygon"
};
//#endregion

//#region Polyline class ///////////////////////////////////////////////////////////////////////////

Artem.Google.Polyline = function Artem_Google_Polyline(map, index, config) {
    this._init(map, index, config);
};

Artem.Google.Polyline.prototype = {

    // Fields -------------------------------------------------------------------------------------

    GPolyline: null,
    Map: null,
    Index: null,
    Clickable: null,
    Color: null,
    Geodesic: null,
    Opacity: null,
    Points: null,
    Weight: null,

    ClientEvents: null,
    ServerEvents: null,

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Polyline$_init(map, index, config) {

        this.Map = map;
        this.Index = index;
        this.Clickable = config.Clickable;
        this.Color = config.Color;
        this.Geodesic = config.Geodesic;
        this.Opacity = config.Opacity;
        this.Points = config.Points;
        this.Weight = config.Weight;

        // origin
        var points = new Array();
        var options = new Object();
        if (this.Points) {
            for (var i = 0; i < this.Points.length; i++) {
                points.push(new GLatLng(this.Points[i].Latitude, this.Points[i].Longitude));
            }
        }
        options.clickable = this.Clickable;
        options.geodesic = this.Geodesic;
        this.GPolyline = new GPolyline(points, this.Color, this.Weight, this.Opacity, options);

        // events
        if (map.PolylineEvents) {
            this.ClientEvents = map.PolylineEvents.ClientEvents;
            this.ServerEvents = map.PolylineEvents.ServerEvents;
        }
        this.attachEvents(this.ClientEvents, true);
        this.attachEvents(this.ServerEvents, false);
    },

    attachEvents: function Artem_Google_Polyline$attachEvents(events, clients) {
        if (events) {
            var key;
            for (var i = 0; i < events.length; i++) {
                key = events[i].Key;
                if (clients) {
                    GEvent.addListener(this.GPolyline, key,
                        Function.Delegate.createFromString(this, events[i].Handler));
                }
                else {
                    var handler = events[i].Handler;
                    var delegate = Function.Delegate.create(this, this.raiseEvent);
                    GEvent.addListener(this.GPolyline, key, function (args) {
                        delegate.call(this, handler, args);
                    });
                }
            }
        }
    },

    raiseEvent: function Artem_Google_Polyline$raiseEvent(handler, args) {
        this.Map.saveState();
        if (handler) {
            handler = handler.replace("INDEX", this.Index);
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
        }
    },

    saveState: function Artem_Google_Polyline$saveState() {
        var state = "{";
        //
        state += "\"Clickable\":" + this.Clickable + ",";
        state += "\"Color\":\"" + this.Color + "\",";
        state += "\"Geodesic\":" + this.Geodesic + ",";
        state += "\"Opacity\":" + this.Opacity + ",";
        state += "\"Weight\":" + this.Weight + ",";
        // points
        if (this.Points) {
            state += "\"Points\":[";
            var point;
            for (var i = 0; i < this.Points.length; i++) {
                point = this.Points[i];
                state += "{\"Latitude\":" + point.Latitude + ",\"Longitude\":" + point.Longitude + "},";
            }
            state = state.substr(0, state.length - 1);
            state += "],";
        }
        // bounds
        var bounds = new Artem.Google.Bounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.saveState();
        //
        state += "}";
        return state;
    },

    // Google Maps API Wrapped --------------------------------------------------------------------

    getBounds: function Artem_Google_Polyline_google$getBounds() {
        return this.GPolyline.getBounds();
    },

    hide: function Artem_Google_Polyline_google$hide() {
        this.GPolyline.hide();
    },

    isHidden: function Artem_Google_Polyline_google$isHidden() {
        return this.GPolyline.isHidden();
    },

    show: function Artem_Google_Polyline_google$show() {
        this.GPolyline.show();
    },

    supportsHide: function Artem_Google_Polyline_google$supportsHide() {
        return this.GPolyline.supportsHide();
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Polyline"
};
//#endregion

//#region Bounds class ///////////////////////////////////////////////////////////////////////////////////

Artem.Google.Bounds = function Artem_Google_Bounds(bounds) {
    this._init(bounds);
};

Artem.Google.Bounds.prototype = {

    // Fields -------------------------------------------------------------------------------------

    GBounds: null,

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Bounds$_init(bounds) {
        this.GBounds = bounds;
    },

    saveState: function Artem_Google_Bounds$saveState() {
        var state = "{";
        if (this.GBounds) {
            var sw = this.GBounds.getSouthWest();
            var ne = this.GBounds.getNorthEast();
            state += "\"SouthWest\":{\"Latitude\":" + sw.lat() + ",\"Longitude\":" + sw.lng() + "},";
            state += "\"NorthEast\":{\"Latitude\":" + ne.lat() + ",\"Longitude\":" + ne.lng() + "}";
        }
        state += "}";
        return state;
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Bounds"
};
//#endregion

//#region Geoloader class ////////////////////////////////////////////////////////////////////////////////

Artem.Google.Geoloader = function Artem_Google_Geoloader(geocoder, callback) {
    this._init(geocoder, callback);
};

Artem.Google.Geoloader.prototype = {

    // Fields -------------------------------------------------------------------------------------

    _callback: null,
    _count: 0,
    _delegate: null,
    _geocoder: null,
    _index: 0,
    _markers: [],

    // Methods ------------------------------------------------------------------------------------

    _init: function Artem_Google_Geoloader$_init(geocoder, callback) {
        this._callback = callback;
        this._delegate = Function.Delegate.create(this, this.resolve);
        this._geocoder = geocoder;
    },

    addMarker: function Artem_Google_Geoloader$addMarker(marker) {
        this._markers.push(marker);
    },

    load: function Artem_Google_Geoloader$load() {
        if (this._markers.length > 0) {
            this._index = (this._markers.length - 1);
            var marker = this._markers[this._index];
            this._geocoder.getLatLng(marker.Address, this._delegate);
        }
        else if (this._callback) {
            this._callback();
        }
    },

    resolve: function Artem_Google_Geoloader$resolve(point) {
        if (point) {
            this._markers[this._index].load(point);
            this._index--;
            if (this._index >= 0) {
                this._count = 0;
                this._geocoder.getLatLng(this._markers[this._index].Address, this._delegate);
            }
            else if (this._callback) {
                this._callback();
            }
        }
        else {
            if (this._count < 6) {
                var delay = this._count * 100;
                this._count++;
                setTimeout(delay, function () { });
                this._geocoder.getLatLng(this._markers[this._index].Address, this._delegate);
            }
            else if ((this._index--) >= 0) {
                this._count = 0;
                this._geocoder.getLatLng(this._markers[this._index].Address, this._delegate);
            }
        }
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Geoloader"
};
//#endregion



//#region MapView enum /////////////////////////////////////////////////////////////////////////////

Artem.Google.MapView = {

    // Fields ------------------------------------------------------------------------------------

    Normal: 0,
    Satellite: 1,
    Hybrid: 2,
    Physical: 3,
    MoonElevation: 4,
    MoonVisible: 5,
    MarsElevation: 6,
    MarsVisible: 7,
    MarsInfrared: 8,
    SkyVisible: 9,
    Satellite3D: 10,
    MapMakerNormal: 11,
    MapMakerHybrid: 12,

    // Methods ------------------------------------------------------------------------------------

    convert: function Artem_Google_MapView$convert(gmapType) {
        switch (gmapType) {
            case G_SATELLITE_MAP:
                return Artem.Google.MapView.Satellite;
            case G_HYBRID_MAP:
                return Artem.Google.MapView.Hybrid;
            case G_PHYSICAL_MAP:
                return Artem.Google.MapView.Physical;
            case G_MOON_ELEVATION_MAP:
                return Artem.Google.MapView.MoonElevation;
            case G_MOON_VISIBLE_MAP:
                return Artem.Google.MapView.MoonVisible;
            case G_MARS_ELEVATION_MAP:
                return Artem.Google.MapView.MarsElevation;
            case G_MARS_VISIBLE_MAP:
                return Artem.Google.MapView.MarsVisible;
            case G_MARS_INFRARED_MAP:
                return Artem.Google.MapView.MarsInfrared;
            case G_SKY_VISIBLE_MAP:
                return Artem.Google.MapView.SkyVisible;
            case G_SATELLITE_3D_MAP:
                return Artem.Google.MapView.Satellite3D;
            case G_MAPMAKER_NORMAL_MAP:
                return Artem.Google.MapView.MapMakerNormal;
            case G_MAPMAKER_HYBRID_MAP:
                return Artem.Google.MapView.MapMakerHybrid;
            default:
                return Artem.Google.MapView.Normal;
        }
    }

};
//#endregion

//#region OpenInfoBehaviour enum /////////////////////////////////////////////////////////////////////////

Artem.Google.OpenInfoBehaviour = {
    Click: 0,
    DoubleClick: 1,
    MouseDown: 2,
    MouseOut: 3,
    MouseOver: 4,
    MouseUp: 5
};
//#endregion


//#region Manager class 

Artem.Google.Manager = {

    // Static Fields ------------------------------------------------------------------------------

    CurrentMap: null,
    Disposed: false,
    Initialized: false,
    Saved: false,
    Maps: new Array(), // here is kept a reference to all maps on the page

    // Static Methods -----------------------------------------------------------------------------

    addMap: function Artem_Google_Manager$addMap(map) {
        for (var i = 0; i < this.Maps.length; i++) {
            if (map.ClientID == this.Maps[i].ClientID) return;
        }
        this.Maps[this.Maps.length] = (this.CurrentMap = map);
    },

    dispose: function Artem_Google_Manager$dispose() {
        if (typeof (Sys) != 'undefined') {
//            var prm = Sys.WebForms.PageRequestManager.getInstance();
//            if (!prm.get_isInAsyncPostBack()) GUnload();
        }
        else
            GUnload();
    },

    initialize: function Artem_Google_Manager$initialize() {
        if (!this.Initialized) {
            this.Initialized = true;
            this.Saved = false;
            if (!GBrowserIsCompatible()) throw "Your browser is not google maps api compatible!";

            var handler = Function.Delegate.create(this, this.dispose);
            if (typeof (Sys) != 'undefined') {
//                var prm = Sys.WebForms.PageRequestManager.getInstance();
//                prm.add_beginRequest(handler);
            }
            Function.Handler.add(window, "unload", handler);
        }
    },

    saveState: function Artem_Google_Manager$saveState() {
        try {
            for (var i = 0; i < this.Maps.length; i++) {
                try {
                    this.Maps[i].saveState();
                }
                catch (ex1) { }
            }
        }
        catch (ex) { }
    },

    // Type ---------------------------------------------------------------------------------------
    __type: "Artem.Google.Manager"
};
//#endregion

// Function Extensions 

Function.Delegate = {

    // Static Methods -----------------------------------------------------------------------------

    call: function Function_Delegate$call(instance, method) {
        Function.Delegate.create(instance, method).call(instance, arguments);
    },

    callFromString: function Function_Delegate$callFromString(instance, methodString) {
        Function.Delegate.createFromString(instance, methodString).call(instance, arguments);
    },

    createFromString: function $Function_Delegate$createFromString(instance, methodString) {
        var rex = new RegExp("\\(.*\\)");
        if (rex.test(methodString))
            return function () { eval(methodString); };
        else {
            var method = eval(methodString);
            return Function.Delegate.create(instance, method);
        }
    }

};

Function.Handler = {
    cache: {}
};

if (typeof (Sys) == 'undefined') {

    Function.Delegate.create = function (instance, method) {
        return function () { return method.apply(instance, arguments); };
    };

    Function.Handler.add = function (element, eventName, handler) {
        var browserHandler;
        if (element.addEventListener) {
            browserHandler = function (e) {
                return handler.call(element, e);
            }
            element.addEventListener(eventName, browserHandler, false);
        }
        else if (element.attachEvent) {
            browserHandler = function () {
                var e = {};
                try { e = window.event; } catch (ex) { }
                return handler.call(element, e);
            }
            element.attachEvent('on' + eventName, browserHandler);
        }
        Function.Handler.cache[Function.Handler.cache.length] = { handler: handler, browserHandler: browserHandler };
    };

    Function.Handler.remove = function (element, eventName, handler) {
        var browserHandler = null;
        var cache = Function.Handler.cache;
        var i = 0;
        var l = cache.length;
        for (; i < l; i++) {
            if (cache[i].handler === handler) {
                browserHandler = cache[i].browserHandler;
                break;
            }
        }
        if (browserHandler) {
            if (element.removeEventListener) {
                element.removeEventListener(eventName, browserHandler, false);
            }
            else if (element.detachEvent) {
                element.detachEvent('on' + eventName, browserHandler);
            }
            cache.splice(i, 1);
        }
    };
}
else {
    Function.Delegate.create = Function.createDelegate;
    Function.Handler.add = $addHandler;
    Function.Handler.remove = $removeHandler;
}

//#region Register Types

Artem.Google.Map.registerClass("Artem.Google.Map");
Artem.Google.MapState.registerClass("Artem.Google.MapState");

//#endregion