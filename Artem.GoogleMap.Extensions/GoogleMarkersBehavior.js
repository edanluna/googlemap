/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("ArtemGoogleMap");


ArtemGoogleMap.GoogleMarkersBehavior = function(element) {
    ArtemGoogleMap.GoogleMarkersBehavior.initializeBase(this, [element]);
}

ArtemGoogleMap.GoogleMarkersBehavior.prototype = {
    initialize: function() {
        ArtemGoogleMap.GoogleMarkersBehavior.callBaseMethod(this, 'initialize');

        alert("BASI");
    },
    dispose: function() {
        //Add custom dispose actions here
        ArtemGoogleMap.GoogleMarkersBehavior.callBaseMethod(this, 'dispose');
    }
}
ArtemGoogleMap.GoogleMarkersBehavior.registerClass('ArtemGoogleMap.GoogleMarkersBehavior', Sys.UI.Behavior);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();