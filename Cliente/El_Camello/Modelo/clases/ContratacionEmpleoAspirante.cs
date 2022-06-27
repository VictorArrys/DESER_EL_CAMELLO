﻿namespace El_Camello.Modelo.clases
{
    public class ContratacionEmpleoAspirante
    {
        private int idAspirante;
        private int idUsuario;
        private string nombreAspiranteContratado;
        private string telefono;
        private string direccion;
        private int valoracionAspirante;

        public int IdAspirante { get => idAspirante; set => idAspirante = value; }
        public string NombreAspiranteContratado { get => nombreAspiranteContratado; set => nombreAspiranteContratado = value; }
        public int ValoracionAspirante { get => valoracionAspirante; set => valoracionAspirante = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
    }
}
