using System.Collections.Generic;

namespace MVCModel
{
    public interface IBaseDetailEntity<TEntityDetail> where TEntityDetail : class
    {
        ICollection<TEntityDetail> GetDetails();
    }
}
