define(function () {
    
    var imageSettings = {
    imageBasePath: 'content/images/reports/',
    unknownPersonImageSource: 'unknown_reporttype.jpg'
    };

    var AssetPartial = function (dto) {
        // Map to observables and add computeds
        return addAssetPartialComputeds(
            mapToObservable(dto));
    };
    var ReportPartial = function (dto) {
        // Map to observables and add computeds
        return addReportPartialComputeds(
            mapToObservable(dto));
    };

    var model = {
    AssetPartial: AssetPartial,
    ReportPartial: ReportPartial
    };

    return model;

    //#region Internal Methods
    function mapToObservable(dto) {
        var mapped = {};
        for (prop in dto) {
            if (dto.hasOwnProperty(prop)) {
                mapped[prop] = ko.observable(dto[prop]);
            }
        }
        return mapped;
    };

    function addAssetPartialComputeds(entity) {
        entity.fullName = ko.computed(function () {
            return entity.firstName() + ' '
                + entity.lastName();
        });
        entity.imageName = ko.computed(function () {
            return makeImageName(entity.imageSource());
        });
        return entity;
    };
    
    function addReportPartialComputeds(entity) {
        entity.fullName = ko.computed(function () {
            return entity.order() + '.'
                + entity.typeName();
        });
        entity.imageName = ko.computed(function () {
            return makeImageName(entity.imageSource());
        });
        return entity;
    };

    function makeImageName(source) {
        return imageSettings.imageBasePath +
            (source || imageSettings.unknownPersonImageSource);
    };

    //#endregion
});