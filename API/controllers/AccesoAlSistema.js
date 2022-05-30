'use strict';

var utils = require('../utils/writer.js');
var AccesoAlSistema = require('../service/AccesoAlSistemaService');

module.exports.cerrarSesion = function cerrarSesion (req, res, next) {
  AccesoAlSistema.cerrarSesion(req)
    .then(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    })
    .catch(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    });
};

module.exports.iniciarSesion = function iniciarSesion (req, res, next, nombreUsuario, clave) {
  AccesoAlSistema.iniciarSesion(nombreUsuario, clave)
    .then(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    })
    .catch(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    });
};

module.exports.patchRestablecer = function patchRestablecer (req, res, next, correoElectronico) {
  AccesoAlSistema.patchRestablecer(correoElectronico)
    .then(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    })
    .catch(function (response) {
      utils.writeJson(res, response['resBody'], response['statusCode']);
    });
};