define(['durandal/system', 'plugins/router', 'services/logger'],
    function (system, router, logger) {
        var shell = {
            activate: activate,
            router: router
        };
        
        return shell;

        //#region Internal Methods
        function activate() {
            return boot();
        }

        function boot() {
            log('Cloud Data Analytics Loaded!', null, true);

            router.on('router:route:not-found', function (fragment) {
                logError('No Route Found', fragment, true);
            });

            var routes = [
                { route: '', moduleId: 'home', title: 'Home', nav: 1 },
                { route: 'dashboard', moduleId: 'dashboard', title: 'Dashboard', nav: 2 },
                { route: 'reports', moduleId: 'reports', title: 'Reports', nav: 3 },
                { route: 'assets', moduleId: 'assets', title: 'Assets', nav: 4 },
                { route: 'configuration', moduleId: 'configuration', title: 'Configuration', nav: 5 },
                { route: 'samplechart', moduleId: 'samplechart', title: 'SampleChart', nav: false }
            
            ];

            return router.makeRelative({ moduleId: 'viewmodels' }) // router will look here for viewmodels by convention
                .map(routes)            // Map the routes
                .buildNavigationModel() // Finds all nav routes and readies them
                .activate();            // Activate the router
        }

        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(shell), showToast);
        }

        function logError(msg, data, showToast) {
            logger.logError(msg, data, system.getModuleId(shell), showToast);
        }
        //#endregion
    });