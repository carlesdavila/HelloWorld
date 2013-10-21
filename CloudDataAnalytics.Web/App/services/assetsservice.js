define(['services/logger', 'durandal/system', 'services/model'],
    function (logger, system, model) {
        
    var getAssetsPartials = function (assetsObservable) {
        //reset the observable
        assetsObservable([]);
        //set ajax call
        var options = {
            url: '/api/assets',
            type: 'GET',
            dataType: 'json'
        };
        //make call
        return $.ajax(options)
            .then(querySucceeded)
            .fail(queryFailed);;
        //handle the ajax callback

        function querySucceeded(data) {
            var assets = [];
            data.sort(sortAssets);
            data.forEach(function (item) {
                var a = new model.AssetPartial(item);
                assets.push(a);
            });
            assetsObservable(assets);
            log('Retrieved' + assets.length + ' assets from remote data source',
            assets,
            true);
        }
    };
    var assetsservice = {
        getAssetsPartials: getAssetsPartials
    };
    return assetsservice;
        
    //#region Internal methods
    function sortAssets(s1, s2) {
        return (s1.firstName + s1.lastName > s2.firstName + s2.lastName)
            ? 1 : -1;
    }


    function queryFailed(jqXhr, textStatus) {
        var msg = 'Error getting data. ' + textStatus;
        logger.log(msg,
            jqXhr,
            system.getModuleId(assetsservice),
            true);
    }

    function log(msg, data, showToast) {
        logger.log(msg,
            data,
            system.getModuleId(assetsservice),
            showToast);
    }
    //#endregion
});
