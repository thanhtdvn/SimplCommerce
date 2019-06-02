/*global angular*/
(function () {
    angular
        .module('simplAdmin.paymentBankTransfer')
        .factory('paymentBankTransferService', paymentBankTransferService);

    /* @ngInject */
    function paymentBankTransferService($http) {
        var service = {
            getSettings: getSettings,
            updateSetting: updateSetting
        };
        return service;

        function getSettings() {
            return $http.get('api/banktransfer/config');
        }

        function updateSetting(settings) {
            return $http.put('api/banktransfer/config', settings);
        }
    }
})();