﻿/// <reference name="MicrosoftAjax.js"/>
// --------------------------------------------------------------------------------
// Copyright (C) ArtemBG. All rights reserved.
// --------------------------------------------------------------------------------
// GoogleMap3.js
// GoogleMap control v3.0 javascipt library.
// Project:    http://http://www.codeplex.com/googlemap
// Author:     Velio Ivanov velio@artembg.com
//
// namespace Artem.Web
// 
if (!Artem) var Artem = {};
if (!Artem.Web) Artem.Web = {};
//
// GoogleMapView
//
Artem.Web.GoogleMapView = {
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
    // methods
    convert: function(gmapType) {
        switch (gmapType) {
            case G_SATELLITE_MAP:
                return Artem.Web.GoogleMapView.Satellite;
            case G_HYBRID_MAP:
                return Artem.Web.GoogleMapView.Hybrid;
            case G_PHYSICAL_MAP:
                return Artem.Web.GoogleMapView.Physical;
            case G_MOON_ELEVATION_MAP:
                return Artem.Web.GoogleMapView.MoonElevation;
            case G_MOON_VISIBLE_MAP:
                return Artem.Web.GoogleMapView.MoonVisible;
            case G_MARS_ELEVATION_MAP:
                return Artem.Web.GoogleMapView.MarsElevation;
            case G_MARS_VISIBLE_MAP:
                return Artem.Web.GoogleMapView.MarsVisible;
            case G_MARS_INFRARED_MAP:
                return Artem.Web.GoogleMapView.MarsInfrared;
            case G_SKY_VISIBLE_MAP:
                return Artem.Web.GoogleMapView.SkyVisible;
            default:
                return Artem.Web.GoogleMapView.Normal;
        }
    }
};
//
// GoogleMapView
//
Artem.Web.OpenInfoBehaviour = {
    Click: 0,
    DoubleClick: 1,
    MouseDown: 2,
    MouseOut: 3,
    MouseOver: 4,
    MouseUp: 5
};
//
// Delegate helper
// 
Artem.Web.Delegate = {
    call: function(instance, method) {
        this.creat(instance, method).call(instance, arguments);
    },
    callFromString: function(instance, methodString) {
        this.createFromString(instance, methodString).call(instance, arguments);
    },
    create: function(instance, method) {
        return function() { return method.apply(instance, arguments); };
    },
    createFromString: function(instance, methodString) {
        var rex = new RegExp("\\(.*\\)");
        if (rex.test(methodString))
            return function() { eval(methodString); };
        else {
            var method = eval(methodString);
            return function() { return method.apply(instance, arguments); };
        }
    }
};
//
// GoogleMapManager
//
Artem.Web.GoogleManager = {
    // Fields //////////////////////////////////////////////////////////////////////
    CurrentMap: null,
    Disposed: false,
    Initialized: false,
    Maps: new Array(), // here is kept a reference to all maps on the page
    // Methods /////////////////////////////////////////////////////////////////////
    addMap: function(map) {
        this.Maps[this.Maps.length] = (this.CurrentMap = map);
    },
    dispose: function() {
        if (!this.Disposed) {
            GUnload();
            this.Disposed = true;
        }
    },
    initialize: function() {
        if (!GBrowserIsCompatible()) throw "Your browser is not google maps api compatible!";
        if (!this.Initialized) {
            var handler = Artem.Web.Delegate.create(this, this.dispose);
            if (typeof (Sys) !== 'undefined')
                Sys.Application.add_disposing(handler);
            else
                document.body.onunload = handler;
        }
    }
};
//
// GoogleBounds ///////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleBounds = function(bounds) {
    this.GBounds = bounds;
};
Artem.Web.GoogleBounds.prototype = {
    // persistance
    save: function() {
        var state = "{";
        if (this.GBounds) {
            var sw = this.GBounds.getSouthWest();
            var ne = this.GBounds.getNorthEast();
            state += "\"SouthWest\":{\"Latitude\":" + sw.lat() + ",\"Longitude\":" + sw.lng() + "},";
            state += "\"NorthEast\":{\"Latitude\":" + ne.lat() + ",\"Longitude\":" + ne.lng() + "}";
        }
        state += "}";
        return state;
    }
};
//
// GoogleDirection ///////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleDirection = function(map, config) {
    this.Locale = config.Locale;
    this.Query = config.Query;
    this.RoutePanelId = config.RoutePanelId;
    // origin
    var pane = null;
    if (this.RoutePanelId) pane = document.getElementById(this.RoutePanelId);
    this.GDirections = new GDirections(map, pane);
};
Artem.Web.GoogleDirection.prototype = {
    // persistance
    save: function() {
        var state = "{";
        state += "\"Locale\":\"" + this.Locale + "\",";
        state += "\"Query\":\"" + this.Query + "\",";
        state += "\"RoutePanelId\":\"" + this.RoutePanelId + "\",";
        // distance
        var distance = this.getDistance();
        state += "\"Distance\":{\"Meters\":" + distance.meters + ",\"Html\":\"" + distance.html + "\"},";
        // duration
        var duration = this.getDuration();
        state += "\"Duration\":{\"Seconds\":" + duration.seconds + ",\"Html\":\"" + duration.html + "\"},";
        // bounds
        var bounds = new Artem.Web.GoogleBounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.save();
        //
        state += "}";
        return state;
    },
    // wrapped
    load: function(query, options) {
        this.GDirections.load(query, options);
    },
    loadDefault: function() {
        var options = (this.Locale) ? { locale: this.Locale} : null;
        this.load(this.Query, options);
    },
    loadFromWaypoints: function(waypoints, options) {
        this.GDirections.loadFromWaypoints(waypoints, options);
    },
    clear: function() {
        this.GDirections.clear();
    },
    getStatus: function() {
        return this.GDirections.getStatus();
    },
    getBounds: function() {
        return this.GDirections.getBounds();
    },
    getNumRoutes: function() {
        return this.GDirections.getNumRoutes();
    },
    getRoute: function(i) {
        return this.GDirections.getRoute(i);
    },
    getNumGeocodes: function() {
        return this.GDirections.getNumGeocodes();
    },
    getGeocode: function(i) {
        return this.GDirections.getGeocode(i);
    },
    getCopyrightsHtml: function() {
        return this.GDirections.getCopyrightsHtml();
    },
    getSummaryHtml: function() {
        return this.GDirections.getSummaryHtml();
    },
    getDistance: function() {
        return this.GDirections.getDistance();
    },
    getDuration: function() {
        return this.GDirections.getDuration();
    },
    getPolyline: function() {
        return this.GDirections.getPolyline();
    },
    getMarker: function(i) {
        return this.GDirections.getMarker(i);
    }
};
//
// GooglePolygon ///////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GooglePolygon = function(config) {
    this.Clickable = config.Clickable;
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
};
Artem.Web.GooglePolygon.prototype = {
    // persistance
    save: function() {
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
        var bounds = new Artem.Web.GoogleBounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.save();
        //
        state += "}";
        return state;
    },
    // wrapped
    getBounds: function() {
        return this.GPolygon.getBounds();
    },
    hide: function() {
        this.GPolygon.hide();
    },
    isHidden: function() {
        return this.GPolygon.isHidden();
    },
    show: function() {
        this.GPolygon.show();
    }
};
//
// GooglePolyline ///////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GooglePolyline = function(config) {
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
};
Artem.Web.GooglePolyline.prototype = {
    // persistance
    save: function() {
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
        var bounds = new Artem.Web.GoogleBounds(this.getBounds());
        state += "\"Bounds\":";
        state += bounds.save();
        //
        state += "}";
        return state;
    },
    // wrapped
    getBounds: function() {
        return this.GPolyline.getBounds();
    },
    hide: function() {
        this.GPolyline.hide();
    },
    isHidden: function() {
        return this.GPolyline.isHidden();
    },
    show: function() {
        this.GPolyline.show();
    },
    supportsHide: function() {
        return this.GPolyline.supportsHide();
    }
};
//
// GoogleMarker ///////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleMarker = function(map, config) {
    // origin
    this.GMarker = null;
    this.Map = map;
    // properties
    this.Address = config.Address;
    this.AutoPan = config.AutoPan;
    this.Bouncy = config.Bouncy;
    this.Clickable = config.Clickable;
    this.Draggable = config.Draggable;
    this.DragCrossMove = config.DragCrossMove;
    this.IconAnchor = config.IconAnchor;
    this.IconSize = config.IconSize;
    this.ImageUrl = config.ImageUrl;
    this.InfoWindowAnchor = config.InfoWindowAnchor;
    this.Latitude = config.Latitude;
    this.Longitude = config.Longitude;
    this.OpenInfoBehaviour = config.OpenInfoBehaviour;
    this.OpenWindowContent = null;
    this.ShadowSize = config.ShadowSize;
    this.ShadowUrl = config.ShadowUrl;
    this.Text = config.Text;
    this.Title = config.Title;
    // events
    if (config.GoogleEvents) {
        this.ClientEvents = config.GoogleEvents.ClientEvents;
        this.ServerEvents = config.GoogleEvents.ServerEvents;
    }
};
Artem.Web.GoogleMarker.prototype = {
    isLoaded: function() {
        return (this.GMarker != null);
        //        if (this.GMarker == null) throw "Cannot use it before marker been loaded!";
    },
    // load, initialize, save
    initialize: function() {
        // open info behaviour
        var eventName;
        switch (this.OpenInfoBehaviour) {
            case Artem.Web.OpenInfoBehaviour.Click:
                eventName = "click";
                break;
            case Artem.Web.OpenInfoBehaviour.DoubleClick:
                eventName = "dblclick";
                break;
            case Artem.Web.OpenInfoBehaviour.MouseDown:
                eventName = "mousedown";
                break;
            case Artem.Web.OpenInfoBehaviour.MouseOut:
                eventName = "mouseout";
                break;
            case Artem.Web.OpenInfoBehaviour.MouseOver:
                eventName = "mouseover";
                break;
            case Artem.Web.OpenInfoBehaviour.MouseUp:
                eventName = "mouseup";
                break;
        }
        if (eventName)
            GEvent.addListener(this.GMarker, eventName, Artem.Web.Delegate.create(this, this.openDefaultInfoWindow));
        //events
        this.initializeEvents(this.ClientEvents, true);
        this.initializeEvents(this.ServerEvents, false);
    },
    initializeEvents: function(events, clients) {
        if (events) {
            for (var i = 0; i < events.length; i++) {
                if (clients) {
                    GEvent.addListener(this.GMarker, events[i].Key,
                        Artem.Web.Delegate.createFromString(this, events[i].Handler));
                }
                else {
                    var handler = events[i].Handler;
                    var map = this.Map;
                    var delegate = this.Map.save;
                    delegate.apply(map);
                    GEvent.addListener(this.GMarker, events[i].Key,
                            function() {
                                delegate.call(map);
                                eval(handler);
                            }
                        );
                }
            }
        }
    },
    load: function(point) {
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
    save: function() {
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
        state += "\"ImageUrl\":\"" + this.ImageUrl + "\",";
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
        state += "\"Text\":\"" + this.Text + "\",";
        state += "\"Title\":\"" + this.Title + "\",";
        // events
//        if (this.ClientEvents || this.ServerEvents) {
//            state += '"GoogleEvents":{';
//            if (this.ClientEvents) {
//                state += "\"ClientEvents\":[";
//                for (var i in this.ClientEvents) {
//                    if (i > 0) state += ",";
//                    state += "{Key:" + this.ClientEvents[i].Key + ",Handler:" + this.ClientEvents[i].Handler + "}";
//                }
//                state += "]";
//            }
//            if (this.ServerEvents) {
//                if (this.ClientEvents) state += ",";
//                state += "\"ServerEvents\":[";
//                for (var i in this.ServerEvents) {
//                    if (i > 0) state += ",";
//                    state += "{Key:" + this.ServerEvents[i].Key + ",Handler:" + this.ServerEvents[i].Handler + "}";
//                }
//                state += "]";
//            }
//            state += "}";
//        }
        //
        state += "}";
        return state;
    },
    // wrapped
    closeInfoWindow: function() {
        if (this.isLoaded())
            this.GMarker.closeInfoWindow();
    },
    createIcon: function() {
        var icon = null;
        if (this.ImageUrl) {
            icon = new GIcon();
            icon.image = this.ImageUrl;
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
    disableDragging: function() {
        if (this.isLoaded())
            this.GMarker.disableDragging();
    },
    draggable: function() {
        if (this.isLoaded())
            return this.GMarker.draggable();
    },
    draggingEnabled: function() {
        if (this.isLoaded())
            return this.GMarker.draggingEnabled();
    },
    enableDragging: function() {
        if (this.isLoaded())
            this.GMarker.enableDragging();
    },
    getIcon: function() {
        if (this.isLoaded())
            return this.GMarker.getIcon();
    },
    getLatLng: function() {
        if (this.isLoaded())
            return this.GMarker.getLatLng();
    },
    getPoint: function() {
        if (this.isLoaded())
            return this.GMarker.getPoint();
    },
    getTitle: function() {
        if (this.isLoaded())
            return this.GMarker.getTitle();
    },
    hide: function() {
        if (this.isLoaded())
            this.GMarker.hide();
    },
    isHidden: function() {
        if (this.isLoaded())
            return this.GMarker.isHidden();
    },
    openDefaultInfoWindow: function() {
        if (this.isLoaded()) {
            if (this.OpenWindowContent) {
                var node = document.getElementById(this.OpenWindowContent);
                this.openInfoWindow(node.cloneNode(true));
            }
            else
                this.openInfoWindowHtml(this.Text);
        }
    },
    openInfoWindow: function(domnode) {
        if (this.isLoaded())
            this.GMarker.openInfoWindow(domnode);
    },
    openInfoWindowHtml: function(content) {
        if (this.isLoaded())
            this.GMarker.openInfoWindowHtml(content);
    },
    setImage: function(url) {
        if (this.isLoaded())
            this.GMarker.setImage(url);
    },
    setLatLng: function(point) {
        if (this.isLoaded())
            this.GMarker.setLatLng(point);
    },
    setPoint: function(point) {
        if (this.isLoaded())
            this.GMarker.setPoint(point);
    },
    show: function() {
        if (this.isLoaded())
            this.GMarker.show();
    }
};
//
// GoogleMap //////////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleMap = function(config) {
    // properties
    this.Address = config.Address;
    this.BaseCountryCode = config.BaseCountryCode;
    this.DefaultMapView = config.DefaultMapView;
    this.ClientID = config.ClientID;
    this.IsGeolocation = false;
    this.IsStatic = config.IsStatic;
    this.Key = config.Key;
    this.Latitude = config.Latitude;
    this.Longitude = config.Longitude;
    this.ShowMapTypeControl = config.ShowMapTypeControl;
    this.ShowScaleControl = config.ShowScaleControl;
    this.ShowTraffic = config.ShowTraffic;
    this.Zoom = config.Zoom;
    this.ZoomPanType = config.ZoomPanType;
    // behaviour
    this.EnableContinuousZoom = config.EnableContinuousZoom;
    this.EnableDoubleClickZoom = config.EnableDoubleClickZoom;
    this.EnableDragging = config.EnableDragging;
    this.EnableGoogleBar = config.EnableGoogleBar;
    this.EnableInfoWindow = config.EnableInfoWindow;
    this.EnableMarkerManager = config.EnableMarkerManager;
    this.EnableScrollWheelZoom = config.EnableScrollWheelZoom;
    // events
    if (config.GoogleEvents) {
        this.ClientEvents = config.GoogleEvents.ClientEvents;
        this.ServerEvents = config.GoogleEvents.ServerEvents;
    }
    this.ClientGeoLoadedIndex = null;
    this.ServerGeoLoadedIndex = null;
    // collections
    this.Actions = null;
    this.Directions = null;
    this.Markers = null;
    this.Polygons = null;
    this.Polylines = null;
    // origin
    this.GMap = null;
    this.MarkerManager = null;
    // geocoder
    this.Geocoder = new GClientGeocoder();
    if (config.BaseCountryCode)
        this.Geocoder.setBaseCountryCode(config.BaseCountryCode);
    // initialize manager
    Artem.Web.GoogleManager.initialize();
    // initialize
    if (!this.IsStatic) {
        var options;
        if (this.Width && this.Height)
            options = { size: new GSize(this.Width, this.Height) };
        this.GMap = new GMap2(this.getMapElement(), options); //document.getElementById(this.ClientID + "_Map"), options); 
    }
};
Artem.Web.GoogleMap.prototype = {
    // add
    addMarker: function(config) {
        if (!this.Markers) this.Markers = new Array();
        this.Markers.push(new Artem.Web.GoogleMarker(this, config));
    },
    addDirection: function(config) {
        if (!this.Directions) this.Directions = new Array();
        this.Directions.push(new Artem.Web.GoogleDirection(this.GMap, config));
    },
    addPolygon: function(config) {
        if (!this.Polygons) this.Polygons = new Array();
        this.Polygons.push(new Artem.Web.GooglePolygon(config));
    },
    addPolyline: function(config) {
        if (!this.Polylines) this.Polylines = new Array();
        this.Polylines.push(new Artem.Web.GooglePolyline(config));
    },
    addAction: function(action) {
        if (!this.Actions) this.Actions = new Array();
        this.Actions.push(action);
    },
    clearMarkers: function() {
        if (this.Markers) this.Markers = new Array();
    },
    clearDirections: function(config) {
        if (this.Directions) this.Directions = new Array();
    },
    clearPolygons: function(config) {
        if (this.Polygons) this.Polygons = new Array();
    },
    clearPolylines: function(config) {
        if (this.Polylines) this.Polylines = new Array();
    },
    clearActions: function(action) {
        if (this.Actions) this.Actions = new Array();
    },
    // load, initialize, save
    getElement: function() {
        return document.getElementById(this.ClientID);
    },
    getMapElement: function() {
        return document.getElementById(this.ClientID); // + "_Map");
    },
    initialize: function() {
        // behaviour
        (this.EnableContinuousZoom) ? this.enableContinuousZoom() : this.disableContinuousZoom();
        (this.EnableDoubleClickZoom) ? this.enableDoubleClickZoom() : this.disableDoubleClickZoom();
        (this.EnableDragging) ? this.enableDragging() : this.disableDragging();
        (this.EnableGoogleBar) ? this.enableGoogleBar() : this.disableGoogleBar();
        (this.EnableInfoWindow) ? this.enableInfoWindow() : this.disableInfoWindow();
        (this.EnableScrollWheelZoom) ? this.enableScrollWheelZoom() : this.disableScrollWheelZoom();
        // controls
        (this.ZoomPanType && this.ZoomPanType == 1)
            ? this.GMap.addControl(new GLargeMapControl()) : this.GMap.addControl(new GSmallMapControl());
        if (this.ShowMapTypeControl) this.GMap.addControl(new GMapTypeControl());
        if (this.ShowScaleControl) this.GMap.addControl(new GScaleControl());
        if (this.ShowTraffic) this.GMap.addOverlay(new GTrafficOverlay());
        if (this.EnableMarkerManager) this.MarkerManager = new GMarkerManager(this.GMap);
        //events
        this.setupEvents(this.ClientEvents, true);
        this.setupEvents(this.ServerEvents, false);
        // map view
        this.setMapView();
    },
    load: function(point) {
        if (point) {
            if (!this.IsStatic) {
                this.Latitude = point.lat();
                this.Longitude = point.lng();
                this.setCenter(point, this.Zoom);
                if (this.IsGeolocation) {
                    this.IsGeolocation = false;
                    if (this.ClientGeoLoadedIndex != null)
                        Artem.Web.Delegate.callFromString(this, this.ClientEvents[this.ClientGeoLoadedIndex].Handler);
                    if (this.ServerGeoLoadedIndex != null)
                        Artem.Web.Delegate.callFromString(this, this.ServerEvents[this.ServerGeoLoadedIndex].Handler);
                }
                this.initialize();
                this.render();
                this.checkResize();
            }
            else {
                this.loadStatic();
            }
        }
        else {
            document.forms[0].onsubmit = Artem.Web.Delegate.create(this, this.save);
            if ((this.Latitude != 0) && (this.Longitude != 0))
                this.load(new GLatLng(this.Latitude, this.Longitude));
            else {
                this.IsGeolocation = true;
                this.Geocoder.getLatLng(this.Address, Artem.Web.Delegate.create(this, this.load));
            }
        }
    },
    loadAddress: function(address) {
        this.Address = address;
        this.IsGeolocation = true;
        this.Geocoder.getLatLng(this.Address, Artem.Web.Delegate.create(this, this.load));
    },
    loadStatic: function() {
        var el = this.getMapElement();
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
    render: function() {
        // markers
        if (this.Markers) {
            var m;
            for (var i = 0; i < this.Markers.length; i++) {
                m = this.Markers[i];
                if ((m.Latitude != 0) && (m.Longitude != 0)) {
                    try {
                        m.load(new GLatLng(m.Latitude, m.Longitude));
                    }
                    catch (ex) { }
                }
                else {
                    try {
                        this.Geocoder.getLatLng(m.Address, Artem.Web.Delegate.create(m, m.load));
                    }
                    catch (ex) {
                    }
                }
            }
        }
        // directions
        if (this.Directions) {
            for (var i = 0; i < this.Directions.length; i++) {
                this.Directions[i].loadDefault();
            }
        }
        // polylines
        if (this.Polylines) {
            for (var i = 0; i < this.Polylines.length; i++) {
                this.addOverlay(this.Polylines[i].GPolyline);
            }
        }
        // polygons
        if (this.Polygons) {
            for (var i = 0; i < this.Polygons.length; i++) {
                this.addOverlay(this.Polygons[i].GPolygon);
            }
        }
        // fire actions
        if (this.Actions) {
            for (var i = 0; i < this.Actions.length; i++) {
                Artem.Web.Delegate.callFromString(this, this.Actions[i]);
            }
        }
    },
    save: function() {
        var state = "{";
        state += "\"Address\":\"" + this.Address + "\"";
        state += ",\"BaseCountryCode\":\"" + this.BaseCountryCode + "\"";
        state += ",\"DefaultMapView\":" + Artem.Web.GoogleMapView.convert(this.getCurrentMapType()); // this.DefaultMapView;
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
        var bounds = new Artem.Web.GoogleBounds(this.getBounds());
        state += ",\"Bounds\":";
        state += bounds.save();
        // markers
        if (this.Markers) {
            state += ",\"Markers\":[";
            for (var i = 0; i < this.Markers.length; i++) {
                if (this.Markers[i].isLoaded())
                    state += this.Markers[i].save();
            }
            state += "]";
        }
        // directions
        if (this.Directions) {
            state += ",\"Directions\":[";
            for (var i = 0; i < this.Directions.length; i++) {
                state += this.Directions[i].save();
            }
            state += "]";
        }
        // polylines
        if (this.Polylines) {
            state += ",\"Polylines\":[";
            for (var i = 0; i < this.Polylines.length; i++) {
                state += this.Polylines[i].save();
            }
            state += "]";
        }
        // polygons
        if (this.Polygons) {
            state += ",\"Polygons\":[";
            for (var i = 0; i < this.Polygons.length; i++) {
                state += this.Polygons[i].save();
            }
            state += "]";
        }
        //
        state += "}";
        var bag = document.getElementById(this.ClientID + "_State");
        bag.value = state;
    },
    setupEvents: function(events, clients) {
        if (events) {
            for (var i = 0; i < events.length; i++) {
                if (events[i].Key != 'geoload') {
                    if (clients) {
                        GEvent.addListener(this.GMap, events[i].Key,
                            Artem.Web.Delegate.createFromString(this, events[i].Handler));
                    }
                    else {
                        var handler = events[i].Handler;
                        var map = this;
                        var delegate = this.save;
                        delegate.apply(this);
                        GEvent.addListener(this.GMap, events[i].Key,
                            function() {
                                delegate.call(map);
                                eval(handler);
                            }
                        );
                    }
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
    raiseServerEvent: function(map, handler) {
        map.save();
        eval(handler);
    },
    setMapView: function() {
        // set view
        if (this.DefaultMapView) {
            switch (this.DefaultMapView) {
                case Artem.Web.GoogleMapView.Normal:
                    this.GMap.setMapType(G_NORMAL_MAP);
                    break;
                case Artem.Web.GoogleMapView.Satellite:
                    this.GMap.setMapType(G_SATELLITE_MAP);
                    break;
                case Artem.Web.GoogleMapView.Hybrid:
                    this.GMap.setMapType(G_HYBRID_MAP);
                    break;
                case Artem.Web.GoogleMapView.Physical:
                    this.GMap.addMapType(G_PHYSICAL_MAP);
                    this.GMap.setMapType(G_PHYSICAL_MAP);
                    break;
                case Artem.Web.GoogleMapView.MoonElevation:
                    this.GMap.addMapType(G_MOON_ELEVATION_MAP);
                    this.GMap.setMapType(G_MOON_ELEVATION_MAP);
                    break;
                case Artem.Web.GoogleMapView.MoonVisible:
                    this.GMap.addMapType(G_MOON_VISIBLE_MAP);
                    this.GMap.setMapType(G_MOON_VISIBLE_MAP);
                    break;
                case Artem.Web.GoogleMapView.MarsElevation:
                    this.GMap.addMapType(G_MARS_ELEVATION_MAP);
                    this.GMap.setMapType(G_MARS_ELEVATION_MAP);
                    break;
                case Artem.Web.GoogleMapView.MarsVisible:
                    this.GMap.addMapType(G_MARS_VISIBLE_MAP);
                    this.GMap.setMapType(G_MARS_VISIBLE_MAP);
                    break;
                case Artem.Web.GoogleMapView.MarsInfrared:
                    this.GMap.addMapType(G_MARS_INFRARED_MAP);
                    this.GMap.setMapType(G_MARS_INFRARED_MAP);
                    break;
                case Artem.Web.GoogleMapView.SkyVisible:
                    this.GMap.addMapType(G_SKY_VISIBLE_MAP);
                    this.GMap.setMapType(G_SKY_VISIBLE_MAP);
                    break;
            }
        }
    },
    // wrapped
    addControl: function(control, position) {
        this.GMap.addControl(control, position);
    },
    addMapType: function(type) {
        this.GMap.addMapType(type);
    },
    addOverlay: function(overlay) {
        this.GMap.addOverlay(overlay);
    },
    checkResize: function() {
        this.GMap.checkResize();
    },
    clearOverlays: function() {
        this.GMap.clearOverlays();
    },
    closeInfoWindow: function() {
        this.GMap.closeInfoWindow();
    },
    continuousZoomEnabled: function() {
        return this.GMap.continuousZoomEnabled();
    },
    disableContinuousZoom: function() {
        this.GMap.disableContinuousZoom();
    },
    disableDoubleClickZoom: function() {
        this.GMap.disableDoubleClickZoom();
    },
    disableDragging: function() {
        this.GMap.disableDragging();
    },
    disableGoogleBar: function() {
        this.GMap.disableGoogleBar();
    },
    disableInfoWindow: function() {
        this.GMap.disableInfoWindow();
    },
    disableScrollWheelZoom: function() {
        this.GMap.disableScrollWheelZoom();
    },
    doubleClickZoomEnabled: function() {
        return this.GMap.doubleClickZoomEnabled();
    },
    draggingEnabled: function() {
        return this.GMap.draggingEnabled();
    },
    enableContinuousZoom: function() {
        this.GMap.enableContinuousZoom();
    },
    enableDoubleClickZoom: function() {
        this.GMap.enableDoubleClickZoom();
    },
    enableDragging: function() {
        this.GMap.enableDragging();
    },
    enableGoogleBar: function() {
        this.GMap.enableGoogleBar();
    },
    enableInfoWindow: function() {
        this.GMap.enableInfoWindow();
    },
    enableScrollWheelZoom: function() {
        this.GMap.enableScrollWheelZoom();
    },
    fromContainerPixelToLatLng: function(pixel) {
        return this.GMap.fromContainerPixelToLatLng(pixel);
    },
    fromDivPixelToLatLng: function(pixel) {
        return this.GMap.fromDivPixelToLatLng(pixel);
    },
    fromLatLngToDivPixel: function(latlng) {
        return this.GMap.fromLatLngToDivPixel(latlng);
    },
    getBounds: function() {
        return this.GMap.getBounds();
    },
    getBoundsZoomLevel: function() {
        return this.GMap.getBoundsZoomLevel();
    },
    getCenter: function() {
        return this.GMap.getCenter();
    },
    getContainer: function() {
        return this.GMap.getContainer();
    },
    getCurrentMapType: function() {
        return this.GMap.getCurrentMapType();
    },
    getDragObject: function() {
        return this.GMap.getDragObject();
    },
    getInfoWindow: function() {
        return this.GMap.getInfoWindow();
    },
    getMapTypes: function() {
        return this.GMap.getMapTypes();
    },
    getPane: function(pane) {
        return this.GMap.getPane();
    },
    getSize: function() {
        return this.GMap.getSize();
    },
    getZoom: function() {
        return this.GMap.getZoom();
    },
    infoWindowEnabled: function() {
        return this.GMap.infoWindowEnabled();
    },
    isLoaded: function() {
        return this.GMap.isLoaded();
    },
    openInfoWindow: function(point, node, opts) {
        this.GMap.openInfoWindow(point, node, opts);
    },
    openInfoWindowHtml: function(point, html, opts) {
        this.GMap.openInfoWindowHtml(point, html, opts);
    },
    panBy: function(distance) {
        this.GMap.panBy(distance);
    },
    panDirection: function(dx, dy) {
        this.GMap.panDirection(dx, dy);
    },
    panTo: function(center) {
        this.GMap.panTo(center);
    },
    removeControl: function(control) {
        this.GMap.removeControl(control);
    },
    removeMapType: function(type) {
        this.GMap.removeMapType();
    },
    removeOverlay: function(overlay) {
        this.GMap.removeOverlay(overlay);
    },
    returnToSavedPosition: function() {
        this.GMap.returnToSavedPosition();
    },
    savePosition: function() {
        this.GMap.savePosition();
    },
    scrollWheelZoomEnabled: function() {
        return this.GMap.scrollWheelZoomEnabled();
    },
    setCenter: function(point, zoom, type) {
        this.GMap.setCenter(point, zoom, type);
    },
    setMapType: function(type) {
        this.GMap.setMapType(type);
    },
    setZoom: function(level) {
        this.GMap.setZoom(level);
    },
    zoomIn: function() {
        this.GMap.zoomIn();
    },
    zoomOut: function() {
        this.GMap.zoomOut();
    }
};