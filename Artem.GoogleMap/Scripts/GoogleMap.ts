/// <reference path="MicrosoftAjax.js" />
// ------------------------------------------------------------------------------------------------
// Copyright (C) ArtemBG.
// ------------------------------------------------------------------------------------------------
// GoogleMap.js
// GoogleMap Control v7.0 javascipt library (debug).
//
// Assembly:    Artem.GooleMap
// Version:     7.0.0.0
// Project:     http://googlemap.codeplex.com
// Demo:        http://googlemap.artembg.com
// Author:      Velio Ivanov - velio@artembg.com
//              http://artembg.com
// License:     The MIT License (MIT)
//              http://www.codeplex.com/googlemap/license
// API:         https://developers.google.com/maps/documentation/javascript/

module Artem.Google {

    // Class
    export class TestMap {
        // Constructor
        constructor (element: Element) {
            Artem.Google.TestMap.initializeBase(this, [element]);
        }

        // Methods
        initialize() {
            Artem.Google.TestMap.callBaseMethod(this, 'initialize');
            alert("Test");
            var test = new Test();
            test.test();
            //this.create();
        }

        dispose() {
            //this.detachEvents();
            Artem.Google.TestMap.callBaseMethod(this, 'dispose');
        }

        // Static member
        static callBaseMethod;
        static initializeBase;
        static registerClass;
    }

    class Test {
        constructor () {
            alert("Basi");
        }
        test() {
            alert("test");
        }
    }
}
Artem.Google.TestMap.registerClass("Artem.Google.TestMap", Sys.UI.Control);