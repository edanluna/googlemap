/// <reference name="MicrosoftAjax.js"/>
// ------------------------------------------------------------------------------------------------
// Copyright (C) ArtemBG.
// ------------------------------------------------------------------------------------------------
// GoogleMap4.debug.js
// GoogleMap Control v4.0 javascipt library (debug).
//
// Assembly:    Artem.GooleMap
// Version:     4.0.0.0
// Project:     http://googlemap.codeplex.com
// Website:     http://artembg.com
// Author:      Velio Ivanov - velio@artembg.com
// License:     Microsoft Permissive License (Ms-PL) v1.1
//              http://www.codeplex.com/googlemap/license
// API:         http://code.google.com/apis/maps/
//
// Function.Delegate //////////////////////////////////////////////////////////////////////////////
//
Function.Delegate = {
    createFromString: function(instance, methodString) {
        var rex = new RegExp("\\(.*\\)");
        if (rex.test(methodString))
            return function() { eval(methodString); };
        else {
            var method = eval(methodString);
            return Function.Delegate.create(instance, method);
        }
    },
    call: function(instance, method) {
        Function.Delegate.create(instance, method).call(instance, arguments);
    },
    callFromString: function(instance, methodString) {
        Function.Delegate.createFromString(instance, methodString).call(instance, arguments);
    }
};
if (typeof (Sys) == 'undefined') {
    Function.Delegate.create = function(instance, method) {
        return function() { return method.apply(instance, arguments); };
    };
}
else {
    Function.Delegate.create = Function.createDelegate;
}
//
// Function.Handler ///////////////////////////////////////////////////////////////////////////////
//
Function.Handler = {
    cache: {}
};
if (typeof (Sys) == 'undefined') {
    Function.Handler.add = function(element, eventName, handler) {
        var browserHandler;
        if (element.addEventListener) {
            browserHandler = function(e) {
                return handler.call(element, e);
            }
            element.addEventListener(eventName, browserHandler, false);
        }
        else if (element.attachEvent) {
            browserHandler = function() {
                var e = {};
                try { e = window.event; } catch (ex) { }
                return handler.call(element, e);
            }
            element.attachEvent('on' + eventName, browserHandler);
        }
        Function.Handler.cache[Function.Handler.cache.length] = { handler: handler, browserHandler: browserHandler };
    };
    Function.Handler.remove = function(element, eventName, handler) {
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
    Function.Handler.add = $addHandler;
    Function.Handler.remove = $removeHandler;
}
//
// namespace Artem.Web ////////////////////////////////////////////////////////////////////////////
// 
if (!Artem) var Artem = {};
if (!Artem.Web) Artem.Web = {};
//
// GoogleMapView //////////////////////////////////////////////////////////////////////////////////
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
    Satellite3D: 10,
    MapMakerNormal: 11,
    MapMakerHybrid: 12,
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
            case G_SATELLITE_3D_MAP:
                return Artem.Web.GoogleMapView.Satellite3D;
            case G_MAPMAKER_NORMAL_MAP:
                return Artem.Web.GoogleMapView.MapMakerNormal;
            case G_MAPMAKER_HYBRID_MAP:
                return Artem.Web.GoogleMapView.MapMakerHybrid;
            default:
                return Artem.Web.GoogleMapView.Normal;
        }
    }
};
//
// OpenInfoBehaviour //////////////////////////////////////////////////////////////////////////////
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
// GoogleMapManager ///////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleManager = {
    // Fields ------------------------------------------------------------------------------------
    CurrentMap: null,
    Disposed: false,
    Initialized: false,
    Maps: new Array(), // here is kept a reference to all maps on the page
    // Methods ------------------------------------------------------------------------------------
    addMap: function(map) {
        for (var i = 0; i < this.Maps.length; i++) {
            if (map.ClientID == this.Maps[i].ClientID) return;
        }
        this.Maps[this.Maps.length] = (this.CurrentMap = map);
    },
    dispose: function(partial) {
        if (!this.Disposed) {
            GUnload();
            this.Disposed = true;
        }
    },
    initialize: function() {
        if (!this.Initialized) {
            if (!GBrowserIsCompatible()) throw "Your browser is not google maps api compatible!";

            var handler = Function.Delegate.create(this, this.dispose);
            if (typeof (Sys) != 'undefined') {
                var rm = Sys.WebForms.PageRequestManager.getInstance();
                rm.add_beginRequest(handler);
            }
            Function.Handler.add(window, "unload", handler);
            for (var i = 0; i < document.forms.length; i++) {
                try {
                    document.forms[i].onsubmit = Function.Delegate.create(this, this.save);
                }
                catch (ex) { }
            }
            this.Initialized = true;
        }
    },
    save: function() {
        try {
            for (var i = 0; i < this.Maps.length; i++) {
                try {
                    this.Maps[i].save();
                }
                catch (ex1) { }
            }
        }
        catch (ex) { }
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
// GoogleDirection ////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GoogleDirection = function(map, config) {
    this.Locale = config.Locale;
    this.Query = config.Query;
    this.RoutePanelId = config.RoutePanelId;
    // origin
    var pane = null;
    if (this.RoutePanelId) pane = document.getElementById(this.RoutePanelId);
    this.GDirections = new GDirections(map.GMap, pane);
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
// GooglePolygon //////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GooglePolygon = function(map, index, config) {
    // init
    this.GPolygon = null;
    this.Map = map;
    this.Index = index;
    //
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
    // events
    if (map.PolygonEvents) {
        this.ClientEvents = map.PolygonEvents.ClientEvents;
        this.ServerEvents = map.PolygonEvents.ServerEvents;
    }
    this.attachEvents(this.ClientEvents, true);
    this.attachEvents(this.ServerEvents, false);
};
Artem.Web.GooglePolygon.prototype = {
    attachEvents: function(events, clients) {
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
                    GEvent.addListener(this.GPolygon, key, function(args) {
                        delegate.call(this, handler, args);
                    });
                }
            }
        }
    },
    raiseEvent: function(handler, args) {
        this.Map.save();
        if (handler) {
            handler = handler.replace("INDEX", this.Index);
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
        }
    },
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
// GooglePolyline /////////////////////////////////////////////////////////////////////////////////
//
Artem.Web.GooglePolyline = function(map, index, config) {
    // init
    this.GPolyline = null;
    this.Map = map;
    this.Index = index;
    //
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
};
Artem.Web.GooglePolyline.prototype = {
    attachEvents: function(events, clients) {
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
                    GEvent.addListener(this.GPolyline, key, function(args) {
                        delegate.call(this, handler, args);
                    });
                }
            }
        }
    },
    raiseEvent: function(handler, args) {
        this.Map.save();
        if (handler) {
            handler = handler.replace("INDEX", this.Index);
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
        }
    },
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
Artem.Web.GoogleMarker = function(map, index, config) {
    // init
    this.GMarker = null;
    this.Map = map;
    this.Index = index;
    // properties
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
    this.OpenInfoBehaviour = config.OpenInfoBehaviour;
    this.OpenWindowContent = null;
    this.ShadowSize = config.ShadowSize;
    this.ShadowUrl = config.ShadowUrl;
    this.Text = config.Text;
    this.Title = config.Title;
    this.IsGeoload = false;
    // events
    if (map.MarkerEvents) {
        this.ClientEvents = map.MarkerEvents.ClientEvents;
        this.ServerEvents = map.MarkerEvents.ServerEvents;
    }
};
Artem.Web.GoogleMarker.prototype = {
    isLoaded: function() {
        return (this.GMarker != null);
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
            GEvent.addListener(this.GMarker, eventName, Function.Delegate.create(this, this.openDefaultInfoWindow));
        //events
        this.attachEvents(this.ClientEvents, true);
        this.attachEvents(this.ServerEvents, false);
    },
    attachEvents: function(events, clients) {
        if (events) {
            var key;
            for (var i = 0; i < events.length; i++) {
                key = events[i].Key;
                if (key != 'geoload') {
                    if (clients) {
                        GEvent.addListener(this.GMarker, key,
                        Function.Delegate.createFromString(this, events[i].Handler));
                    }
                    else {
                        var handler = events[i].Handler;
                        var delegate = Function.Delegate.create(this, this.raiseEvent);
                        GEvent.addListener(this.GMarker, key, function(args) {
                            delegate.call(this, handler, args);
                        });
                    }
                }
                else if(this.IsGeoload){
                    if (clients)
                        Function.Delegate.callFromString(this, events[i].Handler);
                    else
                        this.raiseEvent(events[i].Handler, this.Address);
                }
            }
        }
    },
    raiseEvent: function(handler, args) {
        this.Map.save();
        if (handler) {
            handler = handler.replace("INDEX", this.Index);
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
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
    loadGeo: function(point) {
        this.IsGeoload = true;
        this.load(point);
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
    // Wrapped ------------------------------------------------------------------------------------
    closeInfoWindow: function() {
        if (this.isLoaded())
            this.GMarker.closeInfoWindow();
    },
    createIcon: function() {
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
    this.AddressNotFound = false;
    this.BaseCountryCode = config.BaseCountryCode;
    this.DefaultAddress = config.DefaultAddress;
    this.DefaultMapView = config.DefaultMapView;
    this.ClientID = config.ClientID;
    this.ClientMapID = config.ClientMapID;
    this.IsGeolocation = false;
    this.IsStatic = config.IsStatic;
    this.IsStreetView = config.IsStreetView;
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
    this.ClentAddressNotFoundIndex = null;
    this.ServerAddressNotFoundIndex = null;
    this.ClientGeoLoadedIndex = null;
    this.ServerGeoLoadedIndex = null;
    this.ClientLocationLoadedIndex = null;
    this.ServerLocationLoadedIndex = null;
    // collections
    this.Actions = null;
    this.Directions = null;
    this.Markers = null;
    this.Polygons = null;
    this.Polylines = null;
    // origin
    this.GMap = null;
    this.GMapPano = null;
    this.MarkerManager = null;
    // geocoder
    this.Geocoder = new GClientGeocoder();
    if (config.BaseCountryCode)
        this.Geocoder.setBaseCountryCode(config.BaseCountryCode);
    // initialize manager
    Artem.Web.GoogleManager.initialize();
    Artem.Web.GoogleManager.addMap(this);
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
};
Artem.Web.GoogleMap.prototype = {

    initialize: function() {
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
        if (this.EnableMarkerManager) this.MarkerManager = new GMarkerManager(this.GMap);
        // map view
        this.setMapView();
        // street view
        if (this.IsStreetView && this.StreetViewMode == 1) {
            var panoID = this.StreetViewPanoID || (this.ClientID + "_Pano");
            this.GMapPano = new GStreetviewPanorama(document.getElementById(panoID));
            this.GMap.addOverlay(new GStreetviewOverlay());
            GEvent.addListener(this.GMap, "click", Function.Delegate.create(this, this.setStreetView));
        }
    },
    attachEvents: function(events, clients) {
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
                        GEvent.addListener(this.GMap, key, function(overlay, args) {
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
    raiseEvent: function(handler, args) {
        this.save();
        if (handler) {
            if (args)
                handler = handler.replace("ARGS", args);
            eval(handler);
        }
    },
    load: function(point) {
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
                        this.raiseEvent(this.ServerEvents[this.ServerGeoLoadedIndex].Handler, this.Address);
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
                        this.raiseEvent(this.ServerEvents[this.ServerAddressNotFoundIndex].Handler, this.Address);
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
    loadAddress: function(address) {
        this.Address = address;
        this.IsGeolocation = true;
        this.Geocoder.getLatLng(this.Address, Function.Delegate.create(this, this.load));
    },
    loadStatic: function() {
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
    loadStreetView: function(point) {
        this.GMap = new GStreetviewPanorama(this.getElement(), { latlng: point });
        this.GMap.checkResize();
        //        GEvent.addListener(this.GMapPano, "error", function() {
        //            if (errorCode == 603) {
        //                alert("Error: Flash doesn't appear to be supported by your browser");
        //                return;
        //            }
        //        });
    },
    render: function() {
        // markers
        if (this.Markers) {
            for (var i = 0; i < this.Markers.length; i++) {
                this.renderMarker(this.Markers[i]);
            }
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
    renderDirection: function(d) {
        d.loadDefault();
    },
    renderMarker: function(m) {
        if ((m.Latitude != 0) && (m.Longitude != 0)) {
            try {
                m.load(new GLatLng(m.Latitude, m.Longitude));
            }
            catch (ex) { }
        }
        else {
            try {
                this.Geocoder.getLatLng(m.Address, Function.Delegate.create(m, m.loadGeo));
            }
            catch (ex) {
            }
        }
    },
    renderPolygon: function(pg) {
        this.addOverlay(pg.GPolygon);
    },
    renderPolyline: function(pl) {
        this.addOverlay(pl.GPolyline);
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
                case Artem.Web.GoogleMapView.Satellite3D:
                    this.GMap.addMapType(G_SATELLITE_3D_MAP);
                    this.GMap.setMapType(G_SATELLITE_3D_MAP);
                    break;
                case Artem.Web.GoogleMapView.MapMakerNormal:
                    this.GMap.addMapType(G_MAPMAKER_NORMAL_MAP);
                    this.GMap.setMapType(G_MAPMAKER_NORMAL_MAP);
                    break;
                case Artem.Web.GoogleMapView.MapMakerHybrid:
                    this.GMap.addMapType(G_MAPMAKER_HYBRID_MAP);
                    this.GMap.setMapType(G_MAPMAKER_HYBRID_MAP);
                    break;
            }
        }
    },
    setAddress: function(addresses) {
        if (addresses.Status.code == 200) {
            try {
                this.Address = addresses.Placemark[0].address;
                this.save();
                if (this.ClientLocationLoadedIndex != null) {
                    var delegate = Function.Delegate.createFromString(this, this.ClientEvents[this.ClientLocationLoadedIndex].Handler);
                    delegate.call(this, this.Address);
                }
                if (this.ServerLocationLoadedIndex != null) {
                    this.raiseEvent(this.ServerEvents[this.ServerLocationLoadedIndex].Handler, this.Address);
                }
            }
            catch (ex) { }
        }
    },
    setStreetView: function(overlay, latlng) {
        this.GMapPano.setLocationAndPOV(latlng);
    },
    // Items --------------------------------------------------------------------------------------
    // Marker
    addMarker: function(config, render) {
        if (!this.Markers) this.Markers = new Array();
        var marker = new Artem.Web.GoogleMarker(this, this.Markers.length, config);
        this.Markers.push(marker);
        if (render) this.renderMarker(marker);
    },
    // Polygons
    addPolygon: function(config, render) {
        if (!this.Polygons) this.Polygons = new Array();
        var polygon = new Artem.Web.GooglePolygon(this, this.Polygons.length, config);
        this.Polygons.push(polygon);
        if (render) this.renderPolygon(polygon);
    },
    // Polylines
    addPolyline: function(config, render) {
        if (!this.Polylines) this.Polylines = new Array();
        var polyline = new Artem.Web.GooglePolyline(this, this.Polylines.length, config);
        this.Polylines.push(polyline);
        if (render) this.renderPolyline(polyline);
    },
    // Directions
    addDirection: function(config, render) {
        if (!this.Directions) this.Directions = new Array();
        var dir = new Artem.Web.GoogleDirection(this, config);
        this.Directions.push(dir);
        if (render) this.renderDirection(dir);
    },
    // Actions
    addAction: function(action) {
        if (!this.Actions) this.Actions = new Array();
        this.Actions.push(action);
    },
    clearMarkers: function() {
        if (this.Markers) {
            var len = this.Markers.length;
            for (var i = 0; i < len; i++) {
                this.GMap.removeOverlay(this.Markers[i].GMarker);
            }
            this.Markers = new Array();
        }
    },
    clearPolygons: function(config) {
        if (this.Polygons) this.Polygons = new Array();
    },
    clearPolylines: function(config) {
        if (this.Polylines) this.Polylines = new Array();
    },
    clearDirections: function(config) {
        if (this.Directions) this.Directions = new Array();
    },
    clearActions: function(action) {
        if (this.Actions) this.Actions = new Array();
    },
    // load, initialize, save
    getElement: function() {
        return document.getElementById(this.ClientID);
    },
    // Wrapped ------------------------------------------------------------------------------------
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