app.controller('CtrlEvento', ['$scope', 'SendBox', 'ServicioEvento', function ($scope, SendBox, ServicioEvento) {
    $scope.vista = "Mantenimiento de Eventos";
    $scope.mostrarid = true;
    $scope.mostrarEstado = true;
    $scope.disabledID = true;
    $scope.operacion = "";
    $scope.eventos = [];


    var cargarUsuarios = function () {
        $scope.usuarios = [
            { usuario: 'JVM10', nombre: 'SISTEMA' },
            { usuario: 'EYOL', nombre: 'ERIK YOL' },
            { usuario: 'OLEMUS', nombre: 'OSMAN LEMUS' }
        ];
    };

    ServicioEvento.ObtenerEventos().then(function (data) {
        $scope.eventos = data;
    });

    $scope.nuevo = function () {
        cargarUsuarios();
        $scope.response = {};
        $scope.mostrarid = false;
        $scope.mostrarEstado = false;
        $scope.operacion = "Nuevo";
        $('#myModal').modal('show');
    };

    $scope.editar = function (evento) {
        cargarUsuarios();
        $scope.mostrarid = false;
        $scope.disabledID = true;
        $scope.mostrarEstado = true;
        $scope.operacion = "Editar";

        ServicioEvento.ObtenerEvento(evento).then(function (data) {
            $scope.response = data;
        });
        $('#myModal').modal('show');
    };

    $scope.guardar = function (evento, us) {
        evento.operacion = $scope.operacion;
        evento.usuario = us;
        SendBox.post(evento, 'api/GestionEvento/GuardarEvento')
        .then(function (data) {
            if (data.code==0) {
                alert(data.message);
                $scope.actualizar();
                $('#myModal').modal('hide');
            } else {
                alert(data.message);
            }
        });

    };

    $scope.actualizar = function () {
        ServicioEvento.ObtenerEventos().then(function (data) {
            $scope.eventos = data;
        });
    };
    

}]);

