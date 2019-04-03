app.controller('CtrlReportes', ['$scope', 'SendBox', '$location', 'ServicioEvento', '$window', function ($scope, SendBox, $location, ServicioEvento, $window) {

    $scope.eventos = {};
    $scope.saldos = {};
    $scope.verListado = false;


    ServicioEvento.ObtenerEventos().then(function (data) {
        $scope.eventos = data;
    });


    $scope.ObtenerReportes = function () {
        $scope.verListado = true;
    };

    $scope.ObtenerSaldos = function (evento) {

        $scope.verListado = true;
        ServicioEvento.listaSaldosDiarios(evento.id_evento).then(function (data) {
            $scope.saldos = data;
        });
        
    };


    $scope.InscritosPorEventoPDF = function (evento) {
        $window.open("http://localhost:15872/home/reportaGeneralSaldosOpcionesPdf?idEvento=" + evento.id_evento);
    }

    $scope.InscritosPorEventoExcel = function (evento) {
        $window.open("http://localhost:15872/home/reportaGeneralSaldosOpcionesExcel?idEvento=" + evento.id_evento);
    }

    $scope.opcionesInscritoPDF = function (evento) {
        $window.open("http://localhost:15872/home/opcionesInscritosEventoPdf?idEvento=" + evento.id_evento);
    }

    $scope.opcionesInscritoExcel = function (evento) {
        $window.open("http://localhost:15872/home/opcionesInscritosEventoExcel?idEvento=" + evento.id_evento);
    }


}])
    .filter('sumByKey', function () {
    return function (data, key) {
        if (typeof (data) === 'undefined' || typeof (key) === 'undefined') {
            return 0;
        }

        var sum = 0;
        for (var i = data.length - 1; i >= 0; i--) {
            sum += parseInt(data[i][key]);
        }

        return sum;
    };
});



