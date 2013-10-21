define(['services/logger'], function (logger) {
    var title = 'Home';
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
        

    }
    //#endregion
});