///<reference name="MicrosoftAjax.debug.js"/>
///<reference path="GoogleCommons.js"/>
//#region Info
// ------------------------------------------------------------------------------------------------
// Copyright (C) ArtemBG.
// ------------------------------------------------------------------------------------------------
// GoogleMap4.debug.js
// GoogleMap Control v5.0 javascipt library (debug).
//
// Assembly:    Artem.GooleMap
// Version:     5.0.0.0
// Project:     http://googlemap.codeplex.com
// Demo:        http://googlemap.artembg.com
// Author:      Velio Ivanov - velio@artembg.com
//              http://artembg.com
// License:     Microsoft Permissive License (Ms-PL) v1.1
//              http://www.codeplex.com/googlemap/license
// API:         http://code.google.com/apis/maps/
// Outlining:   VisualStudio 2010 JavaScript Outlining
//              http://jsoutlining.codeplex.com/
//#endregion

Type.registerNamespace("Artem.Google");
Type.registerNamespace("Artem.Google.Events");

//#region Map class ///////////////////////////////////////////////////////////////////////////////

Artem.Google.Map = function Artem_Google_Map(element) {
    /// <summary>This class represents the client GoogleMap control object.</summary>
    /// <field name="get_polygonEvents" type="Sys.EventHandlerList"></field>

    Artem.Google.Map.initializeBase(this, [element]);

    var self = this;

    var clientMapID = null;
    this.get_clientMapID = function () { return clientMapID; };
    this.set_clientMapID = function (value) { clientMapID = value; }

    var clientStateID = null;
    this.get_clientStateID = function () { return clientStateID; };
    this.set_clientStateID = function (value) { clientStateID = value; };

    var directionsEvents = null;
    this.get_directionsEvents = function (read) {
        if (directionsEvents == null && !read)
            directionsEvents = new Sys.EventHandlerList();
        return directionsEvents;
    };

    var loadDelegate = null;
    this.get_loadDelegate = function (getonly) {
        if (!loadDelegate && !getonly)
            loadDelegate = Function.createDelegate(self, self.load)
        return loadDelegate;
    };

    // BEGIN NEW 

    var map = null;
    this.get_map = function () { return map; };
    this.set_map = function (value) { map = value; };

    var geocoder = new google.maps.Geocoder();
    this.get_geocoder = function () { return geocoder; };

    // END NEW

    var mapEvents = null;
    this.get_mapEvents = function (read) {
        if (mapEvents == null && !read)
            mapEvents = new Sys.EventHandlerList();
        return mapEvents;
    };

    var mapPano = null;
    this.get_mapPano = function () { return mapPano; }
    this.set_mapPano = function (value) { mapPano = value; };

    var markerEvents = null;
    this.get_markerEvents = function (read) {
        if (markerEvents == null && !read)
            markerEvents = new Sys.EventHandlerList();
        return markerEvents;
    };

    var markerManager = null;
    this.get_markerManager = function () { return markerManager; };
    this.set_markerManager = function (value) { markerManager = value; }

    var name = null;
    this.get_name = function () { return name; };
    this.set_name = function (value) { name = value; };

    var partialUpdateDelegate = null;
    this.get_partialUpdateDelegate = function (getonly) {
        if (!partialUpdateDelegate && !getonly)
            partialUpdateDelegate = Function.createDelegate(self, self._onPartialUpdate)
        return partialUpdateDelegate;
    };

    var polygonEvents = null;
    this.get_polygonEvents = function (read) {
        if (polygonEvents == null && !read)
            polygonEvents = new Sys.EventHandlerList();
        return polygonEvents;
    };

    var polylineEvents = null;
    this.get_polylineEvents = function (read) {
        if (polylineEvents == null && !read)
            polylineEvents = new Sys.EventHandlerList();
        return polylineEvents;
    };

    var raiseServerEventDelegate = null;
    this.get_raiseServerEventDelegate = function (getonly) {
        if (!raiseServerEventDelegate && !getonly)
            raiseServerEventDelegate = Function.createDelegate(self, self._raiseServerEvent);
        return raiseServerEventDelegate;
    };

    var submitDelegate = null;
    this.get_submitDelegate = function (getonly) {
        if (!submitDelegate && !getonly)
            submitDelegate = Function.createDelegate(self, self._onSubmit);
        return submitDelegate;
    };

    // TODO check if this is needed
    //        var renderMarkerManagerDelegate = null;
};

