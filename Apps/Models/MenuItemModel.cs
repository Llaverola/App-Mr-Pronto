using Xamarin.Forms;

namespace Apps.Models
{
    class MenuItemModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public string Icon { get; set; }
        public string Titulo { get; set; }
        public string Tag { get; set; }
        public string SubTitulo { get; set; }
        public string ImageSource { get; set; }
    }

    public class TerapeutaItemModel
    {
        public int TerapeutaId { get; set; }
        public int Index { get; set; }
        public ImageSource Image { get; set; }
        public string Nome { get; set; }
        public string Resumo { get; set; }
        public string PrecoTitulo { get; set; }
        public string Preco { get; set; }
        public string Tipo { get; set; }
    }
}
