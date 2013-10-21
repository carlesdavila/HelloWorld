define(['services/logger', 'durandal/system', 'services/model'],
    function (logger, system, model) {
        
        var getReportsPartials = function (reportsObservable) {
        
            var fakeReports = [{ typeName: 'Statistics', imageSource: "statisticsicon.png", order: 1 }, { typeName: 'Comparatives', imageSource: "comparativesicon.png", order: 2 }, { typeName: 'Ranking', imageSource: "rankingicon.png", order: 3 }];

            //reset the observable
            reportsObservable([]);

            //set ajax call
            //todo: to be implemented whwn authorization exists

            //make call
            return querySucceeded(fakeReports);

            //handle the ajax callback
            function querySucceeded(data) {
                var reports = [];
                data.sort(sortReports);
                data.forEach(function (item) {
                    var r = new model.ReportPartial(item);
                    reports.push(r);
                });
                reportsObservable(reports);
                log('Retrieved ' + reports.length + ' types of reports from remote data source',
                reports,
                true);
            }
        };
        var authorizationservice = {
            getReportsPartials: getReportsPartials
        };
        return authorizationservice;
        
        //#region Internal methods
        function sortReports(s1, s2) {
            return (s1.Order + s1.Order > s2.Order + s2.Order)
                ? 1 : -1;
        }

        //todo: uncomment when authorization implemented
        //function queryFailed(jqXhr, textStatus) {
        //    var msg = 'Error getting data. ' + textStatus;
        //    logger.log(msg,
        //        jqXhr,
        //        system.getModuleId(authorizationservice),
        //        true);
        //}

        function log(msg, data, showToast) {
            logger.log(msg,
                data,
                system.getModuleId(authorizationservice),
                showToast);
        }
        //#endregion
});