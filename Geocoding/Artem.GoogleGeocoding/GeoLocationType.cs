﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Artem.Google.Net {

    /// <summary>
    /// 
    /// </summary>
    public enum GeoLocationType {
        /// <summary>
        /// indicates that the returned result is a precise geocode for which we have location information accurate down to street address precision.
        /// </summary>
        ROOFTOP,
        /// <summary>
        /// indicates that the returned result reflects an approximation (usually on a road) interpolated between two precise points (such as intersections). 
        /// Interpolated results are generally returned when rooftop geocodes are unavailable for a street address. 
        /// </summary>
        RANGE_INTERPOLATED,
        /// <summary>
        /// indicates that the returned result is the geometric center of a result such as a polyline (for example, a street) or polygon (region). 
        /// </summary>
        GEOMETRIC_CENTER,
        /// <summary>
        /// indicates that the returned result is approximate.
        /// </summary>
        APPROXIMATE
    }
}