Artem.Google.Map.prototype = {

    //#> Fields 

    Address: null,
    Center: null,
    Language: null,
    Region: null,
    Zoom: null,

    Bounds: null,
    DefaultAddress: null,
    DisableClear: false,
    DisableDefaultUI: false,
    DisableDoubleClickZoom: false,
    DisableKeyboardShortcuts: false,
    Draggable: true,
    DraggableCursor: null,
    DraggingCursor: null,
    EnableReverseGeocoding: false,
    EnableScrollWheelZoom: true,
    MapType: null,
    MapTypeControlOptions: null,
    NavigationControlOptions: null,
    ScaleControlOptions: null,
    ShowMapTypeControl: true,
    ShowNavigationControl: true,
    ShowScaleControl: true,
    ShowStreetViewControl: true,
    StreetView: null,

    Markers: [],
    Polygons: [],
    Polylines: [],







    ClientID: null,
    ClientMapID: null,
    ClientStateID: null,
    EnableContinuousZoom: null,

    EnableGoogleBar: null,
    EnableInfoWindow: null,
    EnableMarkerManager: null,
    EnterpriseKey: null,
    Height: null,
    IsGeolocation: false,
    IsLoaded: false,
    IsStatic: null,
    MarkerManagerOptions: null,
    ShowTraffic: null,
    Width: null,
    // collections
    Actions: [],
    Directions: [],


    //    // BEGIN OBSOLETE
    //    // events
    //    ClientEvents: null,
    //    ServerEvents: null,
    //    MarkerEvents: null,
    //    PolygonEvents: null,
    //    PolylineEvents: null,

    //    // END OBSOLETE

    //#endregion

    //#> Properties 

    get_clientMapID: null,
    set_clientMapID: null,
    get_clientStateID: null,
    set_clientStateID: null,
    get_directionsEvents: null,
    get_geocoder: null,
    get_loadDelegate: null,
    get_map: null,
    set_map: null,
    get_mapEvents: null,
    get_mapPano: null,
    set_mapPano: null,
    get_markerEvents: null,
    get_markerManager: null,
    set_markerManager: null,
    get_name: null,
    set_name: null,
    get_partialUpdateDelegate: null,
    get_polygonEvents: null,
    get_polylineEvents: null,
    get_raiseServerEventDelegate: null,
    get_submitDelegate: null,

    //#<

    //#> Base 

    initialize: function Artem_Google_Map$initialize() {
        Artem.Google.Map.callBaseMethod(this, 'initialize');
        var self = this;

        eval("window." + this.get_clientMapID() + " = this;");
        this._loadState();

        // create map
        if (this.Center) {
            this._createMap();
            // if reverse geocoding is enabled then try resolve the address
            if (this.EnableReverseGeocoding) {
                var location = new google.maps.LatLng(this.Center.Latitude, this.Center.Longitude);
                this.geocodeLocation(location, function (address) { self.Address = address; });
                //                var geocoder = this.get_geocoder();
                //                var request = this.getGeocodeRequest(true);
                //                geocoder.geocode(request, function (results, status) {
                //                    if (status == google.maps.GeocoderStatus.OK) {
                //                        if (results.length > 0) {
                //                            self.Address = results[0].formatted_address;
                //                            //                            options.center = new google.maps.LatLng(this.Center.Latitude, this.Center.Longitude);
                //                        }
                //                    }
                //                    else {
                //                        self._handleGeocodeError(status);
                //                    }
                //                });
            }
        }
        else {
            this.geocodeAddress(this.Address, function (location) { self._createMap(location); });
            //            var geocoder = this.get_geocoder();
            //            var request = this.getGeocodeRequest(false);
            //            geocoder.geocode(request, function (results, status) {
            //                if (status == google.maps.GeocoderStatus.OK) {
            //                    self._createMap(results[0].geometry.location);
            //                }
            //                else {
            //                    self._handleGeocodeError(status);
            //                }
            //            });
        }
        //        if (!this.IsStatic && !(this.IsStreetView && this.StreetViewMode == 0)) {
        //            var options = {
        //                center: new google.maps.LatLng(this.Center.Latitude, this.Center.Longitude),
        //                mapTypeId: google.maps.MapTypeId.ROADMAP,
        //                zoom: this.Zoom
        //            };
        //            //            if (this.Width && this.Height)
        //            //                options = { size: new google.maps.Size(this.Width, this.Height) };
        //            this.set_map(new google.maps.Map(this.get_element(), options));
        //        }
        ////        // create geocoder
        ////        //        var geocoder = new GClientGeocoder();
        ////        this.get_geocoder = function () { return geocoder; };

        ////        this._attachEvents();
        ////        this.load();
    },

    dispose: function Artem_Google_Map$dispose() {

        this.clearMarkers();
        this.clearDirections();
        this.clearPolygons();
        this.clearPolylines()

        this._detachEvents();

        //#region properties

        delete this.get_clientMapID;
        delete this.get_clientStateID;
        delete this.get_geocoder;
        delete this.get_loadDelegate;
        delete this.get_map;
        delete this.get_mapEvents;
        delete this.get_mapPano;
        delete this.get_markerManager;
        delete this.get_name;
        delete this.get_partialUpdateDelegate;
        delete this.get_polygonEvents;
        delete this.get_polylineEvents;
        delete this.get_raiseServerEventDelegate;
        delete this.get_submitDelegate;

        //#endregion

        //#region fields 
        delete this.Address;
        delete this.Center;
        delete this.Language;
        delete this.Region;
        delete this.Zoom;

        delete this.Bounds;
        delete this.DefaultAddress;
        delete this.DisableClear;
        delete this.DisableDefaultUI;
        delete this.DisableDoubleClickZoom;
        delete this.DisableKeyboardShortcuts;
        delete this.Draggable;
        delete this.DraggableCursor;
        delete this.DraggingCursor;
        delete this.EnableReverseGeocoding;
        delete this.EnableScrollWheelZoom;
        delete this.MapType;
        delete this.MapTypeControlOptions;
        delete this.NavigationControlOptions;
        delete this.ScaleControlOptions;
        delete this.ShowMapTypeControl;
        delete this.ShowNavigationControl;
        delete this.ShowScaleControl;
        delete this.ShowStreetViewControl;
        delete this.StreetView;

        delete this.Markers;
        delete this.Polygons;
        delete this.Polylines;







        delete this.Actions;
        delete this.ClentAddressNotFoundIndex;
        delete this.ClientEvents;
        delete this.ClientID;
        delete this.ClientMapID;
        delete this.ClientStateID;
        delete this.Directions;
        delete this.EnableContinuousZoom;
        delete this.EnableGoogleBar;
        delete this.EnableInfoWindow;
        delete this.EnableMarkerManager;
        delete this.EnterpriseKey;
        delete this.Height;
        delete this.IsGeolocation;
        delete this.IsLoaded;
        delete this.IsStatic;
        delete this.MarkerEvents;
        delete this.MarkerManagerOptions;
        delete this.PolygonEvents;
        delete this.PolylineEvents;
        delete this.ServerEvents;
        delete this.ShowTraffic;
        delete this.Width;
        //#endregion

        Artem.Google.Map.callBaseMethod(this, 'dispose');
    },
    //#endregion

    //#> State 

    _loadState: function Artem_Google_Map$_loadState() {

        var stateField = $get(this.get_clientStateID());
        if (stateField) {
            var stateContent = stateField.value;
            if (stateContent == 'undefined' || stateContent == '') return;
            var state = Sys.Serialization.JavaScriptSerializer.deserialize(stateContent, true);

            this.Address = state.Address;
            this.Center = state.Center;
            this.Language = state.Language;
            this.Region = state.Region;
            this.Zoom = state.Zoom;

            this.Bounds = state.Bounds;
            this.DefaultAddress = state.DefaultAddress;
            this.DisableClear = state.DisableClear;
            this.DisableDefaultUI = state.DisableDefaultUI;
            this.DisableDoubleClickZoom = state.DisableDoubleClickZoom;
            this.DisableKeyboardShortcuts = state.DisableKeyboardShortcuts;
            this.Draggable = state.Draggable;
            this.DraggableCursor = state.DraggableCursor;
            this.DraggingCursor = state.DraggingCursor;
            this.EnableReverseGeocoding = state.EnableReverseGeocoding;
            this.EnableScrollWheelZoom = state.EnableScrollWheelZoom;
            this.MapType = state.MapType;
            this.MapTypeControlOptions = state.MapTypeControlOptions;
            this.NavigationControlOptions = state.NavigationControlOptions;
            this.ScaleControlOptions = state.ScaleControlOptions;
            this.ShowMapTypeControl = state.ShowMapTypeControl;
            this.ShowNavigationControl = state.ShowNavigationControl;
            this.ShowScaleControl = state.ShowScaleControl;
            this.ShowStreetViewControl = state.ShowStreetViewControl;
            this.StreetView = state.StreetView;

            this.Markers = state.Markers;
            this.Polygons = state.Polygons;
            this.Polylines = state.Polylines;







            this.Directions = state.Directions;
            this.EnableContinuousZoom = state.EnableContinuousZoom;
            this.EnableGoogleBar = state.EnableGoogleBar;
            this.EnableInfoWindow = state.EnableInfoWindow;
            this.EnableMarkerManager = state.EnableMarkerManager;
            this.EnterpriseKey = state.EnterpriseKey;
            this.Height = state.Height;
            this.IsStatic = state.IsStatic;
            this.ShowTraffic = state.ShowTraffic;
            this.Width = state.Width;

            //        // events
            //        if (config.MapEvents) {
            //            this.ClientEvents = config.MapEvents.ClientEvents;
            //            this.ServerEvents = config.MapEvents.ServerEvents;
            //        }
            //        this.MarkerEvents = config.MarkerEvents;
            //        this.PolygonEvents = config.PolygonEvents;
            //        this.PolylineEvents = config.PolylineEvents;
        }
    },

    _saveState: function Artem_Google_Map$_saveState() {

        var stateField = $get(this.get_clientStateID());
        if (stateField) {
            var i;
            var center = this.getCenter();

            this.Bounds = new Artem.Google.Bounds(this.getBounds());
            this.Center = {
                Latitude: (center !== null) ? center.lat() : 0,
                Longitude: (center !== null) ? center.lng() : 0
            };
            this.Zoom = this.getZoom() || 0;

            if (this.Markers) {
                for (i = 0; i < this.Markers.length; i++) {
                    this.Markers[i].save();
                }
            }
            if (this.Directions) {
                for (i = 0; i < this.Directions.length; i++) {
                    this.Directions[i].save();
                }
            }
            if (this.Polygons) {
                for (i = 0; i < this.Polygons.length; i++) {
                    this.Polygons[i].save();
                }
            }
            if (this.Polylines) {
                for (i = 0; i < this.Polylines.length; i++) {
                    this.Polylines[i].save();
                }
            }

            var state = new Artem.Google.State(this);
            stateField.value = Sys.Serialization.JavaScriptSerializer.serialize(state);
        }
    },

    //#<

    //#> Create/Render Map 

    _createMap: function Artem_Google_Map$_createMap(location) {

        if (location)
            this.Center = new Artem.Google.Location(location);
        else
            location = new google.maps.LatLng(this.Center.Latitude, this.Center.Longitude);

        var options = {
            center: null,
            mapTypeId: Artem.Google.Converter.mapTypeId(this.MapType),
            zoom: this.Zoom,
            disableDefaultUI: this.DisableDefaultUI,
            disableDoubleClickZoom: this.DisableDoubleClickZoom,
            draggable: this.Draggable,
            draggableCursor: this.DraggableCursor,
            draggingCursor: this.DraggingCursor,
            keyboardShortcuts: !this.DisableKeyboardShortcuts,
            mapTypeControl: this.ShowMapTypeControl,
            navigationControl: this.ShowNavigationControl,
            noClear: this.DisableClear,
            scaleControl: this.ShowScaleControl,
            scaleControlOptions: this.ScaleControlOptions,
            scrollwheel: this.EnableScrollWheelZoom,
            streetViewControl: this.ShowStreetViewControl
        };

        if (location)
            this.Center = new Artem.Google.Location(location);
        else
            location = new google.maps.LatLng(this.Center.Latitude, this.Center.Longitude);

        options.center = location;

        if (this.MapTypeControlOptions)
            options.mapTypeControlOptions = Artem.Google.Converter.mapTypeControlOptions(this.MapTypeControlOptions);
        if (this.NavigationControlOptions)
            options.navigationControlOptions = Artem.Google.Converter.navigationControlOptions(this.NavigationControlOptions);
        if (this.StreetView)
            options.streetView = Artem.Google.Converter.streetView(this.StreetView, location);

        var map = new google.maps.Map(this.get_element(), options);
        this.set_map(map);

        this._attachEvents();
        this._render();
    },

    _render: function Artem_Google_Map$_render() {

        var i, name, handler;
        // markers
        if (this.Markers) {
            for (i = 0; i < this.Markers.length; i++) {
                this.Markers[i] = new Artem.Google.Marker(this, i, this.Markers[i]);
                // attach events
                for (name in this.get_markerEvents()._list) {
                    handler = this.get_markerEvents().getHandler(name);
                    google.maps.event.addListener(this.Markers[i].get_marker(), name, handler);
                }
            }
        }
        // directions
        if (this.Directions) {
            for (i = 0; i < this.Directions.length; i++) {
                this.Directions[i] = new Artem.Google.Directions(this, this.Directions[i]);
                // attach events
                for (name in this.get_directionsEvents()._list) {
                    handler = this.get_directionsEvents().getHandler(name);
                    google.maps.event.addListener(this.Directions[i].get_directions(), name, handler);
                }
            }
        }
        // polygons
        if (this.Polygons) {
            for (i = 0; i < this.Polygons.length; i++) {
                this.Polygons[i] = new Artem.Google.Polygon(this, this.Polygons[i]);
                // attach events
                for (name in this.get_polygonEvents()._list) {
                    handler = this.get_polygonEvents().getHandler(name);
                    google.maps.event.addListener(this.Polygons[i].get_polygon(), name, handler);
                }
            }
        }
        // polylines
        if (this.Polylines) {
            for (i = 0; i < this.Polylines.length; i++) {
                this.Polylines[i] = new Artem.Google.Polyline(this, this.Polylines[i]);
                // attach events
                for (name in this.get_polylineEvents()._list) {
                    handler = this.get_polylineEvents().getHandler(name);
                    google.maps.event.addListener(this.Polylines[i].get_polyline(), name, handler);
                }
            }
        }
        //        // fire actions
        //        if (this.Actions) {
        //            for (var i = 0; i < this.Actions.length; i++) {
        //                Function.Delegate.callFromString(this, this.Actions[i]);
        //            }
        //        }
    },

    //#> TODO
    //    load: function Artem_Google_Map$load(point) {
    //        if (point) {
    //            if (!this.IsStatic && !(this.IsStreetView && this.StreetViewMode == 0)) {
    //                this.Latitude = point.lat();
    //                this.Longitude = point.lng();
    //                this.setCenter(point, this.Zoom);
    //                if (this.IsGeolocation) {
    //                    this.IsGeolocation = false;
    //                    this._raiseGeoLoad(this.Address);
    //                }
    //                if (this.EnableReverseGeocoding && !this.Address) {
    //                    var delegate = Function.Delegate.create(this, this.setAddress);
    //                    this.get_geocoder().getLocations(point, delegate);
    //                }
    //                this.preRender();
    //                this.render();
    //                //                this.checkResize();
    //            }
    //            else if (this.IsStreetView) {
    //                this.loadStreetView(point);
    //            }
    //            else {
    //                this.loadStatic();
    //            }
    //            this.IsLoaded = true;
    //        }
    //        else {
    //            if ((this.Latitude !== 0) && (this.Longitude !== 0))
    //                this.load(new google.maps.LatLng(this.Latitude, this.Longitude));
    //            else {
    //                if (!this.IsGeolocation) {
    //                    this.IsGeolocation = true;
    //                    this.get_geocoder().getLatLng(this.Address, this.get_loadDelegate());
    //                }
    //            }
    //        }
    //    },

    //    loadAddress: function Artem_Google_Map$loadAddress(address) {
    //        this.Address = address;
    //        this.IsGeolocation = true;
    //        this.get_geocoder().getLatLng(this.Address, this.get_loadDelegate());
    //    },

    //    loadStatic: function Artem_Google_Map$loadStatic() {
    //        var el = this.get_element();
    //        //
    //        var width = 512;
    //        if (this.Didth && this.Width < 512) width = this.Width;
    //        var height = 512;
    //        if (this.Height && this.Height < 512) height = this.Height;
    //        //
    //        var src = "http:\/\/maps.google.com\/staticmap?";
    //        src += "center=" + this.Latitude + "," + this.Longitude + "&";
    //        src += "zoom=" + this.Zoom + "&";
    //        src += "size=" + width + "x" + height + "&";
    //        if (this.EnterpriseKey)
    //            src += "enterpriseKey=" + this.EnterpriseKey + "&";
    //        src += "key=" + this.Key;
    //        // markers
    //        if (this.Markers) {
    //            var i;
    //            src += "&markers=";
    //            for (i = 0; i < this.Markers.length; i++)
    //                src += this.Markers[i].Latitude + "," + this.Markers[i].Longitude + "|";
    //        }
    //        // 
    //        var img = document.createElement("img");
    //        img.src = src;
    //        el.appendChild(img);
    //    },

    //    loadStreetView: function Artem_Google_Map$loadStreetView(point) {
    //        var map = new GStreetviewPanorama(this.get_element(), { latlng: point });
    //        map.checkResize();
    //        this.set_map(map);
    //        //        google.maps.event.addListener(this.GMapPano, "error", function() {
    //        //            if (errorCode == 603) {
    //        //                alert("Error: Flash doesn't appear to be supported by your browser");
    //        //                return;
    //        //            }
    //        //        });
    //    },
    //#<

    //#<

    //#> Private Methods 

    _attachEvents: function Artem_Google_Map$_attachEvents() {

        if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") {
            var requestManager = Sys.WebForms.PageRequestManager.getInstance();
            if (requestManager) {
                Array.add(requestManager._onSubmitStatements, this.get_submitDelegate());
                requestManager.add_endRequest(this.get_partialUpdateDelegate());
            }
        }
        else {
            $addHandler(document.forms[0], "submit", this.get_submitDelegate());
        }

        // map events
        var name, handler;
        var events = this.get_mapEvents();
        var map = this.get_map();
        for (name in events._list) {
            handler = events.getHandler(name);
            google.maps.event.addListener(map, name, handler);
        }
    },

    _detachEvents: function Artem_Google_Map$_detachEvents() {

        var delegate;
        if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") {
            var requestManager = Sys.WebForms.PageRequestManager.getInstance();
            if (requestManager) {
                delegate = this.get_partialUpdateDelegate(true);
                if (delegate) {
                    requestManager.remove_endRequest(delegate);
                }
                delegate = this.get_submitDelegate(true);
                if (delegate) {
                    Array.remove(requestManager._onSubmitStatements, delegate);
                }
            }
        }
        else {
            delegate = this.get_submitDelegate(true);
            if (delegate) {
                $removeHandler(document.forms[0], "submit", delegate);
            }
        }

        // remove map event handlers
        var map = this.get_map();
        if (map) google.maps.event.clearInstanceListeners(map);
    },

    _handleGeocodeError: function Artem_Google_Map$_handleGeocodeError(status) {

        if (!this.Center) {
            var geocoder = this.get_geocoder();
            var request = { 'address': this.DefaultAddress };
            var self = this;

            if (this.Language)
                request.language = this.Language;
            if (this.Region)
                request.region = this.Region;

            geocoder.geocode(request, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    self._createMap(results[0].geometry.location);
                }
            });
        }
    },

    _onPartialUpdate: function Artem_Google_Map$_onPartialUpdate() {
        this._loadState();
        return true;
    },

    _onSubmit: function Artem_Google_Map$_onSubmit() {
        this._saveState();
        return true;
    },

    _raiseServerEvent: function Artem_Google_Map$_raiseServerEvent() {

        if (arguments.length > 0) {
            var index = arguments.length - 1;
            var entry = arguments[index];
            var name = entry.name;
            var type = entry.type;
            var data = type + ":" + name;
            var args;

            switch (name) {
                case "addressnotfound":
                case "geoload":
                case "locationloaded":
                    args = new Artem.Google.Events.AddressEventArgs(arguments[0]);
                    break;
                case "click":
                case "dblclick":
                    if (type == "map")
                    // overlay argument passed is not used so far
                        args = new Artem.Google.Events.LocationEventArgs(arguments[1]);
                    else
                        args = new Artem.Google.Events.LocationEventArgs(arguments[0]);
                    break;
                case "dragend":
                case "dragstart":
                    args = new Artem.Google.Events.BoundsEventArgs(this.getBounds());
                    break;
                case "mousemove":
                case "mouseover":
                case "mouseout":
                case "singlerightclick":
                    args = new Artem.Google.Events.LocationEventArgs(arguments[0]);
                    break;
                case "zoomend":
                    args = Artem.Google.Events.ZoomEventArgs(arguments[0], arguments[1]);
                    break;
                case "addmaptype":
                case "addoverlay":
                case "clearoverlays":
                case "drag":
                case "infowindowbeforeclose":
                case "infowindowclose":
                case "infowindowopen":
                case "load":
                case "maptypechanged":
                case "move":
                case "moveend":
                case "movestart":
                case "removemaptype":
                case "removeoverlay":
                    // overlay argument passed is not used so far
                    break;
            }

            if (args)
                data += "$" + Sys.Serialization.JavaScriptSerializer.serialize(args);
            __doPostBack(this.get_name(), data);
        }
    },

    _validateHandler: function Artem_Google_Map$_validateHandler(type, name, handler) {
        return (handler == Artem.Google.serverHandler)
            ? Function.createCallback(this.get_raiseServerEventDelegate(), { name: name, type: type })
            : handler;
    },

    //#endregion

    //#> Public Methods 

    addDirections: function Artem_Google_Map$addDirections(state) {
        if (!this.Directions) this.Directions = new Array();
        this.Directions.push(new Artem.Google.Directions(this, state));
    },

    addMarker: function Artem_Google_Map$addMarker(state) {
        if (!this.Markers) this.Markers = new Array();
        this.Markers.push(new Artem.Google.Marker(this, state));
    },

    addPolygon: function Artem_Google_Map$addPolygon(state) {
        if (!this.Polygons) this.Polygons = new Array();
        this.Polygons.push(new Artem.Google.Polygon(this, state));
    },

    addPolyline: function Artem_Google_Map$addPolyline(state) {
        if (!this.Polylines) this.Polylines = new Array();
        this.Polylines.push(new Artem.Google.Polyline(this, state));
    },

    clearDirections: function Artem_Google_Map$clearDirections() {

        if (this.Directions) {
            for (var i = 0; i < this.Directions.length; i++) {
                this.Directions[i].dispose();
            }
            this.Directions = new Array();
        }
    },

    clearMarkers: function Artem_Google_Map$clearMarkers() {

        if (this.Markers) {
            for (var i = 0; i < this.Markers.length; i++) {
                this.Markers[i].dispose();
            }
            this.Markers = new Array();
        }
    },

    clearPolygons: function Artem_Google_Map$clearPolygons() {

        if (this.Polygons) {
            for (var i = 0; i < this.Polygons.length; i++) {
                this.Polygons[i].dispose();
            }
            this.Polygons = new Array();
        }
    },

    clearPolylines: function Artem_Google_Map$clearPolylines() {

        if (this.Polylines) {
            for (var i = 0; i < this.Polylines.length; i++) {
                this.Polylines[i].dispose();
            }
            this.Polylines = new Array();
        }
    },

    getGeocodeRequest: function Artem_Google_Map$getGeocodeRequest(latlng, address) {

        return {
            latLng: latlng,
            address: address,
            language: this.Language,
            region: this.Region
        };
    },

    geocodeAddress: function Artem_Google_Map$geocodeAddress(address, callback) {

        var geocoder = this.get_geocoder();
        var request = this.getGeocodeRequest(null, address);

        geocoder.geocode(request, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                try {
                    var location;
                    if (results && results.length)
                        location = results[0].geometry.location;
                    callback(location);
                }
                catch (ex) {
                }
            }
            else {
                self._handleGeocodeError(status);
            }
        });
    },

    geocodeLocation: function Artem_Google_Map$geocodeLocation(latlng, callback) {

        var geocoder = this.get_geocoder();
        var request = this.getGeocodeRequest(latlng, null);
        geocoder.geocode(request, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                try {
                    var address;
                    if (results && results.length)
                        address = results[0].formatted_address;
                    callback(address);
                }
                catch (ex) {
                }
            }
            else {
                self._handleGeocodeError(status);
            }
        });
    },

    preRender: function Artem_Google_Map$preRender() {
        // behaviour
        //        (this.EnableContinuousZoom) ? this.enableContinuousZoom() : this.disableContinuousZoom();
        //        (this.EnableDoubleClickZoom) ? this.enableDoubleClickZoom() : this.disableDoubleClickZoom();
        //        (this.EnableDragging) ? this.enableDragging() : this.disableDragging();
        //        (this.EnableGoogleBar) ? this.enableGoogleBar() : this.disableGoogleBar();
        //        (this.EnableInfoWindow) ? this.enableInfoWindow() : this.disableInfoWindow();
        //        (this.EnableScrollWheelZoom) ? this.enableScrollWheelZoom() : this.disableScrollWheelZoom();
        // controls
        var map = this.get_map();
        //        switch (this.ZoomPanType) {
        //            case 1:
        //                map.addControl(new GLargeMapControl());
        //                break;
        //            case 2:
        //                map.addControl(new GSmallZoomControl());
        //                break;
        //            case 3:
        //                map.addControl(new GSmallZoomControl3D());
        //                break;
        //            case 4:
        //                map.addControl(new GLargeMapControl3D());
        //                break;
        //            default:
        //                map.addControl(new GSmallMapControl());
        //                break;
        //        }
        //        if (this.ShowMapTypeControl) map.addControl(new GMapTypeControl());
        //        if (this.ShowScaleControl) map.addControl(new GScaleControl());
        //        if (this.ShowTraffic) map.addOverlay(new GTrafficOverlay());
        //        // enable marker manager
        //        if (this.EnableMarkerManager)
        //            this.set_markerManager(new MarkerManager(map, this.MarkerManagerOptions));
        // map view
        this.setMapView();
        // street view
        if (this.IsStreetView && this.StreetViewMode == 1) {
            var panoID = this.StreetViewPanoID || (this.ClientID + "_Pano");
            this.set_mapPano(new GStreetviewPanorama(document.getElementById(panoID)));
            map.addOverlay(new GStreetviewOverlay());
            google.maps.event.addListener(map, "click", Function.Delegate.create(this, this.setStreetView));
            //            function(overlay, latlng) {
            //                pano.setLocationAndPOV(latlng);
            //            });
        }
    },

    removeDirections: function Artem_Google_Map$removeDirections(index) {
        var items = this.Directions.splice(index, 1)
        if (items && items.length) items[0].dispose();
    },

    removeMarker: function Artem_Google_Map$removeMarker() {
        var items = this.Markers.splice(index, 1)
        if (items && items.length) items[0].dispose();
    },

    removePolygon: function Artem_Google_Map$removePolygon() {
        var items = this.Polygons.splice(index, 1)
        if (items && items.length) items[0].dispose();
    },

    removePolyline: function Artem_Google_Map$removePolyline() {
        var items = this.Polylines.splice(index, 1)
        if (items && items.length) items[0].dispose();
    },

    renderMarkerManager: function Artem_Google_Map$renderMarkerManager() {
        if (this.EnableMarkerManager) {
            var marker;
            for (var i = 0; i < this.Markers.length; i++) {
                marker = this.Markers[i];
                this.get_markerManager().addMarker(marker.GMarker, marker.MinZoom, marker.MaxZoom);
            }
            this.get_markerManager().refresh();
        }
    },

    setAddress: function Artem_Google_Map$setAddress(addresses) {
        if (addresses.Status.code == 200) {
            try {
                this.Address = addresses.Placemark[0].address;
                this._raiseLocationLoaded(this.Address);
            }
            catch (ex) { }
        }
    },

    setStreetView: function Artem_Google_Map$setStreetView(overlay, latlng) {
        this.get_mapPano().setLocationAndPOV(latlng);
    },
    //#endregion

    //#> Events 

    //#region MapEvents 

    // addmaptype event
    add_addmaptype: function Artem_Google_Map$add_addmaptype(handler) {
        this.get_mapEvents().addHandler("addmaptype", this._validateHandler("map", "addmaptype", handler));
    },
    remove_addmaptype: function Artem_Google_Map$remove_addmaptype(handler) {
        this.get_mapEvents().removeHandler("addmaptype", handler);
    },

    // addoverlay event
    add_addoverlay: function Artem_Google_Map$add_addoverlay(handler) {
        this.get_mapEvents().addHandler("addoverlay", this._validateHandler("map", "addoverlay", handler));
    },
    remove_addoverlay: function Artem_Google_Map$remove_addoverlay(handler) {
        this.get_mapEvents().removeHandler("addoverlay", handler);
    },

    // addressnotfound
    add_addressnotfound: function Artem_Google_Map$add_addressnotfound(handler) {
        this.get_events().addHandler("addressnotfound", this._validateHandler("map", "addressnotfound", handler));
    },
    remove_addressnotfound: function Artem_Google_Map$remove_addressnotfound(handler) {
        this.get_events().removeHandler("addressnotfound", handler);
    },
    _raiseAddressNotFound: function Artem_Google_Map$_raiseAddressNotFound(address) {
        var handler = this.get_events().getHandler('addressnotfound');
        if (handler) handler(address);
    },

    // clearoverlays event
    add_clearoverlays: function Artem_Google_Map$add_clearoverlays(handler) {
        this.get_mapEvents().addHandler("clearoverlays", this._validateHandler("map", "clearoverlays", handler));
    },
    remove_clearoverlays: function Artem_Google_Map$remove_clearoverlays(handler) {
        this.get_mapEvents().removeHandler("clearoverlays", handler);
    },

    // click event
    add_click: function Artem_Google_Map$add_click(handler) {
        this.get_mapEvents().addHandler("click", this._validateHandler("map", "click", handler));
    },
    remove_click: function Artem_Google_Map$remove_click(handler) {
        this.get_mapEvents().removeHandler("click", handler);
    },

    // dblclick event
    add_dblclick: function Artem_Google_Map$add_dblclick(handler) {
        this.get_mapEvents().addHandler("dblclick", this._validateHandler("map", "dblclick", handler));
    },
    remove_dblclick: function Artem_Google_Map$remove_dblclick(handler) {
        this.get_mapEvents().removeHandler("dblclick", handler);
    },

    // drag event
    add_drag: function Artem_Google_Map$add_drag(handler) {
        this.get_mapEvents().addHandler("drag", this._validateHandler("map", "drag", handler));
    },
    remove_drag: function Artem_Google_Map$remove_drag(handler) {
        this.get_mapEvents().removeHandler("drag", handler);
    },

    // dragend event
    add_dragend: function Artem_Google_Map$add_dragend(handler) {
        this.get_mapEvents().addHandler("dragend", this._validateHandler("map", "dragend", handler));
    },
    remove_dragend: function Artem_Google_Map$remove_dragend(handler) {
        this.get_mapEvents().removeHandler("dragend", handler);
    },

    // dragstart event
    add_dragstart: function Artem_Google_Map$add_dragstart(handler) {
        this.get_mapEvents().addHandler("dragstart", this._validateHandler("map", "dragstart", handler));
    },
    remove_dragstart: function Artem_Google_Map$remove_dragstart(handler) {
        this.get_mapEvents().removeHandler("dragstart", handler);
    },

    // geoload event
    add_geoload: function Artem_Google_Map$add_geoload(handler) {
        this.get_events().addHandler("geoload", this._validateHandler("map", "geoload", handler));
    },
    remove_geoload: function Artem_Google_Map$remove_geoload(handler) {
        this.get_events().removeHandler("geoload", handler);
    },
    _raiseGeoLoad: function Artem_Google_Map$_raisegeoload(address) {
        var handler = this.get_events().getHandler('geoload');
        if (handler) handler(address);
    },

    // infowindowbeforeclose event
    add_infowindowbeforeclose: function Artem_Google_Map$add_infowindowbeforeclose(handler) {
        this.get_mapEvents().addHandler("infowindowbeforeclose", this._validateHandler("map", "infowindowbeforeclose", handler));
    },
    remove_infowindowbeforeclose: function Artem_Google_Map$remove_infowindowbeforeclose(handler) {
        this.get_mapEvents().removeHandler("infowindowbeforeclose", handler);
    },

    // infowindowclose event
    add_infowindowclose: function Artem_Google_Map$add_infowindowclose(handler) {
        this.get_mapEvents().addHandler("infowindowclose", this._validateHandler("map", "infowindowclose", handler));
    },
    remove_infowindowclose: function Artem_Google_Map$remove_infowindowclose(handler) {
        this.get_mapEvents().removeHandler("infowindowclose", handler);
    },

    // infowindowopen event
    add_infowindowopen: function Artem_Google_Map$add_infowindowopen(handler) {
        this.get_mapEvents().addHandler("infowindowopen", this._validateHandler("map", "infowindowopen", handler));
    },
    remove_infowindowopen: function Artem_Google_Map$remove_infowindowopen(handler) {
        this.get_mapEvents().removeHandler("infowindowopen", handler);
    },

    // load event
    add_load: function Artem_Google_Map$add_load(handler) {
        this.get_mapEvents().addHandler("load", this._validateHandler("map", "load", handler));
    },
    remove_load: function Artem_Google_Map$remove_load(handler) {
        this.get_mapEvents().removeHandler("load", handler);
    },

    // locationloaded event
    add_locationloaded: function Artem_Google_Map$add_locationloaded(handler) {
        this.get_events().addHandler("locationloaded", this._validateHandler("map", "locationloaded", handler));
    },
    remove_locationloaded: function Artem_Google_Map$remove_locationloaded(handler) {
        this.get_events().removeHandler("locationloaded", handler);
    },
    _raiseLocationLoaded: function Artem_Google_Map$_raiseLocationLoaded(address) {
        var handler = this.get_events().getHandler('locationloaded');
        if (handler) handler(address);
    },

    // maptypechanged event
    add_maptypechanged: function Artem_Google_Map$add_maptypechanged(handler) {
        this.get_mapEvents().addHandler("maptypechanged", this._validateHandler("map", "maptypechanged", handler));
    },
    remove_maptypechanged: function Artem_Google_Map$remove_maptypechanged(handler) {
        this.get_mapEvents().removeHandler("maptypechanged", handler);
    },

    // mousedown event
    add_mousedown: function Artem_Google_Map$add_mousedown(handler) {
        this.get_mapEvents().addHandler("mousedown", this._validateHandler("map", "mousedown", handler));
    },
    remove_mousedown: function Artem_Google_Map$remove_mousedown(handler) {
        this.get_mapEvents().removeHandler("mousedown", handler);
    },

    // mousemove event
    add_mousemove: function Artem_Google_Map$add_mousemove(handler) {
        this.get_mapEvents().addHandler("mousemove", this._validateHandler("map", "mousemove", handler));
    },
    remove_mousemove: function Artem_Google_Map$remove_mousemove(handler) {
        this.get_mapEvents().removeHandler("mousemove", handler);
    },

    // mouseout event
    add_mouseout: function Artem_Google_Map$add_mouseout(handler) {
        this.get_mapEvents().addHandler("mouseout", this._validateHandler("map", "mouseout", handler));
    },
    remove_mouseout: function Artem_Google_Map$remove_mouseout(handler) {
        this.get_mapEvents().removeHandler("mouseout", handler);
    },

    // mouseover event
    add_mouseover: function Artem_Google_Map$add_mouseover(handler) {
        this.get_mapEvents().addHandler("mouseover", this._validateHandler("map", "mouseover", handler));
    },
    remove_mouseover: function Artem_Google_Map$remove_mouseover(handler) {
        this.get_mapEvents().removeHandler("mouseover", handler);
    },

    // mouseup event
    add_mouseup: function Artem_Google_Map$add_mouseup(handler) {
        this.get_mapEvents().addHandler("mouseup", this._validateHandler("map", "mouseup", handler));
    },
    remove_mouseup: function Artem_Google_Map$remove_mouseup(handler) {
        this.get_mapEvents().removeHandler("mouseup", handler);
    },

    // move event
    add_move: function Artem_Google_Map$add_move(handler) {
        this.get_mapEvents().addHandler("move", this._validateHandler("map", "move", handler));
    },
    remove_move: function Artem_Google_Map$remove_move(handler) {
        this.get_mapEvents().removeHandler("move", handler);
    },

    // moveend event
    add_moveend: function Artem_Google_Map$add_moveend(handler) {
        this.get_mapEvents().addHandler("moveend", this._validateHandler("map", "moveend", handler));
    },
    remove_moveend: function Artem_Google_Map$remove_moveend(handler) {
        this.get_mapEvents().removeHandler("moveend", handler);
    },

    // movestart event
    add_movestart: function Artem_Google_Map$add_movestart(handler) {
        this.get_mapEvents().addHandler("movestart", this._validateHandler("map", "movestart", handler));
    },
    remove_movestart: function Artem_Google_Map$remove_movestart(handler) {
        this.get_mapEvents().removeHandler("movestart", handler);
    },

    // removemaptype event
    add_removemaptype: function Artem_Google_Map$add_removemaptype(handler) {
        this.get_mapEvents().addHandler("removemaptype", this._validateHandler("map", "removemaptype", handler));
    },
    remove_removemaptype: function Artem_Google_Map$remove_removemaptype(handler) {
        this.get_mapEvents().removeHandler("removemaptype", handler);
    },

    // removeoverlay event
    add_removeoverlay: function Artem_Google_Map$add_removeoverlay(handler) {
        this.get_mapEvents().addHandler("removeoverlay", this._validateHandler("map", "removeoverlay", handler));
    },
    remove_removeoverlay: function Artem_Google_Map$remove_removeoverlay(handler) {
        this.get_mapEvents().removeHandler("removeoverlay", handler);
    },

    // singlerightclick event
    add_singlerightclick: function Artem_Google_Map$add_singlerightclick(handler) {
        this.get_mapEvents().addHandler("singlerightclick", this._validateHandler("map", "singlerightclick", handler));
    },
    remove_singlerightclick: function Artem_Google_Map$remove_singlerightclick(handler) {
        this.get_mapEvents().removeHandler("singlerightclick", handler);
    },

    // zoomend event
    add_zoomend: function Artem_Google_Map$add_zoomend(handler) {
        this.get_mapEvents().addHandler("zoomend", this._validateHandler("map", "zoomend", handler));
    },
    remove_zoomend: function Artem_Google_Map$remove_zoomend(handler) {
        this.get_mapEvents().removeHandler("zoomend", handler);
    },

    //#endregion

    //#region MarkerEvents

    add_marker_click: function (handler) {
        this.get_markerEvents().addHandler("click", this._validateHandler("marker", "click", handler));
    },
    remove_marker_click: function (handler) {
        this.get_markerEvents().removeHandler("click", handler);
    },

    add_marker_dblclick: function (handler) {
        this.get_markerEvents().addHandler("dblclick", this._validateHandler("marker", "dblclick", handler));
    },
    remove_marker_dblclick: function (handler) {
        this.get_markerEvents().removeHandler("dblclick", handler);
    },

    add_marker_drag: function (handler) {
        this.get_markerEvents().addHandler("drag", this._validateHandler("marker", "drag", handler));
    },
    remove_marker_drag: function (handler) {
        this.get_markerEvents().removeHandler("drag", handler);
    },

    add_marker_dragend: function (handler) {
        this.get_markerEvents().addHandler("dragend", this._validateHandler("marker", "dragend", handler));
    },
    remove_marker_dragend: function (handler) {
        this.get_markerEvents().removeHandler("dragend", handler);
    },

    add_marker_dragstart: function (handler) {
        this.get_markerEvents().addHandler("dragstart", this._validateHandler("marker", "dragstart", handler));
    },
    remove_marker_dragstart: function (handler) {
        this.get_markerEvents().removeHandler("dragstart", handler);
    },

    // TODO geoload

    add_marker_infowindowopen: function (handler) {
        this.get_markerEvents().addHandler("infowindowopen", this._validateHandler("marker", "infowindowopen", handler));
    },
    remove_marker_infowindowopen: function (handler) {
        this.get_markerEvents().removeHandler("infowindowopen", handler);
    },

    add_marker_infowindowbeforeclose: function (handler) {
        this.get_markerEvents().addHandler("infowindowbeforeclose", this._validateHandler("marker", "infowindowbeforeclose", handler));
    },
    remove_marker_infowindowbeforeclose: function (handler) {
        this.get_markerEvents().removeHandler("infowindowbeforeclose", handler);
    },

    add_marker_infowindowclose: function (handler) {
        this.get_markerEvents().addHandler("infowindowclose", this._validateHandler("marker", "infowindowclose", handler));
    },
    remove_marker_infowindowclose: function (handler) {
        this.get_markerEvents().removeHandler("infowindowclose", handler);
    },

    add_marker_mousedown: function (handler) {
        this.get_markerEvents().addHandler("mousedown", this._validateHandler("marker", "mousedown", handler));
    },
    remove_marker_mousedown: function (handler) {
        this.get_markerEvents().removeHandler("mousedown", handler);
    },

    add_marker_mouseout: function (handler) {
        this.get_markerEvents().addHandler("mouseout", this._validateHandler("marker", "mouseout", handler));
    },
    remove_marker_mouseout: function (handler) {
        this.get_markerEvents().removeHandler("mouseout", handler);
    },

    add_marker_mouseover: function (handler) {
        this.get_markerEvents().addHandler("mouseover", this._validateHandler("marker", "mouseover", handler));
    },
    remove_marker_mouseover: function (handler) {
        this.get_markerEvents().removeHandler("mouseover", handler);
    },

    add_marker_mouseup: function (handler) {
        this.get_markerEvents().addHandler("mouseup", this._validateHandler("marker", "mouseup", handler));
    },
    remove_marker_mouseup: function (handler) {
        this.get_markerEvents().removeHandler("mouseup", handler);
    },

    add_marker_remove: function (handler) {
        this.get_markerEvents().addHandler("remove", this._validateHandler("marker", "remove", handler));
    },
    remove_marker_remove: function (handler) {
        this.get_markerEvents().removeHandler("remove", handler);
    },

    add_marker_visibilitychanged: function (handler) {
        this.get_markerEvents().addHandler("visibilitychanged", this._validateHandler("marker", "visibilitychanged", handler));
    },
    remove_marker_visibilitychanged: function (handler) {
        this.get_markerEvents().removeHandler("visibilitychanged", handler);
    },

    //#endregion

    //#region DirectionsEvents

    add_directions_addoverlay: function (handler) {
        this.get_directionsEvents().addHandler("addoverlay", this._validateHandler("directions", "addoverlay", handler));
    },
    remove_directions_addoverlay: function (handler) {
        this.get_directionsEvents().removeHandler("addoverlay", handler);
    },

    add_directions_error: function (handler) {
        this.get_directionsEvents().addHandler("error", this._validateHandler("directions", "error", handler));
    },
    remove_directions_error: function (handler) {
        this.get_directionsEvents().removeHandler("error", handler);
    },

    add_directions_load: function (handler) {
        this.get_directionsEvents().addHandler("load", this._validateHandler("directions", "load", handler));
    },
    remove_directions_load: function (handler) {
        this.get_directionsEvents().removeHandler("load", handler);
    },

    //#endregion

    //#region PolygonEvents

    add_polygon_cancelline: function (handler) {
        this.get_polygonEvents().addHandler("cancelline", this._validateHandler("polygon", "cancelline", handler));
    },
    remove_polygon_cancelline: function (handler) {
        this.get_polygonEvents().removeHandler("cancelline", handler);
    },

    add_polygon_click: function (handler) {
        this.get_polygonEvents().addHandler("click", this._validateHandler("polygon", "click", handler));
    },
    remove_polygon_click: function (handler) {
        this.get_polygonEvents().removeHandler("click", handler);
    },

    add_polygon_endline: function (handler) {
        this.get_polygonEvents().addHandler("endline", this._validateHandler("polygon", "endline", handler));
    },
    remove_polygon_endline: function (handler) {
        this.get_polygonEvents().removeHandler("endline", handler);
    },

    add_polygon_lineupdated: function (handler) {
        this.get_polygonEvents().addHandler("lineupdated", this._validateHandler("polygon", "lineupdated", handler));
    },
    remove_polygon_lineupdated: function (handler) {
        this.get_polygonEvents().removeHandler("lineupdated", handler);
    },

    add_polygon_mouseout: function (handler) {
        this.get_polygonEvents().addHandler("mouseout", this._validateHandler("polygon", "mouseout", handler));
    },
    remove_polygon_mouseout: function (handler) {
        this.get_polygonEvents().removeHandler("mouseout", handler);
    },

    add_polygon_mouseover: function (handler) {
        this.get_polygonEvents().addHandler("mouseover", this._validateHandler("polygon", "mouseover", handler));
    },
    remove_polygon_mouseover: function (handler) {
        this.get_polygonEvents().removeHandler("mouseover", handler);
    },

    add_polygon_remove: function (handler) {
        this.get_polygonEvents().addHandler("remove", this._validateHandler("polygon", "remove", handler));
    },
    remove_polygon_remove: function (handler) {
        this.get_polygonEvents().removeHandler("remove", handler);
    },

    add_polygon_visibilitychanged: function (handler) {
        this.get_polygonEvents().addHandler("visibilitychanged", this._validateHandler("polygon", "visibilitychanged", handler));
    },
    remove_polygon_visibilitychanged: function (handler) {
        this.get_polygonEvents().removeHandler("visibilitychanged", handler);
    },

    //#endregion 

    //#> PolylineEvents

    add_polyline_cancelline: function (handler) {
        this.get_polylineEvents().addHandler("cancelline", this._validateHandler("polyline", "cancelline", handler));
    },
    remove_polyline_cancelline: function (handler) {
        this.get_polylineEvents().removeHandler("cancelline", handler);
    },

    add_polyline_click: function (handler) {
        this.get_polylineEvents().addHandler("click", this._validateHandler("polyline", "click", handler));
    },
    remove_polyline_click: function (handler) {
        this.get_polylineEvents().removeHandler("click", handler);
    },

    add_polyline_endline: function (handler) {
        this.get_polylineEvents().addHandler("endline", this._validateHandler("polyline", "endline", handler));
    },
    remove_polyline_endline: function (handler) {
        this.get_polylineEvents().removeHandler("endline", handler);
    },

    add_polyline_lineupdated: function (handler) {
        this.get_polylineEvents().addHandler("lineupdated", this._validateHandler("polyline", "lineupdated", handler));
    },
    remove_polyline_lineupdated: function (handler) {
        this.get_polylineEvents().removeHandler("lineupdated", handler);
    },

    add_polyline_mouseout: function (handler) {
        this.get_polylineEvents().addHandler("mouseout", this._validateHandler("polyline", "mouseout", handler));
    },
    remove_polyline_mouseout: function (handler) {
        this.get_polylineEvents().removeHandler("mouseout", handler);
    },

    add_polyline_mouseover: function (handler) {
        this.get_polylineEvents().addHandler("mouseover", this._validateHandler("polyline", "mouseover", handler));
    },
    remove_polyline_mouseover: function (handler) {
        this.get_polylineEvents().removeHandler("mouseover", handler);
    },

    add_polyline_remove: function (handler) {
        this.get_polylineEvents().addHandler("remove", this._validateHandler("polyline", "remove", handler));
    },
    remove_polyline_remove: function (handler) {
        this.get_polylineEvents().removeHandler("remove", handler);
    },

    add_polyline_visibilitychanged: function (handler) {
        this.get_polylineEvents().addHandler("visibilitychanged", this._validateHandler("polyline", "visibilitychanged", handler));
    },
    remove_polyline_visibilitychanged: function (handler) {
        this.get_polylineEvents().removeHandler("visibilitychanged", handler);
    },

    //#<

    //#> Google Maps API Wrapped 

    fitBounds: function Artem_Google_Map$fitBounds(bounds) {
        return this.get_map().fitBounds(bounds);
    },

    getBounds: function Artem_Google_Map$getBounds() {
        return this.get_map().getBounds();
    },

    getCenter: function Artem_Google_Map$getCenter() {
        return this.get_map().getCenter();
    },

    getDiv: function Artem_Google_Map$getDiv() {
        return this.get_map().getDiv();
    },

    getMapTypeId: function Artem_Google_Map$getMapTypeId() {
        return this.get_map().getMapTypeId();
    },

    getProjection: function Artem_Google_Map$getProjection() {
        return this.get_map().getProjection();
    },

    getStreetView: function Artem_Google_Map$getStreetView() {
        return this.get_map().getStreetView();
    },

    getZoom: function Artem_Google_Map$getZoom() {
        return this.get_map().getZoom();
    },

    panBy: function Artem_Google_Map$panBy(x, y) {
        this.get_map().panBy(x, y);
    },

    panTo: function Artem_Google_Map$panTo(latlng) {
        this.get_map().panTo(latlng);
    },

    panToBounds: function Artem_Google_Map$panToBounds(bounds) {
        this.get_map().panToBounds(bounds);
    },

    setCenter: function Artem_Google_Map$setCenter(latlng) {
        this.get_map().setCenter(latlng);
    },

    setMapTypeId: function Artem_Google_Map$setMapTypeId(mapTypeId) {
        this.get_map().setMapTypeId(mapTypeId);
    },

    setOptions: function Artem_Google_Map$setOptions(options) {
        this.get_map().setOptions(panorama);
    },

    setStreetView: function Artem_Google_Map$setStreetView(panorama) {
        this.get_map().setStreetView(panorama);
    },

    setZoom: function Artem_Google_Map$setZoom(zoom) {
        this.get_map().setZoom(zoom);
    }

    //#<
};

