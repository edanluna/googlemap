using System;

namespace Artem.Web.UI.Controls {

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    enum GoogleMapBehaviour {
        /// <summary>
        /// 
        /// </summary>
        ContinuousZoom  = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        DoubleClickZoom = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        Dragging        = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        GoogleBar       = 0x0008,
        /// <summary>
        /// 
        /// </summary>
        InfoWindow      = 0x0010,
        /// <summary>
        /// 
        /// </summary>
        ScrollWheelZoom = 0x0020,
        /// <summary>
        /// 
        /// </summary>
        MarkerManager   = 0x0040,
        /// <summary>
        /// 
        /// </summary>
        MapTypeControl  = 0x0080,
        /// <summary>
        /// 
        /// </summary>
        Traffic         = 0x0100,
        /// <summary>
        /// 
        /// </summary>
        ScaleControl    = 0x0200,
        /// <summary>
        /// 
        /// </summary>
        ReverseGeocoding = 0x0400,
        /// <summary>
        /// 
        /// </summary>
        DefaultSet      = Dragging | InfoWindow | MapTypeControl
    }
}
