app.controller('CtrlNotaCredito', ['$scope', 'SendBox', 'ServicioEvento', function ($scope, SendBox, ServicioEvento) {
    $scope.vista = "Nota de Credito";
    $scope.VerParticipantes = false;
    $scope.disabledID = true;
	$scope.ListaEventoNota = new Array();
	$scope.ListaParticipantesNota = new Array();

    $scope.usuarios = [
    /*{ usuario: 'JVM10', nombre: 'SISTEMA' },*/
    { usuario: 'RFLORES', nombre: 'RUT FLORES' },
    { usuario: 'PROSALES', nombre: 'PAOLA ROSALES' },
    { usuario: 'SSIAN', nombre: 'SARA SIAN' },
    { usuario: 'EDEFAJARDO', nombre: 'EMMA DE FAJARDO' },
    { usuario: 'EYOL', nombre: 'ERIK YOL' },
    { usuario: 'OLEMUS', nombre: 'OSMAN LEMUS' }
    ];

    $scope.tipopago = [
    { tipo_pago: 1, descripcion: 'EFECTIVO' },
    { tipo_pago: 2, descripcion: 'CHEQUE' }
    ];


    $scope.configList = {
        itemsPerPage: 5,
        fillLastPage: false,
        maxPages: 10
    };

    var limpiarCamposCobro = function () {
        $scope.tpago = "";
        $scope.usgestion = "";
        $scope.valor = null;
    };

    ServicioEvento.ListarEventosNota().then(function (data) {
        $scope.ListaEventoNota = data;
        $scope.idEventoNota = data.id_evento;

    });

    $scope.ObtenerParticipantesNota = function (evento) {

        ServicioEvento.ListarParticipantesNota(evento.id_evento).then(function (data) {
            $scope.ListaParticipantesNota = data;

            $scope.VerParticipantes = true;
        });
    };

    $scope.irModalNota = function (objparticipante) {

        limpiarCamposCobro();

        //CARGA LA INFORMACION DEL SALDO DEL PARTICIPANTE PARA MOSTRAR EN LA PANTALLA
        ServicioEvento.SaldoEventoNota(objparticipante).then(function (data) {
            $scope.saldoParticipante = data;
        });

        //CARGA LISTADO DE CARGOS REALIZADOS POR NOTA DE CREDITO A UN PARTICIPANTE
        ServicioEvento.ListarCargosNota(objparticipante).then(function (data) {
            $scope.cargosnotas = data;
        });

        $('#ModalNota').modal('show');

    };

    $scope.GenerarNota = function (objMovimiento, valor, tipopago, usuario) {

        var objParticipante = new Object();
        var respuesta = confirm("¿Esta Seguro de Realizar la Nota de Crédito con \n\n Valor de Q " + valor + " \n\n Y tipo de pago " + tipopago.descripcion + "?");

        objParticipante.id = objMovimiento.id_participante;
        objParticipante.idevento = objMovimiento.id_evento;

        objMovimiento.tipo_pago = tipopago.tipo_pago;
        objMovimiento.usuario = usuario;
        objMovimiento.valor = valor;

        if (respuesta == true) {
            SendBox.post(objMovimiento, 'api/GestionNotaDeCredito/GenerarNotaCredito')
            .then(function (data) {
                if (data.code == 0) {
                    alert(data.message);
                    //CARGA LISTADO DE CARGOS REALIZADOS POR NOTA DE CREDITO A UN PARTICIPANTE
                    ServicioEvento.ListarCargosNota(objParticipante).then(function (data) {
                        $scope.cargosnotas = data;
                    });
                    //CARGA LA INFORMACION DEL SALDO DEL PARTICIPANTE PARA MOSTRAR EN LA PANTALLA
                    ServicioEvento.SaldoEventoNota(objParticipante).then(function (data) {
                        $scope.saldoParticipante = data;
                    });
                    limpiarCamposCobro();
                } else {
                    alert(data.message);
                    limpiarCamposCobro();
                }
            });
        } else {
            limpiarCamposCobro();
        }
    };

    $scope.LimpiarFiltro = function () {
        $scope.buscar = {};
    };

}]);