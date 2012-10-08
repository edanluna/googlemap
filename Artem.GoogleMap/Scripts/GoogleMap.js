var Artem;
(function (Artem) {
    (function (Google) {
        var TestMap = (function () {
            function TestMap(element) {
                Artem.Google.TestMap.initializeBase(this, [
                    element
                ]);
            }
            TestMap.prototype.initialize = function () {
                Artem.Google.TestMap.callBaseMethod(this, 'initialize');
                alert("Test");
                var test = new Test();
                test.test();
            };
            TestMap.prototype.dispose = function () {
                Artem.Google.TestMap.callBaseMethod(this, 'dispose');
            };
            TestMap.callBaseMethod = undefined;
            TestMap.initializeBase = undefined;
            TestMap.registerClass = undefined;
            return TestMap;
        })();
        Google.TestMap = TestMap;        
        var Test = (function () {
            function Test() {
                alert("Basi");
            }
            Test.prototype.test = function () {
                alert("test");
            };
            return Test;
        })();        
    })(Artem.Google || (Artem.Google = {}));
    var Google = Artem.Google;

})(Artem || (Artem = {}));

Artem.Google.TestMap.registerClass("Artem.Google.TestMap", Sys.UI.Control);
