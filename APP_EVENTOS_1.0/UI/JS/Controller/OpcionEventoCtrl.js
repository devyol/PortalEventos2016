app.controller('CtrlOpcEvento', ['$scope', '$rootScope', '$location', '$uibModal', 'SendBox', 'ServicioEvento', function ($scope, $rootScope, $location, $uibModal, SendBox, ServicioEvento) {
    $scope.vista = 'Mantenimiento de Opciones de Evento';
    $scope.vistaEliminar = 'Eliminar Opcion del Evento';
    $scope.mostrarOpciones = true;
    $scope.mostrarid = true;
    $scope.mostrarEstado = true;
    $scope.disabledID = true;
    $scope.mostraridopcion = false;
    $scope.mostraridevento = false;
    $scope.mostrarcomboeventos = true;
    $scope.operacion = "";
    $rootScope.var = 0;
    $rootScope.responseapi = {};
    $rootScope.otroresp = {};
    $scope.response = [];

    var cargarUsuarios = function () {
        $rootScope.usuarios = [
            { usuario: 'JVM10', nombre: 'SISTEMA' },
            { usuario: 'EYOL', nombre: 'ERIK YOL' },
            { usuario: 'OLEMUS', nombre: 'OSMAN LEMUS' }
        ];
    };

    ServicioEvento.ListaEventoActivo().then(function (data) {
        $scope.response = data;
        $scope.idEvento = data.id_evento;
        
    });

    $scope.obtenerOpciones = function (eventoid) {        
        ServicioEvento.ObtenerOpcionesEventos(eventoid.id_evento).then(function (data) {
        $scope.opciones = data;
        $scope.mostrarOpciones = true;            
        });
    };

    $scope.nuevo = function () {
        cargarUsuarios();
        ServicioEvento.ListaEventoActivo().then(function (data) {
            $scope.resp = data;
            $scope.idEventoNuevo = data.id_evento;
        });
        $scope.mostrarcomboeventos = true;
        $scope.infoOpcion = {};
        $scope.operacion = "Nuevo";
        $('#ModalGuardar').modal('show');
    };
    

    $scope.editar = function (idopcion) {
        cargarUsuarios();
        $scope.mostrarcomboeventos = false;        
        $scope.operacion = "Editar";
        ServicioEvento.ObtenerOpcionEvento(idopcion).then(function (data) {
            $scope.infoOpcion = data;            
        });

        $('#ModalGuardar').modal('show');
    };

    $scope.elimina = function (idopcion) {
        //$scope.mostrarcomboeventos = false;
        //ServicioEvento.ObtenerOpcionEvento(idopcion).then(function (data) {
        //    $scope.infoElimina = data;
        //    $scope.hola = "HOla";
        //});
        //$('#ModalEliminar').modal('show');
    };


    $scope.guardar = function (objopcion, us, idEventoNuevo) {

        if (typeof (objopcion.evento) == "undefined") {
            objopcion.evento = idEventoNuevo;
        }        

        objopcion.operacion = $scope.operacion;
        objopcion.usuario = us;

        if (objopcion.operacion == "Nuevo") {
            SendBox.post(objopcion, 'api/OpcionEvento/GuardarOpcionEvento')
            .then(function (data) {
                if (data.code == 0) {
                    alert(data.message);
                    ServicioEvento.ObtenerOpcionesEventos(idEventoNuevo).then(function (data) {
                        $scope.opciones = data;
                        $scope.mostrarOpciones = true;
                    });
                    $('#ModalGuardar').modal('hide');
                } else {
                    alert(data.message);
                }
            });
        } else {
            SendBox.post(objopcion, 'api/OpcionEvento/GuardarOpcionEvento')
            .then(function (data) {
                if (data.code == 0) {
                    alert(data.message);
                    ServicioEvento.ObtenerOpcionesEventos(objopcion.evento).then(function (data) {
                        $scope.opciones = data;
                        $scope.mostrarOpciones = true;
                    });
                    $('#ModalGuardar').modal('hide');
                } else {
                    alert(data.message);
                }
            });
        }
    };

    $scope.toEliminar = function (opcion) {
        $location.url("/EliminarOpcionEvento");
        cargarUsuarios();        
        SendBox.post(opcion, 'api/OpcionEvento/InformacionOpcionEvento')
        .then(function (data) {
            if (data.code == 0) {
                $rootScope.responseapi = data.data;
                $scope.descr = data.data.descripcion;
                $rootScope.test = "Hola Variable";
                //alert($rootScope.test + $scope.descr);
                //$scope.$apply();
            } else {
                alert(data.message);
            }
        });
    };



    

    $scope.showModal = function (evento) {
        $scope.objeto = evento;

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'JS/Template/MantOpcEvento/ModalAgregarOpcion.html',
            controller: 'ctrlModal',
            resolve: {
                objetoRecibido: function () {
                    return $scope.objeto;
                }
            }
        });


    };

}]);


app.controller('ctrlModal', ['$scope', '$uibModalInstance', 'SendBox', 'objetoRecibido', function ($scope, $uibModalInstance, objetoRecibido, SendBox) {
    $scope.objetoRecibido = objetoRecibido;
    $scope.iddelevento = $scope.objetoRecibido.evento;
    $scope.otraVariable = "HOLA MUNDO ";
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
}]);