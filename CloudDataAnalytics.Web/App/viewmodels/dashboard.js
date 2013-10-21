define(['services/logger'], function (logger) {
    var title = 'Dashboard';
    var dataSource = ko.observableArray();
    var dataCircular = ko.observableArray();
    
    var vm = {
        activate: activate,
        //attached: attached,
        compositionComplete: compositionComplete,
        title: title,

        chartCircularOptions: {
            scale: {
                startValue: 0,
                endValue: 60,
                majorTick: {
                    tickInterval: 10
                }
            },

            rangeContainer: {
                backgroundColor: "none",
                ranges: [
                    {
                        startValue: 0,
                        endValue: 20,
                        color: "#A6C567"
                    },
                    {
                        startValue: 20,
                        endValue: 40,
                        color: "#FCBB69"
                    },
                    {
                        startValue: 40,
                        endValue: 60,
                        color: "#E19094"
                    }
                ]
            },

            needles: [{value: 24}],

            markers: [{value: 27}, { value: 44}]
        },

        chartOptions: {
            dataSource: dataSource,
            commonSeriesSettings: {
                argumentField: "month"
            },
            panes: [{
                name: "topPane"
            }, {
                name: "bottomPane"
            }],
            series: [{
                pane: "topPane",
                color: "#87CEEB",
                type: "rangeArea",
                rangeValue1Field: "minT",
                rangeValue2Field: "maxT",
                name: "Monthly Temperature Ranges, °C"
            }, {
                pane: "topPane",
                valueField: "avgT",
                name: "Average Temperature, °C",
                label: {
                    visible: true,
                    customizeText: function () {
                        return this.valueText + " °C";
                    }
                }
            }, {
                type: "bar",
                valueField: "prec",
                name: "prec, mm",
                label: {
                    visible: true,
                    customizeText: function () {
                        return this.valueText + " mm";
                    }
                }
            }
            ],
            valueAxis: [{
                pane: "bottomPane",
                grid: {
                    visible: true
                },
                title: {
                    text: "Precipitation, mm"
                }
            }, {
                pane: "topPane",
                min: 0,
                max: 30,
                grid: {
                    visible: true
                },
                title: {
                    text: "Temperature, °C"
                }
            }],
            legend: {
                verticalAlignment: "bottom",
                horizontalAlignment: "center"
            },
            title: {
                text: "Weather in Barcelona"
            }
        }
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log(title + ' View Activated', null, title, true);

        return true;
    }


    function compositionComplete() {        

        var dataS = [ { month: "January", avgT: 9.8, minT: 4.1, maxT: 15.5, prec: 109 },{ month: "February", avgT: 11.8, minT: 5.8, maxT: 17.8, prec: 104 },{ month: "March", avgT: 13.4, minT: 7.2, maxT: 19.6, prec: 92 },{ month: "April", avgT: 15.4, minT: 8.1, maxT: 22.8, prec: 30 },{ month: "May", avgT: 18, minT: 10.3, maxT: 25.7, prec: 10 },{ month: "June", avgT: 20.6, minT: 12.2, maxT: 29, prec: 2 },{ month: "July", avgT: 22.2, minT: 13.2, maxT: 31.3, prec: 2 },{ month: "August", avgT: 22.2, minT: 13.2, maxT: 31.1, prec: 1 },{ month: "September", avgT: 21.2, minT: 12.4, maxT: 29.9, prec: 8 },{ month: "October", avgT: 17.9, minT: 9.7, maxT: 26.1, prec: 24 },{ month: "November", avgT: 12.9, minT: 6.2, maxT: 19.6, prec: 64 },{ month: "December", avgT: 9.6, minT: 3.4, maxT: 15.7, prec: 76 }];

        dataSource(dataS);
    }

//#endregion
});