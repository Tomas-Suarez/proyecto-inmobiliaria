namespace proyecto_inmobiliaria.Constants
{
    public static class ContratoConstants
    {
        public const string NO_SE_ENCONTRO_CONTRATO_POR_ID = "No se encontró el contrato con el id: ";
        public const string ERROR_AL_BORRAR_CONTRATO = "Ocurrio un error al borrar el contrato!";
        public const string ERROR_CONTRATO_FINALIZADO = "El contrato ya se encuentra finalizado!";
        public const string PAGOS_PENDIENTES = "El contrato tiene pagos pendientes. No es posible finalizarse hasta pagarlos.";
        public const string CONTRATO_EXISTE_EN_ESAS_FECHAS = "No fue posible crear un contrato. Ya existe un contrato en esas fechas para este inmueble.";
    }
}