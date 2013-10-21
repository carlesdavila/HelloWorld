define(['services/logger'], function (logger) {
    var title = 'SampleChart';
    var vm = {
        activate: activate,
        attached: attached,
        title: title
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log(title + ' View Activated', null, title, true);
        return true;
    }

    //when DOM is loaded
    function attached() {
            
        var jsonurl = "http://openweathermap.org/data/2.1/history/city/?id=524901&cnt=80";
        $.get(jsonurl, getData);

    }
    
    function getData(JSONtext) {
        JSONobject = ParseJson(JSONtext);
        data = JSONobject.list;
        //showSimpleChart('chart-simple', data);
        //showBarsDouble('chart4', data);
        //showTempMinMax('chart2', data);
        showIconsChart('chart3', data);
        //showTemp('chart5', data);
        //showWind('chart6', data);
        // chartSpeed('chart3', data);
        // showPolarSpeed('chart-wind', data);
        // showPolar('chart-wind', data);
        // chartDoublePress('chart-wind', data);
    }
    //#endregion
});