
        using System.Collections.Generic;
        using System.Threading.Tasks;
        using QLBH_Dion.Models.ViewModels;

        namespace QLBH_Dion.Services.Interfaces
        {
            public interface IBaseService<TModel> where TModel : class
            {
                Task<List<TModel>> List();
                Task<List<TModel>> Search(string keyword);
                Task<List<TModel>> ListPaging(int pageIndex,int pageSize);
                Task<List<TModel>> Detail(int? id);
                Task Add(TModel obj);
                Task Update(TModel obj);
                Task Delete(TModel obj);
                Task<int> DeletePermanently(int? id);
                int Count();
            }
        }
    