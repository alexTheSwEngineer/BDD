using Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        //[Inject]
        protected IDataContext DataContext { get; private set; }

        //[Inject]
        protected ISystemUser SystemUser { get; private set; }
    }
}