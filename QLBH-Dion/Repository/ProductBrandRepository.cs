using Microsoft.EntityFrameworkCore;
using QLBH_Dion.Models;
using QLBH_Dion.Repository.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using System.Globalization;

namespace QLBH_Dion.Repository
{
    public class ProductBrandRepository : IProductBrandRepository
    {
        QLBHContext db;
        public ProductBrandRepository(QLBHContext _db)
        {
            db = _db;
        }
        public async Task<ProductBrand> Add(ProductBrand ProductBrand)
        {
            if(db!=null)
            {
                await db.AddAsync(ProductBrand);
                await db.SaveChangesAsync();
                return ProductBrand;
            }
            return null;
        }

        public int Count()
        {
            if(db != null)
            {
                return db.ProductBrands.Where(x => x.Active == 1).Count();
            }
            return 0;
        }

        public async Task Delete(ProductBrand obj)
        {
            if (db != null)
            {
                //Update that obj
                db.ProductBrands.Attach(obj);
                db.Entry(obj).Property(x => x.Active).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> DeletePermanently(int? objId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the obj for specific obj id
                var obj = await db.ProductBrands.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.ProductBrands.Remove(obj);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<ProductBrand>> Detail(int? postId)
        {
            if(db!= null)
            {
                return await db.ProductBrands.Where(x => x.Active == 1 && x.Id == postId).ToListAsync();
            }
            return null;
        }

        public async Task<List<ProductBrand>> List()
        {
            if( db != null)
            {
                return await db.ProductBrands.Where(x => x.Active == 1).ToListAsync();
            }
            return null;
        }

        public async Task<List<ProductBrand>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await db.ProductBrands.Where(x => x.Active == 1).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }

        public async Task<DTResult<ProductBrand>> ListServerSide(ProductBrandDTParameters parameters)
        {
            if(db != null)
            {
                //0. Options
                string searchAll = parameters.SearchAll.Trim();//Trim text
                string orderCritirea = "Id";//Set default critirea
                int recordTotal, recordFiltered;
                bool orderDirectionASC = true;//Set default ascending
                if (parameters.Order != null)
                {
                    orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                    orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
                }
                //1. Join
                var query = from row in db.ProductBrands


                            where row.Active == 1

                            select new
                            {
                                row
                            };

                recordTotal = await query.CountAsync();
                //2. Fillter
                if (!String.IsNullOrEmpty(searchAll))
                {
                    searchAll = searchAll.ToLower();
                    query = query.Where(c =>
                        EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                        EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                        EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                        EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                        EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                        EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
                    );
                }
                foreach (var item in parameters.Columns)
                {
                    var fillter = item.Search.Value.Trim();
                    if (fillter.Length > 0)
                    {
                        switch (item.Data)
                        {
                            case "id":
                                query = query.Where(c => c.row.Id.ToString().Trim().Contains(fillter));
                                break;
                            case "Name":
                                query = query.Where(c => c.row.Name.Trim().Contains(fillter));
                                break;
                            case "Description":
                                query = query.Where(c => (c.row.Description ?? "").Contains(fillter));
                                break;
                            case "createdTime":
                                if (fillter.Contains(" - "))
                                {
                                    var dates = fillter.Split(" - ");
                                    var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                    query = query.Where(c => c.row.CreatedTime >= startDate && c.row.CreatedTime <= endDate);
                                }
                                else
                                {
                                    var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    query = query.Where(c => c.row.CreatedTime.Date == date.Date);
                                }
                                break;
                        }
                    }
                }

                //3.Query second
                var query2 = query.Select(c => new ProductBrand()
                {
                    Id = c.row.Id,
                    Name = c.row.Name,
                    Description = c.row.Description,
                    Active = c.row.Active,
                    CreatedTime = c.row.CreatedTime,

                });
                //4. Sort
                query2 = query2.OrderByDynamic<ProductBrand>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
                recordFiltered = await query2.CountAsync();
                //5. Return data
                return new DTResult<ProductBrand>()
                {
                    data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                    draw = parameters.Draw,
                    recordsFiltered = recordFiltered,
                    recordsTotal = recordTotal
                };
            }
            return null;
        }

        public Task<List<ProductBrand>> Search(string keyword)
        {
            throw new NotImplementedException();
        }

        public async Task Update(ProductBrand obj)
        {
            if (db != null)
            {
                //Update that object
                db.ProductBrands.Attach(obj);
                db.Entry(obj).Property(x => x.Name).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }
    }
}
