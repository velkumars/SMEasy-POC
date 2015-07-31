using SMEasy.Common;
using SMEasy.Domain.Entity;
using SMEasy.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Xen.Entity;

namespace SMEasy.WebApi.Controllers
{
    public class ContactController : BaseController<Contact>
    {
        [ActionName(ActionName.SaveContact)]
        public HttpResponseMessage ContactSave(ContactViewModel entityModel)
        {
            if (entityModel.IsValid)
            {
                AutoMapper.Mapper.CreateMap<ContactViewModel, Contact>();
                Contact entity = AutoMapper.Mapper.Map<ContactViewModel, Contact>(entityModel);
                return base.Post(entity);
            }
            return Request.CreateResponse<BaseEFEntity>(HttpStatusCode.BadRequest, entityModel);
        }

        [HttpPost]
        [ActionName(ActionName.UpdateContact)]
        public HttpResponseMessage UpdateContact(ContactViewModel entityModel)
        {
            if (entityModel.IsValid)
            {
                AutoMapper.Mapper.CreateMap<ContactViewModel, Contact>();
                Contact entity = AutoMapper.Mapper.Map<ContactViewModel, Contact>(entityModel);
                return base.Put(entity.Id, entity);
            }
            return Request.CreateResponse<BaseEFEntity>(HttpStatusCode.BadRequest, entityModel);
        } 
    }
}
