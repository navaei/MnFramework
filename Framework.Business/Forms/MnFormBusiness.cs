using System;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Mn.Framework.Common;
using Mn.Framework.Common.Forms;
using Mn.Framework.Common.Model;
using Mn.Framework.Business;

namespace Mn.Framework.Business.Forms
{
    public class MnFormBusiness : BaseBusiness<MnForm>, IJbFormBusiness
    {
        public MnForm Get(Int64 id = 0)
        {
            return base.Get(f => f.Id == id);
        }
        public IQueryable<MnForm> Get(string refEntityName)
        {
            return base.GetList(x => x.RefEntityName == refEntityName);
        }
        public void Update(MnForm mnForm)
        {
            if (mnForm.RefEntityName == null || mnForm.RefEntityId == null)
                throw new Exception("RefEntityName and RefEntityId can not be null");
            mnForm.LastModifiedDate = DateTime.Now;
            base.Update(mnForm);
        }

        public void Delete(Int64 jbFormId)
        {
            base.Delete(jbFormId);
        }

        public OperationStatus CreateEdit(MnForm mnForm)
        {
            if (mnForm.Id == 0)
            {
                mnForm.CreatedDate = DateTime.UtcNow;
                return base.Create(mnForm);
            }
            else
            {
                mnForm.LastModifiedDate = DateTime.UtcNow;
                return base.Update(mnForm);
            }
        }
        public OperationStatus UpdateJson(Int64 formId, string jsonElements)
        {
            var form = Get(formId);
            form.JsonElements = jsonElements;
            return CreateEdit(form);
        }


    }
}
