using SupplierRequestsApp.Presentation.Controllers;

namespace SupplierRequestsApp.Presentation.Pages.Supplier
{
    public partial class EditSupplierPage : ContentPage
    {
        private readonly SuppliersPageController _controller;
        private Domain.Models.Supplier? _supplier;

        public EditSupplierPage(SuppliersPageController controller) : this(controller, null)
        {
        }

        public EditSupplierPage(SuppliersPageController controller, Domain.Models.Supplier supplier)
        {
            InitializeComponent();
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _supplier = supplier;
            if (_supplier != null)
            {
                TitleLabel.Text = "Редактировать поставщика";
                NameEntry.Text = _supplier.Name;
                AddressEntry.Text = _supplier.Address;
                PhoneEntry.Text = _supplier.Phone;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
                string.IsNullOrWhiteSpace(AddressEntry.Text) ||
                string.IsNullOrWhiteSpace(PhoneEntry.Text))
            {
                await DisplayAlert("Ошибка", "Заполните все поля", "ОК");
                return;
            }
            try
            {
                if (_supplier == null)
                {
                    var newSupplier = new Domain.Models.Supplier(Guid.NewGuid(),
                        NameEntry.Text.Trim(),
                        AddressEntry.Text.Trim(),
                        PhoneEntry.Text.Trim(),
                        new List<Domain.Models.Product>());
                    _controller.AddItem(newSupplier);
                }
                else
                {
                    _supplier.Name = NameEntry.Text.Trim();
                    _supplier.Address = AddressEntry.Text.Trim();
                    _supplier.Phone = PhoneEntry.Text.Trim();
                    _controller.EditItem(_supplier);
                }
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ошибка", "Не удалось сохранить поставщика", "ОК");
            }
        }
    }
}
