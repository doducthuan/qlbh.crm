
using QLBH_Dion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Globalization;

namespace QLBH_Dion.Repository
{
    public class ProductRepository : IProductRepository
    {
        QLBHContext db;
        public ProductRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<Product>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.Products
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }
        public async Task<List<Product>> ListLargeNumber()
        {
            if (db != null)
            {
                return await (
                    from row in db.Products
                    where (row.Active == 1)
                    orderby row.Id descending
                    select new Product{
                        Id = row.Id,
                        Name = row.Name
                    }
                ).ToListAsync();
            }

            return null;
        }

        public async Task<List<Product>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.Products
                    where (row.Active == 1 && (row.Name.Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<Product>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.Products
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<Product>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.Products
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }


        public async Task<Product> Add(Product obj)
        {
            if (db != null)
            {
                await db.Products.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null; 

        }

        public async Task Update(Product obj)
        {
            if (db != null)
            {
                //Update that object
                db.Products.Attach(obj);
                db.Entry(obj).Property(x => x.ProductCategoryId).IsModified = true;
                db.Entry(obj).Property(x => x.ProductBrandId).IsModified = true;
                db.Entry(obj).Property(x => x.Name).IsModified = true;
                db.Entry(obj).Property(x => x.Code).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.Photo).IsModified = true;
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.ProvinceId).IsModified = true;
                db.Entry(obj).Property(x => x.Search).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(Product obj)
        {
            if (db != null)
            {
                //Update that obj
                db.Products.Attach(obj);
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
                var obj = await db.Products.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.Products.Remove(obj);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }


        public int Count()
        {
            int result = 0;

            if (db != null)
            {
                //Find the obj for specific obj id
                result = (
                    from row in db.Products
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<DTResult<ProductViewModel>> ListServerSide(ProductDTParameters parameters)
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
            var query = from row in db.Products

                        join pc in db.ProductCategories on row.ProductCategoryId equals pc.Id
                        join pb in db.ProductBrands on row.ProductBrandId equals pb.Id
                        join p in db.Provinces on row.ProvinceId equals p.Id

                        where row.Active == 1
                                        && pc.Active == 1
    && pb.Active == 1
    && p.Active == 1

                        select new
                        {
                            row,
                            pc,
                            pb,
                            p
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                    EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Code.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Photo.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Search.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))

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
                        case "name":
                            query = query.Where(c => (c.row.Name ?? "").Contains(fillter));
                            break;
                        case "code":
                            query = query.Where(c => (c.row.Code ?? "").Contains(fillter));
                            break;
                        case "description":
                            query = query.Where(c => (c.row.Description ?? "").Contains(fillter));
                            break;
                        case "photo":
                            query = query.Where(c => (c.row.Photo ?? "").Contains(fillter));
                            break;
                        case "active":
                            query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
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
                        case "search":
                            query = query.Where(c => (c.row.Search ?? "").Contains(fillter));
                            break;

                    }
                }
            }

            if (parameters.ProductCategoryIds.Count > 0)
            {
                query = query.Where(c => parameters.ProductCategoryIds.Contains(c.row.ProductCategory.Id));
            }


            if (parameters.ProductBrandIds.Count > 0)
            {
                query = query.Where(c => parameters.ProductBrandIds.Contains(c.row.ProductBrand.Id));
            }


            if (parameters.ProvinceIds.Count > 0)
            {
                query = query.Where(c => parameters.ProvinceIds.Contains(c.row.Province.Id));
            }


            //3.Query second
            var query2 = query.Select(c => new ProductViewModel()
            {
                Id = c.row.Id,
                ProductCategoryId = c.pc.Id,
                ProductCategoryName = c.pc.Name,
                Name = c.row.Name,
                Code = c.row.Code,
                Description = c.row.Description,
                Photo = c.row.Photo,
                Active = c.row.Active,
                CreatedTime = c.row.CreatedTime,
                ProvinceId = c.p.Id,
                ProvinceName = c.p.Name,
                Search = c.row.Search,
                ProductBrandName = c.row.ProductBrand.Name
            });
            //4. Sort
            query2 = query2.OrderByDynamic<ProductViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<ProductViewModel>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
    }
}


