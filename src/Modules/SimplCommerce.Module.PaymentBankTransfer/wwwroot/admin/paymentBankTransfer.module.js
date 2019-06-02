/*global angular*/
(function () {
    'use strict';

    angular
        .module('simplAdmin.paymentBankTransfer', [])
        .config(['$stateProvider',
            function ($stateProvider) {
                $stateProvider
                    .state('payments-banktransfer-config', {
                        url: '/payments/banktransfer/config',
                        templateUrl: 'modules/paymentbanktransfer/admin/config/config-form.html',
                        controller: 'BankTransferConfigFormCtrl as vm'
                    })
                    ;
            }
        ]);
})();