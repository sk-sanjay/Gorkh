namespace Application.ViewModels
{
    public class SpecificationsVM : BaseVM
    {
        public int Id { get; set; }
        public string SpecfName { get; set; }
        public string TextType { get; set; }
        public bool IsCommon { get; set; }
    }
}
