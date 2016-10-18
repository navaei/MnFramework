using System;
using System.Linq;

namespace Mn.Framework.Common.Forms
{
    public interface IJbFormBusiness
    {
        MnForm Get(Int64 id = 0);
        IQueryable<MnForm> Get(string refEntityName);
        void Update(MnForm mnForm);
        void Delete(Int64 jbFormId);
        OperationStatus CreateEdit(MnForm mnForm);
        OperationStatus UpdateJson(Int64 formId, string jsonElements);
    }
}