Artem.Google.Map.registerClass("Artem.Google.Map", Sys.UI.Control);
//#endregion

//#> Events //////////////////////////////////////////////////////////////////////////////////

// AddressEventArgs
Artem.Google.Events.AddressEventArgs = function Artem_Google_Events_AddressEventArgs(address) {
    Artem.Google.Events.AddressEventArgs.initializeBase(this);
    this.Address = address;
}
Artem.Google.Events.AddressEventArgs.prototype = {
    Address: null
}
Artem.Google.Events.AddressEventArgs.registerClass("Artem.Google.Events.AddressEventArgs", Sys.EventArgs);

// BoundsEventArgs
Artem.Google.Events.BoundsEventArgs = function Artem_Google_Events_BoundsEventArgs(gbouns) {
    Artem.Google.Events.BoundsEventArgs.initializeBase(this);
    this.Bounds = new Artem.Google.Bounds(gbouns);
}
Artem.Google.Events.BoundsEventArgs.prototype = {
    Bounds: null
}
Artem.Google.Events.BoundsEventArgs.registerClass("Artem.Google.Events.BoundsEventArgs", Sys.EventArgs);

// LocationEventArgs
Artem.Google.Events.LocationEventArgs = function Artem_Google_Events_LocationEventArgs(glocation) {
    Artem.Google.Events.LocationEventArgs.initializeBase(this);
    this.Location = new Artem.Google.Location(glocation);
}
Artem.Google.Events.LocationEventArgs.prototype = {
    Location: null
}
Artem.Google.Events.LocationEventArgs.registerClass("Artem.Google.Events.LocationEventArgs", Sys.EventArgs);

// VisibilityEventArgs
Artem.Google.Events.VisibilityEventArgs = function Artem_Google_Events_VisibilityEventArgs(visible) {
    Artem.Google.Events.VisibilityEventArgs.initializeBase(this);
    this.Visible = visible;
}
Artem.Google.Events.VisibilityEventArgs.prototype = {
    Visible: false
}
Artem.Google.Events.VisibilityEventArgs.registerClass("Artem.Google.Events.VisibilityEventArgs", Sys.EventArgs);

// ZoomEventArgs
Artem.Google.Events.ZoomEventArgs = function Artem_Google_Events_ZoomEventArgs(oldlevel, newlevel) {
    Artem.Google.Events.ZoomEventArgs.initializeBase(this);
    this.NewLevel = newlevel;
    this.OldLevel = oldlevel;
}
Artem.Google.Events.ZoomEventArgs.prototype = {
    NewLevel: null,
    OldLevel: null
}
Artem.Google.Events.ZoomEventArgs.registerClass("Artem.Google.Events.ZoomEventArgs", Sys.EventArgs);

//#endregion