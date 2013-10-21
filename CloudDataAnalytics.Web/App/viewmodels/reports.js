define(['services/authorizationservice'], function (authorizationservice) {
    var reports = ko.observableArray();
    var initialized = false;

    var vm = {
        activate: activate,
        reports: reports,
        title: 'Select type of report:',
        refresh: refresh
        };
        return vm;
    
        //#region Internal Methods
        function activate() {
            if (initialized) { return; }
            initialized = true;
            return refresh();
        }
        function refresh() {
            //authorizationservice module to get authorizaton options
            authorizationservice.getReportsPartials(reports);
        }
        //#endregion
    }
);