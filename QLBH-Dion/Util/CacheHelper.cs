using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using QLBH_Dion.Models;

namespace QLBH_Dion.Util
{
    public interface ICacheHelper
    {
        //Menu System Admin - groupId = 3 and menuTypeId = 1000005
        //List<Menu> GetMenuSystemAdmin();
        //void SetMenuSystemAdmin(List<Menu> menuAdmin);
        //List Right
        List<Right> GetRights();
        void SetRights(List<Right> rights);
        //List all Menu
        //List<Menu> GetMenu();
        //void SetMenu(List<Menu> menu);
        //List RoleRights của thằng chưa đăng nhập
        List<RoleRight> GetRoleRightsNotLogin();
        void SetRoleRightsNotLogin(List<RoleRight> roleRightsNotLogin);
    }

    public class CacheHelper : ICacheHelper
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        //System config
        //Menu System Admin
        //public List<Menu> GetMenuSystemAdmin()
        //{
        //    _cache.TryGetValue<List<Menu>>("menuSystemAdmin", out var json);

        //    return json;
        //}
        //public void SetMenuSystemAdmin(List<Menu> menuAdmins)
        //{
        //    _cache.Set("menuSystemAdmin", menuAdmins);
        //}
        //List Rights
        public List<Right> GetRights()
        {
            _cache.TryGetValue<List<Right>>("Rights", out var json);
            return json;
        }
        public void SetRights(List<Right> rights)
        {
            _cache.Set("Rights", rights);
        }
        //list Menu
        //public List<Menu> GetMenu()
        //{
        //    _cache.TryGetValue<List<Menu>>("Menu", out var json);
        //    return json;
        //}
        //public void SetMenu(List<Menu> menu)
        //{
        //    _cache.Set("Menu", menu);
        //}
        //list RoleRegihts not login
        public List<RoleRight> GetRoleRightsNotLogin()
        {
            _cache.TryGetValue<List<RoleRight>>("RoleRightsNotLogin", out var json);
            return json;
        }
        public void SetRoleRightsNotLogin(List<RoleRight> roleRightsNotLogin)
        {
            _cache.Set("RoleRightsNotLogin", roleRightsNotLogin);
        }
    }
}
