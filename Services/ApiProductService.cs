using Poliak_UI_WT.Domain.Entities;
using Poliak_UI_WT.Domain.Models;

namespace Poliak_UI_WT.Blazor.Services
{
    public class ApiProductService : IProductService<Phone>
    {
        List<Phone> _phones;
        int _currentPage = 1;
        int _totalPages = 1;

        HttpClient _httpClient;

        public ApiProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IEnumerable<Phone> Products => _phones;

        public int CurrentPage => _currentPage;

        public int TotalPages => _totalPages;

        public event Action ListChanged;

        public async Task GetProducts(int pageNo, int pageSize)
        {
            var uri = _httpClient.BaseAddress.AbsoluteUri;

            var queryData = new Dictionary<string, string>()
            {
                { "pageNo", pageNo.ToString() },
                {"pageSize", pageSize.ToString() }
            };

            var query = QueryString.Create(queryData);
            var result = await _httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                // получить данные из ответа
                var responseData = await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Phone>>>();
                // обновить параметры
                _currentPage = responseData.Data.CurrentPage;
                _totalPages = responseData.Data.TotalPages;
                _phones = responseData.Data.Items;
                ListChanged?.Invoke();
                //DebugHelper.ShowData($"Count Phones {_phones.Count}", _phones[0].ToString());
            }

            else
            {
                _phones = null;
                _currentPage = 1;
                _totalPages = 1;

            }
        }
    }
}
