using PurseApi.Models.Entity;
using PurseApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PurseApi.Models.Entities
{
    public class CategoryService : IEntity
    {
        private static ServiceRepository repo = new ServiceRepository(false);
        private readonly int _code;
        private List<Service> _services;
        public int Code
        {
            get
            {
                return _code;
            }
        }
        public string Name { get; set; }
        public string Color { get; set; }

        public List<Service> Services
        {
            get
            {
                if (_services == null)
                    _services = repo.List.Where(x => x.CategoryCode == _code).ToList();
                return _services;
            }
        }

        public CategoryService(int code)
        {
            _code = code;
        }
    }
}