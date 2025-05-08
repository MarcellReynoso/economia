namespace economia.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<GastoPorCategoriaViewModel> PorCategoria { get; set; }
        public List<GastoPorFechaViewModel> PorFecha { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal PromedioDiario { get; set; }
    }
}
