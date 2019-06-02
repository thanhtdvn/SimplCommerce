/*global angular, jQuery*/
(function ($) {
    angular
        .module('simplAdmin.paymentBankTransfer')
        .controller('BankTransferConfigFormCtrl', BankTransferConfigFormCtrl);

    /* @ngInject */
    function BankTransferConfigFormCtrl($state, paymentBankTransferService, translateService) {
        var vm = this;
        vm.translate = translateService;
        vm.bankTransferConfig = {};

        vm.save = function save() {
            vm.validationErrors = [];
            paymentBankTransferService.updateSetting(vm.bankTransferConfig)
                .then(function (result) {
                    toastr.success('Settings have been saved');
                })
                .catch(function (response) {
                    var error = response.data;
                    vm.validationErrors = [];
                    if (error && angular.isObject(error)) {
                        for (var key in error) {
                            vm.validationErrors.push(error[key][0]);
                        }
                    } else {
                        vm.validationErrors.push('Could not save settings.');
                    }
                });
        };

        function init() {
            paymentBankTransferService.getSettings().then(function (result) {
                vm.bankTransferConfig = result.data;
            });
        }

        init();
    }
})(jQuery);